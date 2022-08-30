using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Full.Ids;

[Dependency(ReplaceServices = true)]
public class SequentialGuidGenerator : IdGeneratorBase<Guid>, IGuidGenerator, ISingletonDependency
{
    public SequentialGuidType GuidType { get; }

    public SequentialGuidGenerator(IOptions<AbpSequentialGuidGeneratorOptions> options) : this(options.Value.WorkId,
        options.Value.WorkIdBitsLength,
        guidType: options.Value.GuidType)
    {
    }

    public SequentialGuidGenerator(long workId, int workIdBitsLength = 24, int timestampBitsLenght = 48,
        int seqBitsLength = 16,
        int randomBitsLength = 40, SequentialGuidType guidType = SequentialGuidType.SequentialAtEnd,
        DateTime? baseTime = null)
        : base(timestampBitsLenght, seqBitsLength, randomBitsLength, workId, workIdBitsLength, baseTime)
    {
        GuidType = guidType;
        if (workIdBitsLength >= 64)
        {
            throw new InvalidOperationException();
        }

        if (timestampBitsLenght + seqBitsLength + workIdBitsLength + randomBitsLength != 128)
        {
            throw new InvalidOperationException();
        }
    }


    public override Guid Create()
    {
        return Create(GuidType);
    }

    public Guid Create(SequentialGuidType sequentialGuidType)
    {
        return Encode(CreateSequenceId(), sequentialGuidType);
    }

    private Guid Encode(SequenceId sequenceId, SequentialGuidType guidType)
    {
        // We start with 16 bytes of cryptographically strong random data.


        // An alternate method: use a normally-created GUID to get our initial
        // random data:
        // byte[] randomBytes = Guid.NewGuid().ToByteArray();
        // This is faster than using RNGCryptoServiceProvider, but I don't
        // recommend it because the .NET Framework makes no guarantee of the
        // randomness of GUID data, and future versions (or different
        // implementations like Mono) might use a different method.

        // Now we have the random basis for our GUID.  Next, we need to
        // create the six-byte block which will be our timestamp.

        // We start with the number of milliseconds that have elapsed since
        // DateTime.MinValue.  This will form the timestamp.  There's no use
        // being more specific than milliseconds, since DateTime.Now has
        // limited resolution.

        // Using millisecond resolution for our 48-bit timestamp gives us
        // about 5900 years before the timestamp overflows and cycles.
        // Hopefully this should be sufficient for most purposes. :)


        // Then get the bytes
        var timestampAndSeq = (sequenceId.Timestamp << SeqBitsLength) | sequenceId.Seq;
        var timestampAndSeqBytes = BitConverter.GetBytes(timestampAndSeq);

        var workIdAndRandom = (sequenceId.WorkId << RandomBitsLength) | sequenceId.Random;
        var workIdAndRandomBytes = BitConverter.GetBytes(workIdAndRandom);

        // Since we're converting from an Int64, we have to reverse on
        // little-endian systems.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampAndSeqBytes);
        }

        byte[] guidBytes = new byte[16];

        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:

                // For string and byte-array version, we copy the timestamp first, followed
                // by the random data.
                Buffer.BlockCopy(timestampAndSeqBytes, 0, guidBytes, 0, 8);
                Buffer.BlockCopy(workIdAndRandomBytes, 0, guidBytes, 8, 8);

                // If formatting as a string, we have to compensate for the fact
                // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
                // respectively.  That means that it switches the order on little-endian
                // systems.  So again, we have to reverse.
                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 8);
                    // Array.Reverse(guidBytes, 4, 4);
                }

                break;

            case SequentialGuidType.SequentialAtEnd:

                // For sequential-at-the-end versions, we copy the random data first,
                // followed by the timestamp.
                Buffer.BlockCopy(workIdAndRandomBytes, 0, guidBytes, 0, 8);
                Buffer.BlockCopy(timestampAndSeqBytes, 0, guidBytes, 8, 8);
                break;
        }

        return new Guid(guidBytes);
    }

    public override Guid Encode(SequenceId sequenceId)
    {
        return Encode(sequenceId, GuidType);
    }

    public override SequenceId Parse(Guid id)
    {
        return Parse(id, GuidType);
    }

    public SequenceId Parse(Guid id, SequentialGuidType guidType)
    {
        var bytes = id.ToByteArray();

        long timestampAndSeq, workIdAndRandom;
        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:

                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes, 0, 8);
                    // Array.Reverse(bytes, 4, 4);
                }

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes, 0, 8);
                }

                var span = bytes.AsSpan();

                timestampAndSeq = BitConverter.ToInt64(span[..8]);
                workIdAndRandom = BitConverter.ToInt64(span[8..16]);
                break;
            case SequentialGuidType.SequentialAtEnd:
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes, 8, 8);
                }

                span = bytes.AsSpan();
                timestampAndSeq = BitConverter.ToInt64(span[8..16]);
                workIdAndRandom = BitConverter.ToInt64(span[..8]);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(guidType), guidType, null);
        }

        var timestamp = (timestampAndSeq >> SeqBitsLength) & ~(-1L << TimestampBitsLength);
        var seq = timestampAndSeq & ~(-1L << SeqBitsLength);

        var workId = (workIdAndRandom >> RandomBitsLength) & ~(-1L << WorkIdBitsLength);
        ;
        var random = workIdAndRandom & ~(-1L << RandomBitsLength);

        return new SequenceId() { Timestamp = timestamp, Random = random, Seq = seq, WorkId = workId };
    }
}
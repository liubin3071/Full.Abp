using Volo.Abp.DependencyInjection;

namespace Full.Ids;

public class SnowflakeIdGenerator : IdGeneratorBase<long>, ISingletonDependency
{
    public const int TotalBitsLength = 63;

    public SnowflakeIdGenerator(long workId, int workIdBitsLength = 13, int timestampBitsLength = 41, int seqBitsLength = 9, int randomBitsLength = 0,
        DateTime? baseTime = null)
        : base(timestampBitsLength, seqBitsLength, randomBitsLength, workId, workIdBitsLength, baseTime)
    {
        if (timestampBitsLength + seqBitsLength + randomBitsLength + workIdBitsLength != 63)
        {
            throw new InvalidOperationException();
        }
    }

    public override long Encode(SequenceId sequenceId)
    {
        return (((((sequenceId.Timestamp << SeqBitsLength) | sequenceId.Seq) << WorkIdBitsLength) | WorkId) <<
                RandomBitsLength) | sequenceId.Random;
    }

    public override SequenceId Parse(long id)
    {
        var random = id & ~(-1L << RandomBitsLength);
        var workId = id >> RandomBitsLength & ~(-1L << WorkIdBitsLength);
        var seq = id >> (RandomBitsLength + WorkIdBitsLength) & ~(-1L << SeqBitsLength);
        var timestamp = id >> (RandomBitsLength + WorkIdBitsLength + SeqBitsLength) & ~(-1L << TimestampBitsLength);
        return new SequenceId() {
            Timestamp = (long)timestamp, Random = (long)random, Seq = (long)seq, WorkId = (long)workId
        };
    }
}
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using Volo.Abp.DependencyInjection;

namespace Full.Ids;

public abstract class IdGeneratorBase<T> : ISingletonDependency
{
    public readonly static DateTime DefaultBaseTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public DateTime BaseTime { get; }
    public int TimestampBitsLength { get; }
    public int SeqBitsLength { get; }
    public int RandomBitsLength { get; }
    public long WorkId { get; }
    public int WorkIdBitsLength { get; }

    private readonly long _maxSeq;

    private long _seq;
    private long _lastTimestamp;

    // ReSharper disable once StaticMemberInGenericType
    private readonly static RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    private readonly long _maxTimestamp;

    public int SeqFormatLength { get; }
    public int RandomFormatLength { get; }
    public int WorkIdFormatLength { get; }

    protected IdGeneratorBase(int timestampBitsLength, int seqBitsLength, int randomBitsLength,
        long workId, int workIdBitsLength, DateTime? baseTime = null)
    {
        if (timestampBitsLength > sizeof(long) * 8)
        {
            throw new ArgumentOutOfRangeException(nameof(timestampBitsLength));
        }

        if (seqBitsLength > sizeof(long) * 8)
        {
            throw new ArgumentOutOfRangeException(nameof(timestampBitsLength));
        }

        if (workId >= Math.Pow(2, workIdBitsLength))
        {
            throw new ArgumentOutOfRangeException(nameof(workId));
        }

        WorkId = workId;
        BaseTime = baseTime ?? DefaultBaseTime;

        TimestampBitsLength = timestampBitsLength;
        SeqBitsLength = seqBitsLength;
        RandomBitsLength = randomBitsLength;
        WorkIdBitsLength = workIdBitsLength;

        SeqFormatLength = Math.Pow(2, seqBitsLength).ToString(CultureInfo.InvariantCulture).Length;
        RandomFormatLength = Math.Pow(2, randomBitsLength).ToString(CultureInfo.InvariantCulture).Length;
        WorkIdFormatLength = Math.Pow(2, workIdBitsLength).ToString(CultureInfo.InvariantCulture).Length;

        _maxSeq = (long)Math.Pow(2, seqBitsLength) - 1;
        _maxTimestamp = (long)Math.Pow(2, timestampBitsLength) - 1;
    }

    protected virtual long MakeRandom()
    {
        if (RandomBitsLength == 0)
        {
            return 0;
        }

        var bytes = new byte[sizeof(long)];
        RandomNumberGenerator.GetBytes(bytes);
        return BitConverter.ToInt64(bytes) & ~(-1L << RandomBitsLength);
    }

    protected virtual long MakeTimestamp()
    {
        return (long)(DateTimeOffset.UtcNow - BaseTime).TotalMilliseconds;
    }

    protected virtual (long Timestamp, long Seq) MakeTimestampAndSeq()
    {
        var timestamp = MakeTimestamp();
        if (timestamp > _maxTimestamp)
        {
            throw new InvalidOperationException("Timestamp has reached the limit.");
        }

        if (timestamp > _lastTimestamp)
        {
            _lastTimestamp = timestamp;
            _seq = 1;
        }
        else
        {
            Debug.Assert(_seq <= _maxSeq);
            if (_seq == _maxSeq)
            {
                _lastTimestamp++;
                _seq = 1;
            }
            else
            {
                _seq++;
            }
        }

        return (_lastTimestamp, _seq);
    }

    public abstract T Encode(SequenceId sequenceId);

    private int _producingSign = 0;

    public SequenceId CreateSequenceId2()
    {
        var random = MakeRandom();
        while (Interlocked.CompareExchange(ref _producingSign, 1, 0) == 1)
        {
            Thread.Yield();
        }

        var (timestamp, seq) = MakeTimestampAndSeq();
        return new SequenceId() { Timestamp = timestamp, Seq = seq, Random = random, WorkId = WorkId };
    }

    private readonly object _lock = new();

    public SequenceId CreateSequenceId()
    {
        long timestamp, seq;
        var random = MakeRandom();
        lock (_lock)
        {
            (timestamp, seq) = MakeTimestampAndSeq();
        }

        return new SequenceId() { Timestamp = timestamp, Seq = seq, Random = random, WorkId = WorkId };
    }

    public virtual T Create()
    {
        return Encode(CreateSequenceId());
    }

    public abstract SequenceId Parse(T id);

    public string CreateFormatId(string separator = "-")
    {
        var sequence = CreateSequenceId();
        return sequence.ToString(BaseTime, SeqFormatLength, RandomFormatLength, WorkIdFormatLength, separator);
    }
}
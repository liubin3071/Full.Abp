using System.Text;

namespace Full.Ids;

public ref struct SequenceId
{
    public long Timestamp;
    public long Seq;
    public long Random;
    public long WorkId;

    public string ToString(DateTimeOffset baseTime, int seqFormatLength, int randomFormatLength, int workIdFormatLength,
        string separator)
    {
        var sb = new StringBuilder();
        sb.Append($"{baseTime.LocalDateTime.AddMilliseconds(Timestamp):yyyyMMddHHmmssfff}");
        if (seqFormatLength > 0)
        {
            sb.Append($"{separator}{Seq.ToString($"D{seqFormatLength}")}");
        }
        if (workIdFormatLength > 0)
        {
            sb.Append($"{separator}{WorkId.ToString($"D{workIdFormatLength}")}");
        }
        if (randomFormatLength > 0)
        {
            sb.Append($"{separator}{Random.ToString($"D{randomFormatLength}")}");
        }
        return sb.ToString();
    }
}
using System.Globalization;
using Volo.Abp.DependencyInjection;

namespace Full.Ids;

public class SerialNumberGenerator : IdGeneratorBase<string>, ISingletonDependency
{
    public string Separator { get; }

    public SerialNumberGenerator(long workId,
        int workIdBitsLength = 16, int timestampBitsLength = 48, int seqBitsLength = 16, int randomBitsLength = 16,
        string separator = "", DateTime? baseTime = null)
        : base(timestampBitsLength, seqBitsLength, randomBitsLength, workId, workIdBitsLength, baseTime)
    {
        Separator = separator;
    }

    public override string Encode(SequenceId sequenceId)
    {
        return CreateFormatId(Separator);
    }

    public SequenceId Parse(string id, string? separator)
    {
        id = string.IsNullOrEmpty(separator) ? id : id.Replace(Separator, "");

        // const int timeEnd = 17;
        var seqEnd = 17 + SeqFormatLength;
        var workIdEnd = seqEnd + WorkIdFormatLength;
        var randomEnd = workIdEnd + RandomFormatLength;

        var timePart = id[..17];
        var seqPart = id[17..seqEnd];
        var workId = id[seqEnd ..workIdEnd];
        var randomPart = id[workIdEnd..randomEnd];

        return new SequenceId() {
            Timestamp =
                (long)(DateTimeOffset.ParseExact(timePart, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) -
                       BaseTime).TotalMilliseconds,
            Seq = string.IsNullOrEmpty(seqPart) ? 0 : long.Parse(seqPart),
            Random = string.IsNullOrEmpty(randomPart) ? 0 : long.Parse(randomPart),
            WorkId = string.IsNullOrEmpty(workId) ? 0 : long.Parse(workId)
        };
    }

    public override SequenceId Parse(string id)
    {
        return Parse(id, Separator);
    }
}
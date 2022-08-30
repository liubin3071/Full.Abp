using Volo.Abp.Guids;

namespace Full.Ids;

public class AbpSequentialGuidGeneratorOptions
{
    private int _workId;
    public SequentialGuidType GuidType { get; set; }

    public int WorkIdBitsLength { get; set; } = 24;

    public int WorkId {
        get => _workId == 0 ? (int)Random.Shared.NextInt64(1, (long)Math.Pow(2, 24) - 1) : _workId;
        set => _workId = value;
    }
}
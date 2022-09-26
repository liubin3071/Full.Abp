using AutoMapper;
using Volo.Abp.SettingManagement;

namespace Full.Abp.SettingManagement.Blazor.AntDesignUI;

public class SettingManagementBlazorAutoMapperProfile : Profile
{
    public SettingManagementBlazorAutoMapperProfile()
    {
        CreateMap<EmailSettingsDto, UpdateEmailSettingsDto>();
    }
}

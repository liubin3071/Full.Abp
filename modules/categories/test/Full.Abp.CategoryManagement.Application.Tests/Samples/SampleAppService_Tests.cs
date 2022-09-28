// using System.Threading.Tasks;
// using Full.Abp.Categories.Samples;
// using Shouldly;
// using Xunit;
//
// namespace Full.Abp.CategoryManagement.Samples;
//
// public class SampleAppService_Tests : CategoryManagementApplicationTestBase
// {
//     private readonly ISampleAppService _sampleAppService;
//
//     public SampleAppService_Tests()
//     {
//         _sampleAppService = GetRequiredService<ISampleAppService>();
//     }
//
//     [Fact]
//     public async Task GetAsync()
//     {
//         var result = await _sampleAppService.GetAsync();
//         result.Value.ShouldBe(42);
//     }
//
//     [Fact]
//     public async Task GetAuthorizedAsync()
//     {
//         var result = await _sampleAppService.GetAuthorizedAsync();
//         result.Value.ShouldBe(42);
//     }
// }

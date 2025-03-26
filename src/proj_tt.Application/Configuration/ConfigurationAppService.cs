using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using proj_tt.Configuration.Dto;

namespace proj_tt.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : proj_ttAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}

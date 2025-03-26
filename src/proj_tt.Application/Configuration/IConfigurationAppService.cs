using System.Threading.Tasks;
using proj_tt.Configuration.Dto;

namespace proj_tt.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}

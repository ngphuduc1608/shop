using System.Threading.Tasks;
using proj_tt.Models.TokenAuth;
using proj_tt.Web.Controllers;
using Shouldly;
using Xunit;

namespace proj_tt.Web.Tests.Controllers
{
    public class HomeController_Tests: proj_ttWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
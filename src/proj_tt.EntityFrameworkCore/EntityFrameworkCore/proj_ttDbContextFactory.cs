using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using proj_tt.Configuration;
using proj_tt.Web;

namespace proj_tt.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class proj_ttDbContextFactory : IDesignTimeDbContextFactory<proj_ttDbContext>
    {
        public proj_ttDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<proj_ttDbContext>();
            
            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            proj_ttDbContextConfigurer.Configure(builder, configuration.GetConnectionString(proj_ttConsts.ConnectionStringName));

            return new proj_ttDbContext(builder.Options);
        }
    }
}

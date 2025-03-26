using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace proj_tt.EntityFrameworkCore
{
    public static class proj_ttDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<proj_ttDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<proj_ttDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

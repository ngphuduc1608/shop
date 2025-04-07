using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using proj_tt.Authorization.Roles;
using proj_tt.Authorization.Users;
using proj_tt.MultiTenancy;
using Abp.EntityFrameworkCore;
using proj_tt.Persons;
using proj_tt.Tasks;
using proj_tt.Products;
using proj_tt.Categories;


namespace proj_tt.EntityFrameworkCore
{
    public class proj_ttDbContext : AbpZeroDbContext<Tenant, Role, User, proj_ttDbContext>
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }



        public proj_ttDbContext(DbContextOptions<proj_ttDbContext> options)
            : base(options)
        {
        }



    }

}

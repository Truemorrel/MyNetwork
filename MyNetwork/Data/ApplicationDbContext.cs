using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyNetwork.Configs;
using MyNetwork.Models.Users;

namespace MyNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
			//Database.EnsureDeleted();
			//Database.EnsureCreated();
		}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Friend>(new FriendConfiguration());
            builder.ApplyConfiguration<Message>(new MessageConfuiguration());
        }
    }
}

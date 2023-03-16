using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyNetwork.Models.Users;

namespace MyNetwork.Configs
{
	public class MessageConfuiguration : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			builder.ToTable("Mesages").HasKey(p => p.Id);
			builder.Property(x => x.Id).UseIdentityColumn();
		}
	}
}

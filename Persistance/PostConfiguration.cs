using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PullDataApi.Models;

namespace PullDataApi.Persistance
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            //builder.("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Body).IsRequired();
        }
    }
}

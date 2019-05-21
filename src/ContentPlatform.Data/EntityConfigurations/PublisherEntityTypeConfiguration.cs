using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentPlatform.Data.EntityConfigurations
{
    public class PublisherEntityTypeConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder
                .HasMany(p => p.Authors)
                .WithOne(a => a.Publisher)
                .HasForeignKey(a => a.PublisherId);
            //.HasConstraintName("FK_Author_Publisher_PublisherId")

            builder
                .HasMany(p => p.Blogs)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherId);

            builder
                .Property(p => p.Name)
                .IsRequired();
            builder
                .Property(p => p.MainWebsite)
                .IsRequired();
        }
    }
}
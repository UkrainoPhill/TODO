using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TODO.Core.Enum;
using Task = TODO.Core.Models.Task;

namespace TODO.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.DueDate);
        builder.Property(x => x.Status)
            .HasConversion<int>();
        builder.Property(x => x.Priority)
            .HasConversion<int>();
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.UpdatedAt);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.UserId);
    }
}
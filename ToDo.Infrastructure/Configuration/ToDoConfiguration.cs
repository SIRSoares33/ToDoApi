using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Configuration;

public class ToDoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("ToDos");

        builder.HasKey(t => t.Id);

        builder.OwnsOne(t => t.Title, title =>
        { title.Property(t => t.Value).HasColumnName("Title").IsRequired(); });

        builder.OwnsOne(t => t.Description, description =>
        { description.Property(d => d.Value).HasColumnName("Description").IsRequired(false); });

        builder.Property(t => t.IsCompleted).IsRequired();
        builder.Property(t => t.CreateAt).IsRequired();
        builder.Property(t => t.UpdateAt).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
    }
}

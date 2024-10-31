using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Configurations;

public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.HasKey(e => e.Name);

        builder.Property(e => e.Name)
            .HasMaxLength(DomainConstants.Dog.MaxNameLength);
        
        builder.Property(e => e.Color)
            .IsRequired()
            .HasMaxLength(DomainConstants.Dog.MaxColorLength);
        
        builder.Property(e => e.TailLength)
            .IsRequired();
        
        builder.Property(e => e.Weight)
            .IsRequired();

        builder.ToTable(tb => 
        {
            tb.HasCheckConstraint("ck_dog_tail_length", "tail_length >= 0");
            tb.HasCheckConstraint("ck_dog_weight", "weight > 0");
        });
    }
}
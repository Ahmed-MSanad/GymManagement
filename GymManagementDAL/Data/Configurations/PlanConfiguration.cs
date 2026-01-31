using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);

            builder.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(200);

            builder.Property(p => p.Price).HasPrecision(10, 2);

            builder.ToTable(p =>
            {
                p.HasCheckConstraint("Plan_DurationCheck", "DurationDays Between 1 and 365");
            });
        }
    }
}

using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.HealthRecord).WithOne().HasForeignKey<HealthRecord>(h => h.Id);

            base.Configure(builder);
        }
    }
}

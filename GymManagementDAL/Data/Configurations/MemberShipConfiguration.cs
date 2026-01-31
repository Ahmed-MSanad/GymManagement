using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Ignore(m => m.Id);
            builder.HasKey(m => new { m.PlanId, m.MemberId });

            builder.Property(m => m.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.Plan).WithMany(p => p.PlanMembers).HasForeignKey(m => m.PlanId);
            builder.HasOne(m => m.Member).WithMany(me => me.MemberPlans).HasForeignKey(m => m.MemberId);

        }
    }
}

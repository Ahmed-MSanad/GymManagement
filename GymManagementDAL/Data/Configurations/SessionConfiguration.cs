using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagementDAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasMany(s => s.SessionMembers).WithOne(m => m.Session).HasForeignKey(m => m.SessionId);

            builder.HasOne(s => s.Trainer).WithMany(t => t.Sessions).HasForeignKey(s => s.TrainerId);

            builder.HasOne(s => s.Category).WithMany(c => c.Sessions).HasForeignKey(s => s.CategoryId);

            builder.ToTable(s =>
            {
                s.HasCheckConstraint("Session_CapacityCheck", "Capacity between 1 and 25");
                s.HasCheckConstraint("Session_EndDateCheck", "EndDate > StartDate");
            });
        }
    }
}

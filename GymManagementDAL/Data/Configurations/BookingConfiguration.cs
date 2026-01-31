using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.Property(b => b.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Session).WithMany(s => s.SessionMembers).HasForeignKey(b => b.SessionId);
            builder.HasOne(b => b.Member).WithMany(m => m.MemberSessions).HasForeignKey(b => b.MemberId);

            builder.HasKey(b => new { b.SessionId , b.MemberId });
        }
    }
}

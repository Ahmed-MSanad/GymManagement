using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(GU => GU.Name).HasColumnType("varchar").HasMaxLength(50);
            
            builder.Property(GU => GU.Email).HasColumnType("varchar(100)");
            builder.HasIndex(GU => GU.Email).IsUnique();
            
            builder.Property(GU => GU.Phone).HasColumnType("varchar(11)");
            builder.HasIndex(GU => GU.Phone).IsUnique();

            builder.ToTable(GU =>
            {
                GU.HasCheckConstraint("GymUser_EmailCheck", "Email LIKE '_%@_%._%'");
                GU.HasCheckConstraint("GymUser_PhoneCheck", "Phone LIKE '01%' and Phone Not LIKE '%[^0-9]%'");
            });

            builder.OwnsOne(GU => GU.Address, address =>
            {
                address.Property(Ad => Ad.BuildingNumber).HasColumnName("BuildingNumber");
                address.Property(Ad => Ad.Street).HasColumnType("varchar").HasMaxLength(30).HasColumnName("Street");
                address.Property(Ad => Ad.City).HasColumnType("varchar").HasMaxLength(30).HasColumnName("City");
            });
        }
    }
}

//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace InPlayWiseData.DbContexts
//{
//    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
//    {
//        public void Configure(EntityTypeBuilder<IdentityRole> builder)
//        {
//            string userRoleId = Guid.NewGuid().ToString();
//            string adminRoleId = Guid.NewGuid().ToString();

//            var userRole = new IdentityRole
//            {
//                Id = userRoleId,
//                Name = "user",
//                NormalizedName = "USER",
//                ConcurrencyStamp = userRoleId
//            };
//            var adminRole = new IdentityRole
//            {
//                Id = adminRoleId,
//                Name = "Admin",
//                NormalizedName = "ADMIN",
//                ConcurrencyStamp = adminRoleId
//            };
//            builder.HasData(new List<IdentityRole> { userRole, adminRole });
//        }

//    }
//}

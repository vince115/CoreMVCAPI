using Microsoft.EntityFrameworkCore;
using CoreMVCAPI.Models;

namespace CoreMVCAPI.Data
{	
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Staff> Staffs { get; set; }

        // 配置種子數據
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //配置精度
            modelBuilder.Entity<Staff>().Property(s => s.Salary).HasColumnType("decimal(18,2)");

            // 為 Staffs 表添加初始數據
            modelBuilder.Entity<Staff>().HasData(
                new Staff { Id = 1, Name = "John Doe", Position = "Manager", Salary = 50000 },
                new Staff { Id = 2, Name = "Jane Smith", Position = "Developer", Salary = 40000 },
                new Staff { Id = 3, Name = "Michael Brown", Position = "Designer", Salary = 45000 },
                new Staff { Id = 4, Name = "Emily Davis", Position = "Tester", Salary = 35000 },
                new Staff { Id = 5, Name = "Chris Johnson", Position = "HR", Salary = 38000 },
                new Staff { Id = 6, Name = "Sarah Wilson", Position = "Accountant", Salary = 42000 }
            );
        }
    }
}

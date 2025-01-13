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
	}
}

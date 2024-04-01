using AcademicShare.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademicShare.Web.Context;


public class AcademicShareDbContext : IdentityDbContext<User>
{
	public AcademicShareDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Post> Posts { get; set; }
	public override DbSet<User> Users { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<UserProfile> Profiles { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Post>(entity =>
		{
			entity.HasMany(p => p.Comments)
				.WithOne(c => c.Post)
				.OnDelete(DeleteBehavior.Cascade);

		});

		modelBuilder.Entity<User>()
			.HasMany(u => u.Posts)
			.WithOne(p => p.User)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<UserProfile>()
			.HasOne(p => p.User)
			.WithOne(u => u.Profile)
			.OnDelete(DeleteBehavior.Cascade);
	

		base.OnModelCreating(modelBuilder);
	}

}

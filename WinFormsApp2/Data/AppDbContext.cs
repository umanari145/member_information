using Microsoft.EntityFrameworkCore;

namespace WinFormsApp2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Member>(e =>
        {
            e.ToTable("Members");
            e.HasKey(x => x.MemberId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.Property(x => x.Gender).IsRequired().HasMaxLength(10);
        });
    }
}
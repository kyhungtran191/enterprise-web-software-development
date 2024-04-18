using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using Server.Domain.Entity.System;
using Server.Infrastructure.Common.Constants;
using File = Server.Domain.Entity.Content.File;

namespace Server.Infrastructure;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    public DbSet<ContributionComment> ContributionComments { get; set; }
    public DbSet<ContributionPublic> ContributionPublics { get; set; }
    public DbSet<ContributionPublicComment> ContributionPublicComments { get; set; }
    public DbSet<ContributionPublicFavorite> ContributionPublicFavorites { get; set; }
    public DbSet<ContributionPublicReadLater> ContributionPublicReadLaters { get; set; }
    public DbSet<ContributionActivityLog> ContributionActivityLogs { get; set; }
    public DbSet<ContributionTag> ContributionTags { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ContributionPublicRating> ContributionPublicRatings { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Identity Configuration

        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);

        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

        builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.UserId, x.RoleId });

        builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
            .HasKey(x => new { x.UserId });

        #endregion Identity Configuration
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added);

        foreach (var entityEntry in entries)
        {
            var dateCreatedProp = entityEntry.Entity.GetType().GetProperty(SystemConstants.DateCreatedField);

            if (entityEntry.State == EntityState.Added && dateCreatedProp is not null)
            {
                dateCreatedProp.SetValue(entityEntry.Entity, DateTime.Now);
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
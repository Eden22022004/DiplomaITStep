using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;

namespace SpaceRythm.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public DbSet<Playlist> Playlists { get; set; }

    public DbSet<PlaylistTracks> PlaylistTracks { get; set; }
    public DbSet<TrackMetadata> TrackMetadatas { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<AdminLog> AdminLogs { get; set; }
    public DbSet<SongLiked> SongsLiked { get; set; }

    public DbSet<ArtistLiked> ArtistsLiked { get; set; }

    public DbSet<CategoryLiked> CategoriesLiked { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistTracks>()
            .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

        modelBuilder.Entity<Like>()
       .HasKey(l => new { l.UserId, l.TrackId });

        modelBuilder.Entity<Follower>()
            .HasKey(f => new { f.UserId, f.FollowedUserId });
        modelBuilder.Entity<Follower>()
           .HasOne(f => f.User)
           .WithMany(u => u.Followers)
           .HasForeignKey(f => f.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Follower>()
            .HasOne(f => f.FollowedUser)
            .WithMany(u => u.FollowedBy)
            .HasForeignKey(f => f.FollowedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

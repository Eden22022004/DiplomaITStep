using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Models;

namespace SpaceRythm.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Artist> Artist { get; set; }

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

        modelBuilder.Entity<Track>()
       .HasOne(t => t.Artist) // Один Track належить одному Artist
       .WithMany(a => a.Tracks) // Один Artist може мати багато Tracks
       .HasForeignKey(t => t.ArtistId) // Вказуємо, що ArtistId є зовнішнім ключем
       .OnDelete(DeleteBehavior.Cascade); // Вибір поведінки при видаленні

        Seed(modelBuilder);
    }

    private void Seed(ModelBuilder modelBuilder)
    {
        // Тестові дані для Artists
        modelBuilder.Entity<Artist>().HasData(
     new Artist { ArtistId = 1, Name = "Artist One", Bio = "Bio of artist one" },
     new Artist { ArtistId = 2, Name = "Artist Two", Bio = "Bio of artist two" }
 );

        // Тестові дані для Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "user1@example.com", Password = "hashedpassword1", Username = "user1", ProfileImage = "avatar1.png", Biography = "Biography of user 1", DateJoined = DateTime.UtcNow, IsEmailConfirmed = true },
            new User { Id = 2, Email = "user2@example.com", Password = "hashedpassword2", Username = "user2", ProfileImage = "avatar2.png", Biography = "Biography of user 2", DateJoined = DateTime.UtcNow, IsEmailConfirmed = true }
        );

        // Тестові дані для Playlists
        modelBuilder.Entity<Playlist>().HasData(
            new Playlist { PlaylistId = 1, UserId = 1, Title = "My First Playlist", Description = "This is my first playlist", CreatedDate = DateTime.Now, IsPublic = true },
            new Playlist { PlaylistId = 2, UserId = 2, Title = "Chill Vibes", Description = "A playlist for relaxation", CreatedDate = DateTime.Now, IsPublic = true }
        );

        // Тестові дані для Tracks
        modelBuilder.Entity<Track>().HasData(
            new Track { TrackId = 1, ArtistId = 1, Title = "Track One", Genre = "Pop", Tags = "tag1,tag2", Description = "This is track one", FilePath = "/tracks/track1.mp3", Duration = TimeSpan.FromMinutes(3), UploadDate = DateTime.UtcNow },
            new Track { TrackId = 2, ArtistId = 2, Title = "Track Two", Genre = "Rock", Tags = "tag3,tag4", Description = "This is track two", FilePath = "/tracks/track2.mp3", Duration = TimeSpan.FromMinutes(4), UploadDate = DateTime.UtcNow }
        );

        // Тестові дані для Comments
        modelBuilder.Entity<Comment>().HasData(
            new Comment { CommentId = 1, TrackId = 1, UserId = 2, Content = "Great track!", PostedDate = DateTime.Now },
            new Comment { CommentId = 2, TrackId = 2, UserId = 1, Content = "I love this!", PostedDate = DateTime.Now }
        );

        // Тестові дані для Followers
        modelBuilder.Entity<Follower>().HasData(
            new Follower { UserId = 1, FollowedUserId = 2, FollowDate = DateTime.Now },
            new Follower { UserId = 2, FollowedUserId = 1, FollowDate = DateTime.Now }
        );

        // Тестові дані для PlaylistTracks
        modelBuilder.Entity<PlaylistTracks>().HasData(
            new PlaylistTracks { PlaylistId = 1, TrackId = 1, AddedDate = DateTime.UtcNow },
            new PlaylistTracks { PlaylistId = 2, TrackId = 2, AddedDate = DateTime.UtcNow }
        );

        // Тестові дані для Likes
        modelBuilder.Entity<Like>().HasData(
            new Like { UserId = 1, TrackId = 1, LikedDate = DateTime.Now },
            new Like { UserId = 2, TrackId = 2, LikedDate = DateTime.Now }
        );

        // Тестові дані для Subscriptions
        modelBuilder.Entity<Subscription>().HasData(
            new Subscription { SubscriptionId = 1, UserId = 1, Type = SubscriptionType.Premium, SubscriptionStartDate = DateTime.UtcNow, SubscriptionEndDate = DateTime.UtcNow.AddYears(1) },
            new Subscription { SubscriptionId = 2, UserId = 2, Type = SubscriptionType.Free, SubscriptionStartDate = DateTime.UtcNow, SubscriptionEndDate = DateTime.UtcNow.AddMonths(1) }
        );

        // Тестові дані для AdminLogs
        modelBuilder.Entity<AdminLog>().HasData(
            new AdminLog { LogId = 1, AdminId = 1, ActionType = "UserCreated", TargetId = 1, Timestamp = DateTime.Now },
            new AdminLog { LogId = 2, AdminId = 1, ActionType = "TrackUploaded", TargetId = 1, Timestamp = DateTime.Now }
        );
    }
}

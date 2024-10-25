﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpaceRythm.Data;

#nullable disable

namespace SpaceRythm.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20241021073312_FixIdentityFields")]
    partial class FixIdentityFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SpaceRythm.Entities.AdminLog", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LogId"));

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<int?>("TargetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("LogId");

                    b.HasIndex("AdminId");

                    b.ToTable("AdminLogs");

                    b.HasData(
                        new
                        {
                            LogId = 1,
                            ActionType = "UserCreated",
                            AdminId = 1,
                            TargetId = 1,
                            Timestamp = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(6011)
                        },
                        new
                        {
                            LogId = 2,
                            ActionType = "TrackUploaded",
                            AdminId = 1,
                            TargetId = 1,
                            Timestamp = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(6013)
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ArtistId"));

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ArtistId");

                    b.ToTable("artist");

                    b.HasData(
                        new
                        {
                            ArtistId = 1,
                            Bio = "Bio of artist one",
                            Name = "Artist One"
                        },
                        new
                        {
                            ArtistId = 2,
                            Bio = "Bio of artist two",
                            Name = "Artist Two"
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.ArtistLiked", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArtistId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("artists_liked");
                });

            modelBuilder.Entity("SpaceRythm.Entities.CategoryLiked", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("categories_liked");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("TrackId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            CommentId = 1,
                            Content = "Great track!",
                            PostedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5869),
                            TrackId = 1,
                            UserId = 2
                        },
                        new
                        {
                            CommentId = 2,
                            Content = "I love this!",
                            PostedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5871),
                            TrackId = 2,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.Follower", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("FollowedUserId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("FollowDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId", "FollowedUserId");

                    b.HasIndex("FollowedUserId");

                    b.ToTable("Followers");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            FollowedUserId = 2,
                            FollowDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5891)
                        },
                        new
                        {
                            UserId = 2,
                            FollowedUserId = 1,
                            FollowDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5894)
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.Like", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("TrackId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("LikedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId", "TrackId");

                    b.HasIndex("TrackId");

                    b.ToTable("Likes");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            TrackId = 1,
                            LikedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5934)
                        },
                        new
                        {
                            UserId = 2,
                            TrackId = 2,
                            LikedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5935)
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.Playlist", b =>
                {
                    b.Property<int>("PlaylistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PlaylistId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PlaylistId");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");

                    b.HasData(
                        new
                        {
                            PlaylistId = 1,
                            CreatedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5814),
                            Description = "This is my first playlist",
                            IsPublic = true,
                            Title = "My First Playlist",
                            UserId = 1
                        },
                        new
                        {
                            PlaylistId = 2,
                            CreatedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5816),
                            Description = "A playlist for relaxation",
                            IsPublic = true,
                            Title = "Chill Vibes",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.PlaylistTracks", b =>
                {
                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("PlaylistId", "TrackId");

                    b.HasIndex("TrackId");

                    b.ToTable("PlaylistTracks");

                    b.HasData(
                        new
                        {
                            PlaylistId = 1,
                            TrackId = 1,
                            AddedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5911)
                        },
                        new
                        {
                            PlaylistId = 2,
                            TrackId = 2,
                            AddedDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5913)
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.SongLiked", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("SongId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("songs_liked");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Subscription", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("SubscriptionId"));

                    b.Property<DateTime?>("SubscriptionEndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SubscriptionStartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");

                    b.HasData(
                        new
                        {
                            SubscriptionId = 1,
                            SubscriptionEndDate = new DateTime(2025, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5958),
                            SubscriptionStartDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5958),
                            Type = 1,
                            UserId = 1
                        },
                        new
                        {
                            SubscriptionId = 2,
                            SubscriptionEndDate = new DateTime(2024, 11, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5980),
                            SubscriptionStartDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5979),
                            Type = 0,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.Track", b =>
                {
                    b.Property<int>("TrackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("TrackId"));

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(6)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TrackId");

                    b.HasIndex("ArtistId");

                    b.HasIndex("UserId");

                    b.ToTable("Tracks");

                    b.HasData(
                        new
                        {
                            TrackId = 1,
                            ArtistId = 1,
                            Description = "This is track one",
                            Duration = new TimeSpan(0, 0, 3, 0, 0),
                            FilePath = "/tracks/track1.mp3",
                            Genre = "Pop",
                            Tags = "tag1,tag2",
                            Title = "Track One",
                            UploadDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5843)
                        },
                        new
                        {
                            TrackId = 2,
                            ArtistId = 2,
                            Description = "This is track two",
                            Duration = new TimeSpan(0, 0, 4, 0, 0),
                            FilePath = "/tracks/track2.mp3",
                            Genre = "Rock",
                            Tags = "tag3,tag4",
                            Title = "Track Two",
                            UploadDate = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5846)
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.TrackMetadata", b =>
                {
                    b.Property<int>("MetadataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("MetadataId"));

                    b.Property<int>("CommentsCount")
                        .HasColumnType("int");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<int>("Plays")
                        .HasColumnType("int");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.HasKey("MetadataId");

                    b.HasIndex("TrackId")
                        .IsUnique();

                    b.ToTable("TrackMetadatas");
                });

            modelBuilder.Entity("SpaceRythm.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Biography")
                        .HasColumnType("TEXT")
                        .HasColumnName("biography");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_joined");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_email_confirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("OAuthProvider")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("oauth_provider");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordResetToken")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password_reset_token");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ProfileImage")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("profile_image");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("reset_token_expires");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            Biography = "Biography of user 1",
                            ConcurrencyStamp = "714eaac4-97b4-49aa-a32e-2e8d3a4959d3",
                            DateJoined = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5729),
                            Email = "user1@example.com",
                            EmailConfirmed = false,
                            IsAdmin = false,
                            IsEmailConfirmed = true,
                            LockoutEnabled = false,
                            Password = "hashedpassword1",
                            PhoneNumberConfirmed = false,
                            ProfileImage = "avatar1.png",
                            SecurityStamp = "dd275890-3b4d-439b-82df-c5cfb4b14553",
                            TwoFactorEnabled = false,
                            Username = "user1"
                        },
                        new
                        {
                            Id = 2,
                            AccessFailedCount = 0,
                            Biography = "Biography of user 2",
                            ConcurrencyStamp = "7bdd07af-c747-4f80-97dd-20ff544e38ea",
                            DateJoined = new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5761),
                            Email = "user2@example.com",
                            EmailConfirmed = false,
                            IsAdmin = false,
                            IsEmailConfirmed = true,
                            LockoutEnabled = false,
                            Password = "hashedpassword2",
                            PhoneNumberConfirmed = false,
                            ProfileImage = "avatar2.png",
                            SecurityStamp = "233bf4d2-7813-455c-bc88-2eae2a91e3d5",
                            TwoFactorEnabled = false,
                            Username = "user2"
                        });
                });

            modelBuilder.Entity("SpaceRythm.Entities.AdminLog", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("SpaceRythm.Entities.ArtistLiked", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("ArtistsLiked")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.CategoryLiked", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("CategoriesLiked")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Comment", b =>
                {
                    b.HasOne("SpaceRythm.Entities.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Follower", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "FollowedUser")
                        .WithMany("FollowedBy")
                        .HasForeignKey("FollowedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("Followers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FollowedUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Like", b =>
                {
                    b.HasOne("SpaceRythm.Entities.Track", "Track")
                        .WithMany("Likes")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Playlist", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.PlaylistTracks", b =>
                {
                    b.HasOne("SpaceRythm.Entities.Playlist", "Playlist")
                        .WithMany("PlaylistTracks")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpaceRythm.Entities.Track", "Track")
                        .WithMany("PlaylistTracks")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("SpaceRythm.Entities.SongLiked", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("SongsLiked")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Subscription", b =>
                {
                    b.HasOne("SpaceRythm.Entities.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Track", b =>
                {
                    b.HasOne("SpaceRythm.Entities.Artist", "Artist")
                        .WithMany("Tracks")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpaceRythm.Entities.User", null)
                        .WithMany("Tracks")
                        .HasForeignKey("UserId");

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("SpaceRythm.Entities.TrackMetadata", b =>
                {
                    b.HasOne("SpaceRythm.Entities.Track", "Track")
                        .WithOne("TrackMetadata")
                        .HasForeignKey("SpaceRythm.Entities.TrackMetadata", "TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Artist", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Playlist", b =>
                {
                    b.Navigation("PlaylistTracks");
                });

            modelBuilder.Entity("SpaceRythm.Entities.Track", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("PlaylistTracks");

                    b.Navigation("TrackMetadata")
                        .IsRequired();
                });

            modelBuilder.Entity("SpaceRythm.Entities.User", b =>
                {
                    b.Navigation("ArtistsLiked");

                    b.Navigation("CategoriesLiked");

                    b.Navigation("FollowedBy");

                    b.Navigation("Followers");

                    b.Navigation("Likes");

                    b.Navigation("SongsLiked");

                    b.Navigation("Subscriptions");

                    b.Navigation("Tracks");
                });
#pragma warning restore 612, 618
        }
    }
}

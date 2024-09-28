using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SpaceRythm.Models.User;
using SpaceRythm.Util;



namespace SpaceRythm.Entities;


public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")] 
    public int Id { get; set; } 

    [Required]
    [MaxLength(255)]
    [Column("email")] 
    public string Email { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("password_hash")] 
    public string Password { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("username")] 
    public string Username { get; set; }
    public bool IsAdmin { get; set; } = false;
    [MaxLength(255)]
    [Column("profile_image")] 
    public string ProfileImage { get; set; }

    [Column("biography", TypeName = "TEXT")] 
    public string Biography { get; set; }

    [Column("date_joined")]
    public DateTime DateJoined { get; set; } = DateTime.UtcNow; 

    [MaxLength(50)]
    [Column("oauth_provider")] 
    public string OAuthProvider { get; set; }

    [Column("is_email_confirmed")]
    public bool IsEmailConfirmed { get; set; } = false; // Значення за замовчуванням

    // Навігаційні властивості
    public List<SongLiked> SongsLiked { get; set; } = new List<SongLiked>();
    public List<ArtistLiked> ArtistsLiked { get; set; } = new List<ArtistLiked>();
    public List<CategoryLiked> CategoriesLiked { get; set; } = new List<CategoryLiked>();

    public ICollection<Track> Tracks { get; set; }

    public ICollection<Follower> Followers { get; set; }
    public ICollection<Follower> FollowedBy { get; set; }
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public User(CreateUserRequest req)
    {
        Email = req.Email;
        Username = req.Username;
        Password = PasswordHash.Hash(req.Password);
        DateJoined = DateTime.UtcNow; // Установіть дату при створенні користувача
    }

    // Порожній конструктор, необхідний для ORM
    public User() { }
}

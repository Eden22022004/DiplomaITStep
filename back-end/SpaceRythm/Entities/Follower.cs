using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Entities;

public class Follower
{
    // Композитний ключ
    [Key, Column(Order = 0)]
    public int UserId { get; set; } // Підписник

    [Key, Column(Order = 1)]
    public int FollowedUserId { get; set; } // Користувач, на якого підписалися

    public DateTime FollowDate { get; set; }

    // Навігаційні властивості
    public User User { get; set; }
    public User FollowedUser { get; set; } // Користувач, на якого підписалися
}
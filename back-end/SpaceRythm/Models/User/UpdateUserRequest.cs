using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Models.User;

public class UpdateUserRequest
{
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(100)]
    public string Username { get; set; }

    [MinLength(6)]
    public string Password { get; set; }

    public IEnumerable<string> SongsLiked { get; set; } = new List<string>();

    public IEnumerable<string> SongsUnliked { get; set; } = new List<string>();

    public IEnumerable<string> ArtistsLiked { get; set; } = new List<string>();

    public IEnumerable<string> ArtistsUnliked { get; set; } = new List<string>();

    public string ProfileImage { get; set; }

    public string Biography { get; set; }
}

namespace SpaceRythm.Models.User;

public class CreateUserResponse
{
    public int Id { get; set; } 
    public string Email { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }
    public string ProfileImage { get; set; }
    public string Biography { get; set; }
    public DateTime DateJoined { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string EmailConfirmationToken { get; set; }
    public List<string> SongsLiked { get; set; }
    public List<string> ArtistsLiked { get; set; }
    public List<string> CategoriesLiked { get; set; }

    public bool Succeeded { get; set; }

    public CreateUserResponse(Entities.User user, string token, bool succeeded, string emailConfirmationToken)
    {
        Id = user.Id; 
        Email = user.Email;
        Username = user.Username;
        JwtToken = token;
        ProfileImage = user.ProfileImage;
        Biography = user.Biography;
        DateJoined = user.DateJoined;
        IsEmailConfirmed = user.IsEmailConfirmed;
        EmailConfirmationToken = emailConfirmationToken;
        Succeeded = succeeded;

        SongsLiked = user.SongsLiked.Select(s => s.SongId.ToString()).ToList();
        ArtistsLiked = user.ArtistsLiked.Select(a => a.ArtistId.ToString()).ToList();
        CategoriesLiked = user.CategoriesLiked.Select(c => c.CategoryId.ToString()).ToList();
    }

   
}
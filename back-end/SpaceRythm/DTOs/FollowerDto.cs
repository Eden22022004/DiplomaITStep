namespace SpaceRythm.DTOs;

public class FollowerDto
{
    public int Id { get; set; }            // ID підписника
    public string Username { get; set; }    // Ім'я користувача
    public string Avatar { get; set; }      // URL аватара
    public DateTime FollowDate { get; set; } // Дата підписки
}

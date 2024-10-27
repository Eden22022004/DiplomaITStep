using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Entities;

public class Listening
{
    [Key]
    public int ListeningId { get; set; } 

    [ForeignKey("Track")]
    public int TrackId { get; set; }

    public Track Track { get; set; } // Навигационное свойство

    [ForeignKey("User")]
    public int UserId { get; set; } // Идентификатор пользователя

    public User User { get; set; } // Навигационное свойство

    public DateTime ListenedDate { get; set; } = DateTime.Now; // Дата прослушивания
}

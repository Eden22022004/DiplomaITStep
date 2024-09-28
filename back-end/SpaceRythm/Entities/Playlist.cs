using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Entities;


public class Playlist
{
    // Первинний ключ для плейлисту
    [Key]
    public int PlaylistId { get; set; }

    // Зовнішній ключ до таблиці Users, яка вказує на користувача, що створив плейлист
    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public bool IsPublic { get; set; } = true;

    public User User { get; set; }

    public ICollection<PlaylistTracks> PlaylistTracks { get; set; }
}

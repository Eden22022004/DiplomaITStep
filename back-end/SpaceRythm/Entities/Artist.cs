using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceRythm.Entities;

[Table("artist")]
public class Artist
{
    [Key] 
    public int ArtistId { get; set; }

    [Required] 
    public string Name { get; set; }

    public string Bio { get; set; }
  
    public ICollection<Track> Tracks { get; set; }
}

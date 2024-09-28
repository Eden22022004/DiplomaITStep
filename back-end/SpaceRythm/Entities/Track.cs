using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Entities;

public class Track
{
    [Key] 
    public int TrackId { get; set; }

    [Required] 
    public string Title { get; set; }

    public string Genre { get; set; }
    public string Tags { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime UploadDate { get; set; }

    [ForeignKey("Artist")] 
    public int ArtistId { get; set; }

    public Artist Artist { get; set; }
    public TrackMetadata TrackMetadata { get; set; }
    public ICollection<PlaylistTracks> PlaylistTracks { get; set; }
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}

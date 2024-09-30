using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaceRythm.Entities;

public class TrackMetadata
{
    [Key]
    public int MetadataId { get; set; } 

    [ForeignKey("Track")] 
    public int TrackId { get; set; }

    public int Plays { get; set; } 
    public int Likes { get; set; }  
    public int CommentsCount { get; set; } 

    public Track Track { get; set; }
}



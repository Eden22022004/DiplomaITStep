using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceRythm.Entities;

public class PlaylistTracks
{
    // Позначаємо PlaylistId як частину композитного ключа
    public int PlaylistId { get; set; }

    // Позначаємо TrackId як частину композитного ключа
    public int TrackId { get; set; }

    public DateTime AddedDate { get; set; }

    public Playlist Playlist { get; set; }
    public Track Track { get; set; }
}
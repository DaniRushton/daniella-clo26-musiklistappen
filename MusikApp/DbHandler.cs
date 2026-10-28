using Microsoft.Data.Sqlite;

namespace MusikApp;

public class DbHandler
{
    private readonly string _connectionString;

    public DbHandler(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Song> GetAllSongs() // visar alla låtar
    {
        var songs = new List<Song>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, album_id, title, length_sec FROM song ORDER BY id";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                Title = reader.GetString(2),
                LengthSec = reader.GetInt32(3)
            });
        }
        return songs;
    }

    public void AddSong(Song song) // lägger till en ny låt
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO song (album_id, title, length_sec) VALUES (@albumId, @title, @length)";
        command.Parameters.AddWithValue("@albumId", song.AlbumId);
        command.Parameters.AddWithValue("@title", song.Title);
        command.Parameters.AddWithValue("@length", song.LengthSec);
        command.ExecuteNonQuery();
    }

    public bool DeleteSong(int id) // tar bort en låt
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();


        using var deleteLinks = connection.CreateCommand();
        deleteLinks.CommandText = "DELETE FROM song_genre WHERE song_id = @id";
        deleteLinks.Parameters.AddWithValue("@id", id);
        deleteLinks.ExecuteNonQuery();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM song WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        return command.ExecuteNonQuery() > 0;
    }

    public List<(string SongTitle, string AlbumName, string ArtistName)> GetSongsWithAlbum() //låtar tillsammans med låttitel, albumnamn och artistnamn.
    {
        var result = new List<(string SongTitle, string AlbumName, string ArtistName)>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT song.title, album.title, artist.name FROM song
                                JOIN album ON song.album_id = album.id
                                JOIN artist ON album.artist_id = artist.id
                                ORDER BY album.title, song.title";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add((reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        return result;
    }
}
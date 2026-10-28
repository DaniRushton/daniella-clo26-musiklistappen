using Microsoft.Extensions.Configuration;
using MusikApp;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Hittar ingen connection string 'Default' i appsettings.json.");

bool running = true;
var db = new DbHandler(connectionString);

while (running)
{
    Console.WriteLine("=== Min Musikapp ===");
    Console.WriteLine("1) Visa alla låtar");
    Console.WriteLine("2) Lägg till låt");
    Console.WriteLine("3) Ta bort låt");
    Console.WriteLine("4) Visa låtar med album och artist");
    Console.WriteLine("5) Avsluta");
    Console.WriteLine();
    Console.WriteLine("Välj:_");

    string input = Console.ReadLine();

    if (input == "1") // Visa alla låtar
    {
        var songs = db.GetAllSongs();
        Console.WriteLine("=== Visar alla låtar ===");
        foreach (var song in songs)
        {
            Console.WriteLine($"{song.Id+".",-3} {song.Title,-35} ({song.LengthSec / 60}:{song.LengthSec % 60:D2}) Album-id: {song.AlbumId}");
        }
        Console.WriteLine();
    }

    else if (input == "2") // Lägg till låt
    {
        Console.Write("Titel: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Längd i sekunder: ");
        bool lengthSecs = int.TryParse(Console.ReadLine(), out int lengthSec);


        Console.Write("Album-id: ");
        bool albumIdt = int.TryParse(Console.ReadLine(), out int albumId);

        if (title == "" || !lengthSecs || !albumIdt)
        {
            Console.WriteLine("Ogiltig inmatning, låt ej tillagd.");
        }
        else
        {
            db.AddSong(new Song { Title = title, LengthSec = lengthSec, AlbumId = albumId });
            Console.WriteLine("Låten lades till.");
        }
        Console.WriteLine();
    }

    else if (input == "3") // Ta bort låt
    {
        Console.Write("Id på låten som skall tas bort (referera till låtlistan):");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Är du säker? j/n: ");
            if (Console.ReadLine()?.ToLower() == "j")
            {
                bool deleted = db.DeleteSong(id);
                if (deleted)
                {
                    Console.WriteLine("Låten har tagits bort.");
                }
                else
                {
                    Console.WriteLine("Hittade ingen låt med det id:et.");
                }
            }
            else
            {
                Console.WriteLine("Avbrutet");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt id.");
        }
        Console.WriteLine();
    }

    else if (input == "4") // Visa album och artister
    {
        Console.WriteLine("=== Låtar, Album, Artister ===");

        var rows = db.GetSongsWithAlbum();
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.SongTitle,-35} {row.AlbumName,-35} {row.ArtistName}");

        }
        Console.WriteLine();
    }

    else if (input == "5") // Avsluta
    {
        Console.WriteLine("Programmet stängs ner.");
        running = false;
    }

    else
    {
        Console.WriteLine("Ogiltig input, vänligen välj alternativ mellan 1-5.");
        Console.WriteLine();
    }
}

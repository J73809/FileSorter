using Spectre.Console;

public static class FileSort
{
    // --- The types ---
    public static readonly Dictionary<string, string[]> FileTypes =
        new Dictionary<string, string[]>
    {
        { "images", new[] { "png", "jpg", "jpeg", "gif", "bmp", "tiff", "svg", "heic", "raw" } },
        { "videos", new[] { "mp4", "avi", "mov", "wmv", "flv", "mkv", "webm", "m4v", "mpeg", "3gp" } },
        { "sounds", new[] { "wav", "ogg", "mp3", "flac", "aac", "midi", "m4a", "aiff", "alac" } },
        { "documents", new[] { "pdf", "txt", "docx", "md", "xlsx", "xls", "pptx", "ppt", "odt", "ods", "odp", "csv", "rtf", "epub", "tex" } },
        { "archives", new[] { "zip", "rar", "7z", "tar", "gz", "bz2", "xz" } },
        { "fonts", new[] { "ttf", "otf", "woff", "woff2" } },
        { "executables", new[] { "exe", "msi", "apk", "app" } },
        { "scripts", new[] { "py", "html", "css", "js", "cs", "java", "ru", "sh", "bat", "ps1", "php", "pl", "swift", "go", "ts", "lua", "scala", "vb", "sql", "r" } },
        { "3d_models", new[] { "blend", "obj", "fbx", "stl", "dae" } },
        { "temp", new[] { "tmp", "log", "bak", "ds_store", "thumbs.db" } }
    };

    public static string path;

    // --- Sorting method ---
    public static void Sort(string path, IEnumerable<string> SelectedCategories)
    {
        // check if path exists
        if (!Directory.Exists(path))
        {
            AnsiConsole.MarkupLine("[red]Path does not exist.[/]");
            return;
        }

        // get extensions
        foreach (var file in Directory.GetFiles(path))
        {
            string ext = Path.GetExtension(file).TrimStart('.').ToLower();

            if (string.IsNullOrEmpty(ext))
                continue;

            // start sort
            foreach (var category in SelectedCategories)
            {
                if (FileTypes[category].Contains(ext))
                {
                    string targetDir = Path.Combine(path, category);
                    Directory.CreateDirectory(targetDir);

                    string targetPath = Path.Combine(
                        targetDir,
                        Path.GetFileName(file)
                    );

                    File.Move(file, targetPath, overwrite: true);
                    break;
                }
            }
        }
    }

    // --- Main method ---
    public static void Run()
    {
        Console.Clear();

        // ask user for path
        AnsiConsole.Markup("Enter path: \n --> ");
        path = Console.ReadLine();

        // ask user for categories
        var selectedCategories = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title("What type of files would you like to sort?")
                        .AddChoices(FileTypes.Keys));

        Console.Clear();

        // display info
        var panel = new Panel("Path: " + path + "\n" + "Categories: " + string.Join(", ", selectedCategories)
)
              .Header("[bold yellow] Sort [/]")
              .RoundedBorder()
              .Padding(3, 1, 3, 1);
        AnsiConsole.Write(panel);

        // sort
        AnsiConsole.Status()
        .Start("Sorting...", ctx =>
          {
              Thread.Sleep(2000);
              Sort(path, selectedCategories);
          });

        AnsiConsole.MarkupLine("[lime] Sorting complete![/]");

    }
}


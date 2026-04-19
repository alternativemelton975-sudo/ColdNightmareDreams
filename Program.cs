using System;
using System.Collections.Generic;
using System.IO;

class StoryRunner
{
    private readonly List<string> _ChaptersPath = new List<string>();
    private int _index = 0;

    public StoryRunner(string ChaptersDirectory)
    {
        if (!Directory.Exists(ChaptersDirectory))
            throw new DirectoryNotFoundException(ChaptersDirectory);

        foreach (var file in Directory.GetFiles(ChaptersDirectory, "Chapter*.txt"))
        {
            _ChaptersPath.Add(file);
            Console.WriteLine($"Added chapter file: {file}");
        }

        // FIX: Sort chapters numerically instead of alphabetically
        _ChaptersPath.Sort((a, b) =>
        {
            int ExtractNumber(string path)
            {
                var name = Path.GetFileNameWithoutExtension(path);
                var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return int.TryParse(parts[^1], out var n) ? n : int.MaxValue;
            }

            return ExtractNumber(a).CompareTo(ExtractNumber(b));
        });

        if (_ChaptersPath.Count == 0)
            throw new InvalidOperationException("No story chapters found in directory: " + ChaptersDirectory);
    }

    public void Run()
    {
        PrintCurrentTitle();
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
                continue;

            var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            var arg = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            switch (cmd)
            {
                case "next":
                case "n":
                    if (_index < _ChaptersPath.Count - 1) _index++;
                    PrintCurrentTitle();
                    break;

                case "prev":
                case "p":
                    if (_index > 0) _index--;
                    PrintCurrentTitle();
                    break;

                case "goto":
                case "g":
                    if (string.IsNullOrWhiteSpace(arg))
                    {
                        Console.WriteLine("Usage: goto <number>");
                        break;
                    }

                    if (int.TryParse(arg, out var num) && num >= 1 && num <= _ChaptersPath.Count)
                    {
                        _index = num - 1;
                        PrintCurrentTitle();
                    }
                    else
                    {
                        Console.WriteLine("Invalid chapter number. Use 'list' to see available chapters.");
                    }
                    break;

                case "list":
                    for (var i = 0; i < _ChaptersPath.Count; i++)
                        Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(_ChaptersPath[i])}");
                    break;

                case "read":
                case "r":
                    PrintCurrentFull();
                    break;

                case "help":
                    Console.WriteLine("Commands: next (n), prev (p), goto (g) <num>, list, read (r), help, quit (q)");
                    break;

                case "quit":
                case "q":
                    Console.WriteLine("Goodbye.");
                    return;

                default:
                    Console.WriteLine("Unknown command. Type 'help' for commands.");
                    break;
            }
        }
    }

    private void PrintCurrentTitle()
    {
        var name = Path.GetFileNameWithoutExtension(_ChaptersPath[_index]);
        Console.WriteLine();
        Console.WriteLine($"Chapter {_index + 1}/{_ChaptersPath.Count}: {name}");
        Console.WriteLine("Type 'read' to read this chapter or 'list' to see all chapters.");
        Console.WriteLine();
    }

    private void PrintCurrentFull()
    {
        Console.WriteLine();
        var content = File.ReadAllText(_ChaptersPath[_index]);
        Console.WriteLine(content);
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        var chaptersDir = LocateChaptersDirectory();
        if (chaptersDir == null)
        {
            Console.WriteLine("Chapters directory not found. Creating temporary Chapters set for debugging.");
            chaptersDir = CreateTemporaryChapters();
        }

        try
        {
            var runner = new StoryRunner(chaptersDir);
            Console.WriteLine("Cold Nightmare — Interactive Story (chronological)");
            Console.WriteLine("Type 'help' for commands.");
            runner.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private static string LocateChaptersDirectory()
    {
        var starts = new[] { AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory() };
        foreach (var start in starts)
        {
            var dir = start;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                var candidate = Path.Combine(dir, "Chapters");
                if (Directory.Exists(candidate))
                    return candidate;

                dir = Directory.GetParent(dir)?.FullName;
            }
        }
        return null;
    }

    private static string CreateTemporaryChapters()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ColdNightmare_Chapters_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        File.WriteAllText(Path.Combine(tempDir, "Chapter 1.txt"),
            "Chapter 1 — Introduction\n\nWaking up in Ghost Yard, Pyrix feels the cold bite through his bones.\n");

        File.WriteAllText(Path.Combine(tempDir, "Chapter 2.txt"),
            "Chapter 2 — The Problem\n\nA relentless cold steals the voice of the living.\n");

        File.WriteAllText(Path.Combine(tempDir, "Chapter 3.txt"),
            "Chapter 3 — The Quest\n\nPyrix journeys across Ghost Yard seeking answers.\n");

        File.WriteAllText(Path.Combine(tempDir, "Chapter 4.txt"),
            "Chapter 4 — The Revelation\n\nThe crow is both guardian and guide.\n");

        File.WriteAllText(Path.Combine(tempDir, "Chapter 5.txt"),
            "Chapter 5 — The Resolution\n\nPyrix builds a small light to keep nightmares at bay.\n");

        return tempDir;
    }
}

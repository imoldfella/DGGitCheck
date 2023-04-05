
using DiffMatchPatch;

public class CompareFiles
{
    static void ListFiles(string directoryPath, HashSet<string> filePaths)
    {

        try
        {
            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                filePaths.Add(filePath);
            }

            foreach (string subdirectoryPath in Directory.GetDirectories(directoryPath))
            {
                ListFiles(subdirectoryPath, filePaths);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    static public async ValueTask<HashSet<String>> allFiles(string dir)
    {
        await Task.CompletedTask;
        HashSet<String> a = new();
        ListFiles(dir, a);
        HashSet<String> b = new();
        foreach (var x in a)
        {
            b.Add(x.Substring(dir.Length));
        }
        return b;
    }

    static public string removePrefix(string s)
    {
        var lines = s.Split("\n");
        int i = 0;
        while (i < lines.Length && (lines[i].Contains("using") || lines[i].Contains("namespace") || lines[i].Trim().Length == 0))
        {
            i++;
        }
        lines = lines.Skip(i).ToArray();
        return String.Join("\n", lines);
    }
    static public async ValueTask compare1(string from, string to)
    {
        var a = File.ReadAllText(from);
        var b = File.ReadAllText(to);

        a = removePrefix(a);
        b = removePrefix(b);

        if (a != b)
        {
            diff_match_patch dmp = new diff_match_patch();
            List<Diff> diff = dmp.diff_main(a, b);
            // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
            dmp.diff_cleanupSemantic(diff);
            // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
            for (int i = 0; i < diff.Count; i++)
            {
                Console.WriteLine(diff[i]);
            }
            Console.WriteLine($"<> {to})");
        }
        await Task.CompletedTask;
    }
    static public async ValueTask compare(string from, string to)
    {
        HashSet<String> a = await allFiles(from);
        HashSet<String> b = await allFiles(to);

        foreach (var ax in a)
        {
            if (!b.Contains(ax))
            {
                Console.WriteLine($"- {ax}");
            }
        }
        foreach (var bx in b)
        {
            if (!a.Contains(bx))
            {
                Console.WriteLine($"+ {bx}");
            }
        }
        foreach (var bx in b)
        {
            if (a.Contains(bx))
            {
                await compare1(Path.Join(from, bx), Path.Join(to, bx));
            }
        }
    }

}

public class Recover
{
    static public async ValueTask run()
    {
        var x = new string[] { "Asi.Selenium.Web", "V10", "V100", "SeleniumCore" };

        foreach (var f in x)
        {
            await CompareFiles.compare($"../asi1/src/Asi.Test/Selenium/{f}", $"../iMIS/test/Selenium/{f}");
        }
    }

}
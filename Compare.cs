
using System.Text;
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
            Environment.Exit(1);
        }
    }

    static public async ValueTask<List<String>> allFiles(string dir)
    {
        await Task.CompletedTask;
        HashSet<String> a = new();
        ListFiles(dir, a);
        List<String> b = new();
        foreach (var x in a)
        {
            b.Add(x.Substring(dir.Length));
        }
        return b;
    }

    static public string removePrefix(string s, out string prefix)
    {
        var lines = s.Split("\n");
        int i = 0;
        while (i < lines.Length && (lines[i].Contains("using") || lines[i].Contains("namespace") || lines[i].Trim().Length == 0))
        {
            i++;
        }
        prefix = String.Join("\n", lines.Take(i));
        lines = lines.Skip(i).ToArray();
        return String.Join("\n", lines);
    }
    static public async ValueTask<string> compare1(string from, string to, StringBuilder sb)
    {
        var a = File.ReadAllText(from);
        var b = File.ReadAllText(to);

        string ap, bp;
        a = removePrefix(a, out ap);
        b = removePrefix(b, out bp);

        if (a != b)
        {
            // 
            // diff_match_patch dmp = new diff_match_patch();
            // List<Diff> diff = dmp.diff_main(a, b);
            // // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
            // dmp.diff_cleanupSemantic(diff);
            // // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
            // for (int i = 0; i < diff.Count; i++)
            // {
            //     Console.WriteLine(diff[i]);
            // }
            sb.AppendLine($"\n<> {to}");
            //Diff.Compare(a, b);
            var o = Diffplex.compare(a, b);
            sb.AppendLine(o);
            return ap + "\n" + b;
        }
        await Task.CompletedTask;
        return "";
    }
    static public async ValueTask compare(string from, string to, StringBuilder sb)
    {
        var ok = (string e) => Path.GetExtension(e) == ".cs" || Path.GetExtension(e) == ".feature";
        var a = new HashSet<string>((await allFiles(from)).Where(ok));
        var b = new HashSet<string>((await allFiles(to)).Where(ok));

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
                var f = await compare1(Path.Join(from, bx), Path.Join(to, bx), sb);
                if (f != "")
                {
                    var replace = Path.Join(from, bx);
                    File.Move(replace, replace + ".replace");
                    File.WriteAllText(replace, f);

                }
            }
        }
    }

}

public class Recover
{
    static public async ValueTask<string> run()
    {
        var sb = new StringBuilder();
        var x = new string[] { "Asi.Selenium.Web", "V10", "V100", "SeleniumCore" };

        foreach (var f in x)
        {
            await CompareFiles.compare($"../iMIS/test/Selenium/{f}", $"../asi1/src/Asi.Test/Selenium/{f}", sb);
        }
        return sb.ToString();
    }

}

using System.Text;
using DiffPlex.Chunkers;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

public class Diffplex
{

    public static string compare(string before, string after)
    {
        StringBuilder sb = new();
        var diff = InlineDiffBuilder.Diff(before, after, true, true, new LineChunker());

        foreach (var line in diff.Lines)
        {
            switch (line.Type)
            {
                case ChangeType.Inserted:
                    sb.AppendLine("+ " + line.Text);
                    break;
                case ChangeType.Deleted:
                    sb.AppendLine("- " + line.Text);
                    break;
                    // default:
                    //     Console.ForegroundColor = ConsoleColor.Gray; // compromise for dark or light background
                    //     Console.Write("  ");
                    //     break;
            }
        }
        return sb.ToString();
    }
}
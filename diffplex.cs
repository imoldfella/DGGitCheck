
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
        List<String> sc = new();
        var context = () =>
        {
            sb.AppendLine(string.Join("\n", sc.TakeLast(2)));
            sc.Clear();
        };
        foreach (var line in diff.Lines)
        {
            switch (line.Type)
            {
                case ChangeType.Inserted:
                    context();
                    sb.AppendLine("+ " + line.Text);
                    break;
                case ChangeType.Deleted:
                    context();
                    sb.AppendLine("- " + line.Text);
                    break;
                default:
                    sc.Add("= " + line.Text);
                    break;
            }
        }
        return sb.ToString();
    }
}
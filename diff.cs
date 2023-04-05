using System;
using System.IO;
using System.Text;

public class Diff
{
    public static void Compare(string originalBytes, string modifiedBytes)
    {

        var patchStreamWriter = new StringBuilder();
        {
            int i = 0;
            while (i < originalBytes.Length && i < modifiedBytes.Length)
            {
                if (originalBytes[i] != modifiedBytes[i])
                {
                    int j = i + 1;
                    while (j < originalBytes.Length && j < modifiedBytes.Length && originalBytes[j] != modifiedBytes[j])
                    {
                        j++;
                    }
                    int length = j - i;
                    patchStreamWriter.Append($"@@ -{i},{length} +{i},{length} @@");
                    for (int k = i; k < j; k++)
                    {
                        patchStreamWriter.Append($"-{originalBytes[k]}");
                    }

                    for (int k = i; k < j; k++)
                    {
                        patchStreamWriter.Append($"+{modifiedBytes[k]}");
                    }

                    i = j;
                }
                else
                {
                    i++;
                }
            }
        }

        Console.WriteLine(patchStreamWriter.ToString());
    }
}

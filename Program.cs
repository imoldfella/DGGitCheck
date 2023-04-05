using LibGit2Sharp;
var o = await Recover.run();
File.WriteAllText("recover.txt", o);

// using (var repo = new Repository(@"D:\source\LibGit2Sharp"))
// {
//     Commit commit = repo.Lookup<Commit>("73b48894238c3e9c37f9f3a696bbd4bffcf45ce5");
//     Console.WriteLine("Author: {0}", commit.Author.Name);
//     Console.WriteLine("Message: {0}", commit.MessageShort);
// }

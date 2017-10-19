using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GitHubStarCounter
{
    class Worker
    {

        public async Task Work()
        {
            var userfiles = ReadInput("provide the files you want to examine with commas between them");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await GetGitHubStars(userfiles);
            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            Console.WriteLine($"execution time is {elapsedTime} ms");
            Console.ReadLine();
        }

        public List<string> ReadInput(string userInput)
        {
            Console.WriteLine(userInput);
            var rInput = Console.ReadLine();
            var elements = rInput?.Split(',').ToList();
            return elements;
        }

        public async Task GetGitHubStars(List<string> uInput)
        {
            var tasks = new List<Task<SearchRepositoryResult>>();
            var orderedList = new List<Repository>();
            foreach (var t in uInput)
            {
                var name = t.Trim();
                var github = new GitHubClient(new ProductHeaderValue(name));
                var request = new SearchRepositoriesRequest(name)
                {
                    In = new[] { InQualifier.Name },
                    PerPage = 1
                };
                var result = github.Search.SearchRepo(request);
                tasks.Add(result);
            }
            var allResults = await Task.WhenAll(tasks);
            foreach (var x in allResults)
            {
                var repo = x.Items.FirstOrDefault();
                Console.WriteLine(repo?.StargazersCount);
                orderedList.Add(repo);
            }
            var sortedList = orderedList.OrderByDescending(o => o.StargazersCount);

            foreach (var item in sortedList)
            {
                Console.WriteLine($"Repo name is {item.Name} and it has {item.StargazersCount} stars");
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using ProductHeaderValue = System.Net.Http.Headers.ProductHeaderValue;

namespace GitHubStarCounter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var worker = new Worker();
            worker.Work().Wait();
        }
    }
}

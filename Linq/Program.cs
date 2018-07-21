namespace Bars.NuGet.Querying
{
    using global::Bars.Linq.Async;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }

        public static async Task MainAsync(string[] args)
        {
            string[] possiblePackages = { "BarsUp.App", "BarsUp.Core" };

            var MyGet = new NuGetFeed("https://barsgroup.myget.org/F/barsup-net-core/auth/122a4baf-5686-4675-8420-3132823267c7/api/v3/index.json", "https://api.nuget.org/v3-index/index.json");
            
            await MyGet
                .Where(x => x.Id.Contains("BarsUp"))
                .ForFramework(NetFramework.NetFramework, "4.6")
                .ForFramework(NetFramework.NetStandard, "2.1")
                .IncludePrerelease()
                .Latest()
                .OrderBy(x => x.Id)
                .Skip(5)
                .Take(10)
                .ToAsync()
                .ForEach(x => Console.WriteLine(x.Id));


            Console.WriteLine($"Finded packages:");

            //foreach (var package in packages)
            //{
            //    Console.WriteLine(package?.Id ?? "empty");
            //}

            //var enumerator = packages.GetAsyncEnumerator();
            //while (await enumerator.MoveNext())
            //{
            //    var package = await enumerator.CurrentAsync;
            //    Console.WriteLine(package?.Id ?? "Empty package");
            //}

            Console.WriteLine();
            Console.WriteLine("end");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;
using WebCrawler.Console.Model;
using WebCrawler.Console.Reporting;

namespace WebCrawler.Console
{
    class Program
    {
        private static readonly HyperLinksGraph Graph;
        
        static Program()
        {
            log4net.Config.XmlConfigurator.Configure();
            Graph = new HyperLinksGraph();
        }

        private static void Main(string[] args)
        {
            try
            {
                Uri uriToCrawl = GetSiteToCrawl();

                // I'm using the default crawler
                var crawler = new PoliteWebCrawler();

                // I need to subscribe to this event in order to process pages that have been crawled 
                crawler.PageCrawlCompletedAsync += ProcessPageCrawlCompleted;

                // Start the crawl
                CrawlResult crawlResult = crawler.Crawl(uriToCrawl);

                // Generate report
                Task<ReportResult> reportTask = GenerateReport();

                PrintResultInformation(reportTask.Result);
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("There was an error when trying to crawl page.");
                System.Console.Write(ex);
                System.Console.ReadKey();
            }
        }

        private static void ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            string key = e.CrawledPage.Uri.AbsoluteUri;
            List<string> edges = e.CrawledPage.ParsedLinks?.Select(l => l.AbsoluteUri).ToList();
            Graph.AddNode(key, edges);
        }

        private static Uri GetSiteToCrawl()
        {
            string userInput = null;

            while (string.IsNullOrWhiteSpace(userInput) || !Uri.IsWellFormedUriString(userInput, UriKind.Absolute))
            {
                System.Console.WriteLine("Please enter an absolute url to crawl:");
                userInput = System.Console.ReadLine();
            }

            return new Uri(userInput);
        }

        private static async Task<ReportResult> GenerateReport()
        {
            var rg = new ReportGenerator(Graph);
            return await rg.GenerateReport(0, Graph.First().GetKey());
        }

        private static void PrintResultInformation(ReportResult result)
        {
            if (result.Success)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("Report path: {0}", result.FileName);
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Unable to generate report. Error:");
                System.Console.Write(result.Error);
            }

            System.Console.ReadKey();
        }
    }
}
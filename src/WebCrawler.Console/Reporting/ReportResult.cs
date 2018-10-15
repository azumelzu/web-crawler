using System;

namespace WebCrawler.Console.Reporting
{
    public class ReportResult
    {
        public string FileName { get; set; }
        public bool Success { get; set; }
        public Exception Error { get; set; }
    }
}

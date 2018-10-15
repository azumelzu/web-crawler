using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Console.Model;

namespace WebCrawler.Console.Reporting
{
    public class ReportGenerator
    {
        private const string FilePathTemplate = "./web-crawler-{0}.tsv";
        private const int MaxDepthLevel = 4;
        private readonly HyperLinksGraph _graph;

        public ReportGenerator(HyperLinksGraph graph)
        {
            _graph = graph;
        }

        public async Task<ReportResult> GenerateReport(int level, string key)
        {
            var result = new ReportResult();

            try
            {
                string fileName = string.Format(FilePathTemplate, DateTime.Now.ToString("yyyy-MM-ddThh-mm-ss"));
                using (var fs = new FileStream(fileName, FileMode.Append))
                {
                    await Traverse(level, key, fs);
                }

                result.FileName = fileName;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        private async Task Traverse(int level, string key, FileStream fs)
        {
            if (level == MaxDepthLevel)
            {
                return;
            }
            
            byte[] data = Encoding.ASCII.GetBytes($"{new String('\t', level)} {key} {Environment.NewLine}");
            await fs.WriteAsync(data, 0, data.Length);
            
            level++;
            foreach (var v in _graph.GetAdjacentKeys(key))
            {
                await Traverse(level, v, fs);
            }
        }
    }
}

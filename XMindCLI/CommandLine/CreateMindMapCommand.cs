using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using OfficeOpenXml;
using Serilog;
using XMindAPI;
namespace XMindCLI.CommandLine
{
    public class CreateMindMapCommand : ICommand
    {
        private readonly ILogger logger;
        public CreateMindMapCommand(ILogger logger)
        {
            this.logger = logger;
        }

        void ICommand.Execute<T>(T opts)
        {
            var options = opts as MindMapOptions;
            var outPath = Path.Combine(options.Path, options.FileName)
                .Replace("\\", "/");
            logger.Information($"Request: {opts.Command}: from file {options.SourcePath} to destination file {outPath}");
            var book = new XMindConfiguration()
                .WithFileWriter(options.Path, zip: true)
                .CreateWorkBook(options.FileName);
            var rootTopic = book
                .GetPrimarySheet()
                .GetRootTopic();
            ReadPayload(options.SourcePath);
            rootTopic.Add(book.CreateTopic("Child"));
            book.Save().GetAwaiter().GetResult();
        }

        private void ReadPayload(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets[0];
            var entries = new List<EduScopeEntry>();
            var cells = sheet.Cells;
            for (int i = 2; ; i++)
            {
                var cellValue = cells[i, 1].Value;
                if (cellValue is null)
                {
                    break;
                }
                var entry = new EduScopeEntry()
                {
                    Name = cells[i, 1].Value?.ToString(),
                    Progress = int.Parse(cells[i, 4].Value?.ToString()),
                    Status = cells[i, 5].Value?.ToString(),
                    Priority = cells[i, 6].Value?.ToString(),
                    Type = cells[i, 7].Value?.ToString(),
                    Epic = cells[i, 8].Value?.ToString(),
                    Link = cells[i, 9].Value?.ToString(),
                };
                entries.Add(entry);
                // logger.Debug($"{entry}");
            }
            var groupedEntries = entries.ToLookup(entry => entry.Epic);
            foreach (var group in groupedEntries)
            {
                logger.Debug($"");
                logger.Debug($"{group.Key}");
                logger.Debug($"");
                foreach (var e in group)
                {
                    logger.Debug($"\t\t{e}");
                }
            }
            // https://github.com/EPPlusSoftware/EPPlus.Sample.NetCore/blob/master/01-GettingStarted/GettingStartedSample.cs
        }

        private class EduScopeEntry
        {
            public string Name { get; set; }

            [Range(0, 100)]
            public int Progress { get; set; }

            public string Status { get; set; }

            public string Priority { get; set; }

            public string Type { get; set; }

            public string Epic { get; set; }

            public string Link { get; set; }

            public override string ToString() =>
                // $"{Name} - {Status} - {Priority} - {Epic}";
                $"{Name}";

        }
    }
}

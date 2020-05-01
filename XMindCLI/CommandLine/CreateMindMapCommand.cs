using System.Linq;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Serilog;
using XMindAPI;
using XMindCLI.Services;

namespace XMindCLI.CommandLine
{
    public partial class CreateMindMapCommand : ICommand
    {
        private readonly ILogger logger;
        private readonly IMindMapBuilder mindMapBuilder;

        public CreateMindMapCommand(ILogger logger, IMindMapBuilder mindMapBuilder)
        {
            this.logger = logger;
            this.mindMapBuilder = mindMapBuilder;
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

            var entries = ReadPayload(options.SourcePath);
            LogGroupedEntries(entries);
            mindMapBuilder.BuildMindMapContent(book, entries);
            book.Save().GetAwaiter().GetResult();
        }

        private IEnumerable<EduScopeEntry> ReadPayload(string path)
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
            }
            return entries;
        }

        private void LogGroupedEntries(IEnumerable<EduScopeEntry> entries)
        {
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
        }
    }
}

using System.IO;
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

            rootTopic.Add(book.CreateTopic("Child"));
            book.Save().GetAwaiter().GetResult();
        }
    }
}

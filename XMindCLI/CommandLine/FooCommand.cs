using Serilog;
using XMindAPI;
using XMindAPI.Configuration;
using XMindAPI.Writers;

namespace XMindCLI.CommandLine
{
    public class FooCommand : ICommand
    {

        private readonly ILogger logger;
        public FooCommand(ILogger logger)
        {
            this.logger = logger;
        }

        public void Execute()
        {
            logger.Information("FooCommand. Invoked!");
            var writer = (InMemoryWriter)new InMemoryWriter()
                .SetOutput(new InMemoryWriterOutputConfig(outputName: "root"));
            //Arrange
            var book = new XMindConfiguration()
                .WriteTo
                .Writer(writer)
                .CreateWorkBook(workbookName: "test");
        }
    }
}

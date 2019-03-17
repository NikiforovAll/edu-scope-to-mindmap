using Serilog;

namespace LearningScopeToMindmapClient.CommandLine
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
        }
    }
}
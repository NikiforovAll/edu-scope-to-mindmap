using Serilog;

namespace XMindCLI.CommandLine
{
    public class CreateMindMapCommand : ICommand
    {
        private readonly ILogger logger;
        public CreateMindMapCommand(ILogger logger)
        {
            this.logger = logger;
        }

        void ICommand.Execute<MindMapOptions>(MindMapOptions opts)
        {
            logger.Information("MindMapCommand. Invoked!");
        }
    }
}

using Serilog;
using CommandLine;

namespace LearningScopeToMindmapClient.CommandLine
{
    [Verb("read", HelpText = "Read reports from configured input")]
    public class FooOptions: ICommandOptions
    {
        public string Command { get; } = nameof(FooCommand);
        [Option("Verbose", HelpText="Verbose param")]
        public bool Verbose { get; set; }
        public FooOptions()
        {
        }
    }
}
using Serilog;
using CommandLine;

namespace XMindCLI.CommandLine
{
    [Verb("build", HelpText = "")]
    public class MindMapOptions : ICommandOptions
    {
        public string Command { get; } = nameof(CreateMindMapCommand);

        [Option("verbose", HelpText = "Verbose option", Required = false)]
        public bool Verbose { get; set; }

        [Option("path", Required = true, HelpText = "Path to create xmind file")]
        public string Path { get; set; }
        public MindMapOptions()
        {
        }
    }
}

namespace XMindCLI.CommandLine
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(string commandName, ICommandOptions opts);
    }
}

namespace LearningScopeToMindmapClient.CommandLine
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(string commandName, ICommandOptions opts);
    }
}
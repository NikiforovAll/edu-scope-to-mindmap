namespace XMindCLI.CommandLine
{
    public interface ICommand
    {
        void Execute<TOptions>(TOptions opts) where TOptions: ICommandOptions;
    }
}

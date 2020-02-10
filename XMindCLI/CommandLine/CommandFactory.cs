using Autofac;
using System;
namespace XMindCLI.CommandLine
{
    public class CommandFactory : ICommandFactory
    {
        private ILifetimeScope lifetimeScope;

        public CommandFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }
        public ICommand CreateCommand(string commandName, ICommandOptions opts)
        {
            ICommand command = null;
            if(commandName.Equals(nameof(FooCommand), StringComparison.OrdinalIgnoreCase)){
                command = lifetimeScope
                    .Resolve<ICommand>(new TypedParameter(typeof(FooOptions), opts as FooOptions));
            }
            return command;
        }
    }
}

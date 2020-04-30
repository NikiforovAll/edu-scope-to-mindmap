using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using XMindCLI.CommandLine;
using Serilog;
using XMindCLI.Infrastructure;

namespace XMindCLI
{
    public class CommandLineApplication : IApplication, IStartable
    {
        private static ILogger _logger;
        private readonly ICommandFactory _commandFactory;

        public CommandLineApplication(ILogger logger, ICommandFactory commandFactory)
        {
            _logger = logger;
            _commandFactory = commandFactory;
        }
        public void Run(string[] args)
        {
            var result = Parser.Default.ParseArguments<MindMapOptions>(args)
                .MapResult((MindMapOptions opts) =>
                {
                    var command = _commandFactory.CreateCommand(opts.Command, opts);
                    command.Execute(opts);
                    return 0;
                },
                errs =>
                {
                    Environment.Exit(1);
                    return 1;
                }
                // throw new ArgumentException("Arguments are not parsed. Please see --help information")
            );
        }
        public void Start() => AppDomain.CurrentDomain.UnhandledException
            += UnhandledExceptionTrapper;

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            _logger.Fatal(eventArgs.ExceptionObject.ToString());
            // Environment.Exit(1);
        }
    }
}

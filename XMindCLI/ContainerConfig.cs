using System;
using Autofac;
using XMindCLI;
using XMindCLI.CommandLine;
using Serilog;

namespace XMindCLI.Infrastructure
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ClientApplication>().As<IApplication>();
            builder
                .AddLogger()
                .AddStartupHandler()
                // .AddFileConfig()
                .RegisterCommands();
            return builder.Build();
        }

        public static ContainerBuilder AddLogger(this ContainerBuilder builder)
        {
            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .WriteTo.File("logs/log.txt",
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true)
                        .CreateLogger();
            }).SingleInstance();
            return builder;
        }

        // public static ContainerBuilder AddFileConfig(this ContainerBuilder builder)
        // {
        //     IOConfigSection config = System.Configuration
        //         .ConfigurationManager
        //         .GetSection(IOConfigSection.SECTION_NAME) as IOConfigSection;
        //     builder.RegisterInstance(config).As<IOConfigSection>().SingleInstance();
        //     return builder;
        // }

        public static ContainerBuilder AddStartupHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<ClientApplication>()
                .As<IStartable>()
                .SingleInstance();
            return builder;
        }

        public static ContainerBuilder RegisterCommands(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandFactory>().As<ICommandFactory>();
            builder
                .RegisterType<FooCommand>()
                .As<ICommand>()
                .WithParameter(new TypedParameter(typeof(FooOptions), null))
                .InstancePerDependency();
            return builder;
        }
    }
}


using Serilog;
using Serilog.Events;

namespace Scores.Infrastructure.IoC
{
    public class SerilogConfigurationBuilder
    {
        private readonly LoggerConfiguration _configuration;

        public SerilogConfigurationBuilder()
        {
            _configuration = new LoggerConfiguration();
        }

        public SerilogConfigurationBuilder WithConsoleOutput(LogEventLevel logEventLevel = LogEventLevel.Information,
            string template = "[{Timestamp: HH: mm: ss}{Properties}{Level: u3}] {Message: lj}{NewLine}{Exception}")
        {
            _configuration.WriteTo.Console(logEventLevel, template);

            return this;
        }

        public SerilogConfigurationBuilder WithEventsFromLogContext()
        {
            _configuration.Enrich.FromLogContext();

            return this;
        }

        public SerilogConfigurationBuilder WithApplicationName(string applicationName)
        {
            _configuration.Enrich.WithProperty("Application", applicationName);

            return this;
        }

        public SerilogConfigurationBuilder WithMachineName()
        {
            _configuration.Enrich.WithMachineName();

            return this;
        }

        public LoggerConfiguration Build()
        {
            return _configuration;
        }
    }
}

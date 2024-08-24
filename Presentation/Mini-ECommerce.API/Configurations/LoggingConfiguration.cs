using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
using Serilog;
using Serilog.Core;
using Microsoft.AspNetCore.HttpLogging;
using Serilog.Events;

namespace Mini_ECommerce.API.Configurations
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {

            // Create and configure the logger
            Logger log = new LoggerConfiguration()
     .WriteTo.Console()
     .WriteTo.File("logs/log.txt")
     .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
         needAutoCreateTable: true,
         columnOptions: new Dictionary<string, ColumnWriterBase>
         {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text)},
            {"level", new LevelColumnWriter(true , NpgsqlDbType.Varchar)},
            {"time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text)},
            {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Json)},
            {"user_name", new UsernameColumnWriter()}
         })
     .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
     .Enrich.FromLogContext()
     .MinimumLevel.Information()
     .CreateLogger();

            // Apply Serilog to the application
            builder.Host.UseSerilog(log);

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                // logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                // logging.CombineLogs = true;
            });
        }

        public class UsernameColumnWriter : ColumnWriterBase
        {
            public UsernameColumnWriter() : base(NpgsqlDbType.Varchar)
            { }
            public override object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
            {
                var (username, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "user_name");

                return value?.ToString() ?? null;
            }
        }
    }
}

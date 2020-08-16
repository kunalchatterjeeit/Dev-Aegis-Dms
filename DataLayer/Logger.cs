using Serilog;
using Serilog.Exceptions;
using System;
using System.Configuration;
using System.Text;

namespace DataLayer
{
    public class Logger
    {
        public Logger() { }

        public ILogger Serilog_ExceptionLogger()
        {
            ILogger logger;
            string connectionString = ConfigurationManager.ConnectionStrings["constr"].ToString();
            string serilogTable = (string)new AppSettingsReader().GetValue("SerilogTableName", typeof(string));
            logger = new LoggerConfiguration().Enrich.WithExceptionDetails().WriteTo.MySQL(connectionString, serilogTable).CreateLogger();
            return logger;
        }

        public static void LogCustomException(string message, string actionName)
        {
            ILogger logger = new Logger().Serilog_ExceptionLogger();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("=====Error=====");
            stringBuilder.Append("=====Action=====");
            stringBuilder.Append(actionName);
            stringBuilder.Append("\r\n");
            stringBuilder.Append(message);
            logger.Error(stringBuilder.ToString());
        }

        public static void LogException(Exception ex, string actionName)
        {
            ILogger logger = new Logger().Serilog_ExceptionLogger();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(PrepareExceptionString(ex));
            stringBuilder.Append("=====Action=====");
            stringBuilder.Append(actionName);
            stringBuilder.Append("\r\n");
            logger.Error(stringBuilder.ToString());
        }

        private static string PrepareExceptionString(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("=====Error Message=====");
            stringBuilder.Append(ex.Message);
            stringBuilder.Append("=====Stack Trance=====");
            stringBuilder.Append((ex.StackTrace != null) ? ex.StackTrace.ToString() : string.Empty);
            stringBuilder.Append("<---->\r\n");
            if (ex.InnerException != null)
                PrepareExceptionString(ex.InnerException);
            return stringBuilder.ToString();
        }
    }
}

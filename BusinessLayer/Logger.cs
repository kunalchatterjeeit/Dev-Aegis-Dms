using Serilog;
using System;

namespace BusinessLayer
{
    public  class Logger
    {
        public Logger() { }

        public ILogger Serilog_ExceptionLogger()
        {
            return new DataLayer.Logger().Serilog_ExceptionLogger();
        }

        public void LogCustomException(string message, string actionName)
        {
            DataLayer.Logger.LogCustomException(message, actionName);
        }

        public void LogException(Exception ex, string actionName)
        {
            DataLayer.Logger.LogException(ex, actionName);
        }
    }
}

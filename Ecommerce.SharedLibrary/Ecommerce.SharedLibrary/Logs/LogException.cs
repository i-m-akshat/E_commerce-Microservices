
using Serilog;

namespace Ecommerce.SharedLibrary.Logs
{
  public static class LogException
    {
        public static void LogExceptions( this Exception ex)
        {
            LogToFile(ex.Message);//Log to file 
            LogToConsole(ex.Message);//log to console
            LogToDebugger(ex.Message);//Log to debugger
        }

        public static void LogToFile(string exceptionMessage)
        {
             Log.Information(exceptionMessage);
        }
        public static void LogToConsole(string exceptionMessage)
        {
            Log.Warning(exceptionMessage);
        }
        public static void LogToDebugger(string exceptionMessage)
        {
            Log.Debug(exceptionMessage);
        }
    }
}

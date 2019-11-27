using System;
using System.Diagnostics;
using System.Threading;
using Realms;

namespace Awesome.Player.Core.ExtentionMethods
{
    /// <summary>
    ///     Class DebugLogExtention.
    /// </summary>
    public static class DebugLogExtention
    {
        /// <summary>
        ///     Write Exception log to the output.
        ///     this only works in Debug mode. the caller member can be commented out in Production Code.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void DebugModeExceptionLog(this Exception ex , string methodName)
        {
#if DEBUG

            #region DebugModeLog Output

            Debug.WriteLine("**********--------- Start of Exception MethodName: ---------************");
            Debug.WriteLine($"**********--------- {methodName} Error ---------************");
            Debug.WriteLine($"**********--------- Line Number of Exception is : {ex.LineNumber()}---------************");
            Debug.WriteLine("**********--------- Exception Message: ---------************");
            Debug.WriteLine($"**********--------- {ex.Message} ---------************");
            Debug.WriteLine("**********--------- Exception InnerException: ---------************");
            Debug.WriteLine($"**********--------- {ex.InnerException} ---------************");
            Debug.WriteLine("**********--------- Exception Data: ---------************");
            Debug.WriteLine($"**********--------- {ex.Data} ---------************");
            Debug.WriteLine("**********--------- Exception Source: ---------************");
            Debug.WriteLine($"**********--------- {ex.Source} ---------************");
            Debug.WriteLine("**********--------- Exception GetBaseException: ---------************");
            Debug.WriteLine($"**********--------- {ex.GetBaseException()} ---------************");
            Debug.WriteLine("**********--------- Exception StackTrace: ---------************");
            Debug.WriteLine($"**********--------- {ex.StackTrace} ---------************");
            Debug.WriteLine("**********--------- Exception HResult: ---------************");
            Debug.WriteLine($"**********--------- {ex.HResult} ---------************");
            Debug.WriteLine("**********--------- End of Exception MethodName: ---------************");
            Debug.WriteLine($"**********--------- {methodName} Error ---------************");

            #endregion DebugModeLog Output

#endif
        }

        /// <summary>
        ///     Write custom log to the output.
        ///     this can be helpful when One want to Navigate to the method Execution in the output.
        ///     this only works in Debug mode. the caller member can be commented out in Production Code.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void DebugModeGrandiosity(this string message , string methodName = null)
        {
#if DEBUG

            #region DebuModeGrandiosity Output

            Debug.WriteLine($"**********--------- {methodName} Executed ---------************");
            Debug.WriteLine($"**********--------- with Message : {message} ---------************");

            #endregion DebuModeGrandiosity Output

#endif
        }

        // FIXME: this method currently get the path to database. this is smelly, but ok for now.
        // this code will be removed later, but for now it's only for Clarity about when the App crash during Startup (used with breakpoints).
        /// <summary>
        /// Gets the realm file path.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetRealmFilePath(this RealmConfiguration realmConfiguration)
        {
            #region Realm Database filePath

            var defaultConfigurationDatabasePath = realmConfiguration.DatabasePath;
            Debug.WriteLine($"*********************** RealmConfiguration.DefaultConfiguration.DatabasePath : {defaultConfigurationDatabasePath} *************************");

            return defaultConfigurationDatabasePath;

            #endregion Realm Database filePath
        }

        public static void GetCurrentThreadId(this Thread thread)
        {
#if DEBUG

            #region Catch current-Thread to the debugger

            Debug.WriteLine("**********--------- Current Thread the code is Running on: ---------************");
            Debug.WriteLine($"**********--------- Thread number is: {thread.ManagedThreadId} ---------************");

            #endregion

#endif
        }
    }
}
using System;
using System.Diagnostics;

namespace Awesome.Player.Core.ExtentionMethods
{
    /// <summary>
    /// Class ExceptionHelper.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Returns the Line number of the caller Context.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>System.Int32.</returns>
        public static int LineNumber(this Exception e)
        {
            var lineNumber = 0;
            try
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(e , true);
                // Get the top stack frame
                var frame = st.GetFrame(st.FrameCount - 1);
                // Get the line number from the stack frame
                lineNumber = frame.GetFileLineNumber();
            }
            catch
            {
                //Stack trace is not available!
            }

            return lineNumber;
        }
    }
}
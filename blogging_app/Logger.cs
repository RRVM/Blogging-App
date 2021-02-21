using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace blogging_app
{
    public class cls_logger
    {
        public static string log_file_path = Startup.getLogFilePath();
        public static void LogError(string message)
        {
            try
            {
                var exceptionMessage = message;
                //var stackTrace = filterContext.Exception.StackTrace;
                //var controllerName = filterContext.RouteData.Values["controller"].ToString();
                //var actionName = filterContext.RouteData.Values["action"].ToString();

                string Message = DateTime.Now.ToString() + " : " + exceptionMessage
                                + Environment.NewLine + "---------------------------------------------------";


                string logfilepath = Path.Combine(log_file_path, "Exception-" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log");

                using (StreamWriter writeFile = new StreamWriter(logfilepath, true))
                {
                    writeFile.WriteAsync(Message);
                }


            }
            catch (Exception e)
            {

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace laba2.Filters
{
    public class LoggingFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            using (StreamWriter sw = File.AppendText("logging.txt"))
            {
                sw.WriteLine(context.HttpContext.Request.Path.ToString());
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}

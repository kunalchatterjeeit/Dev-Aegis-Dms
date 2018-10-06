using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class ExceptionHelper
    {
        public static int Log(this Exception ex, string exceptionUrl, int userId)
        {
            return DataLayer.ExceptionLog.ErrorLog_Save(ex.Message, ex.GetType().Name, ex.StackTrace.ToString(), exceptionUrl, userId);
        }
    }
}

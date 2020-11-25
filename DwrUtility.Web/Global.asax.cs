using DwrUtility.CreanFiles;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace DwrUtility.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //清理日志
            CreanFileHelper.Start(new CreanFileParam()
            {
                FileTimes = new List<FileTime>()
                {
                    new FileTime()
                    {
                        DeleteTime = TimeSpan.FromSeconds(20),
                        Directories = new List<string>()
                        {
                            $"{AppDomain.CurrentDomain.BaseDirectory}Logs"
                        }
                    }
                },
                Log = ex => WriteHelper.Log(ex.ToString()),
                DueTime = TimeSpan.FromSeconds(5),
                Period = TimeSpan.FromSeconds(30)
            });
        }
    }
}

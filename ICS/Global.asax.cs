using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
                if (((HttpException)ex).GetHttpCode() == 400 || ((HttpException)ex).GetHttpCode() == 403
                    || ((HttpException)ex).GetHttpCode() == 404 || ((HttpException)ex).GetHttpCode() == 500)
                    Response.Redirect("http://intercs.az/" + ((HttpException)ex).GetHttpCode().ToString());
                else Response.Redirect("http://intercs.az/" + "Error");
        }
    }
}

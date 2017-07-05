using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return Redirect("https://dashboard.tawk.to/login");
        }
    }
}
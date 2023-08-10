using Microsoft.AspNetCore.Mvc;

namespace GreenGrassUI.Models
{
    public class BaseController: Controller
    {
        public BaseController()
        {
            ViewBag.UserId = Request.Cookies["UserId"]; ;
        }
    }
}

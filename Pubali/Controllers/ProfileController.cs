using Microsoft.AspNetCore.Mvc;

namespace Pubali.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            try
            {
                return View();
            }
            catch(Exception ex) 
            {
                return View("Error Message: ",ex);
            }
        }
    }
}

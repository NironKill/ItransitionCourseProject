using Microsoft.AspNetCore.Mvc;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class FormController : Controller
    {
        public FormController() 
        {
        }

        [HttpGet("Question")]
        public IActionResult Question()
        {
            return View();
        }
    }
}

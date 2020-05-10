using Microsoft.AspNetCore.Mvc;

namespace LM.MSEmail.Api.Controllers
{
    [ApiController, Route("")]
    public class MeController : ControllerBase
    {
        public IActionResult Get() => Ok(new
        {
            name = "lm-ms-email",
            version = "1.0"
        });
    }
}
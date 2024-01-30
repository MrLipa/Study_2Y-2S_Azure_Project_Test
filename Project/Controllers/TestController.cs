using Microsoft.AspNetCore.Mvc;
using Project.Helper;

namespace Project.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EventGridHelper _eventGridHelper;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            _eventGridHelper = new EventGridHelper(configuration);
        }

        [HttpGet()]
        public IActionResult GetConfigValues()
        {

            var baseUrl = _configuration["ExternalApi:BaseUrl"];
            var myDbConnection = _configuration.GetConnectionString("MyDbConnection");

            var configValues = new
            {
                BaseUrl = baseUrl,
                MyDbConnection = myDbConnection
            };

            return Ok(configValues);
        }

        [HttpGet("sendEventToEventGrid")]
        public async Task<IActionResult> SendEventToEventGrid()
        {
            var payload = new { email = "skyla.schulist85@ethereal.email", subject = "Test", message = "Test" };

            string subject = "Your Fixed Subject";
            string eventType = "Your.Fixed.EventType";

            await _eventGridHelper.SendEventToEventGrid(subject, eventType, payload);

            return Ok("Zdarzenie zostało wysłane do Event Grid");
        }
    }
}

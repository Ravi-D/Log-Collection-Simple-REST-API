using Microsoft.AspNetCore.Mvc;

namespace LogCollection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaceholderController : ControllerBase
    {
        private readonly ILogger<PlaceholderController> _logger;

        public PlaceholderController(ILogger<PlaceholderController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Placeholder")]
        public IEnumerable<Placeholder> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Placeholder
            {
                Date = DateTime.Now,
                Id = 0,
                Guid = Guid.NewGuid(),
            })
            .ToArray();
        }
    }
}
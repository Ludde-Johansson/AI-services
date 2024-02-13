using Microsoft.AspNetCore.Mvc;

namespace ai_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "GPT", "Moxtral", "LLama"
        };

        private readonly ILogger<AIController> _logger;

        public AIController(ILogger<AIController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSupportedAiServices")]
        public IEnumerable<AiService> Get()
        {
            return Enumerable.Range(0, Summaries.Length).Select(index => new AiService
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Name = Summaries.ElementAt(index),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

using ai_api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ai_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;


        private static readonly string[] Summaries = new[]
        {
            "Translate"
        };

        private readonly ILogger<AIController> _logger;
        //private static string FEATURE;
        private static readonly string TRANSLATE_ROUTE = $"/translate?api-version=3.0&to=es";

        public AIController(ILogger<AIController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient("translator");
        }

        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] TranslationRequest transReq)
        {
            string textToTranslate = transReq.Text;

            string requestBody = "[{'Text':'" + textToTranslate + "'}]";
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(TRANSLATE_ROUTE, content);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Ok(result);
            }
            _logger.LogError(result);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred processing your request.");
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

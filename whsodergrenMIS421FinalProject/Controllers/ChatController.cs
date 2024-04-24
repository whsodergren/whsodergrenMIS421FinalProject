using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using whsodergrenMIS421FinalProject.Models;

public class ChatController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _openAiApiKey = "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";

    public ChatController()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _openAiApiKey);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ChatModel());
    }

    [HttpPost]
    public async Task<IActionResult> GetResponse(ChatModel model)
    {
        var messages = new[]
        {
        new { role = "user", content = model.UserQuestion }
    };

        var payload = new
        {
            model = "gpt-3.5-turbo",
            messages = messages,
            max_tokens = 150
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);
        var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var openAiResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
            model.ResponseText = openAiResponse.choices[0].message.content.ToString();
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            model.ResponseText = $"Error: {response.StatusCode} - {errorResponse}";
        }
        return View("Index", model);
    }



}


using Newtonsoft.Json;
using System.Text;

namespace SistemaParqueo.MVC.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7020/api/");
    }

    public async Task<string> PostAsync(string endpoint, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        return await response.Content.ReadAsStringAsync();
    }
}
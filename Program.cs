using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ✅ Serve static files (index.html, script.js, style.css)
app.UseDefaultFiles();
app.UseStaticFiles();

// ✅ Weather endpoint
app.MapGet("/weather/{city}", async (string city) =>
{
    string apiKey = "e37d0158f9a3fc7a34d48dd993e33719"; // <-- yaha apna OpenWeather API key daalo
    string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

    using var httpClient = new HttpClient();
    try
    {
        var response = await httpClient.GetFromJsonAsync<OpenWeatherResponse>(url);
        if (response == null)
        {
            return Results.NotFound(new { message = "Weather data not found" });
        }

        var weather = new
        {
            City = response.Name,
            Temperature = response.Main.Temp + " °C",
            Condition = response.Weather[0].Description
        };

        return Results.Ok(weather);
    }
    catch
    {
        return Results.Problem("Error fetching weather data");
    }
});

// ✅ Fallback to index.html (for unknown routes)
app.MapFallbackToFile("index.html");

app.Run();


// ---------------- Models ----------------
public class OpenWeatherResponse
{
    public string Name { get; set; } = "";
    public MainInfo Main { get; set; } = new();
    public WeatherInfo[] Weather { get; set; } = [];
}

public class MainInfo
{
    public double Temp { get; set; }
}

public class WeatherInfo
{
    public string Description { get; set; } = "";
}

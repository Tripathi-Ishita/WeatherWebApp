using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Register CORS so your frontend (Live Server) can call this API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors();

// 2️⃣ API endpoint: /weather/{city}
app.MapGet("/weather/{city}", async (string city) =>
{
    string apiKey = "e37d0158f9a3fc7a34d48dd993e33719"; // your OpenWeather API key
    string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

    using var httpClient = new HttpClient();

    try
    {
        var response = await httpClient.GetFromJsonAsync<OpenWeatherResponse>(url);

        if (response == null || response.Main == null)
            return Results.NotFound(new { error = "City not found" });

        return Results.Ok(new
        {
            city = response.Name,
            temperature = response.Main.Temp
        });
    }
    catch
    {
        return Results.Problem("Error fetching weather data");
    }
});

app.Run();


// 3️⃣ Classes to parse OpenWeather JSON
public class OpenWeatherResponse
{
    public string Name { get; set; } = string.Empty; // avoid null warning
    public MainInfo? Main { get; set; } // nullable to avoid warning

    public class MainInfo
    {
        public double Temp { get; set; }
    }
}

async function getWeather() {
    const city = document.getElementById("cityInput").value;
    try {
        const response = await fetch(`http://localhost:5069/weather/${city}`);
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        const data = await response.json();
        document.getElementById("weatherResult").innerText =
            `🌡️ Temperature in ${data.city}: ${data.temperature}°C`;
    } catch (error) {
        document.getElementById("weatherResult").innerText =
            "⚠️ Error fetching weather data.";
        console.error(error);
    }
}


async function getWeather() {
    const city = document.getElementById("cityInput").value.trim();
    if (!city) {
        alert("Please enter a city name");
        return;
    }

    try {
        const response = await fetch(`/weather/${city}`);
        if (!response.ok) {
            document.getElementById("result").innerText = "City not found!";
            return;
        }

        const data = await response.json();
        document.getElementById("result").innerHTML =
            `<h2>${data.city}</h2>
             <p>🌡 Temperature: ${data.temperature}</p>
             <p>☁ Condition: ${data.condition}</p>`;
    } catch (err) {
        document.getElementById("result").innerText = "Error fetching data.";
    }
}

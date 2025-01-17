namespace W9._1WeatherAppAPI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android)
                stack.Background = Brush.MediumPurple;
        }


        private async void OnGetWeatherBtnClicked(object sender, EventArgs e)
        {
            string url;

            if (!string.IsNullOrWhiteSpace(CityEntry.Text))
            {
                // Use the city name entered by the user
                string cityName = CityEntry.Text;
                url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}" +
                    $"&units=metric&appid=adee4d9d26685357054efd2f06359807";
            }
            else
            {
                // Use geolocation to get the current location
                var location = await Geolocation.Default.GetLocationAsync();

                if (location == null)
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                    return;
                }

                var lat = location.Latitude;
                var lon = location.Longitude;

                url = $"https://api.openweathermap.org/data/2.5/weather?lat=" +
                    $"{lat}&lon={lon}&units=metric&appid=adee4d9d26685357054efd2f06359807";
            }

            var myWeather = await WeatherProxy.GetWeather(url);

            CityLbl.Text = myWeather.name;
            TemperatureLbl.Text = myWeather.main.temp.ToString("F0") + " \u00B0C";
            ConditionsLbl.Text = myWeather.weather[0].description.ToUpper();

            string icon = $"http://openweathermap.org/img/wn/{myWeather.weather[0].icon}@2x.png";
            WeatherImg.Source = ImageSource.FromUri(new Uri(icon));
        }
    }

}

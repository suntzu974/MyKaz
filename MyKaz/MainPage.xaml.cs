using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyKaz.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409



namespace MyKaz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        DispatcherTimer sensorCollection;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task SetupDevice()
        {
            
            this.sensorCollection = new DispatcherTimer();
            this.sensorCollection.Interval = TimeSpan.FromMinutes(1);
            this.sensorCollection.Tick += this.sensorCollection_Tick;
            this.sensorCollection.Start();
        }

        private void sensorCollection_Tick(object sender, object e)
        {
            loadDataFromServer();

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.progress.IsActive = false;
            await SetupDevice();
            loadDataFromServer();
        }

        private async void loadDataFromServer()
        {
            try
            {
                this.progress.IsActive = true;
                //Create HttpClient
                HttpClient httpClient = new HttpClient();
                //Define Http Headers
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                String forecastAddress = string.Format("{0}/last",App.IP_ADDRESS);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage wcfResponse = await httpClient.GetAsync(forecastAddress);
                var responseString = await wcfResponse.Content.ReadAsStringAsync();
                //Replace current URL with your URL
                ForecastResponse data = JsonConvert.DeserializeObject<ForecastResponse>(responseString);
                Forecast forecast = data.forecast;
                if (wcfResponse.IsSuccessStatusCode)
                {
                    this.Date.Text = data.forecast.created_at.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss");
                    this.Location.Text = data.forecast.location.ToString();
                    this.Temperature.Text = data.forecast.temperature.ToString("f2")+(" °C");
                    this.Pression.Text = data.forecast.pressure.ToString("f2")+(" Hpa");
                    this.Altitude.Text = data.forecast.altitude.ToString("f2")+(" m");
                    this.Humidity.Text = data.forecast.humidity.ToString("f2")+(" %");
                    this.Light.Text = data.forecast.light.ToString("N")+(" Lux");
                    this.progress.IsActive = false;
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("GET temperature from forecast !!" + data.forecast.temperature.ToString());
                    System.Diagnostics.Debug.WriteLine("GET Response !!" + responseString.ToString());
                    System.Diagnostics.Debug.WriteLine("GET Response status code " + wcfResponse.StatusCode);
#endif
                }
            }

            catch (Exception ex)
            {
                //....
            }

        }

        private void refreshData(object sender, RoutedEventArgs e)
        {
            loadDataFromServer();
        }
    }
}

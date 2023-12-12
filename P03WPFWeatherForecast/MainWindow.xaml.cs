using P03WPFWeatherForecast.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P03WPFWeatherForecast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtCity.Text = "Kraków\r\nPoznań\r\nWarszawa\r\nWrocław\r\nBerlin\r\nParyż";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hej");
        }

        private void btnGetTemperature_Click(object sender, RoutedEventArgs e)
        {
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
           
            foreach (var city in cities)
            {
                int temp = wfs.GetTemperature(city);
                tbTemperature.Text += $"Temperature in {city} is currently {temp} C" + Environment.NewLine;
            }   
        }

        // Scenariusz 1: wywołanie asynchroniczne: jedno miasto po drugim 
        private async void btnGetTemperatureAsync1_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            foreach (var city in cities) 
            {
                lvLogger.Items.Add($"Currently processing: {city}");
                var t = await Task.Run<int>(() => // to co jest w ciele bedzie wykonane asynchronicznie 
                {
                    int temp = wfs.GetTemperature(city);
                    return temp;
                });   
                tbTemperature.Text += $"Temperature in {city} is currently {t} C" + Environment.NewLine;
            }
        }

        // Scenariusz 2: wywolanie asynchroniczne ale czekamy az wszystkie zadania sie wykonaja
        // niestety na razie nie mamy dostepu do miasta 
        private async void btnGetTemperatureAsync2_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            List<Task<int>> tasks = new List<Task<int>>();
            foreach (var city in cities)
            {
                var t = Task.Run<int>(() => // to co jest w ciele bedzie wykonane asynchronicznie 
                {
                    int temp = wfs.GetTemperature(city);
                    return temp;
                });
                tasks.Add(t);
            }

            // koniec pertli. wszystkie zadania zostaly dodane do listy
            // i w tle sie spokojnie wykonuja 

            lvLogger.Items.Add("Stared processing all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add("Finished processing all cities");

            foreach (var task in tasks)
            {
                int temp = task.Result;
                tbTemperature.Text += $"Temperature in ... is currently {temp} C" + Environment.NewLine;
            }

        }

        // Scenariusz 2: wywolanie asynchroniczne ale czekamy az wszystkie zadania sie wykonaja
        // tym razem rozszerzyymy taska tak aby przechowywał dowolne dane  
        private async void btnGetTemperatureAsync3_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            //List<Task<ExtendedResult>> tasks = new List<ExtendedResult>();

            //List<Task> tasks2 = new List<Task>(); // rozne sposoby deklarowania 
            var tasks = new List<Task>();
            //List<Task> task3 = new();
          
            foreach (var city in cities)
            {
                var t = Task.Run(() => // to co jest w ciele bedzie wykonane asynchronicznie 
                {
                    int temp = wfs.GetTemperature(city);
                    return (temp,city);
                });
                tasks.Add(t);
            }

            // koniec pertli. wszystkie zadania zostaly dodane do listy
            // i w tle sie spokojnie wykonuja 

            lvLogger.Items.Add("Stared processing all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add("Finished processing all cities");

            foreach (Task<(int Temperature, string City)> task in tasks)
            {
                int temp = task.Result.Temperature;
                string cityName = task.Result.City;
                tbTemperature.Text += $"Temperature in {cityName} is currently {temp} C" + Environment.NewLine;
            }
        }


        // Scenariusz 4: wywolanie asynchroniczne ale nie czekamy na wszsytkich 
        // tyko wypisz wynik od razu gdy gotowy 
        private async void btnGetTemperatureAsync4_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            foreach (var city in cities)
            {
                var t = Task.Run(() => // to co jest w ciele bedzie wykonane asynchronicznie 
                {
                    int temp = wfs.GetTemperature(city);
                    return (temp, city);
                });

                lvLogger.Items.Add($"Stared processing city {city}");

                t.GetAwaiter().OnCompleted(() =>
                { // tutaj definije dowolny kod , ktory wykona sie gdy dane zadanie zostanie zakonczone 
                   tbTemperature.Text += $"Temperature in {city} is currently {t.Result.temp} C" + Environment.NewLine;
                   // tbTemperature.Text += $"Temperature in {t.Result.city} is currently {t.Result.temp} C" + Environment.NewLine;
                });
            }
        }

        // Scenariusz 1 ale z progress bar 
        private async void btnGetTemperatureAsync5_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            pbProgresss.Maximum = cities.Length;
            pbProgresss.Value = 0;

            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently processing: {city}");
                var t = await Task.Run<int>(() => // to co jest w ciele bedzie wykonane asynchronicznie 
                {
                    int temp = wfs.GetTemperature(city);
                    return temp;
                });
                pbProgresss.Value += 1;
                tbTemperature.Text += $"Temperature in {city} is currently {t} C" + Environment.NewLine;
            }
        }

        // Scenarisuz 6 : eleganckie podjescie, w ktorym zywamy gotowej metody asynchronicznej 
        private async void btnGetTemperatureAsync6_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            pbProgresss.Maximum = cities.Length;
            pbProgresss.Value = 0;
            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently processing: {city}");
                int temp = await wfs.GetTemperatureAsync(city);
                pbProgresss.Value += 1;
                tbTemperature.Text += $"Temperature in {city} is currently {temp} C" + Environment.NewLine;
            }
        }
    }
}
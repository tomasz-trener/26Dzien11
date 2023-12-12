using P04WeatherForecastWPF.Client.Models;
using P04WeatherForecastWPF.Client.Services;
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

namespace P04WeatherForecastWPF.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccuWeatherService accuWeatherService;
        public MainWindow()
        {
            InitializeComponent();
            accuWeatherService = new AccuWeatherService();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            City[] cities = await accuWeatherService.GetLocations(txtCity.Text);

            // standardowy sposob dodawania elementów 
            //foreach (var c in cities)
            //    lbData.Items.Add(c.LocalizedName);

            lbData.ItemsSource = cities;


        }
    }
}
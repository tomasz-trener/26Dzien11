﻿using P03WPFWeatherForecast.Services;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hej");
        }

        private void btnGetTemperature_Click(object sender, RoutedEventArgs e)
        {
            WeatherForecastService wfs = new WeatherForecastService();
            string city = txtCity.Text;
            int temp = wfs.GetTemperature(city);

            tbTemperature.Text += $"Temperature in {city} is currently {temp} C" + Environment.NewLine;
        }
    }
}
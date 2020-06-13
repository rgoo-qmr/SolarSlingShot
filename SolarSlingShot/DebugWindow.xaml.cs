using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SolarSlingShot
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {

        private SolarSlingShot.MainWindow objMainWindow;
        public DebugWindow()
        {
            InitializeComponent();
        }

        public DebugWindow(SolarSlingShot.MainWindow pvMainWindow)
        {

            InitializeComponent();
            objMainWindow = pvMainWindow;

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnReset_Click(sender, e);
        }

        private void btnPlanetOrbit_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnPlanetOrbit_Click(sender, e);
        }

        private void btnSatelliteOrbit_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnSatelliteOrbit_Click(sender, e);
        }

        private void btnSatelliteTravel_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnSatelliteTravel_Click(sender, e);
        }

        private void btnTracePlanet_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnTracePlanet_Click(sender, e);
        }

        private void btnTraceSatOrb_Click(object sender, RoutedEventArgs e)
        {
            objMainWindow.btnTraceSatOrb_Click(sender, e);
        }

        private void btnTraceSatTrav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStep_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbxStep.IsChecked == true)
            {
                TravellerRoutedEventArgs tRe = new TravellerRoutedEventArgs("true");


                objMainWindow.StepClicked(sender, tRe);
            }
            else
            {
                TravellerRoutedEventArgs tRe = new TravellerRoutedEventArgs("false");


                objMainWindow.StepClicked(sender, tRe);
            }
           

        }
    }
}

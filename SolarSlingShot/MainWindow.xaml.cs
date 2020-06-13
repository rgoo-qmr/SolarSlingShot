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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Animation;
using System.Threading;

namespace SolarSlingShot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public const int SCREEN_W = 5024;
        public const int SCREEN_H = 5024;

        public const double Pi = 3.14159;
        public static double NINETY_DEGREES_AS_RADIANS = 90;

        public bool bKeepGoing = true;
        public bool bRunning = false;

        public Sun objSun;
        public Planet objSunPlanet;
        public PlanetUI objSunUI;

        public Planet objPlanet;
        public PlanetUI objPlanetUI;

        public Planet objPlanet2;
        public PlanetUI objPlanetUI2;

        public Planet objPlanet3;
        public PlanetUI objPlanetUI3;

        public Planet objPlanet4;
        public PlanetUI objPlanetUI4;

        public Planet objPlanet5;
        public PlanetUI objPlanetUI5;

        public Planet objPlanetMoon;
        public PlanetUI objPlanetMoonUI;

        public Satellite objSatellite;
        public SatelliteUI objSatelliteUI;

        public delegate void UpdateCanvasDel();
        public delegate void UpdatePositionTraceDel(PlanetUI pvPlanetUI);

        private ViewObserver objViewObserver;
        private TravellerRunTime objTravellerRunTime;
        private List<Planet> lstPlanets;
        private List<PlanetUI> lstPlanetUI;

        public bool bTracePlanets;
        public bool bTraceSatOrbit;
        public bool bTraceSatTravel;

        public bool bStep;

        public bool bDebug;

        public DebugWindow objDebugWindow;

        // constructor loads the objects in the system
        public MainWindow()
        {
            InitializeComponent();


            // initialise the model
            lstPlanets = new List<Planet>();
            lstPlanetUI = new List<PlanetUI>();


            NINETY_DEGREES_AS_RADIANS = 90 * (Pi / 180);
            // sun
            {
                PositionParams objSunPosition = new PositionParams();

                objSunPosition.PosX = 550;
                objSunPosition.PosY = 500;
                objSunPosition.Angle = 100;
                objSunPosition.Velocity = 0;


                objSun = new Sun(objSunPosition);

                objSunPlanet = new Planet(objSun, objSunPosition, "Sun", 40);

                Ellipse objSunEllipse = new Ellipse();
                objSunEllipse.Stroke = System.Windows.Media.Brushes.Orange;
                objSunEllipse.Fill = System.Windows.Media.Brushes.Yellow;
                objSunEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                objSunEllipse.VerticalAlignment = VerticalAlignment.Center;
                objSunEllipse.Width = 30;
                objSunEllipse.Height = 30;
                objSunUI = new PlanetUI(objSunPlanet, objSunEllipse);

                Canvas.SetLeft(objSunUI.Ellipse, objSunPosition.PosX);
                Canvas.SetTop(objSunUI.Ellipse, objSunPosition.PosY);

                mainCanvas.Children.Add(objSunUI.Ellipse);
            }

            // mars
            {
                PositionParams objPlanetParams = new PositionParams();
                objPlanetParams.PosX = 250;
                objPlanetParams.PosY = 250;
                objPlanetParams.Angle = 100;
                // was 0.003
                objPlanetParams.Velocity = 0.005;
                // was 400
                objPlanetParams.Radius = 200;

                double dblMarsPullRadius = 20;
                objPlanet = new Planet(objSun, objPlanetParams, "Mars", dblMarsPullRadius);
                lstPlanets.Add(objPlanet);

                Ellipse objPlanetEllipse = new Ellipse();
                objPlanetEllipse.Stroke = System.Windows.Media.Brushes.Red;
                objPlanetEllipse.Fill = System.Windows.Media.Brushes.DarkBlue;
                objPlanetEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                objPlanetEllipse.VerticalAlignment = VerticalAlignment.Center;
                objPlanetEllipse.Width = 10;
                objPlanetEllipse.Height = 10;
                objPlanetUI = new PlanetUI(objPlanet, objPlanetEllipse);
                lstPlanetUI.Add(objPlanetUI);

                Canvas.SetLeft(objPlanetUI.Ellipse, objPlanetParams.PosX);
                Canvas.SetTop(objPlanetUI.Ellipse, objPlanetParams.PosY);

                mainCanvas.Children.Add(objPlanetUI.Ellipse);

                // moon around mars
                //{
                //    PositionParams objMoonParams = new PositionParams();
                //    objMoonParams.PosX = 50;
                //    objMoonParams.PosY = 50;
                //    objMoonParams.Angle = 100;
                //    // was 0.003
                //    objMoonParams.Velocity = 0.070;
                //    // was 400
                //    objMoonParams.Radius = 50;

                //    double dblMoonPullRadius = 10;
                //    objPlanetMoon = new Planet(objPlanet, objMoonParams, "MarsMoon", dblMoonPullRadius);
                //    lstPlanets.Add(objPlanetMoon);

                //    Ellipse objMoonEllipse = new Ellipse();
                //    objMoonEllipse.Stroke = System.Windows.Media.Brushes.DarkOrange;
                //    objMoonEllipse.Fill = System.Windows.Media.Brushes.DarkOrange;
                //    objMoonEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                //    objMoonEllipse.VerticalAlignment = VerticalAlignment.Center;
                //    objMoonEllipse.Width = 5;
                //    objMoonEllipse.Height = 5;
                //    objPlanetMoonUI = new PlanetUI(objPlanetMoon, objMoonEllipse);
                //    lstPlanetUI.Add(objPlanetMoonUI);

                //    Canvas.SetLeft(objPlanetMoonUI.Ellipse, objMoonParams.PosX);
                //    Canvas.SetTop(objPlanetMoonUI.Ellipse, objMoonParams.PosY);

                //    mainCanvas.Children.Add(objPlanetMoonUI.Ellipse);


                //}
            }



            // earth
            {
                PositionParams objPlanet2Params = new PositionParams();
                objPlanet2Params.PosX = 150;
                objPlanet2Params.PosY = 150;
                objPlanet2Params.Angle = 100;
                objPlanet2Params.Velocity = 0.01;
                objPlanet2Params.Radius = 150;


                double dblEarthPullRadius = 15;
                objPlanet2 = new Planet(objSun, objPlanet2Params, "Earth", dblEarthPullRadius);
                lstPlanets.Add(objPlanet2);

                Ellipse objPlanet2Ellipse = new Ellipse();
                objPlanet2Ellipse.Stroke = System.Windows.Media.Brushes.Blue;
                objPlanet2Ellipse.Fill = System.Windows.Media.Brushes.LightBlue;
                objPlanet2Ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                objPlanet2Ellipse.VerticalAlignment = VerticalAlignment.Center;
                objPlanet2Ellipse.Width = 10;
                objPlanet2Ellipse.Height = 10;
                objPlanetUI2 = new PlanetUI(objPlanet2, objPlanet2Ellipse);
                lstPlanetUI.Add(objPlanetUI2);

                Canvas.SetLeft(objPlanetUI2.Ellipse, objPlanet2Params.PosX);
                Canvas.SetTop(objPlanetUI2.Ellipse, objPlanet2Params.PosY);

                mainCanvas.Children.Add(objPlanetUI2.Ellipse);
            }

            // venus
            {
                PositionParams objPlanet3Params = new PositionParams();
                objPlanet3Params.PosX = 160;
                objPlanet3Params.PosY = 160;
                objPlanet3Params.Angle = 100;
                objPlanet3Params.Velocity = 0.01;
                objPlanet3Params.Radius = 50;

                double dblVenusPullRadius = 15;
                objPlanet3 = new Planet(objSun, objPlanet3Params, "Venus", dblVenusPullRadius);
                lstPlanets.Add(objPlanet3);

                Ellipse objPlanet3Ellipse = new Ellipse();
                objPlanet3Ellipse.Stroke = System.Windows.Media.Brushes.Gold;
                objPlanet3Ellipse.Fill = System.Windows.Media.Brushes.Goldenrod;
                objPlanet3Ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                objPlanet3Ellipse.VerticalAlignment = VerticalAlignment.Center;
                objPlanet3Ellipse.Width = 10;
                objPlanet3Ellipse.Height = 10;
                objPlanetUI3 = new PlanetUI(objPlanet3, objPlanet3Ellipse);
                lstPlanetUI.Add(objPlanetUI3);

                Canvas.SetLeft(objPlanetUI3.Ellipse, objPlanet3Params.PosX);
                Canvas.SetTop(objPlanetUI3.Ellipse, objPlanet3Params.PosY);

                mainCanvas.Children.Add(objPlanetUI3.Ellipse);
            }

            // Jupiter
            {
                PositionParams objPlanet4Params = new PositionParams();
                objPlanet4Params.PosX = 165;
                objPlanet4Params.PosY = 165;
                objPlanet4Params.Angle = 100;
                objPlanet4Params.Velocity = 0.009;
                objPlanet4Params.Radius = 420;

                double dblSaturnPullRadius = 30;
                objPlanet4 = new Planet(objSun, objPlanet4Params, "Jupiter", dblSaturnPullRadius);
                lstPlanets.Add(objPlanet4);

                Ellipse objPlanet4Ellipse = new Ellipse();
                objPlanet4Ellipse.Stroke = System.Windows.Media.Brushes.Blue;
                objPlanet4Ellipse.Fill = System.Windows.Media.Brushes.DarkBlue;
                objPlanet4Ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                objPlanet4Ellipse.VerticalAlignment = VerticalAlignment.Center;
                objPlanet4Ellipse.Width = 12;
                objPlanet4Ellipse.Height = 12;
                objPlanetUI4 = new PlanetUI(objPlanet4, objPlanet4Ellipse);
                lstPlanetUI.Add(objPlanetUI4);

                Canvas.SetLeft(objPlanetUI4.Ellipse, objPlanet4Params.PosX);
                Canvas.SetTop(objPlanetUI4.Ellipse, objPlanet4Params.PosY);

                mainCanvas.Children.Add(objPlanetUI4.Ellipse);
            }

            // saturn
            {
                PositionParams objPlanet5Params = new PositionParams();
                objPlanet5Params.PosX = 170;
                objPlanet5Params.PosY = 170;
                objPlanet5Params.Angle = 100;
                objPlanet5Params.Velocity = 0.008;
                objPlanet5Params.Radius = 450;

                double dblPlanet5PullRadius = 40;
                objPlanet5 = new Planet(objSun, objPlanet5Params, "Saturn", dblPlanet5PullRadius);
                lstPlanets.Add(objPlanet5);

                Ellipse objPlanet5Ellipse = new Ellipse();
                objPlanet5Ellipse.Stroke = System.Windows.Media.Brushes.Green;
                objPlanet5Ellipse.Fill = System.Windows.Media.Brushes.DarkGreen;
                objPlanet5Ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                objPlanet5Ellipse.VerticalAlignment = VerticalAlignment.Center;
                objPlanet5Ellipse.Width = 15;
                objPlanet5Ellipse.Height = 15;
                objPlanetUI5 = new PlanetUI(objPlanet5, objPlanet5Ellipse);
                lstPlanetUI.Add(objPlanetUI5);

                Canvas.SetLeft(objPlanetUI5.Ellipse, objPlanet5Params.PosX);
                Canvas.SetTop(objPlanetUI5.Ellipse, objPlanet5Params.PosY);

                mainCanvas.Children.Add(objPlanetUI5.Ellipse);
            }

            // satellite
            {
                PositionParams objSatelliteParams = new PositionParams();
                objSatelliteParams.PosX = 0;
                objSatelliteParams.PosY = 0;
                objSatelliteParams.Angle = 100;
                // 20/11/16 original test params
                objSatelliteParams.Velocity = 0.08;
                //objSatelliteParams.Radius = 20;
                //objSatelliteParams.Velocity = 0.2;
                // was 40
                objSatelliteParams.Radius = 20;

                // start round earth
                objSatellite = new Satellite(objPlanet2, objSatelliteParams, false, lstPlanets);

                Ellipse objSatelliteEllipse = new Ellipse();
                objSatelliteEllipse.Stroke = System.Windows.Media.Brushes.Black;
                objSatelliteEllipse.Fill = System.Windows.Media.Brushes.Red;
                objSatelliteEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                objSatelliteEllipse.VerticalAlignment = VerticalAlignment.Center;
                objSatelliteEllipse.Width = 10;
                objSatelliteEllipse.Height = 10;
                objSatelliteUI = new SatelliteUI(objSatellite, objSatelliteEllipse);

                Canvas.SetLeft(objSatelliteUI.Ellipse, objSatelliteParams.PosX);
                Canvas.SetTop(objSatelliteUI.Ellipse, objSatelliteParams.PosY);

                mainCanvas.Children.Add(objSatelliteUI.Ellipse);
            }







            // initialise view observer
            objViewObserver = new ViewObserver(this, lstPlanetUI, objSatelliteUI);

            // initialise runitme controller
            objTravellerRunTime = new TravellerRunTime(lstPlanets, objSatellite, objSun, objViewObserver);

            // set true to use debugging window
            bDebug = false;
            if (bDebug)
            {
                bTracePlanets = false;
                bTraceSatOrbit = false;
                bTraceSatTravel = false;
                bStep = false;
                objDebugWindow = new DebugWindow(this);
                objDebugWindow.Show();
            }

            // start the main thread running
            objTravellerRunTime.StartRunning();

        }

        //  // a test move function
        // NB. May really want more direct control than using this animation approach
        public void MoveTo(UIElement target, double newX, double newY)
        {

            // ? need some sort of dependency property to return the X, Y values for the UI object?
            double top = Canvas.GetTop(target);
            double left = Canvas.GetLeft(target);
            TranslateTransform trans = new TranslateTransform(newX, newY);
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(1));
            DoubleAnimation anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
            Canvas.SetTop(target, newY);
            Canvas.SetLeft(target, newX);


        }

        void repeatTransform(UIElement planet, double left, double top)
        {
            for (int i = 0; i < 20; i++)
            {

                //putpixel(mainCanvas, left, top, planet);
                MoveTo(planet, left, top);
                left = left + i;
                top = top + i;
                //System.Threading.Thread.Sleep(1000);
            }

        }

        void UpdateOrbitPosition(PlanetUI pvPlanet)
        {

            if (bKeepGoing)
            {
                pvPlanet.Planet.updateOrbitPosition();
                //MoveTo(pvPlanet.Ellipse, pvPlanet.Planet.Position.PosX, pvPlanet.Planet.Position.PosX);
                //putCanvas();
            }


        }

        void UpdateOrbitPosition(SatelliteUI pvSatellite)
        {

            if (bKeepGoing)
            {
                pvSatellite.Satellite.updateOrbitPosition();
                //MoveTo(pvPlanet.Ellipse, pvPlanet.Planet.Position.PosX, pvPlanet.Planet.Position.PosX);
                //putCanvas();
            }


        }

        void UpdateInterPlanetaryPosition(SatelliteUI pvSatellite)
        {
            if (bKeepGoing)
            {
                pvSatellite.Satellite.UpdateInterPlanetaryPosition();
                //MoveTo(pvPlanet.Ellipse, pvPlanet.Planet.Position.PosX, pvPlanet.Planet.Position.PosX);
                //putCanvas();
            }
        }



        //void orbit ()
        //{
        //    int x = 0, y = 0;

        //    fixed angle = itofix (0);
        //    fixed angle_stepsize = itofix (1);

        //    // These determine the radius of the orbit.
        //    // See what happens if you change length_x to 100 :)
        //    int length_x = 50;
        //    int length_y = 50;

        //    // repeat this until a key is pressed
        //    while (!keypressed())
        //    {
        //        // erase the point from the old position
        //        //putpixel (screen,
        //        //    fixtoi(x) + SCREEN_W / 2, fixtoi(y) + SCREEN_H / 2,
        //        //    makecol (0, 0, 0));

        //        // calculate the new position
        //        x = length_x * fcos (angle);
        //        y = length_y * fsin (angle);

        //        // draw the point in the new position
        //        putpixel (screen,
        //            fixtoi(x) + SCREEN_W / 2, fixtoi(y) + SCREEN_H / 2,
        //            makecol (255, 255, 255));

        //        // increment the angle so that the point moves around in circles
        //        angle += angle_stepsize;

        //        // make sure angle is in range
        //        angle &= 0xFFFFFF;

        //        // wait 10 milliseconds, or else it'd go too fast
        //        rest (10);
        //    }
        //}

        bool keypressed()
        {
            return true;
        }

        //// remove the referenced object and put it back at a new position
        //void putpixel(Canvas pvCanvas, double left, double top, UIElement pvObject)
        //  {

        //  pvCanvas.Children.Remove(pvObject);
        //  Canvas.SetLeft(pvObject,left);
        //  Canvas.SetTop(pvObject, top);
        //  pvCanvas.Children.Add(pvObject);


        //  }

        void putpixelLayoutTransform(Canvas pvCanvas, double left, double top, UIElement pvObject)
        {

            pvCanvas.Children.Remove(pvObject);
            Canvas.SetLeft(pvObject, left);
            Canvas.SetTop(pvObject, top);
            pvCanvas.Children.Add(pvObject);


        }


        //canvas.Dispatcher.Invoke(emptyDelegate, DispatcherPriority.Render); where emptyDelegate is Action emptyDelegate = delegate { };
        void putCanvas()
        {

            mainCanvas.Children.Remove(objPlanetUI.Ellipse);
            Canvas.SetLeft(objPlanetUI.Ellipse, objPlanetUI.Planet.Position.PosX);
            Canvas.SetTop(objPlanetUI.Ellipse, objPlanetUI.Planet.Position.PosY);
            mainCanvas.Children.Add(objPlanetUI.Ellipse);

            mainCanvas.Children.Remove(objPlanetUI2.Ellipse);
            Canvas.SetLeft(objPlanetUI2.Ellipse, objPlanetUI2.Planet.Position.PosX);
            Canvas.SetTop(objPlanetUI2.Ellipse, objPlanetUI2.Planet.Position.PosY);
            mainCanvas.Children.Add(objPlanetUI2.Ellipse);


            mainCanvas.Children.Remove(objSatelliteUI.Ellipse);
            Canvas.SetLeft(objSatelliteUI.Ellipse, objSatelliteUI.Satellite.Position.PosX);
            Canvas.SetTop(objSatelliteUI.Ellipse, objSatelliteUI.Satellite.Position.PosY);
            mainCanvas.Children.Add(objSatelliteUI.Ellipse);

            this.lblSatAngle.Content = objSatelliteUI.Satellite.InterPlanetaryAngle;
            this.lblPlanetAngle.Content = objPlanetUI.Planet.Position.Angle;
            this.lblIncrement.Content = objSatelliteUI.Satellite.InterPlanetaryIncrement;

            //Action emptyDelegate = delegate { };
            //mainCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, emptyDelegate);

        }



        void ellipseDemo()
        {

            Ellipse myEllipse = new Ellipse();
            myEllipse.Stroke = System.Windows.Media.Brushes.Black;
            //myEllipse.Fill = System.Windows.Media.Brushes.DarkBlue;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 150;
            myEllipse.Height = 250;

            Canvas.SetLeft(myEllipse, 50);
            Canvas.SetTop(myEllipse, 50);
            mainCanvas.Children.Add(myEllipse);




            Ellipse planet = new Ellipse();

            planet.Stroke = System.Windows.Media.Brushes.Red;
            planet.Fill = System.Windows.Media.Brushes.DarkBlue;
            planet.HorizontalAlignment = HorizontalAlignment.Left;
            planet.VerticalAlignment = VerticalAlignment.Center;
            planet.Width = 10;
            planet.Height = 10;
            Canvas.SetLeft(planet, 10);
            Canvas.SetTop(planet, 10);

            mainCanvas.Children.Add(planet);



            double left = 5;
            double top = 5;
            //bool keepGoing = true;
            //while (keepGoing)
            //  {
            //for (int i = 0; i < 20; i++)
            //  {

            //putpixel(mainCanvas, left, top, planet);
            //MoveTo(planet, left, top);
            //left = left + i;
            //top = top + i;
            //System.Threading.Thread.Sleep(1000);
            //}
            repeatTransform(planet, left, top);

            Canvas.SetLeft(planet, 10);
            Canvas.SetTop(planet, 10);
            //}



            //mainCanvas.Children.Add(planet);

            //SpacemanUI traveller = new SpacemanUI(10, 10);

            //MoveTo(traveller,100,100);

        }

        private void launch1_Click(object sender, RoutedEventArgs e)
        {
            bRunning = true;
            objTravellerRunTime.Running = true;
            objTravellerRunTime.KeepGoing = true;

            if (objSatellite.InOrbit == true)
            {
                objSatellite.InOrbit = false;
                objSatellite.IntoOrbit = false;
            }
            else
            {
                objSatellite.InOrbit = true;
                objSatellite.IntoOrbit = true;
            }
        }




        public void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }


        void trackOnCanvas()
        {

            if (this.bTracePlanets == true)
            {
                Ellipse newEllipse = new Ellipse();
                newEllipse.Stroke = System.Windows.Media.Brushes.Green;
                newEllipse.Fill = System.Windows.Media.Brushes.Green;
                newEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                newEllipse.VerticalAlignment = VerticalAlignment.Center;
                newEllipse.Width = 4;
                newEllipse.Height = 4;
                Canvas.SetLeft(newEllipse, objPlanetUI.Planet.Position.PosX);
                Canvas.SetTop(newEllipse, objPlanetUI.Planet.Position.PosY);
                mainCanvas.Children.Add(newEllipse);


                Ellipse newEllipse2 = new Ellipse();
                newEllipse2.Stroke = System.Windows.Media.Brushes.Purple;
                newEllipse2.Fill = System.Windows.Media.Brushes.Purple;
                newEllipse2.HorizontalAlignment = HorizontalAlignment.Left;
                newEllipse2.VerticalAlignment = VerticalAlignment.Center;
                newEllipse2.Width = 4;
                newEllipse2.Height = 4;
                Canvas.SetLeft(newEllipse2, objPlanetUI2.Planet.Position.PosX);
                Canvas.SetTop(newEllipse2, objPlanetUI2.Planet.Position.PosY);
                mainCanvas.Children.Add(newEllipse2);
            }

            if ((this.bTraceSatOrbit == true & objSatelliteUI.Satellite.InOrbit == true)
            | (this.bTraceSatTravel == true & objSatelliteUI.Satellite.InOrbit == false))
            {
                Ellipse newEllipse3 = new Ellipse();
                newEllipse3.Stroke = System.Windows.Media.Brushes.Gray;
                newEllipse3.Fill = System.Windows.Media.Brushes.Gray;
                newEllipse3.HorizontalAlignment = HorizontalAlignment.Left;
                newEllipse3.VerticalAlignment = VerticalAlignment.Center;
                newEllipse3.Width = 4;
                newEllipse3.Height = 4;
                Canvas.SetLeft(newEllipse3, objSatelliteUI.Satellite.Position.PosX);
                Canvas.SetTop(newEllipse3, objSatelliteUI.Satellite.Position.PosY);
                mainCanvas.Children.Add(newEllipse3);
            }



            //Action emptyDelegate = delegate { };
            //mainCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, emptyDelegate);

        }

        public void btnPlanetOrbit_Click(object sender, RoutedEventArgs e)
        {
            if (objTravellerRunTime.UpdatePlanetOrbitPosition == true)
            {
                objTravellerRunTime.UpdatePlanetOrbitPosition = false;

            }
            else
            {
                objTravellerRunTime.UpdatePlanetOrbitPosition = true;
            }

        }

        public void btnSatelliteOrbit_Click(object sender, RoutedEventArgs e)
        {
            if (objTravellerRunTime.UpdateSatelliteOrbitPosition == true)
            {
                objTravellerRunTime.UpdateSatelliteOrbitPosition = false;

            }
            else
            {
                objTravellerRunTime.UpdateSatelliteOrbitPosition = true;
            }
        }

        public void btnSatelliteTravel_Click(object sender, RoutedEventArgs e)
        {
            if (objTravellerRunTime.UpdateSatelliteInterPlanetaryPosition == true)
            {
                objTravellerRunTime.UpdateSatelliteInterPlanetaryPosition = false;

            }
            else
            {
                objTravellerRunTime.UpdateSatelliteInterPlanetaryPosition = true;
            }
        }

        public void btnTracePlanet_Click(object sender, RoutedEventArgs e)
        {

            if (bTracePlanets == true)
            {
                bTracePlanets = false;
            }
            else
            {
                bTracePlanets = true;
            }
        }

        public void btnTraceSatOrb_Click(object sender, RoutedEventArgs e)
        {
            if (bTraceSatOrbit == true)
            {
                bTraceSatOrbit = false;
            }
            else
            {
                bTraceSatOrbit = true;
            }
        }

        public void btnTraceSatTrav_Click(object sender, RoutedEventArgs e)
        {
            if (this.bTraceSatTravel == true)
            {
                bTraceSatTravel = false;
            }
            else
            {
                bTraceSatTravel = true;
            }
        }

        public void btnStep_Click(object sender, RoutedEventArgs e)
        {
            /*if (this.cbxStep.IsChecked == true)
            {
              objTravellerRunTime.bUseStep = true;*/
            if (bStep == true)
            {
                objTravellerRunTime.bUseStep = false;
                bStep = false;

            }
            else
            {
                objTravellerRunTime.bUseStep = true;
                bStep = true;
            }

            /*}
            else
            {
              objTravellerRunTime.bUseStep = false;
              bStep = true;
            } */
            objTravellerRunTime.bStep = bStep;
        }



        public void StepClicked(object sender, RoutedEventArgs e)
        {
            if (e.GetType() == typeof(TravellerRoutedEventArgs))
            {

                bool bUseStep = ((TravellerRoutedEventArgs)(Object)e).TravellerEventArgs == "true";
                objTravellerRunTime.bUseStep = bUseStep;
                if (bUseStep)
                {

                    if (bStep == true)
                    {
                        bStep = false;

                    }
                    else
                    {
                        bStep = true;
                    }

                }
                else
                {
                    bStep = false;
                }
                objTravellerRunTime.bStep = bStep;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.Shutdown();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        // detect left mouse press on the canvas and launch the satellite
        // ? any threading issues with this event and travellerRunTime thread
        private void cnv_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                bRunning = true;
                objTravellerRunTime.Running = true;
                objTravellerRunTime.KeepGoing = true;

                if (objSatellite.InOrbit == true)
                {
                    objSatellite.InOrbit = false;
                    objSatellite.IntoOrbit = false;

                    // TODO: trigger last calculation of the distance travelled in orbit
                }
                else
                {
                    objSatellite.InOrbit = true;
                    objSatellite.IntoOrbit = true;
                }


            }
        }
    }

    public class TravellerRoutedEventArgs : RoutedEventArgs
    {
        private string strTravellerEventArgs;

        public TravellerRoutedEventArgs(string pvEventArgs)
        {
            strTravellerEventArgs = pvEventArgs;
        }

        public string TravellerEventArgs
        {
            get => strTravellerEventArgs;
            set => strTravellerEventArgs = value;


        }

    }
}

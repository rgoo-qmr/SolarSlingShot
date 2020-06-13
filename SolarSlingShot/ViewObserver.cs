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
    class ViewObserver
    {

        private Canvas mainCanvas;
        private List<PlanetUI> lstPlanetUI;
        private SatelliteUI objSatelliteUI;
        private SolarSlingShot.MainWindow objMainWindow;

        public delegate void UpdateCanvasDel();
        public delegate void UpdatePositionTraceDel(PlanetUI pvPlanetUI);
        public delegate void JourneyCompleteDel(Satellite pvSatellite, int pvTimeToComplete);
        public delegate void FlashSatelitteDel();

        private bool bFlashSattelite;

        public ViewObserver(SolarSlingShot.MainWindow pvMainWindow, List<PlanetUI> pvLstPlanetUI, SatelliteUI pvSatelliteUI)
        {
            mainCanvas = pvMainWindow.mainCanvas;
            lstPlanetUI = pvLstPlanetUI;
            objSatelliteUI = pvSatelliteUI;
            objMainWindow = pvMainWindow;
            bFlashSattelite = false;
        }


        void putCanvas()
        {
            foreach (PlanetUI objPlanetUI in lstPlanetUI)
            {
                mainCanvas.Children.Remove(objPlanetUI.Ellipse);
                Canvas.SetLeft(objPlanetUI.Ellipse, objPlanetUI.Planet.Position.PosX);
                Canvas.SetTop(objPlanetUI.Ellipse, objPlanetUI.Planet.Position.PosY);
                mainCanvas.Children.Add(objPlanetUI.Ellipse);
            }


            mainCanvas.Children.Remove(objSatelliteUI.Ellipse);
            Canvas.SetLeft(objSatelliteUI.Ellipse, objSatelliteUI.Satellite.Position.PosX);
            Canvas.SetTop(objSatelliteUI.Ellipse, objSatelliteUI.Satellite.Position.PosY);
            mainCanvas.Children.Add(objSatelliteUI.Ellipse);

            objMainWindow.lblSatAngle.Content = objSatelliteUI.Satellite.InterPlanetaryAngle;
            objMainWindow.lblPlanetAngle.Content = lstPlanetUI[0].Planet.Position.Angle;
            objMainWindow.lblSatIncrement.Content = objSatelliteUI.Satellite.InterPlanetaryIncrement;
            objMainWindow.lblSatOrbitAngle.Content = objSatelliteUI.Satellite.AroundPlanet;

        }


        private void UpdatePositionTrace(PlanetUI pvPlanet)
        {
            // TODO: move to debug window if used
            //objMainWindow.tbxPlanetX.Text = pvPlanet.Planet.Position.PosX.ToString();
            //objMainWindow.tbxPlanetY.Text = pvPlanet.Planet.Position.PosY.ToString();
        }


        void trackOnCanvas()
        {

            foreach (PlanetUI objPlanetUI in lstPlanetUI)
            {
                if (objMainWindow.bTracePlanets == true)
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
                }
            }

            if ((objMainWindow.bTraceSatOrbit == true & objSatelliteUI.Satellite.InOrbit == true)
            | (objMainWindow.bTraceSatTravel == true & objSatelliteUI.Satellite.InOrbit == false))
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


        }

        public void Update()
        {
            UpdateFlashSatellite();

            //putCanvas();
            UpdateCanvasDel putCanvasDelegate = new UpdateCanvasDel(putCanvas);
            mainCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, putCanvasDelegate);

            UpdatePositionTraceDel positionTraceDelegate = new UpdatePositionTraceDel(UpdatePositionTrace);
            mainCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, positionTraceDelegate, lstPlanetUI[0]);



            //trackOnCanvas();
            UpdateCanvasDel putCanvasDelegate2 = new UpdateCanvasDel(trackOnCanvas);
            mainCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, putCanvasDelegate2);

        }

        public void UpdateJourneyComplete(Satellite pvSatellite, int pvTimeToComplete)
        {

            JourneyCompleteDel objJourneyCompleteDel = new JourneyCompleteDel(JourneyComplete);
            List<Satellite> lstSatellite = new List<Satellite>();

            lstSatellite.Add(pvSatellite);


            // parameters
            object[] lstParams = { pvSatellite, pvTimeToComplete };
            mainCanvas.Dispatcher.BeginInvoke(objJourneyCompleteDel, System.Windows.Threading.DispatcherPriority.Render, lstParams);

        }

        public void JourneyComplete(Satellite pvSatellite, int pvTimeToComplete)
        {


            double dblDistanceTravelled = pvSatellite.DistanceBetweenPlanets + pvSatellite.DistanceInPlanetaryOrbit;
            string strMessageComplete = "    JOURNEY COMPLETE!";
            string strMessageDistanceTravelled = "Distance Travelled: " + dblDistanceTravelled.ToString();
            string strTimeToComplete = "In " + (pvTimeToComplete / 365) + " Years " + (pvTimeToComplete % 365) + " Days";

            pvSatellite.Reset();

            MessageBox.Show(strTimeToComplete, strMessageComplete, MessageBoxButton.OK);
            



        }

        public void UpdateFlashSatellite()
        {
            if (bFlashSattelite == true)
            {
                FlashSatelitteDel objFlashSatelitteDel = new FlashSatelitteDel(FlashSatellite);
                mainCanvas.Dispatcher.Invoke(objFlashSatelitteDel, System.Windows.Threading.DispatcherPriority.Render);
                bFlashSattelite = false;
            }

        }

        public bool SetFlashSatellite
        {
            get { return bFlashSattelite; }
            set { bFlashSattelite = value; }
        }


        public void FlashSatellite()
        {
            if (objSatelliteUI.Ellipse.Fill == System.Windows.Media.Brushes.Red)
            {

                objSatelliteUI.Ellipse.Fill = System.Windows.Media.Brushes.White;
            }
            else
            {
                
                objSatelliteUI.Ellipse.Fill =  System.Windows.Media.Brushes.Red;
            }

        }


    }
  }

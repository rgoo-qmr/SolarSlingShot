using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolarSlingShot
  {
  class TravellerRunTime
    {

    private bool bKeepGoing;
    public bool KeepGoing
      {
      get { return bKeepGoing; }
      set { bKeepGoing = value; }

      }
    private bool bRunning;
    public bool Running
      {
      get { return bRunning; }
      set { bRunning = value; }
      }

      private bool bUpdatePlanetOrbitPosition;
      public bool UpdatePlanetOrbitPosition
      {
      get { return bUpdatePlanetOrbitPosition; }
      set { bUpdatePlanetOrbitPosition = value; }
      }


      private bool bUpdateSatelliteOrbitPosition;
      public bool UpdateSatelliteOrbitPosition
      {
      get { return bUpdateSatelliteOrbitPosition; }
      set { bUpdateSatelliteOrbitPosition = value; } 
      }

      private bool bUpdateSatelliteInterPlanetaryPosition;
      public bool UpdateSatelliteInterPlanetaryPosition
      {
      get { return bUpdateSatelliteInterPlanetaryPosition; }
      set { bUpdateSatelliteInterPlanetaryPosition = value; }
      } 

    private List<Planet> lstPlanets;
    private Satellite objSatellite;
    private Sun objSun;
    private ViewObserver objViewObserver;
    private Thread objThread;

    public bool bStep;
    public bool bUseStep;

        private int timeToComplete;

        private Thread objThreadFlash;


        public TravellerRunTime(List<Planet> pvListPlanets, Satellite pvSatellite, Sun pvSun, ViewObserver pvViewObserver)
      {
      bKeepGoing = true;
      bRunning = false;
      bUpdatePlanetOrbitPosition = true;
      bUpdateSatelliteOrbitPosition = true;
      bUpdateSatelliteInterPlanetaryPosition = true;

      bStep = false;
      bUseStep = false;


      lstPlanets = pvListPlanets;
      objSatellite = pvSatellite;
      objSun = pvSun;
      objViewObserver = pvViewObserver;
      objThread = new Thread(Run);
      objThread.Name = "TravellerRunTime";
      timeToComplete = 1;


            objThreadFlash = new Thread(FlashSatellite);
        }

      public void StartRunning()
      {
        objThread.Start();
            objThreadFlash.Start();
      }

  private void Run()
    {
    try
    {
    while (bKeepGoing)
      {

      if (bRunning == true & bStep == true)
      {
        timeToComplete += 1;
        // Start planets in orbit
        if (bUpdatePlanetOrbitPosition == true)
        {
          foreach (Planet  objPlanet in lstPlanets)
          {
            UpdateOrbitPosition(objPlanet);
          }
        }
      

        //
        // TESTS FOR SATELLITE MOVEMENT
        // KEEP SATELLITE IN ORBIT AROUND A PLANET
        if (objSatellite.InOrbit)
        {
          if (bUpdateSatelliteOrbitPosition == true)
          {
            UpdateOrbitPosition(objSatellite);
          }
        }
        else
        {
          //MOVE SATELLITE BETWEEN PLANETS
          if (bUpdateSatelliteInterPlanetaryPosition == true )
          {
            UpdateInterPlanetaryPosition(objSatellite);
          }

         foreach (Planet objPlanet in lstPlanets)
         {
             bool bWithinOrbitPull = false;
             bWithinOrbitPull = objPlanet.WithinOrbitPull(objSatellite);
             if (bWithinOrbitPull == true)
             {
                 objSatellite.EnterOrbit(objPlanet);
                 if(objSatellite.CompletedTrip == true)
                 {
                    objViewObserver.UpdateJourneyComplete(objSatellite, this.timeToComplete);
                 }
                 break;
             }
         }



       }




        objViewObserver.Update();
        
        }
        if (bUseStep == true)
        {

          bStep = false;
         }
        else
        {
          bStep = true;
        }
      System.Threading.Thread.Sleep(50);

      }
      }
      catch (Exception e)
      {
       var gotHere = -1;
      }



    }

  void UpdateOrbitPosition(Planet pvPlanet)
    {
      pvPlanet.updateOrbitPosition();
    }

  void UpdateOrbitPosition(Satellite pvSatellite)
    {
      pvSatellite.updateOrbitPosition();
    }


  void UpdateInterPlanetaryPosition(Satellite pvSatellite)
    {
      pvSatellite.UpdateInterPlanetaryPosition();
    }

        //public void FlashSatellite()
        //{
        //    objViewObserver.UpdateFlashSatellite();


        //}

        private void FlashSatellite()
        {
            while (true)
            {
                objViewObserver.SetFlashSatellite = true;
                System.Threading.Thread.Sleep(1000);
            }


        }

    }
}

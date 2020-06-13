using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace SolarSlingShot
{
public class Satellite
  {
  // planet satellite is currently orbiting
    private Planet objPlanet;
    private Planet objHomePlanet;
    private Planet objLastPlanet;


    private PositionParams objPosition;

    private double  interPlanetaryAngle;
    private double interPlanetaryAngleOnEntryToOrbit;
    private double orbitAngleOnEntry;
    private double angleAroundPlanet;

    private double interPlanetarySpeed;

    PositionParams objDirection;

    private bool bInOrbit;
  // satellite going into orbit
    private bool bIntoOrbit;

    // debug attributes
    private double lastInterPlanetaryAngle;
    private double interPlanetaryIncrement;

    // the calculated launch angle for interplanetary travel
    private double interPlanetaryLaunchAngle;
    private bool bUseZero;
    private double lastAngle;

        private bool bInitialised;

        // variables for caching the progess of the satellite
        private List<Planet> lstPlanetsToVisit;

        private List<Planet> lstPlanetsVisited;

        private double distanceTravelled;
        private double planetOrbitDistanceTravelled;

        // angle of the current planet on entry to it's oribot
        private double planetAngleOnEntry;
        // last angle checked for the current planet, used for determining how many revolutions of the planet orbit the satellite ahs been in it's orbit
        private double lastPlanetAngle;

        private Boolean bCompletedTrip;

        


        // constructor
        public Satellite(Planet pvPlanet, PositionParams pvPosition, bool pvInOrbit, List<Planet> pvlstPlanets)
      {
      objPlanet = pvPlanet;

       // set the first planet as the home planet
       objHomePlanet = pvPlanet;

      objLastPlanet = pvPlanet;
      objPosition = pvPosition;

      // update the interplanetary position using a vector

      interPlanetarySpeed = 5;
      interPlanetaryAngle = 0;
      objDirection = new PositionParams();

      bInOrbit = pvInOrbit;
      angleAroundPlanet = 0;

      lastInterPlanetaryAngle = 0;
      interPlanetaryIncrement = 0;

      interPlanetaryLaunchAngle = 0;
      bUseZero = true;
      lastAngle = 0;

            lstPlanetsToVisit = pvlstPlanets;

            // start with an empty list pf planets. When this list matches the list of planets in the system including the home planet the
            // satellite has visited every planet
            lstPlanetsVisited = new List<Planet>();

            distanceTravelled = 0;

            lastPlanetAngle = 0;


      bInitialised = false;
            bCompletedTrip = false;

      }

    public void updateOrbitPosition()
    {

      if (this.bIntoOrbit == true)
      {
                //// TODO: temp on entry the satellite is always at the top of the orbit
                //objPosition.PosX = objPlanet.Position.PosX;
                //objPosition.PosY = objPlanet.Position.PosY - objPosition.Radius;
                //interPlanetaryAngleOnEntryToOrbit = 0;
                //this.bIntoOrbit = false;
                //objPosition.Angle = 0;
                //interPlanetaryAngle = 0;
                //angleAroundPlanet = 0;
                if (bInitialised == false)
                {
                    InitialiseOrbit();
                    bInitialised = true;
                }
                else
                {
                    this.bIntoOrbit = false;
                    objPosition.Angle = 0;
                    interPlanetaryAngle = 0;
                    angleAroundPlanet = 0;
                }

            }
      else
      {
        //Only need angle of the satellite around the planet - SE

        angleAroundPlanet = objPosition.Angle + objPosition.Velocity;
        if (angleAroundPlanet >= 2 * MainWindow.Pi)
        {
            angleAroundPlanet = 0;
        }
        if (angleAroundPlanet == 0 & lastAngle != 0)
        {
            bUseZero = true;
        }
        else
        {
            bUseZero = false;
        }
        lastAngle = angleAroundPlanet;


        //As interPlanetaryAngle = angleAroundPlanet + 90 degrees, at its calculation it ranges from 90 to 450 degrees - SE
        interPlanetaryAngle = angleAroundPlanet + MainWindow.NINETY_DEGREES_AS_RADIANS;

        //Thus for interPlanetaryAngle>360 degrees, it must be interPlanetaryAngle=interPlanetaryAngle-360 - SE
        if (interPlanetaryAngle > 2* MainWindow.Pi)
        {
            interPlanetaryAngle = interPlanetaryAngle - 2*MainWindow.Pi;
        }

        if (interPlanetaryAngle > 0)
        {
            interPlanetaryIncrement = interPlanetaryAngle - lastInterPlanetaryAngle;
            lastInterPlanetaryAngle = interPlanetaryAngle;
        }
        else
        {
            interPlanetaryIncrement = interPlanetaryAngle - lastInterPlanetaryAngle + 2 * MainWindow.Pi;
            lastInterPlanetaryAngle = interPlanetaryAngle;
        }

        // anticlockwise
        //objPosition.PosX = objPlanet.Position.PosX + (objPosition.Radius * Math.Sin(angle));
        //objPosition.PosY = objPlanet.Position.PosY + (objPosition.Radius * Math.Cos(angle));
        // clockwise
        objPosition.PosX = objPlanet.Position.PosX + (objPosition.Radius * Math.Cos(angleAroundPlanet));
        objPosition.PosY = objPlanet.Position.PosY + (objPosition.Radius * Math.Sin(angleAroundPlanet));
        objPosition.Angle = angleAroundPlanet;
                //interPlanetaryAngle = Math.Abs(Math.Tan(objPosition.Angle));
                //double angleChange = (objPosition.Angle + objPlanet.Position.Angle) - orbitAngleOnEntry;
                //if (angleChange < 0)
                //{
                //  angleChange = 0;
                //}
                //interPlanetaryAngle = interPlanetaryAngleOnEntryToOrbit + angleChange;
                ////if (interPlanetaryAngle > 360 )
                //if (interPlanetaryAngle >= 2 * MainWindow.Pi)
                //{
                //  interPlanetaryAngle = 0;
                //}

                // update the cached travel around planets
                UpdateDistanceInOrbit(objPlanet);
      }
    }

    // calculate a launch angle to use for interplanetary travel
    private void CalculateLaunchAngle()
    {
    interPlanetaryLaunchAngle = interPlanetaryAngle;
    if (interPlanetaryAngle == 0 & bUseZero == true)
    {
      interPlanetaryLaunchAngle = interPlanetaryAngle;
    }

    if (interPlanetaryAngle == 0 & bUseZero == false)
      {
      interPlanetaryLaunchAngle = lastInterPlanetaryAngle - 2*MainWindow.Pi; //Changed from -6. SE

      }



    }

    // use calcuated launch angle
    public void UpdateInterPlanetaryPosition()
      {
      double x1 = interPlanetarySpeed * Math.Cos(interPlanetaryLaunchAngle);
      double y1 = interPlanetarySpeed * Math.Sin(interPlanetaryLaunchAngle);
      double xOldPos = objPosition.PosX;
      double yOldPos = objPosition.PosY;
      objPosition.PosX = x1 + xOldPos;
      objPosition.PosY = y1 + yOldPos;
      }

      // use straight calculation of interplanetary angle
    public void UpdateInterPlanetaryPosition3()
    {
    double x1 = interPlanetarySpeed * Math.Cos(interPlanetaryAngle);
    double y1 = interPlanetarySpeed * Math.Sin(interPlanetaryAngle);
    double xOldPos = objPosition.PosX;
        double yOldPos = objPosition.PosY;
        objPosition.PosX = x1 + xOldPos;
        objPosition.PosY = y1 + yOldPos;
    }






//  function normalize()
//{
//    magnitude = sqrt(x * x + y * y)
//    if (magnitude > 0.0)
//    {
//        x = x / magnitude
//        y = y / magnitude
//    }
//}
    public void NormalizePos(PositionParams pvPositionParams)
      {
      double magnitude = Math.Sqrt((pvPositionParams.PosX * pvPositionParams.PosX) + (pvPositionParams.PosY * pvPositionParams.PosY));

      if (magnitude > 0)
        {
        pvPositionParams.PosX = pvPositionParams.PosX / magnitude;
        pvPositionParams.PosY = pvPositionParams.PosY / magnitude;
        }




      }




//  // make the length of direction one
//direction.normalize()

//// set the velocity of the bullet
//bullet.velocity = speed * direction
    
//// remember, the above code is the same as
//bullet.velocity.x = speed * direction.x
//bullet.velocity.y = speed * direction.y
//// since speed is just a number value, not a vector, it
//// scales both components equalily

    public void UpdateInterPlanetaryPosition2()
      {
      // TODO: how to set the direction based on the current position of the satellite
      PositionParams objDestination = new PositionParams();
      objDestination.PosX = 250;
      objDestination.PosY = 500;

      //PositionParams objDirection = new PositionParams();
      // the above code is really just shorthand for
//direction.x = player.position.x - turret.position.x
//direction.y = player.position.y - turret.position.y
      objDirection.PosX = objDestination.PosX - objPosition.PosX;
      objDirection.PosY = objDestination.PosY - objPosition.PosY;




      NormalizePos(objDirection);

      // TODO: check for overshoot destination should always be edge of screen - up to code to check whether we have gone into orbit again)
      objPosition.PosX = objPosition.PosX + (interPlanetarySpeed * objDirection.PosX);
      // TODO: check for overshoot
      objPosition.PosY = objPosition.PosY + (interPlanetarySpeed * objDirection.PosY);



      }




    public PositionParams Position
      {
      get { return objPosition; }
      set { objPosition = value; }
      }

    public bool InOrbit
      {
      get { return bInOrbit; }
      set { bInOrbit = value; }
      }

  public double InterPlanetaryAngle
    {
    get { return interPlanetaryAngle; }
      }

  public bool IntoOrbit
    {

    get { return bIntoOrbit; }
    set 
    { bIntoOrbit = value; 

    if (bIntoOrbit == false)
    {
      CalculateLaunchAngle();
      // calculate the last segment of planetary orbit travel
      UpdateDistanceInOrbit(objPlanet);

    }
 
    
    }

    }

    public double LastInterplanetaryAngle
    {
    get { return lastInterPlanetaryAngle; } 

    }

    public double InterPlanetaryIncrement
    {
    get { return this.interPlanetaryIncrement; }
    }

      public double LaunchAngle
      {
      get { return interPlanetaryLaunchAngle; }
      }

      public double AroundPlanet
      {
      get { return angleAroundPlanet; }
      }

        public void EnterOrbit(Planet pvPlanet)
        {
            if (this.objPlanet.PlanetName != pvPlanet.PlanetName)
            { 
 
            this.objPlanet = pvPlanet;
            this.objPosition.Radius = pvPlanet.PullRadius;

            this.bIntoOrbit = true;
            this.InOrbit = true;

                // add this planet to the list of planets visited if not already there
                if (this.lstPlanetsVisited.Contains(pvPlanet) == false)
                {

                    if (pvPlanet != this.objLastPlanet)
                    {
                        // must be going to a new planet from the last visited
                        lstPlanetsVisited.Add(pvPlanet);
                        this.objLastPlanet = pvPlanet;

                        // does the list of planets visted match all the planets in the system - including the home planet?
                        //if (lstPlanetsVisited.Count() == lstPlanetsToVisit.Count())
                        //{
                        //    if (pvPlanet == this.objHomePlanet)
                        //    {
                        //        // completed a trip
                        //        bCompletedTrip = true;




                        //    }



                        //}
                    }

                }
                // does the list of planets visted match all the planets in the system - including the home planet?
                if (lstPlanetsVisited.Count() == lstPlanetsToVisit.Count())
                {
                    if (pvPlanet == this.objHomePlanet)
                    {
                        // completed a trip
                        bCompletedTrip = true;




                    }



                }

                // cache the angle of the planets orbit for distance travelled calculations
                // treat 0 as 1
                double currentPlanetAngle = pvPlanet.Position.Angle;
                if (currentPlanetAngle == 0)
                {
                    currentPlanetAngle = 1;
                }
                planetAngleOnEntry = currentPlanetAngle;
                lastPlanetAngle = currentPlanetAngle;



            }

        }

        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            throw new NotImplementedException();
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }

        public void InitialiseOrbit()
        {
            // TODO: temp on entry the satellite is always at the top of the orbit
            objPosition.PosX = objPlanet.Position.PosX;
            objPosition.PosY = objPlanet.Position.PosY - objPosition.Radius;
            interPlanetaryAngleOnEntryToOrbit = 0;
            this.bIntoOrbit = false;
            objPosition.Angle = 0;
            interPlanetaryAngle = 0;
            angleAroundPlanet = 0;


        }

        public double distance(int x1, int y1, int x2, int y2)
        {

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) * 1.0);
        }

        public void UpdateDistanceInOrbit(Planet pvPlanet)
        {
            // what is the current angle of the planet
            double currentPlanetAngle = pvPlanet.Position.Angle;

            if (this.lastPlanetAngle < this.planetAngleOnEntry)
            {

                if (currentPlanetAngle >= this.planetAngleOnEntry)
                {
                    // planet has completed a revolution of the sun since satellite entered orbit so update distance travelled
                    planetOrbitDistanceTravelled += pvPlanet.DistanceOrbit;
               }


            }

            // satellite has been launched from the planet
            if (this.bIntoOrbit == false)
            {
                // TODO: segment based calculation
                // if the current planet angle is > its angle when the satellite entered orbit the satellite is on segment 1 of the
                // planet orbit
                // if the current planet angle is < its angle when the satellite entered orbit the satellite is on segment 2 of the
                // planet orbit
                double angleT = 0;
                if (currentPlanetAngle >= this.planetAngleOnEntry)
                {
                    // segment 1
                    angleT = currentPlanetAngle - this.planetAngleOnEntry;
                    if (angleT == 0)
                    {
                        angleT = 1;

                    }


                }
                else
                {
                    // segment 2
                    angleT = currentPlanetAngle;

               }

                double arc = pvPlanet.DistanceOrbit * (angleT / 360);
                planetOrbitDistanceTravelled += arc;

            }



        }

        public double DistanceBetweenPlanets
        {
            get { return distanceTravelled; }
        }

        public double DistanceInPlanetaryOrbit
        {
            get { return planetOrbitDistanceTravelled; }
        }

        public Boolean CompletedTrip
        {
            get { return bCompletedTrip; }
        }

        public void Reset()
        {

            lstPlanetsVisited = new List<Planet>();

            distanceTravelled = 0;

            bCompletedTrip = false;


        }







    }




}

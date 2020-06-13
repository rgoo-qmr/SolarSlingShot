using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSlingShot
  {
  public class Planet : IEquatable<Planet>

   {
        private static int nextID = 0;

    private Sun objSun;

        // allow one planet to orbit another
        private Planet objPlanet;
 
    private PositionParams objPosition;

        private string strName;

        private double dblSatellitePullRadius;

        private int planetID;

        private double orbitDistance;

        private bool bPlanetMoon;

    /** while the planet is in orbit its velocity is the rate at which the angle changes
     * and its new position is based on that and its distance from the sun centre (Radius of the orbit)
     * */
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

    // constructor
    public Planet(Sun pvSun, PositionParams pvPosition, string pvName, double pvSatelitePullRadius)
    {
      nextID = nextID + 1;
      objSun = pvSun;
      objPosition = pvPosition;
      strName = pvName;
      dblSatellitePullRadius = pvSatelitePullRadius;
      planetID = nextID;

      orbitDistance = DistanceTravelled(0,360);
      bPlanetMoon = false;

    }

    // planet acting as a moon
    public Planet(Planet pvPlanet, PositionParams pvPosition, string pvName, double pvSatelitePullRadius)
    {
        nextID = nextID + 1;
        objPlanet = pvPlanet;
        objPosition = pvPosition;
        strName = pvName;
        dblSatellitePullRadius = pvSatelitePullRadius;
        planetID = nextID;

        orbitDistance = DistanceTravelled(0, 360);

        bPlanetMoon = true;

    }

        public void updateOrbitPosition()
      {
            if (bPlanetMoon == false)
            {
                double angle = objPosition.Angle + objPosition.Velocity;
                //angle &= 0xFFFFFF;
                //if (angle >= 360)
                if (angle >= 2 * MainWindow.Pi)
                {
                    angle = 0;
                }

                // anticlockwise
                //objPosition.PosX = objSun.Position.PosX + (objPosition.Radius * Math.Sin(angle));
                //objPosition.PosY = objSun.Position.PosY + (objPosition.Radius * Math.Cos(angle));
                // clockwise
                objPosition.PosX = objSun.Position.PosX + (objPosition.Radius * Math.Cos(angle));
                objPosition.PosY = objSun.Position.PosY + (objPosition.Radius * Math.Sin(angle));
                objPosition.Angle = angle;
            }
            else
            {
                double angle = objPosition.Angle + objPosition.Velocity;
                //angle &= 0xFFFFFF;
                //if (angle >= 360)
                if (angle >= 2 * MainWindow.Pi)
                {
                    angle = 0;
                }

                // anticlockwise
                //objPosition.PosX = objSun.Position.PosX + (objPosition.Radius * Math.Sin(angle));
                //objPosition.PosY = objSun.Position.PosY + (objPosition.Radius * Math.Cos(angle));
                // clockwise
                objPosition.PosX = objPlanet.Position.PosX + (objPosition.Radius * Math.Cos(angle));
                objPosition.PosY = objPlanet.Position.PosY + (objPosition.Radius * Math.Sin(angle));
                objPosition.Angle = angle;
            }
      }




    public PositionParams Position
      {
      get { return objPosition; }
      set { objPosition = value; }
      }




        public bool WithinOrbitPull(Satellite pvSatellite)
        {
            bool bWithinOrbitPull = false;

            bool bCouldBeInOrbitX = false;
            bool bCouldBeInOrbitY = false;

            /* test A satellite is at an X co-ordinate > planet X but <= planet X + radius */
            if ((pvSatellite.Position.PosX > this.Position.PosX) & (pvSatellite.Position.PosX <= this.Position.PosX + this.dblSatellitePullRadius))
             {
                bCouldBeInOrbitX = true;

            }

            /* test B satellite is at an X co-ordinate < planet X but >= planet X - radius */
            if ((pvSatellite.Position.PosX < this.Position.PosX) & (pvSatellite.Position.PosX >= this.Position.PosX - this.dblSatellitePullRadius))
            {
                bCouldBeInOrbitX = true;

            }

            /* test C satellite is at a Y co-ordinate > planet Y but <= planet Y + radius*/
            if ((pvSatellite.Position.PosY > this.Position.PosY) & (pvSatellite.Position.PosY <= this.Position.PosY + this.dblSatellitePullRadius))
            {
                bCouldBeInOrbitY = true;

            }

            /* test D satellite is at a Y co-ordinate < planet Y but >= planet Y - radius
             */
            if ((pvSatellite.Position.PosY < this.Position.PosY) & (pvSatellite.Position.PosY >= this.Position.PosY - this.dblSatellitePullRadius))
            {
                bCouldBeInOrbitY = true;

            }

            bWithinOrbitPull = (bCouldBeInOrbitX & bCouldBeInOrbitY);


            return bWithinOrbitPull;

        }

        public double DistanceTravelled(double startingAngle, double currentAngle)
        {
            double orbitCircumference = (objPosition.Radius * 2) * MainWindow.Pi;
            // C * (T/ 360)
            double arc = orbitCircumference * ((currentAngle - startingAngle) / 360);

            return arc;
;
        }

        // uses the satellite radius attribute for the pull checks
        // RG 24/05/20  not used
        public bool WithinOrbitPullSatelliteRadius(Satellite pvSatellite)
        {
            bool bWithinOrbitPull = false;

            bool bCouldBeInOrbitX = false;
            bool bCouldBeInOrbitY = false;

            /* test A satellite is at an X co-ordinate > planet X but <= planet X + radius */
            if ((pvSatellite.Position.PosX > this.Position.PosX) & (pvSatellite.Position.PosX <= this.Position.PosX + pvSatellite.Position.Radius))
            {
                bCouldBeInOrbitX = true;

            }

            /* test B satellite is at an X co-ordinate < planet X but >= planet X - radius */
            if ((pvSatellite.Position.PosX < this.Position.PosX) & (pvSatellite.Position.PosX >= this.Position.PosX - pvSatellite.Position.Radius))
            {
                bCouldBeInOrbitX = true;

            }

            /* test C satellite is at a Y co-ordinate > planet Y but <= planet Y + radius*/
            if ((pvSatellite.Position.PosY > this.Position.PosY) & (pvSatellite.Position.PosY <= this.Position.PosY + pvSatellite.Position.Radius))
            {
                bCouldBeInOrbitY = true;

            }

            /* test D satellite is at a Y co-ordinate < planet Y but >= planet Y - radius
             */
            if ((pvSatellite.Position.PosY < this.Position.PosY) & (pvSatellite.Position.PosY >= this.Position.PosY - pvSatellite.Position.Radius))
            {
                bCouldBeInOrbitY = true;

            }

            bWithinOrbitPull = (bCouldBeInOrbitX & bCouldBeInOrbitY);


            return bWithinOrbitPull;

        }


        public override string ToString()
        {
            return strName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Planet objAsPart = obj as Planet;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return planetID;
        }
        public bool Equals(Planet other)
        {
            if (other == null) return false;
            return (this.planetID.Equals(other.planetID));
        }
        // Should also override == and != operators.


        public string PlanetName => strName;

        public double PullRadius => dblSatellitePullRadius;

        public double DistanceOrbit => orbitDistance;


    }

    

 
  }

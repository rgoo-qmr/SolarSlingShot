using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSlingShot
{
  /* for a body orbiting another body, the angle will be changing 
   * for a body mving in a straight line away from it the angle will be the same */
  public class PositionParams
  {
    // polar coordinates
    // velocity - could be the rate of change for the angle, or the length of vector if the object is moving in a straight line
    private double velocity;
    // angle of object in obrbit or the angle of the vector if moving in a straight line
    private double angle;

    // the distance of this object from the centre of another object it is orbiting
    private double radius; 

    // cartesian coordinates
    private double x;
    private double y;

    public double Velocity
      {
      get { return velocity; }
      set { velocity = value; }
      }

    public double Angle
      {
      get { return angle; }
      set { angle = value; }

      }

    public double PosX
      {
      get { return x; }
      set { x = value; }

      }
    public double PosY
      {
      get { return y; }
      set { y = value; }

      }

    public double Radius
    {
    get { return radius; }
    set { radius = value; }
      }


    
  }

}

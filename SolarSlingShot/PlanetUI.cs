using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace SolarSlingShot
{

  // position of the spaceman on the canvas
  // currently using coordinate system X,Y (0,0) == top left
  // where increasing X moves left and increasing Y moves down
  public class PlanetUI
  {


    private Planet objPlanet;
    private Ellipse objEllipse;

    public PlanetUI(Planet pvPlanet, Ellipse pvEllipse)
    {
    objPlanet = pvPlanet;
    objEllipse = pvEllipse;
    }

    public Ellipse Ellipse
               {
      get { return objEllipse; }
      set { objEllipse = value; }
      }


    public Planet Planet
      {
      get { return objPlanet; }
      set { objPlanet = value; }
      }
  }
}

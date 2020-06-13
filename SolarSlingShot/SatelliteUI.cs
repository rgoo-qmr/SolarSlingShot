using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace SolarSlingShot
  {
    public class SatelliteUI
    {



      private Satellite objSatellite;
    private Ellipse objEllipse;

    public SatelliteUI(Satellite pvSatellite, Ellipse pvEllipse)
    {
    objSatellite = pvSatellite;
    objEllipse = pvEllipse;
    }

    public Ellipse Ellipse
               {
      get { return objEllipse; }
      set { objEllipse = value; }
      }


    public Satellite Satellite
      {
      get { return objSatellite; }
      set { objSatellite = value; }
      }



    }
  }

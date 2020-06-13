using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSlingShot
  {
  public class Sun
    {

    private PositionParams objPosition;

    public Sun(PositionParams pvPosition)
      {
      objPosition = pvPosition;
      }

    public PositionParams Position
      {
      get { return objPosition; }
      set { objPosition = value; }
      }
    }
  }

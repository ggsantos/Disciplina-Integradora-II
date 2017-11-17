using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLib
{
    public class PointXY
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PointXY()
        {
        }

        public PointXY(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}

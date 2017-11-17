using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossLib
{
    public class SatelliteImage
    {
        //-------------------------------------------------------------------------------------
        public Bitmap RoadImg;
        public Bitmap SatelliteImg;
        public int UserX;
        public int UserY;


        //-------------------------------------------------------------------------------------
        public void GetImages(string latitude, string longitude) 
        {
            RoadImg = (Bitmap)GetImage(latitude, longitude, MapType.roadmap);
            using (MemoryStream mStream = new MemoryStream())
            {
                RoadImg.Save(mStream, ImageFormat.Jpeg);
                RoadImg = new Bitmap(mStream);
                mStream.Dispose();
            }

            SatelliteImg = (Bitmap)GetImage(latitude, longitude, MapType.satellite);
            using (MemoryStream mStream = new MemoryStream())
            {
                SatelliteImg.Save(mStream, ImageFormat.Jpeg);
                SatelliteImg = new Bitmap(mStream);
                mStream.Dispose();
            }

            UserX = (RoadImg.Width / 2);
            UserY = (RoadImg.Height / 2);



        }


        //-------------------------------------------------------------------------------------
        public enum MapType
        {
            satellite,
            roadmap
        }

        //-------------------------------------------------------------------------------------
        public const double latOffSet = 0.000747;
        public const double lngOffSet = 0.000858;

        //-------------------------------------------------------------------------------------
        public static string ToCoord(string ponto)
        {
            return ponto.Replace(",", ".");
        }
        public static string ToCoord(double ponto)
        {
            return (ponto).ToString().Replace(",", ".");
        }
        public static string ToCoord(string ponto1, string ponto2)
        {
            return ponto1.Replace(",", ".") + "," + ponto2.Replace(",", ".");
        }


        //-------------------------------------------------------------------------------------
        public static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            // generally used geo measurement function
            var R = 6378.137; // Radius of earth in KM
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d * 1000; // meters
        }


        //-------------------------------------------------------------------------------------
        public static double ToDouble(string texto)
        {
            if (Convert.ToDouble("0.10") > 0)
            {
                texto = texto.Replace('.', ',');
            }
            else
            {
                texto = texto.Replace(',', '.');
            }
            return Convert.ToDouble(texto);
        }

        //-------------------------------------------------------------------------------------
        public System.Drawing.Bitmap GetImageBitMap(string latitude, string longitude, MapType mt)
        {
            System.Drawing.Bitmap image;

            var request = WebRequest.Create("http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=20&size=640x640&maptype=" + mt.ToString() + "&sensor=false&scale=1&markers=color:blue%7Clabel:I%7C-30.029228,-51.211041");
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                image = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(stream);
            }

            return image;
        }

        public System.Drawing.Image GetImage(string latitude, string longitude, MapType mt)
        {
            System.Drawing.Image image;

            //var request = WebRequest.Create("http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=20&size=640x640&maptype=" + mt.ToString() + "&sensor=false&scale=1&markers=color:blue%7Clabel:I%7C-30.029228,-51.211041");
            var request = WebRequest.Create("http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=20&size=640x640&maptype=" + mt.ToString() + "&sensor=false&scale=1");
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                image = System.Drawing.Image.FromStream(stream);
            }

            return image;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CrossLib
{
    public class AdressSearch
    {

        public string GetAdress(string latitude, string longitude)
        {
            System.Drawing.Image image;

            //http://maps.googleapis.com/maps/api/geocode/json?latlng=-30.0278384,-51.21061209999&sensor=false

            var request = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&sensor=false");
            request.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)request.GetResponse();

            string text = string.Empty;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            text = text.Replace("\n", "").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");

            object obj1 = new JavaScriptSerializer().DeserializeObject(text);

            Dictionary<string, object> obj2 = (Dictionary<string, object>)(((object[])((Dictionary<string, object>)(obj1))["results"])[0]);

            text = (string)obj2["formatted_address"] ;

            int p = text.IndexOf(" - ");
            if (p > 0)
            {
                text = text.Substring(0, p);
            }
            
            return text;
        }
    }
}

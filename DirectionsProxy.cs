using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
namespace DoctorsAppointment
{
   
    public class DirectionsProxy
    {
        public string dist;
        public double GetDrivingDistanceInMiles(string originlat, string originlong,double destinationlat, double destinationlong)
        {
            
                //origin = 41.43206,-81.38992 | -33.86748,151.20699;
                string origin = originlat + "%2C" + originlong;
                string destination = destinationlat + "%2C" + destinationlong;
                string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" +
                  origin + "&destinations=" + destination +
                  "&mode=driving&language=en-EN&units=metric&key=AIzaSyB1-ujT9YwKi_uOcf04-OmP3Fv-7SmbwKE";
                //key=AIzaSyB1 - ujT9YwKi_uOcf04 - OmP3Fv - 7SmbwKE
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);

                
                if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
                {
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                dist = distance[0].ChildNodes[1].InnerText;
                try
                {
                    return Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" km", ""));
                }
                catch (Exception e)
                {
                    return Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" m", ""));
                }
                //return 7;
               
                }

                else
                {
                    dist=xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText;
                    return 0;
                }

                
            
        
            
        }
    }
    
}
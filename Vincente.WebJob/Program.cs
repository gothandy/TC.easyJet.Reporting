using System;
using System.IO;
using System.Xml.Serialization;

namespace Vincente.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var lastRunTimes = new LastRunTimes
            {
                TogglToTask = DateTime.Now,
                TogglToTimeEntry = DateTime.Now,
                TrelloToCard = DateTime.Now
            };

            XmlSerializer xmlserializer = new XmlSerializer(typeof(LastRunTimes));

            using (var fileStream = new FileStream("App_Data/LastRunTimes.xml", FileMode.Create))
            {
                xmlserializer.Serialize(fileStream, lastRunTimes);
            }
        }
    }
}

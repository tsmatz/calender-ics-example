using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.IO;
using Newtonsoft.Json.Linq;

namespace OutlookSampleWebRole.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OutlookIcs(int EventId)
        {
            string uri = "http://api.atnd.org/events/?event_id=" + EventId + "&format=json";
            HttpClient cl = new HttpClient();
            HttpResponseMessage res = cl.GetAsync(uri).Result;

            //JsonObject resobj = (JsonObject)res.Content.ReadAsAsync<JsonValue>(new[] { new JsonValueMediaTypeFormatter() }).Result;
            JObject resobj = res.Content.ReadAsAsync<JObject>(new[] { new JsonMediaTypeFormatter() }).Result;
            JArray events = (JArray)resobj["events"];
            JObject eventObj = (JObject) ((JObject)events[0])["event"];

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("BEGIN:VCALENDAR");
            writer.WriteLine("PRODID:-//TESTApp/");
            writer.WriteLine("VERSION:2.0");
            writer.WriteLine("BEGIN:VTIMEZONE");
            writer.WriteLine("TZID:Tokyo Standard Time");
            writer.WriteLine("END:VTIMEZONE");
            writer.WriteLine("BEGIN:VEVENT");
            DateTime startTime = DateTime.Parse(((JValue)eventObj["started_at"]).Value.ToString());
            DateTime endTime = DateTime.Parse(((JValue)eventObj["ended_at"]).Value.ToString());
            writer.WriteLine(
                string.Format("DTSTART;TZID=\"Tokyo Standard Time\":{0}",
                startTime.ToString("yyyyMMddTHHmmss")));
            writer.WriteLine(
                string.Format("DTEND;TZID=\"Tokyo Standard Time\":{0}",
                endTime.ToString("yyyyMMddTHHmmss")));
            string title = ((JValue)eventObj["title"]).Value.ToString();
            writer.WriteLine(string.Format("SUMMARY;LANGUAGE=ja:{0}", MyUtil.EscapeForICS(title)));
            string desc = ((JValue)eventObj["description"]).Value.ToString();
            writer.WriteLine(string.Format("DESCRIPTION:{0}", MyUtil.EscapeForICS(desc)));
            string place = ((JValue)eventObj["place"]).Value.ToString();
            writer.WriteLine(string.Format("LOCATION:{0}", MyUtil.EscapeForICS(place)));
            writer.WriteLine("BEGIN:VALARM");
            writer.WriteLine("TRIGGER:-PT15M");
            writer.WriteLine("ACTION:DISPLAY");
            writer.WriteLine("DESCRIPTION:Reminder");
            writer.WriteLine("END:VALARM");
            writer.WriteLine("END:VEVENT");
            writer.WriteLine("END:VCALENDAR");
            writer.Flush();

            stream.Position = 0;
            return new FileStreamResult(stream, "text/calendar");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace OutlookSampleWebRole.Controllers
{
    public class EventData
    {
        public int event_id { get; set; }
        public string title { get; set; }
        public string started_at { get; set; }
        public string ended_at { get; set; }
        public string owner_nickname { get; set; }
        public string description { get; set; }
    }

    public class ItemsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<EventData> Get()
        {
            //
            // We use static dummy data, since Connpass API doesn't work on server-side
            //

            //string uri = "https://connpass.com/api/v1/event/?count=100";
            //HttpClient cl = new HttpClient();
            //cl.DefaultRequestHeaders.Add("User-Agent", "Other");
            //HttpResponseMessage res = cl.GetAsync(uri).Result;
            //JObject resobj = res.Content.ReadAsAsync<JObject>(new[] { new JsonMediaTypeFormatter() }).Result;
            //JArray events = (JArray)resobj["events"];
            //List<EventData> event_result = new List<EventData>();
            //foreach(JObject evtoken in events)
            //{
            //    var evobj = (JObject) evtoken["event"];
            //    event_result.Add(new EventData
            //    {
            //        event_id = (int) evobj["event_id"],
            //        title = (string) evobj["title"],
            //        started_at = (string) evobj["started_at"],
            //        owner_nickname = (string) evobj["owner_nickname"]
            //    });
            //}
            //return event_result;

            JObject resobj = JObject.Parse(Dummy.Events);
            JArray events = (JArray)resobj["events"];
            List<EventData> event_result = new List<EventData>();
            foreach (JObject evobj in events)
            {
                event_result.Add(new EventData
                {
                    event_id = (int)evobj["event_id"],
                    title = (string)evobj["title"],
                    started_at = (string)evobj["started_at"],
                    ended_at = (string)evobj["ended_at"],
                    owner_nickname = (string)evobj["owner_nickname"],
                    description = (string)evobj["description"]
                });
            }
            return event_result;
        }

        // GET api/<controller>/5
        public EventData Get(int id)
        {
            JObject resobj = JObject.Parse(Dummy.Events);
            JArray events = (JArray)resobj["events"];
            //return "value";
            var evobj = (JObject) events.FirstOrDefault(x => x.Value<int>("event_id") == id);
            var event_result = new EventData()
            {
                event_id = (int)evobj["event_id"],
                title = (string)evobj["title"],
                started_at = (string)evobj["started_at"],
                ended_at = (string)evobj["ended_at"],
                owner_nickname = (string)evobj["owner_nickname"],
                description = (string)evobj["description"]
            };
            return event_result;
        }

    }
}
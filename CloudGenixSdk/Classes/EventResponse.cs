using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class EventResponse
    {
        #region Public-Members

        [JsonProperty("alarm")]
        public EventsCount Alarm { get; set; }

        [JsonProperty("alert")]
        public EventsCount Alert { get; set; }

        [JsonProperty("included_count")]
        public int? IncludedCount { get; set; }

        [JsonProperty("total_count")]
        public int? TotalCount { get; set; }

        [JsonProperty("items")]
        public List<EventDetails> Events { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public EventResponse()
        {
            Alarm = null;
            Alert = null;
        }

        #endregion

        #region Public-Methods
          
        public override string ToString()
        {
            return string.Format("[EventResponse: Alarm={0}, Alert={1}, IncludedCount={2}, TotalCount={3}, Events={4}]", Alarm, Alert, IncludedCount, TotalCount, Events);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes

        public class EventsCount
        {
            [JsonProperty("critical")]
            public int Critical { get; set; }

            [JsonProperty("major")]
            public int Major { get; set; }

            [JsonProperty("minor")]
            public int Minor { get; set; }

            public EventsCount()
            {
                
            }

            public override string ToString()
            {
                return string.Format("[EventsCount: Critical={0}, Major={1}, Minor={2}]", Critical, Major, Minor);
            }
        }

        public class EventDetails
        { 
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("cleared")]
            public bool? Cleared { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("correlation_id")]
            public string CorrelationId { get; set; }

            [JsonProperty("entity_ref")]
            public string EntityRef { get; set; }

            [JsonProperty("info")]
            public object Info { get; set; }

            [JsonProperty("severity")]
            public string Severity { get; set; }

            [JsonProperty("site_id")]
            public string SiteId { get; set; }

            [JsonProperty("time")]
            public DateTime Timestamp { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
             
            public EventDetails()
            {
            }
             
            public override string ToString()
            {
                return string.Format("[EventDetails: Id={0}, Cleared={1}, Code={2}, CorrelationId={3}, EntityRef={4}, Info={5}, Severity={6}, SiteId={7}, Timestamp={8}, Type={9}]", Id, Cleared, Code, CorrelationId, EntityRef, Info, Severity, SiteId, Timestamp, Type);
            }
        }

        #endregion
    }
}

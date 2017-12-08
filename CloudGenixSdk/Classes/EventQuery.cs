using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class EventQuery
    {
        #region Public-Members

        [JsonProperty("severity")]
        public List<string> Severity { get; set; }

        [JsonProperty("query")]
        public QueryType Query { get; set; }

        [JsonProperty("_offset")]
        public string Offset { get; set; }

        [JsonProperty("summary")]
        public bool Summary { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("end_time")]
        public string EndTime { get; set; } 

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public EventQuery()
        {
            Query = new QueryType();
            Severity = new List<string>();
        }

        public EventQuery(string startTime, string endTime, string offset, string queryType, bool summary)
        {
            StartTime = startTime;
            EndTime = endTime;
            Offset = offset;
            Summary = summary;
            Query = new QueryType();
            Severity = new List<string>();

            if (!String.IsNullOrEmpty(queryType)) Query.Type = queryType; 

            if (summary) 
            {
                Query = null;
            }
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[EventQuery: Severity={0}, Query={1}, Offset={2}, Summary={3}, StartTime={4}, EndTime={5}]", Severity, Query, Offset, Summary, StartTime, EndTime);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes
         
        public class QueryType 
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            public QueryType()
            {
                Type = null;
            }

            public override string ToString()
            {
                return string.Format("[QueryType: Type={0}]", Type);
            }
        }

        #endregion
    }
}

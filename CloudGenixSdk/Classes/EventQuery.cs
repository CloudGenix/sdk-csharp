/*

    CloudGenix Controller SDK
    (c) 2018 CloudGenix, Inc.
    All Rights Reserved

    https://www.cloudgenix.com

    This SDK is released under the MIT license.
    For support, please contact us on:

        NetworkToCode Slack channel #cloudgenix: http://slack.networktocode.com
        Email: developers@cloudgenix.com

 */

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
        public QuerySettings Query { get; set; }

        [JsonProperty("_offset")]
        public string Offset { get; set; }

        [JsonProperty("view")]
        public ViewSettings View { get; set; }
         
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
            Query = new QuerySettings();
            Severity = new List<string>();
            View = new ViewSettings();
        }

        public EventQuery(string startTime, string endTime, string offset, string queryType, bool summary)
        {
            Query = new QuerySettings();
            Severity = new List<string>();
            View = new ViewSettings();

            StartTime = startTime;
            EndTime = endTime;
            Offset = offset;
            View.Summary = summary;
            Query = new QuerySettings();
            Severity = new List<string>();

            if (!String.IsNullOrEmpty(queryType)) Query.Type.Add(queryType); 

            if (summary) 
            {
                Query = null;
            }
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[EventQuery: Severity={0}, Query={1}, Offset={2}, View={3}, StartTime={4}, EndTime={5}]", Severity, Query, Offset, View, StartTime, EndTime);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes
         
        public class QuerySettings 
        {
            [JsonProperty("code")]
            public List<string> Codes { get; set; }

            [JsonProperty("site")]
            public List<string> Sites { get; set; }

            [JsonProperty("type")]
            public List<string> Type { get; set; }

            [JsonProperty("entity_ref")]
            public List<string> Entities { get; set; }

            [JsonProperty("category")]
            public List<string> Categories { get; set; }

            [JsonProperty("correlation_id")]
            public List<string> CorrelationIDs { get; set; }

            public QuerySettings()
            {
                Codes = new List<string>();
                Sites = new List<string>();
                Type = new List<string>();
                Entities = new List<string>();
                Categories = new List<string>();
                CorrelationIDs = new List<string>();
            }

            public override string ToString()
            {
                return string.Format("[QueryType: Codes={0}, Sites={1}, Type={2}, Entities={3}, Categories={4}, CorrelationIDs={5}]", Codes, Sites, Type, Entities, Categories, CorrelationIDs);
            }
        }

        public class ViewSettings
        {
            [JsonProperty("summary")]
            public bool Summary { get; set; }

            public ViewSettings()
            {
                Summary = true;
            }

            public override string ToString()
            {
                return string.Format("[ViewParams: Summary={0}]", Summary);
            }
        }

        #endregion
    }
}

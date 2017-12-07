using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CloudGenix.Classes
{
    public class FlowQuery
    {
        #region Public-Members

        [JsonProperty("start_time")] 
        public string StartTime { get; set; }

        [JsonProperty("end_time")] 
        public string EndTime { get; set; }

        [JsonProperty("debug_level")]
        public string DebugLevel { get; set; }
         
        [JsonProperty("filter")]
        public FilterSettings Filter { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public FlowQuery(
            string startTime,
            string endTime,
            string debugLevel 
            )
        {
            StartTime = startTime;
            EndTime = endTime;
            DebugLevel = debugLevel;
 
            Filter = new FilterSettings();
        }

        public FlowQuery()
        {
            StartTime = null;
            EndTime = null;
            DebugLevel = null; 
            Filter = null;
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[FlowQuery: StartTime={0}, EndTime={1}, DebugLevel={2}, Filter={3}]", StartTime, EndTime, DebugLevel, Filter);
        }
         
        public void AddSiteFilter(string siteId)
        {
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            if (Filter == null) Filter = new FilterSettings();
            if (Filter.SiteIds == null) Filter.SiteIds = new List<string>();
            Filter.SiteIds.Add(siteId);
        }

        public void AddAppFilter(string appId)
        {
            if (String.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));
            if (Filter == null) Filter = new FilterSettings();
            if (Filter.AppIds == null) Filter.AppIds = new List<string>();
            Filter.AppIds.Add(appId);
        }

        public void AddPathTypeFilter(string pathType)
        {
            if (String.IsNullOrEmpty(pathType)) throw new ArgumentNullException(nameof(pathType));
            if (!ValidatePathType(pathType)) throw new ArgumentException("Invalid path type");
            if (Filter == null) Filter = new FilterSettings();
            if (Filter.PathTypes == null) Filter.PathTypes = new List<string>();
            Filter.PathTypes.Add(pathType);
        }

        public void AddDirectionFilter(string direction)
        {
            if (String.IsNullOrEmpty(direction)) throw new ArgumentNullException(nameof(direction));
            if (!ValidateDirection(direction)) throw new ArgumentException("Invalid direction");
            if (Filter == null) Filter = new FilterSettings();
            Filter.Direction = direction;
        }

        #endregion

        #region Private-Methods
           
        private bool ValidateDirection(string direction)
        {
            List<string> values = new List<string>()
            {
                "ingress",
                "egress"
            };

            if (!values.Contains(direction))
            {
                return false;
            }

            return true;
        }

        private bool ValidatePathType(string pathType)
        {
            List<string> values = new List<string>()
            {
                "DirectInternet",
                "VPN",
                "PrivateVPN",
                "PrivateWAN"
            };

            if (!values.Contains(pathType))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Public-Embedded-Classes
         
        public class FilterSettings
        {
            [JsonProperty("site")]
            public List<string> SiteIds { get; set; }

            [JsonProperty("app")]
            public List<string> AppIds { get; set; }

            [JsonProperty("path_type")]
            public List<string> PathTypes { get; set; }

            [JsonProperty("direction")]
            public string Direction { get; set; }

            public FilterSettings()
            {
                SiteIds = null;
                AppIds = null;
                PathTypes = null;
                Direction = null;
            }

            public override string ToString()
            {
                return string.Format("[FilterSettings: SiteIds={0}, AppIds={1}, PathTypes={2}, Direction={3}]", SiteIds, AppIds, PathTypes, Direction);
            }
        }
 
        #endregion 
    }
}

/*

    CloudGenix Controller SDK
    (c) 2017 CloudGenix, Inc.
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
using Newtonsoft.Json.Converters;

namespace CloudGenix.Classes
{
    public class TopNQuery
    {
        #region Public-Members

        [JsonProperty("topn_basis")] 
        public string Basis { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("end_time")] 
        public string EndTime { get; set; }

        [JsonProperty("top_n")]
        public TopNSettings TopN { get; set; }
         
        [JsonProperty("filter")]
        public FilterSettings Filter { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public TopNQuery(
            string startTime,
            string endTime, 
            string basis,
            string topnType,
            int limit
            )
        {
            if (String.IsNullOrEmpty(startTime)) throw new ArgumentNullException(nameof(startTime));
            if (String.IsNullOrEmpty(endTime)) throw new ArgumentNullException(nameof(endTime));
            if (String.IsNullOrEmpty(basis)) throw new ArgumentNullException(nameof(basis));
            if (String.IsNullOrEmpty(topnType)) throw new ArgumentNullException(nameof(topnType));

            if (!ValidateBasis(basis)) throw new ArgumentException("Invalid basis");
            if (!ValidateType(topnType)) throw new ArgumentException("Invalid top N type");

            StartTime = startTime;
            EndTime = endTime;
            Basis = basis;

            TopN = new TopNSettings();
            TopN.Type = topnType;
            TopN.Limit = limit;

            Filter = null;
        }

        public TopNQuery()
        {
            StartTime = null;
            EndTime = null;
            Basis = null;
            TopN = new TopNSettings();
            Filter = new FilterSettings();
        }

        #endregion

        #region Public-Methods
          
        public void AddSiteFilter(string siteId)
        {
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            Filter.SiteIds.Add(siteId);
        }
         
        #endregion

        #region Private-Methods

        private bool ValidateBasis(string name)
        {
            List<string> values = new List<string>()
            {
                "traffic_volume",
                "initiation_failure",
                "transaction_failure",
                "tcp_flow",
                "udp_flow" 
            };

            if (!values.Contains(name))
            {
                return false;
            }

            return true;
        }

        private bool ValidateType(string name)
        {
            List<string> values = new List<string>()
            {
                "app",
                "site"
            };

            if (!values.Contains(name))
            {
                return false;
            }

            return true;
        }
         
        #endregion

        #region Public-Embedded-Classes

        public class TopNSettings
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }
             
            public TopNSettings()
            {
                
            }

            public override string ToString()
            {
                return string.Format("[TopNSettings: Type={0}, Limit={1}]", Type, Limit);
            }
        }
         
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
                SiteIds = new List<string>();
                AppIds = new List<string>();
                PathTypes = new List<string>();
            }

            public override string ToString()
            {
                return string.Format("[FilterSettings: SiteIds={0}, AppIds={1}, PathTypes={2}, Direction={3}]", SiteIds, AppIds, PathTypes, Direction);
            }
        }
 
        #endregion 
    }
}

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
    public class MetricsQuery
    {
        #region Public-Members

        [JsonProperty("start_time")] 
        public string StartTime { get; set; }

        [JsonProperty("end_time")] 
        public string EndTime { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("metrics")]
        public List<MetricsSettings> Metrics { get; set; }

        [JsonProperty("view")]
        public ViewSettings View { get; set; }
 
        [JsonProperty("filter")]
        public FilterSettings Filter { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public MetricsQuery(
            string startTime,
            string endTime,
            string interval,
            string view
            )
        {
            StartTime = startTime;
            EndTime = endTime;
            Interval = interval;

            View = new ViewSettings();
            View.Individual = view;

            Metrics = new List<MetricsSettings>();
            View = new ViewSettings();
            Filter = new FilterSettings();
        }

        public MetricsQuery()
        {
            StartTime = null;
            EndTime = null;
            Interval = null;
            Metrics = new List<MetricsSettings>();
            View = new ViewSettings();
            Filter = new FilterSettings();
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[MetricsQuery: StartTime={0}, EndTime={1}, Interval={2}, Metrics={3}, View={4}, Filter={5}]", StartTime, EndTime, Interval, Metrics, View, Filter);
        }

        public void AddMetric(string name, List<string> statistics, string unit)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (statistics == null || statistics.Count < 1) throw new ArgumentNullException(nameof(statistics));
            if (String.IsNullOrEmpty(unit)) throw new ArgumentNullException(nameof(unit));
            if (!ValidateName(name)) throw new ArgumentException("Invalid value for name");
            if (!ValidateStatistics(statistics)) throw new ArgumentException("Invalid value for statistics");
            if (!ValidateUnit(unit)) throw new ArgumentException("Invalid value for unit");

            MetricsSettings metrics = new MetricsSettings();
            metrics.Name = name;
            metrics.Statistics = statistics;
            metrics.Unit = unit;

            Metrics.Add(metrics);
        }

        public void AddSiteFilter(string siteId)
        {
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            Filter.SiteIds.Add(siteId);
        }

        public void AddAppFilter(string appId)
        {
            if (String.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));
            Filter.AppIds.Add(appId);
        }

        public void AddPathTypeFilter(string pathType)
        {
            if (String.IsNullOrEmpty(pathType)) throw new ArgumentNullException(nameof(pathType));
            if (!ValidatePathType(pathType)) throw new ArgumentException("Invalid path type");
            Filter.PathTypes.Add(pathType);
        }

        public void AddDirectionFilter(string direction)
        {
            if (String.IsNullOrEmpty(direction)) throw new ArgumentNullException(nameof(direction));
            if (!ValidateDirection(direction)) throw new ArgumentException("Invalid direction");
            Filter.Direction = direction;
        }

        #endregion

        #region Private-Methods

        private bool ValidateStatistics(List<string> statistics)
        {
            List<string> values = new List<string>()
            {
                "average",
                "max",
                "sum",
                "min"
            };

            foreach (string curr in statistics)
            {
                if (!values.Contains(curr))
                {
                    return false;    
                }
            }

            return true;
        }

        private bool ValidateName(string name)
        {
            List<string> values = new List<string>()
            {
                "BandwidthUsage",
                "AppSuccessfulConnections",
                "AppSuccessfulTransactions",
                "AppFailedToEstablish",
                "AppTransactionFailures",
                "AppUnreachable",
                "AppSiteHealth",
                "AppNormalizedNetworkTransferTime",
                "AppRoundTripTime",
                "AppServerResponseTime",
                "AppUDPTransactionResponseTime",
                "TCPFlowCount",
                "UDPFlowCount",
                "TCPConcurrentFlows",
                "UDPConcurrentFlows",
                "AppPerfUDPAudioBandwidth",
                "AppPerfUDPVideoBandwidth",
                "AppPerfUDPAudioJitter",
                "AppPerfUDPVideoJitter",
                "AppPerfUDPAudioPacketLoss",
                "AppAudioMos",
                "LqmLinkHealth",
                "LqmLatency",
                "LqmJitter",
                "LqmPacketLoss",
                "LqmMos"
            };

            if (!values.Contains(name))
            {
                return false;
            }

            return true;
        }

        private bool ValidateUnit(string unit)
        {
            List<string> values = new List<string>()
            {
                "Mbps",
                "count",
                "milliseconds",
                "Percentage"
            };

            if (!values.Contains(unit))
            {
                return false;
            }

            return true;
        }

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

        public class MetricsSettings
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("statistics")]
            public List<string> Statistics { get; set; }

            [JsonProperty("unit")]
            public string Unit { get; set; }

            public MetricsSettings()
            {
                Statistics = new List<string>();
            }

            public override string ToString()
            {
                return string.Format("[MetricsSettings: Name={0}, Statistics={1}, Unit={2}]", Name, Statistics, Unit);
            }
        }

        public class ViewSettings
        {
            [JsonProperty("individual")]
            public string Individual { get; set; }  // app, site, path_type

            public override string ToString()
            {
                return string.Format("[ViewSettings: Individual={0}]", Individual);
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

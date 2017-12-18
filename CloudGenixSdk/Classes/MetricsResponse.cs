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
    public class MetricsResponse
    {
        #region Public-Members

        [JsonProperty("metrics")]
        public List<MetricsContainer> Metrics { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public MetricsResponse()
        {
            Metrics = new List<MetricsContainer>();
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[MetricsResponse: Metrics={0}]", Metrics);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes

        public class MetricsContainer
        {
            [JsonProperty("series")]
            public List<SeriesContainer> Series { get; set; }

            public MetricsContainer()
            {
                Series = new List<SeriesContainer>();    
            }

            public override string ToString()
            {
                return string.Format("[MetricsContainer: Series={0}]", Series);
            }

            public class SeriesContainer
            {
                [JsonProperty("data")]
                public List<DataContainer> Data { get; set; }

                [JsonProperty("interval")]
                public string Interval { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("unit")]
                public string Unit { get; set; }

                [JsonProperty("view")]
                public string View { get; set; }

                public SeriesContainer()
                {
                    Data = new List<DataContainer>();
                }

                public override string ToString()
                {
                    return string.Format("[SeriesContainer: Data={0}, Interval={1}, Name={2}, Unit={3}, View={4}]", Data, Interval, Name, Unit, View);
                }

                public class DataContainer
                {
                    [JsonProperty("datapoints")]
                    public List<DataPoint> DataPoints { get; set; }

                    [JsonProperty("statistics")]
                    public string Statistics { get; set; }

                    public DataContainer()
                    {
                        DataPoints = new List<DataPoint>();
                    }

                    public override string ToString()
                    {
                        return string.Format("[DataContainer: DataPoints={0}, Statistics={1}]", DataPoints, Statistics);
                    }

                    public class DataPoint
                    {
                        [JsonProperty("time")]
                        public DateTime Time { get; set; }

                        [JsonProperty("value")]
                        public decimal Value { get; set; }

                        public override string ToString()
                        {
                            return string.Format("[DataPoint: Time={0}, Value={1}]", Time, Value);
                        }
                    }
                }
            }
        }

        #endregion 
    }
}

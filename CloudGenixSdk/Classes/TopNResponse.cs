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
using Newtonsoft.Json.Converters;

namespace CloudGenix.Classes
{
    public class TopNResponse
    {
        #region Public-Members

        [JsonProperty("top_n")]
        public TopNData TopN { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public TopNResponse()
        {
            TopN = new TopNData();
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[TopNResponse: TopN={0}]", TopN);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes

        public class TopNData
        {
            [JsonProperty("start_time")]
            public DateTime StartTime { get; set; }

            [JsonProperty("end_time")]
            public DateTime EndTime { get; set; }

            [JsonProperty("items")]
            public List<string> Items { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }

            [JsonProperty("topn_basis")]
            public string Basis { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            public TopNData()
            {
                Items = new List<string>();   
            }
 
            public override string ToString()
            {
                return string.Format("[TopNData: StartTime={0}, EndTime={1}, Items={2}, Limit={3}, Basis={4}, Type={5}]", StartTime, EndTime, Items, Limit, Basis, Type);
            }
        }

        #endregion 
    }
}

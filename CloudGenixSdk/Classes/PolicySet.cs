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
    public class PolicySet
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public int Etag { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("default_policy")]
        public bool DefaultPolicy { get; set; }

        [JsonProperty("bandwidth_allocation_schemes")]
        public object BandwidthAllocationSchemes { get; set; }

        [JsonProperty("business_priority_names")]
        public object BusinessPriorityNames { get; set; }
         
        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public PolicySet()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[PolicySet: Id={0}, Name={1}, Description={2}, DefaultPolicy={3}, BandwidthAllocationSchemes={4}, BusinessPriorityNames={5}]", Id, Name, Description, DefaultPolicy, BandwidthAllocationSchemes, BusinessPriorityNames);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

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

namespace CloudGenix.Classes
{
    public class SnmpAgent
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public int Etag { get; set; }

        [JsonProperty("tags")]
        public object Tags { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("v2_config")]
        public object V2Config { get; set; }

        [JsonProperty("v3_config")]
        public object V3Config { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SnmpAgent()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[SnnpAgent: Id={0}, Tags={1}, Description={2}, V2Config={3}, V3Config={4}]", Id, Tags, Description, V2Config, V3Config);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

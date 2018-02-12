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
    public class LanNetwork
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public int Etag { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("ipv4_config")]
        public IpV4Configuration IpV4Config { get; set; }

        [JsonProperty("security_policy_set")]
        public object SecurityPolicySet { get; set; }

        [JsonProperty("network_context_id")]
        public string NetworkContextId { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public LanNetwork()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[LanNetwork: Id={0}, Name={1}, Scope={2}, IpV4Config={3}, SecurityPolicySet={4}, NetworkContextId={5}]", Id, Name, Scope, IpV4Config, SecurityPolicySet, NetworkContextId);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

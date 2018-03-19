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
    public class ApplicationDefinition
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ingress_traffic_pct")]
        public int IngressTrafficPercent { get; set; }

        [JsonProperty("conn_idle_timeout")]
        public int ConnectionIdleTimeout { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("path_affinity")]
        public string PathAffinity { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("app_type")]
        public string AppType { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("transfer_type")]
        public string TransferType { get; set; }

        [JsonProperty("domains")]
        public List<string> Domains { get; set; }

        [JsonProperty("tcp_rules")]
        public object TcpRules { get; set; }

        [JsonProperty("udp_rules")]
        public object UdpRules { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public ApplicationDefinition()
        {
        }

        #endregion

        #region Public-Methods
          
        public override string ToString()
        {
            return string.Format("[ApplicationDefinition: Id={0}, IngressTrafficPercent={1}, ConnectionIdleTimeout={2}, Abbreviation={3}, PathAffinity={4}, DisplayName={5}, AppType={6}, Category={7}, TransferType={8}, Domains={9}, TcpRules={10}, UdpRules={11}]", Id, IngressTrafficPercent, ConnectionIdleTimeout, Abbreviation, PathAffinity, DisplayName, AppType, Category, TransferType, Domains, TcpRules, UdpRules);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

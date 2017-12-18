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
    public class IpV4Configuration
    {
        #region Public-Members

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("dhcp_config")]
        public object DhcpConfig { get; set; }

        [JsonProperty("static_config")]
        public object StaticConfig { get; set; }

        [JsonProperty("dns_v4_config")]
        public object DnsV4Config { get; set; }

        [JsonProperty("routes")]
        public object Routes { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public IpV4Configuration()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[IpV4Configuration: Type={0}, DhcpConfig={1}, StaticConfig={2}, DnsV4Config={3}, Routes={4}]", Type, DhcpConfig, StaticConfig, DnsV4Config, Routes);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

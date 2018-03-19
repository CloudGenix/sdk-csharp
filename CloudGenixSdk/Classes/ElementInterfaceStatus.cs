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
    public class ElementInterfaceStatus
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("element_id")]
        public string ElementId { get; set; }
         
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("negotiated_mtu")]
        public int NegotiatedMtu { get; set; }

        [JsonProperty("extended_state")]
        public object ExtendedState { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("pppoe_remote_v4_addr")]
        public object PppoeRemoteV4Address { get; set; }

        [JsonProperty("ipv4_addresses")]
        public List<string> IpV4Addresses { get; set; }

        [JsonProperty("ipv6_addresses")]
        public List<string> IpV6Addresses { get; set; }

        [JsonProperty("dns_v4_config")]
        public object DnsV4Config { get; set; }

        [JsonProperty("dns_v6_config")]
        public object DnsV6Config { get; set; }

        [JsonProperty("routes")]
        public object Routes { get; set; }

        [JsonProperty("operational_state")]
        public string OperationalState { get; set; }

        [JsonProperty("mac_address")]
        public string MacAddress { get; set; }

        [JsonProperty("port")]
        public object Port { get; set; }
         
        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public ElementInterfaceStatus()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[ElementInterfaceStatus: Id={0}, Region={1}, ElementId={2}, Name={3}, NegotiatedMtu={4}, ExtendedState={5}, Device={6}, PppoeRemoteV4Address={7}, IpV4Addresses={8}, IpV6Addresses={9}, DnsV4Config={10}, DnsV6Config={11}, Routes={12}, OperationalState={13}, MacAddress={14}, Port={15}]", Id, Region, ElementId, Name, NegotiatedMtu, ExtendedState, Device, PppoeRemoteV4Address, IpV4Addresses, IpV6Addresses, DnsV4Config, DnsV6Config, Routes, OperationalState, MacAddress, Port);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

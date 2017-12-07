using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class ElementInterface
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attached_lan_networks")]
        public object AttachedLanNetworks { get; set; }

        [JsonProperty("site_wan_interface_ids")]
        public List<string> SiteWanInterfaceIds { get; set; }
         
        [JsonProperty("name")]
        public string Name { get; set; }
         
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("mac_address")]
        public string MacAddress { get; set; }

        [JsonProperty("mtu")]
        public int Mtu { get; set; }

        [JsonProperty("ipv4_config")]
        public IpV4Configuration IpV4Config { get; set; }

        [JsonProperty("dhcp_relay")]
        public object DhcpRelay { get; set; }

        [JsonProperty("ethernet_port")]
        public object EthernetPort { get; set; }

        [JsonProperty("admin_up")]
        public bool AdminUp { get; set; }

        [JsonProperty("nat_address")]
        public string NatAddress { get; set; }

        [JsonProperty("nat_port")]
        public int NatPort { get; set; }

        [JsonProperty("used_for")]
        public string UsedFor { get; set; }

        [JsonProperty("bypass_pair")]
        public object BypassPair { get; set; }

        [JsonProperty("bound_interfaces")]
        public object BoundInterfaces { get; set; }

        [JsonProperty("sub_interface")]
        public object SubInterface { get; set; }
         
        [JsonProperty("pppoe_config")]
        public object PppoeConfig { get; set; }

        [JsonProperty("parent")]
        public object Parent { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public ElementInterface()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[ElementInterface: Id={0}, Type={1}, AttachedLanNetworks={2}, SiteWanInterfaceIds={3}, Name={4}, Description={5}, MacAddress={6}, Mtu={7}, IpV4Config={8}, DhcpRelay={9}, EthernetPort={10}, AdminUp={11}, NatAddress={12}, NatPort={13}, UsedFor={14}, BypassPair={15}, BoundInterfaces={16}, SubInterface={17}, PppoeConfig={18}, Parent={19}]", Id, Type, AttachedLanNetworks, SiteWanInterfaceIds, Name, Description, MacAddress, Mtu, IpV4Config, DhcpRelay, EthernetPort, AdminUp, NatAddress, NatPort, UsedFor, BypassPair, BoundInterfaces, SubInterface, PppoeConfig, Parent);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

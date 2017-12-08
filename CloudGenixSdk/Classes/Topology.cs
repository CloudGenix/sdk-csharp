using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class Topology
    {
        #region Public-Members

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("nodes")]
        public List<SiteNode> Sites { get; set; }

        [JsonProperty("links")]
        public List<WanLink> Links { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Topology()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[Topology: Type={0}, Sites={1}, Links={2}]", Type, Sites, Links);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes

        public class SiteNode
        {
            [JsonProperty("id")]
            public string SiteId { get; set; }

            [JsonProperty("tenant_id")]
            public string TenantId { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("location")]
            public Location Geocoordinates { get; set; }

            [JsonProperty("address")]
            public Address Address { get; set; }

            [JsonProperty("state")]
            public string State { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }

            public SiteNode()
            {
                
            }

            public override string ToString()
            {
                return string.Format("[SiteNode: SiteId={0}, TenantId={1}, Type={2}, Name={3}, Geocoordinates={4}, Address={5}, State={6}, Role={7}]", SiteId, TenantId, Type, Name, Geocoordinates, Address, State, Role);
            }
        }

        public class WanLink
        {
            [JsonProperty("path_id")]
            public string PathId { get; set; }

            [JsonProperty("source_node_id")]
            public string SourceSiteId { get; set; }

            [JsonProperty("source_site_name")]
            public string SourceSiteName { get; set; }

            [JsonProperty("target_node_id")]
            public string TargetSiteId { get; set; }

            [JsonProperty("target_site_name")]
            public string TargetSiteName { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("sub_type")]
            public string SubType { get; set; }

            [JsonProperty("source_wan_if_id")]
            public string SourceWanInterfaceId { get; set; }

            [JsonProperty("source_wan_network")]
            public string SourceWanNetworkName { get; set; }

            [JsonProperty("source_wan_nw_id")]
            public string SourceWanNetworkId { get; set; }

            [JsonProperty("target_wan_if_id")]
            public string TargetWanInterfaceId { get; set; }

            [JsonProperty("target_wan_network")]
            public string TargetWanNetworkName { get; set; }

            [JsonProperty("target_wan_nw_id")]
            public string TargetWanNetworkId { get; set; }

            [JsonProperty("source_circuit_name")]
            public string SourceCircuitName { get; set; }

            [JsonProperty("target_circuit_name")]
            public string TargetCircuitName { get; set; }

            [JsonProperty("admin_up")]
            public bool? AdminUp { get; set; }

            [JsonProperty("vpnlinks")]
            public List<string> VpnLinkIds { get; set; }

            public WanLink()
            {
                
            }

            public override string ToString()
            {
                return string.Format("[WanLink: PathId={0}, SourceSiteId={1}, SourceSiteName={2}, TargetSiteId={3}, TargetSiteName={4}, Status={5}, Type={6}, SubType={7}, SourceWanInterfaceId={8}, SourceWanNetworkName={9}, SourceWanNetworkId={10}, TargetWanInterfaceId={11}, TargetWanNetworkName={12}, TargetWanNetworkId={13}, SourceCircuitName={14}, TargetCircuitName={15}, AdminUp={16}, VpnLinkIds={17}]", PathId, SourceSiteId, SourceSiteName, TargetSiteId, TargetSiteName, Status, Type, SubType, SourceWanInterfaceId, SourceWanNetworkName, SourceWanNetworkId, TargetWanInterfaceId, TargetWanNetworkName, TargetWanNetworkId, SourceCircuitName, TargetCircuitName, AdminUp, VpnLinkIds);
            }
        }

        #endregion
    }
}

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
    public class SiteWanInterface
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

        [JsonProperty("network_id")]
        public string NetworkId { get; set; }

        [JsonProperty("link_bw_down")]
        public int LinkBandwidthDownload { get; set; }

        [JsonProperty("link_bw_up")]
        public int LinkBandwidthUpload { get; set; }

        [JsonProperty("bw_config_mode")]
        public string BandwidthConfigMode { get; set; }

        [JsonProperty("bfd_mode")]
        public string BfdMode { get; set; }

        [JsonProperty("label_id")]
        public string LabelId { get; set; }

        [JsonProperty("lqm_enabled")]
        public bool? LqmEnabled { get; set; }

        [JsonProperty("BwcEnabled")]
        public bool? BwcEnabled { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SiteWanInterface()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[SiteWanInterface: Id={0}, Name={1}, Description={2}, NetworkId={3}, LinkBandwidthDownload={4}, LinkBandwidthUpload={5}, BandwidthConfigMode={6}, BfdMode={7}, LabelId={8}, LqmEnabled={9}, BwcEnabled={10}]", Id, Name, Description, NetworkId, LinkBandwidthDownload, LinkBandwidthUpload, BandwidthConfigMode, BfdMode, LabelId, LqmEnabled, BwcEnabled);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

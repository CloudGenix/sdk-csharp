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
    public class Site
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("admin_state")]
        public string AdminState { get; set; }

        [JsonProperty("address")]
        public Address SiteAddress { get; set; }

        [JsonProperty("location")]
        public Location SiteLocation { get; set; }

        [JsonProperty("policy_set_id")]
        public string PolicySetId { get; set; }

        [JsonProperty("element_cluster_role")]
        public string ElementClusterRole { get; set; }

        [JsonProperty("security_policyset_id")]
        public string SecurityPolicySetId { get; set; }

        [JsonProperty("service_binding")]
        public string ServiceBinding { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Site()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[Site: Id={0}, Name={1}, Description={2}, AdminState={3}, SiteAddress={4}, SiteLocation={5}, PolicySetId={6}, ElementClusterRole={7}, SecurityPolicySetId={8}, ServiceBinding={9}]", Id, Name, Description, AdminState, SiteAddress, SiteLocation, PolicySetId, ElementClusterRole, SecurityPolicySetId, ServiceBinding);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

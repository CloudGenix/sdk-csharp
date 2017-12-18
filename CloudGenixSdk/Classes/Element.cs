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
    public class Element
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("hw_id")]
        public string HardwareId { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty("model_name")]
        public string ModelName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("site_id")]
        public string SiteId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("allowed_roles")]
        public List<string> AllowedRoles { get; set; }

        [JsonProperty("cluster_insertion_mode")]
        public string ClusterInsertionMode { get; set; }

        [JsonProperty("cluster_member_id")]
        public string ClusterMemberId { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("admin_action")]
        public string AdminAction { get; set; }

        [JsonProperty("deployment_op")]
        public string DeploymentOp { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Element()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[Element: Id={0}, SoftwareVersion={1}, HardwareId={2}, SerialNumber={3}, ModelName={4}, Name={5}, SiteId={6}, Description={7}, Role={8}, State={9}, AllowedRoles={10}, ClusterInsertionMode={11}, ClusterMemberId={12}, Connected={13}, AdminAction={14}, DeploymentOp={15}]", Id, SoftwareVersion, HardwareId, SerialNumber, ModelName, Name, SiteId, Description, Role, State, AllowedRoles, ClusterInsertionMode, ClusterMemberId, Connected, AdminAction, DeploymentOp);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

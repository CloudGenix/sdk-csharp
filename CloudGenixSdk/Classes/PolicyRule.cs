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
    public class PolicyRule
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

        [JsonProperty("app_def_id")]
        public string ApplicationDefinitionId { get; set; }

        [JsonProperty("network_context_id")]
        public string NetworkContextId { get; set; }

        [JsonProperty("priority_num")]
        public int PriorityNumber { get; set; }

        [JsonProperty("paths_allowed")]
        public object PathsAllowed { get; set; }

        [JsonProperty("service_context")]
        public object ServiceContext { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public PolicyRule()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[PolicyRule: Id={0}, Name={1}, Description={2}, ApplicationDefinitionId={3}, NetworkContextId={4}, PriorityNumber={5}, PathsAllowed={6}, ServiceContext={7}]", Id, Name, Description, ApplicationDefinitionId, NetworkContextId, PriorityNumber, PathsAllowed, ServiceContext);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

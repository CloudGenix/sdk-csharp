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
    public class SecurityPolicySet
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("policyrule_order")]
        public List<string> PolicyRuleOrder { get; set; }
         
        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SecurityPolicySet()
        {
        }

        #endregion

        #region Public-Methods
            
        public override string ToString()
        {
            return string.Format("[SecurityPolicySet: Id={0}, Name={1}, Description={2}, PolicyRuleOrder={3}]", Id, Name, Description, PolicyRuleOrder);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}

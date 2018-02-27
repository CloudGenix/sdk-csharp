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
using System.IO;
using System.Text;
using RestWrapper;
using CloudGenix.Api;
using CloudGenix.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudGenix
{
    public class CgnxController : IDisposable
    {
        #region Public-Members

        public bool IgnoreCertErrors { get; set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Endpoint { get; set; }
        public string AuthToken { get; private set; }
        public string TenantId { get; private set; }

        #endregion

        #region Private-Members

        private bool _LoggedIn = false;
        private bool _Disposed = false;
        private Dictionary<string, string> _AuthHeaders = null;
        private EndpointManager _Endpoints;

        #endregion

        #region Constructors-and-Factories

        public CgnxController(string email, string password)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (String.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            IgnoreCertErrors = true;
            Email = email;
            Password = password;
            Endpoint = "https://api.cloudgenix.com:443";

            _Endpoints = new EndpointManager();

            Debug.WriteLine("CgnxController initialized");
        }

        private CgnxController()
        {
        }

        #endregion

        #region Public-Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Login()
        {
            string url = BuildUrl("login");

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                LoginRequest());

            if (resp == null)
            {
                Debug.WriteLine("Login no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("Login non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("Login no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("Login response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("x_auth_token"))
            {
                Debug.WriteLine("Login no x_auth_token key found in response data");
                return false;
            }
            else
            {
                BuildAuthHeaders(respDict["x_auth_token"].ToString());
                AuthToken = respDict["x_auth_token"].ToString();
            }

            if (respDict.ContainsKey("api_endpoint") && !String.IsNullOrEmpty(respDict["api_endpoint"].ToString()))
            {
                Debug.WriteLine("Login new API endpoint found in response: " + respDict["api_endpoint"]);
                Endpoint = respDict["api_endpoint"].ToString();

                if (!Endpoint.EndsWith("/")) Endpoint += "/";
            }

            Debug.WriteLine("Login building endpoints");
            BuildEndpoints();

            Debug.WriteLine("Login retrieving tenant ID");
            GetTenantId();

            Debug.WriteLine("Login authenticated successfully");

            _LoggedIn = true;

            return true;
        }

        public bool Logout()
        {
            string url = BuildUrl("logout");

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("Logout no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("Logout non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            return true;
        }

        public Dictionary<string, string> GetAllVersions()
        {
            return _Endpoints.GetAllVersions();    
        }

        public Dictionary<string, string> GetAllEndpoints()
        {
            return _Endpoints.GetAllEndpoints();
        }

        public bool GetContexts(out List<Context> contexts)
        {
            contexts = null;
            string url = BuildUrl("networkcontexts");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetContexts no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetContexts non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetContexts no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetContexts response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            contexts = resResp.GetItems<List<Context>>();

            Debug.WriteLine("GetContexts returning " + contexts.Count + " context(s)");
            return true;
        }

        public bool GetSites(out List<Site> sites)
        {
            sites = null;
            string url = BuildUrl("sites");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSites no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSites non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSites no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSites response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            sites = resResp.GetItems<List<Site>>();

            Debug.WriteLine("GetSites returning " + sites.Count + " site(s)");
            return true;
        }

        public bool GetElements(out List<Element> elements)
        {
            elements = null;
            string url = BuildUrl("elements");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetElements no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetElements non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetElements no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetElements response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            elements = resResp.GetItems<List<Element>>();

            Debug.WriteLine("GetElements returning " + elements.Count + " element(s)");
            return true;
        }

        public bool GetElementInterfaces(string siteId, string elementId, out List<ElementInterface> interfaces)
        {
            interfaces = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            if (String.IsNullOrEmpty(elementId)) throw new ArgumentNullException(nameof(elementId));

            string url = BuildUrl("interfaces");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);
            url = Common.StringReplaceFirst(url, "%s", elementId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetElementInterfaces no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetElementInterfaces non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetElementInterfaces no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetElementInterfaces response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            interfaces = resResp.GetItems<List<ElementInterface>>();

            Debug.WriteLine("GetElementInterfaces returning " + interfaces.Count + " element interface(s)");
            return true;
        }

        public bool GetElementInterfaceStatus(string siteId, string elementId, string interfaceId, out ElementInterfaceStatus status)
        {
            status = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            if (String.IsNullOrEmpty(elementId)) throw new ArgumentNullException(nameof(elementId));
            if (String.IsNullOrEmpty(interfaceId)) throw new ArgumentNullException(nameof(interfaceId));

            string url = BuildUrl("status_interfaces");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);
            url = Common.StringReplaceFirst(url, "%s", elementId);
            url = Common.StringReplaceFirst(url, "%s", interfaceId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetElementInterfaceStatus no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetElementInterfaceStatus non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetElementInterfaceStatus no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetElementInterfaceStatus response: " + Encoding.UTF8.GetString(resp.Data));

            status = Common.DeserializeJson<ElementInterfaceStatus>(resp.Data); 
            Debug.WriteLine("GetElementInterfaceStatus returning status");
            return true;
        }

        public bool GetWanNetworks(out List<WanNetwork> wans)
        {
            wans = null;
            string url = BuildUrl("wannetworks");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetWanNetworks no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetWanNetworks non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetWanNetworks no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetWanNetworks response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            wans = resResp.GetItems<List<WanNetwork>>();

            Debug.WriteLine("GetWanNetworks returning " + wans.Count + " WAN network(s)");
            return true;
        }

        public bool GetLanNetworks(string siteId, out List<LanNetwork> lans)
        {
            lans = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));

            string url = BuildUrl("lannetworks");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetLanNetworks no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetLanNetworks non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetLanNetworks no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetLanNetworks response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            lans = resResp.GetItems<List<LanNetwork>>();

            Debug.WriteLine("GetLanNetworks returning " + lans.Count + " LAN network(s)");
            return true;
        }

        public bool GetApplicationDefinitions(out List<ApplicationDefinition> appDefs)
        {
            appDefs = null;
            string url = BuildUrl("appdefs");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetApplicationDefinitions no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetApplicationDefinitions non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetApplicationDefinitions no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetApplicationDefinitions response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            appDefs = resResp.GetItems<List<ApplicationDefinition>>();

            Debug.WriteLine("GetApplicationDefinitions returning " + appDefs.Count + " application definition(s)");
            return true;
        }

        public bool GetPolicySets(out List<PolicySet> policySets)
        {
            policySets = null;
            string url = BuildUrl("policysets");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetPolicySets no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetPolicySets non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetPolicySets no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetPolicySets response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            policySets = resResp.GetItems<List<PolicySet>>();

            Debug.WriteLine("GetPolicySets returning " + policySets.Count + " policy set(s)");
            return true;
        }

        public bool GetPolicyRules(string policySetId, out List<PolicyRule> rules)
        {
            rules = null;
            if (String.IsNullOrEmpty(policySetId)) throw new ArgumentNullException(nameof(policySetId));

            string url = BuildUrl("policyrules");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", policySetId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetPolicyRules no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetPolicyRules non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetPolicyRules no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetPolicyRules response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            rules = resResp.GetItems<List<PolicyRule>>();

            Debug.WriteLine("GetPolicyRules returning " + rules.Count + " policy rule(s)");
            return true;
        }

        public bool GetSecurityZones(out List<SecurityZone> zones)
        {
            zones = null;
            string url = BuildUrl("securityzones");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSecurityZones no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSecurityZones non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSecurityZones no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSecurityZones response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            zones = resResp.GetItems<List<SecurityZone>>();

            Debug.WriteLine("GetSecurityZones returning " + zones.Count + " security zone(s)");
            return true;
        }

        public bool GetSiteSecurityZones(string siteId, out List<SiteSecurityZone> zones)
        {
            zones = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));

            string url = BuildUrl("sitesecurityzones");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSiteSecurityZones no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSiteSecurityZones non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSiteSecurityZones no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSiteSecurityZones response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            zones = resResp.GetItems<List<SiteSecurityZone>>();

            Debug.WriteLine("GetSiteSecurityZones returning " + zones.Count + " site security zone(s)");
            return true;
        }

        public bool GetSecurityPolicySet(out List<SecurityPolicySet> policySets)
        {
            policySets = null;
            string url = BuildUrl("securitypolicysets");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSecurityPolicySet no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSecurityPolicySet non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSecurityPolicySet no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSecurityPolicySet response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            policySets = resResp.GetItems<List<SecurityPolicySet>>();

            Debug.WriteLine("GetSecurityPolicySet returning " + policySets.Count + " security policy set(s)");
            return true;
        }

        public bool GetSecurityPolicyRules(string secPolicySetId, out List<SecurityPolicyRule> rules)
        {
            rules = null;
            if (String.IsNullOrEmpty(secPolicySetId)) throw new ArgumentNullException(nameof(secPolicySetId));

            string url = BuildUrl("securitypolicyrules");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", secPolicySetId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSecurityPolicyRules no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSecurityPolicyRules non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSecurityPolicyRules no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSecurityPolicyRules response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            rules = resResp.GetItems<List<SecurityPolicyRule>>();

            Debug.WriteLine("GetSecurityPolicyRules returning " + rules.Count + " security policy rule(s)");
            return true;
        }

        public bool GetSiteWanInterfaces(string siteId, out List<SiteWanInterface> interfaces)
        {
            interfaces = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));

            string url = BuildUrl("waninterfaces");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSiteWanInterfaces no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSiteWanInterfaces non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSiteWanInterfaces no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSiteWanInterfaces response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            interfaces = resResp.GetItems<List<SiteWanInterface>>();

            Debug.WriteLine("GetSiteWanInterfaces returning " + interfaces.Count + " site WAN interface(s)");
            return true;
        }

        public bool GetSiteTopology(string siteId, out Topology topology)
        {
            topology = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));

            string url = BuildUrl("topology");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);
 
            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                TopologyRequest(siteId));

            if (resp == null)
            {
                Debug.WriteLine("GetSiteTopology no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSiteTopology non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSiteTopology no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSiteTopology response: " + Encoding.UTF8.GetString(resp.Data));

            topology = Common.DeserializeJson<Topology>(resp.Data);
            return true;
        }

        public bool GetSnmpAgents(string siteId, string elementId, out List<SnmpAgent> agents)
        {
            agents = null;
            if (String.IsNullOrEmpty(siteId)) throw new ArgumentNullException(nameof(siteId));
            if (String.IsNullOrEmpty(elementId)) throw new ArgumentNullException(nameof(elementId));

            string url = BuildUrl("snmpagents");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", siteId);
            url = Common.StringReplaceFirst(url, "%s", elementId);
 
            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetSnmpAgents no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetSnmpAgents non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetSnmpAgents no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetSnmpAgents response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resResp = Common.DeserializeJson<ResourceResponse>(resp.Data);
            agents = resResp.GetItems<List<SnmpAgent>>();

            Debug.WriteLine("GetSnmpAgents returning " + agents.Count + " SNMP agent(s)");
            return true;
        }

        public bool GetMetrics(MetricsQuery query, out MetricsResponse metrics)
        {
            metrics = null;
            if (query == null) throw new ArgumentNullException(nameof(query));

            string url = BuildUrl("metrics_monitor");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                Encoding.UTF8.GetBytes(Common.SerializeJson(query, true)));

            if (resp == null)
            {
                Debug.WriteLine("GetMetrics no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetMetrics non-200/201 status returned from server for URL " + url);
                Debug.WriteLine("Request:");
                Debug.WriteLine(Common.SerializeJson(query, true));
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetMetrics no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetMetrics response: " + Encoding.UTF8.GetString(resp.Data));
            metrics = Common.DeserializeJson<MetricsResponse>(resp.Data);
            return true;
        }

        public bool GetEvents(EventQuery query, out EventResponse events)
        {
            events = null;
            if (query == null) throw new ArgumentNullException(nameof(query));

            string url = BuildUrl("query_events");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            Debug.WriteLine("GetEvents sending query: ");
            Debug.WriteLine(Common.SerializeJson(query, true));

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                Encoding.UTF8.GetBytes(Common.SerializeJson(query, true)));

            if (resp == null)
            {
                Debug.WriteLine("GetEvents no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetEvents non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetEvents no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetEvents response: " + Encoding.UTF8.GetString(resp.Data));

            events = Common.DeserializeJson<EventResponse>(resp.Data);
            return true;
        }

        public bool GetTopN(TopNQuery query, out TopNResponse topn)
        {
            topn = null;
            if (query == null) throw new ArgumentNullException(nameof(query));

            string url = BuildUrl("topn_monitor");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                Encoding.UTF8.GetBytes(Common.SerializeJson(query, true)));

            if (resp == null)
            {
                Debug.WriteLine("GetTopN no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetTopN non-200/201 status returned from server for URL " + url);
                Debug.WriteLine("Request:");
                Debug.WriteLine(Common.SerializeJson(query, true));
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetTopN no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetTopN response: " + Encoding.UTF8.GetString(resp.Data));
            topn = Common.DeserializeJson<TopNResponse>(resp.Data);
            return true;
        }

        public bool GetFlows(FlowQuery query, out FlowResponse flows)
        {
            flows = null;
            if (query == null) throw new ArgumentNullException(nameof(query));

            string url = BuildUrl("flows_monitor");
            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                Encoding.UTF8.GetBytes(Common.SerializeJson(query, true)));

            if (resp == null)
            {
                Debug.WriteLine("GetFlows no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetFlows non-200/201 status returned from server for URL " + url);
                Debug.WriteLine("Request:");
                Debug.WriteLine(Common.SerializeJson(query, true));
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetFlows no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetFlows response: " + Encoding.UTF8.GetString(resp.Data));
            flows = Common.DeserializeJson<FlowResponse>(resp.Data);
            return true;
        }

        #endregion

        #region Private-Methods

        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_LoggedIn) Logout();
            }

            _Disposed = true;
        }

        private byte[] LoginRequest()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("email", Email);
            ret.Add("password", Password);
            return Encoding.UTF8.GetBytes(Common.SerializeJson(ret, true));
        }

        private byte[] TopologyRequest(string siteId)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("type", "basenet");
            List<string> nodes = new List<string>() { siteId };
            ret.Add("nodes", nodes);
            return Encoding.UTF8.GetBytes(Common.SerializeJson(ret, true));
        }

        private string BuildUrl(string key)
        {
            string endpoint = _Endpoints.GetEndpoint(key);
            return Endpoint + endpoint;
        }

        private void BuildAuthHeaders(string token)
        {
            _AuthHeaders = new Dictionary<string, string>();
            _AuthHeaders.Add("x-auth-token", token);
        }

        private bool BuildEndpoints()
        {
            string url = BuildUrl("permissions");

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("BuildEndpoints no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("BuildEndpoints non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("BuildEndpoints no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("BuildEndpoints response: " + Encoding.UTF8.GetString(resp.Data));

            dynamic respDict = Common.DeserializeJson<dynamic>(resp.Data);
            if (respDict.resource_version_map == null
                || respDict.resource_uri_map == null)
            {
                Debug.WriteLine("BuildEndpoints response data does not include resource_version_map or resource_uri_map");
                return false;
            }

            // populate version
            JObject verMap = (JObject)respDict.resource_version_map;
            foreach (var currVer in verMap)
            {
                if (String.IsNullOrEmpty(currVer.Key) || String.IsNullOrEmpty(currVer.Value.ToString())) continue;
                _Endpoints.AddVersion(currVer.Key, currVer.Value.ToString());  
            }

            // populate endpoints
            JObject epMap = (JObject)respDict.resource_uri_map;
            foreach (var currMap in epMap)
            { 
                if (String.IsNullOrEmpty(currMap.Key) || String.IsNullOrEmpty(currMap.Value.ToString())) continue;
                _Endpoints.AddEndpoint(currMap.Key, currMap.Value.ToString()); 
            }
             
            return true;
        }

        private bool GetTenantId()
        {
            string url = BuildUrl("profile");

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetTenantId no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetTenantId non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetTenantId no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetTenantId response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("tenant_id"))
            {
                Debug.WriteLine("GetTenantId no tenant_id key found in response data");
                return false;
            }
            else if (!String.IsNullOrEmpty(respDict["tenant_id"].ToString()))
            {
                TenantId = respDict["tenant_id"].ToString(); 
                Debug.WriteLine("GetTenantId tenant ID found in response: " + TenantId);
            }
            else
            {
                Debug.WriteLine("GetTenantId null or empty tenant ID found in response");
                return false;
            }

            return true;
        }

        #endregion
    }
}

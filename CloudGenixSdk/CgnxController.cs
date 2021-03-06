﻿/*

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
        public string OperatorId { get; private set; }

        #endregion

        #region Private-Members

        private string _SamlRequestId = null;
        private bool _LoggedIn = false;
        private bool _Disposed = false;
        private Dictionary<string, string> _AuthHeaders = null;
        private EndpointManager _Endpoints;

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Initializes CloudGenix SDK for credential-based login.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
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

        /// <summary>
        /// Initializes CloudGenix SDK for SAML login.
        /// </summary>
        /// <param name="email">Email.</param>
        public CgnxController(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email)); 

            IgnoreCertErrors = true;
            Email = email;
            Password = null;
            Endpoint = "https://api.cloudgenix.com:443";

            _Endpoints = new EndpointManager();

            Debug.WriteLine("CgnxController initialized");
        }

        /// <summary>
        /// Initializes the CloudGenix SDK for static auth token login.
        /// </summary>
        /// <param name="token">Authentication token.</param>
        /// <param name="isToken">Ignore, set to either true or false.</param>
        public CgnxController(string token, bool isToken)
        {
            if (String.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));

            IgnoreCertErrors = true;
            Email = null;
            Password = null;
            AuthToken = token;
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
            GetTenantAndOperatorId();

            Debug.WriteLine("Login authenticated successfully");

            _LoggedIn = true;

            return true;
        }

        public bool LoginSamlStart(out string loginUrl)
        {
            loginUrl = null;
            string url = BuildUrl("login");

            _AuthHeaders = new Dictionary<string, string>();
            string refererUrl = Endpoint + "/v2.0/api/login";
            _AuthHeaders.Add("Referer", refererUrl);

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                LoginSamlStartRequest());

            if (resp == null)
            {
                Debug.WriteLine("LoginSamlStart no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("LoginSamlStart non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("LoginSamlStart no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("LoginSamlStart response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("requestId"))
            {
                Debug.WriteLine("LoginSamlStart no requestId key found in response data");
                return false;
            }
            else
            {
                _SamlRequestId = respDict["requestId"].ToString();
            }

            if (!respDict.ContainsKey("urlpath"))
            {
                Debug.WriteLine("LoginSamlStart no urlpath key found in response data");
                return false;
            }
    
            loginUrl = respDict["urlpath"].ToString();
            Debug.WriteLine("LoginSamlStart referring user to URL " + loginUrl);
            return true; 
        }

        public bool LoginSamlFinish()
        { 
            string url = BuildUrl("login");

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                LoginSamlFinishRequest());

            if (resp == null)
            {
                Debug.WriteLine("LoginSamlFinish no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("LoginSamlFinish non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("LoginSamlFinish no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("LoginSamlFinish response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("x_auth_token"))
            {
                Debug.WriteLine("LoginSamlFinish no x_auth_token key found in response data");
                return false;
            }
            else
            {
                BuildAuthHeaders(respDict["x_auth_token"].ToString());
                AuthToken = respDict["x_auth_token"].ToString();
            }

            if (respDict.ContainsKey("api_endpoint") && !String.IsNullOrEmpty(respDict["api_endpoint"].ToString()))
            {
                Debug.WriteLine("LoginSamlFinish new API endpoint found in response: " + respDict["api_endpoint"]);
                Endpoint = respDict["api_endpoint"].ToString();

                if (!Endpoint.EndsWith("/")) Endpoint += "/";
            }

            Debug.WriteLine("LoginSamlFinish building endpoints");
            BuildEndpoints();

            Debug.WriteLine("LoginSamlFinish retrieving tenant ID");
            GetTenantAndOperatorId();

            Debug.WriteLine("LoginSamlFinish authenticated successfully");

            _LoggedIn = true;

            return true;
        }

        public bool LoginWithToken()
        { 
            BuildAuthHeaders(AuthToken); 

            Debug.WriteLine("LoginWithToken building endpoints");
            BuildEndpoints();

            Debug.WriteLine("LoginWithToken retrieving tenant ID");
            GetTenantAndOperatorId();

            Debug.WriteLine("LoginWithToken authenticated successfully");

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

        public bool GetClients(out List<Client> clients)
        {
            #region Retrieve-Clients

            clients = null;
            string url = BuildUrl("clients_t");
            if (String.IsNullOrEmpty(url))
            {
                Debug.WriteLine("GetClients MSP/ESP login not available for this login (clients URL not found in digest)");
                return false;
            }

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
                Debug.WriteLine("GetClients no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetClients non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetClients no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetClients response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resRespClients = Common.DeserializeJson<ResourceResponse>(resp.Data);
            clients = resRespClients.GetItems<List<Client>>();

            Debug.WriteLine("GetClients found " + clients.Count + " client(s)");

            #endregion

            #region Retrieve-Permissions

            url = BuildUrl("permissions_clients_d");
            if (String.IsNullOrEmpty(url))
            {
                Debug.WriteLine("GetClients MSP/ESP login not available for this login (permissions URL not found in digest)");
                return false;
            }

            url = Common.StringReplaceFirst(url, "%s", TenantId.ToString());
            url = Common.StringReplaceFirst(url, "%s", OperatorId.ToString());

            resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "GET",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                null);

            if (resp == null)
            {
                Debug.WriteLine("GetClients no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetClients non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetClients no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetClients response: " + Encoding.UTF8.GetString(resp.Data));

            ResourceResponse resRespPerms = Common.DeserializeJson<ResourceResponse>(resp.Data);
            List<ClientPermission> permissions = resRespPerms.GetItems<List<ClientPermission>>();

            Debug.WriteLine("GetClients found " + permissions.Count + " client permission(s)");

            #endregion

            #region Filter-and-Respond

            List<Client> filtered = new List<Client>();
            foreach (Client currClient in clients)
            {
                foreach (ClientPermission currPerm in permissions)
                {
                    if (currPerm.ClientId == currClient.Id)
                    {
                        filtered.Add(currClient);
                        break;
                    }
                }
            }

            clients = filtered;
            Debug.WriteLine("GetClients returning " + clients.Count + " permitted client(s)");
            return true;

            #endregion
        }

        public bool LoginAsClient(string clientId)
        {
            if (String.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
             
            string url = BuildUrl("login_clients"); 
            url = Common.StringReplaceFirst(url, "%s", TenantId);
            url = Common.StringReplaceFirst(url, "%s", clientId);

            string reqBody = "{}";

            RestResponse resp = RestRequest.SendRequestSafe(
                url,
                "application/json",
                "POST",
                null, null, false, IgnoreCertErrors,
                _AuthHeaders,
                Encoding.UTF8.GetBytes(reqBody));

            if (resp == null)
            {
                Debug.WriteLine("LoginAsClient no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("LoginAsClient non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("LoginAsClient no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("LoginAsClient response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("x_auth_token"))
            {
                Debug.WriteLine("LoginAsClient no x_auth_token key found in response data");
                return false;
            }
            else
            {
                BuildAuthHeaders(respDict["x_auth_token"].ToString());
                AuthToken = respDict["x_auth_token"].ToString();
            }

            if (respDict.ContainsKey("api_endpoint") && !String.IsNullOrEmpty(respDict["api_endpoint"].ToString()))
            {
                Debug.WriteLine("LoginAsClient new API endpoint found in response: " + respDict["api_endpoint"]);
                Endpoint = respDict["api_endpoint"].ToString();

                if (!Endpoint.EndsWith("/")) Endpoint += "/";
            }

            Debug.WriteLine("LoginAsClient building endpoints");
            BuildEndpoints();

            Debug.WriteLine("LoginAsClient retrieving tenant ID");
            GetTenantAndOperatorId();

            Debug.WriteLine("LoginAsClient authenticated successfully");

            _LoggedIn = true;

            return true;
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

        public bool GetAllEvents(EventQuery query, out EventResponse events)
        {
            events = null;
            if (query == null) throw new ArgumentNullException(nameof(query));

            while (true)
            {
                EventResponse currResponse = null;
                if (!GetEvents(query, out currResponse))
                {
                    Debug.WriteLine("GetAllEvents failed to retrieve events");
                    return false;
                }

                if (events == null)
                {
                    // replace
                    events = currResponse;
                    if (events.Events == null)
                    {
                        events.Events = new List<EventResponse.EventDetails>();
                    }
                }
                else 
                {
                    // amend
                    if (currResponse != null)
                    {
                        if (events.Events == null)
                        {
                            events.Events = new List<EventResponse.EventDetails>();
                        }

                        if (currResponse.Events != null && currResponse.Events.Count > 0)
                        {
                            foreach (EventResponse.EventDetails currEventDetails in currResponse.Events)
                            {
                                events.Events.Add(currEventDetails);
                            }

                            events.IncludedCount += currResponse.Events.Count;
                            events.TotalCount += currResponse.Events.Count;
                        }

                        if (!String.IsNullOrEmpty(currResponse.Offset))
                        {
                            query.Offset = currResponse.Offset;
                        }
                        else
                        {
                            // end reached
                            return true;
                        }
                    } 
                }
            }
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

        private byte[] LoginSamlStartRequest()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("email", Email);
            return Encoding.UTF8.GetBytes(Common.SerializeJson(ret, true));
        }

        private byte[] LoginSamlFinishRequest()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("email", Email);
            ret.Add("requestId", _SamlRequestId);
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

        private bool GetTenantAndOperatorId()
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
                Debug.WriteLine("GetTenantAndOperatorId no response received from server for URL " + url);
                return false;
            }

            if (resp.StatusCode != 200 && resp.StatusCode != 201)
            {
                Debug.WriteLine("GetTenantAndOperatorId non-200/201 status returned from server for URL " + url);
                Debug.WriteLine(resp.ToString());
                return false;
            }

            if (resp.Data == null || resp.Data.Length < 1)
            {
                Debug.WriteLine("GetTenantAndOperatorId no data returned from server for URL " + url);
                return false;
            }

            Debug.WriteLine("GetTenantAndOperatorId response: " + Encoding.UTF8.GetString(resp.Data));

            Dictionary<string, object> respDict = Common.DeserializeJson<Dictionary<string, object>>(resp.Data);
            if (!respDict.ContainsKey("tenant_id"))
            {
                Debug.WriteLine("GetTenantAndOperatorId no tenant_id key found in response data");
                return false;
            }
            else if (!String.IsNullOrEmpty(respDict["tenant_id"].ToString()))
            {
                TenantId = respDict["tenant_id"].ToString(); 
                Debug.WriteLine("GetTenantAndOperatorId tenant ID found in response: " + TenantId);
            }
            else
            {
                Debug.WriteLine("GetTenantAndOperatorId null or empty tenant ID found in response");
                return false;
            }

            if (respDict.ContainsKey("id"))
            {
                OperatorId = respDict["id"].ToString();
                Debug.WriteLine("GetTenantAndOperatorId operator ID found in response: " + OperatorId);
            }

            return true;
        }

        #endregion
    }
}


# CloudGenix Controller SDK in C#
C# software development kit and test application for the CloudGenix Controller.

## Help or Feedback
For issues, please contact joel@cloudgenix.com or open a support ticket with developers@cloudgenix.com.  We can also be found on the NetworkToCode Slack #cloudgenix channel at http://slack.networktocode.com.

## Before You Begin
The CloudGenix Controller is only accessible to CloudGenix customers with a valid login using an IP address that has been whitelisted.  Please contact us at one of the aforementioned methods if you need to have your IP addresses whitelisted.

## New
- Support for managed service provider (MSP) and enterprise service provider (ESP) login and client emulation; use ```GetClients()``` method followed by ```LoginAsClient()``` method
- Test project for MSP/ESP login (refer to ```MspTest``` project)

## Outstanding Items
- Several classes contain members with generic types, which will require casting prior to use in consuming code.  This can be fixed with additional details on the object model from engineering
- Parameters in query bodies are currently strings when they should be enumerations

## Quickstart
Refer to the SdkCli project for a full examination of consuming the SDK.  The SDK can be initialized and instantiated rather quickly:
```
using CloudGenix;
using CloudGenix.Classes;
...
CgnxController sdk = new CgnxController("your email", "your password");
if (!sdk.Login()) { // handle errors } 

// From here, you can use the SDK APIs.  All APIs are of the form:
if (!sdk.GetContexts(out _Contexts)) { // handle errors }
```

## Running under Mono
This library should work in Mono environments.  It is recommended that when running under Mono, you execute the containing EXE using --server and after using the Mono Ahead-of-Time Compiler (AOT).
```
mono --aot=nrgctx-trampolines=8096,nimt-trampolines=8096,ntrampolines=4048 --server myapp.exe
mono --server myapp.exe
```

## Version History
Notes from previous versions (starting with v1.0.0) will be moved here.

v1.0.x
- Initial release
- Authentication, profile retrieval, and dynamic URL building (including API version) with override support
- Includes GET APIs for tenant, elements, sites, interfaces, WANs, LANs, application definitions, policy sets, policy rules, security zones, security policy sets, and security policy rules
- Includes POST APIs to retrieve metrics data, top N data, and flow records
- Basic API infrastructure and plumbing
- Etags in resources
- SAML login support via ```LoginSamlStart``` and ```LoginSamlFinish``` methods
- SAML login test project
- Static auth token login support via ```LoginWithToken``` and constructor ```CgnxController(token, true)```
- Static auth token login test project
- ```GetAllEvents()``` method

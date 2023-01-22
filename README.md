# Micro Web Server  [![xiosec - micro-web-server](https://img.shields.io/static/v1?label=xiosec&message=micro-web-server&color=blue&logo=github)](https://github.com/xiosec/micro-web-server)
[![stars - micro-web-server](https://img.shields.io/github/stars/xiosec/micro-web-server?style=social)](https://github.com/xiosec/micro-web-server)
[![forks - micro-web-server](https://img.shields.io/github/forks/xiosec/micro-web-server?style=social)](https://github.com/xiosec/micro-web-server) [![GitHub release](https://img.shields.io/github/release/xiosec/micro-web-server?include_prereleases=&sort=semver)](https://github.com/xiosec/micro-web-server/releases/)
[![License](https://img.shields.io/badge/License-MIT-blue)](#license)
[![issues - micro-web-server](https://img.shields.io/github/issues/xiosec/micro-web-server)](https://github.com/xiosec/micro-web-server/issues)

A micro web server for a restful api and etc...
<br>
This project is for my associate thesis.
<br>
In this project, we have tried to design a simple tool so that programmers of other languages ​​such as Python can set up their web server with C #.

## Languages and Tools
<p align="left"> <a href="https://getbootstrap.com" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/bootstrap/bootstrap-plain-wordmark.svg" alt="bootstrap" width="40" height="40"/> </a> <a href="https://www.w3schools.com/cs/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://www.w3schools.com/css/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/css3/css3-original-wordmark.svg" alt="css3" width="40" height="40"/> </a> <a href="https://dotnet.microsoft.com/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original-wordmark.svg" alt="dotnet" width="40" height="40"/> </a> <a href="https://git-scm.com/" target="_blank"> <img src="https://www.vectorlogo.zone/logos/git-scm/git-scm-icon.svg" alt="git" width="40" height="40"/> </a> <a href="https://www.w3.org/html/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/html5/html5-original-wordmark.svg" alt="html5" width="40" height="40"/> </a> <a href="https://developer.mozilla.org/en-US/docs/Web/JavaScript" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/javascript/javascript-original.svg" alt="javascript" width="40" height="40"/> </a> <a href="https://www.microsoft.com/en-us/sql-server" target="_blank"> <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="mssql" width="40" height="40"/> </a> <a href="https://www.nginx.com" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/nginx/nginx-original.svg" alt="nginx" width="40" height="40"/> </a> <a href="https://nodejs.org" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/nodejs/nodejs-original-wordmark.svg" alt="nodejs" width="40" height="40"/> </a> <a href="https://www.typescriptlang.org/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/typescript/typescript-original.svg" alt="typescript" width="40" height="40"/> </a> </p>


# Capabilities
Logging (console and syslog)
<br>
Middleware
<br>
Fast and Secure
&...
# Example
```csharp

public static void Info(Requests requests, Response response)
{
    Dictionary<string, string> myInfo = new Dictionary<string, string>()
    {
        {"name",requests.getArg("name","null")},
        {"age",requests.getArg("age","null") },
        {"github","https://github.com/xiosec" },
    };
    response.sendJson(myInfo, 200);

}
        
static void Main(string[] args)
{
    ConsoleLog consoleLog = new ConsoleLog();
    Dictionary<string, Action<Requests, Response>> urlPatterns = new Dictionary<string, Action<Requests, Response>>()
    {
        {@"^\/info\?name\=[a-z]+\&age=\d+$", Info},
    };

    Server server = new Server(IPAddress.Parse("127.0.0.1"), 8080, 10, urlPatterns, consoleLog);
    if (server.Start())
    {
        consoleLog.Informational("Started");
    }
}
```
```
http://127.0.0.1:8080/info?name=xiosec&age=19
```
# Request
Request Information:
```csharp
//request method (GET,POST,PUT,DELETE,...)
request.requestInfo["method"]

//request full path
request.requestInfo["path"]

//request http version
request.requestInfo["httpVersion"]

```
Headers:
```csharp
//get header value with key
request.getHeader(key,defaultValue)
```
Cookies:
```csharp
//get cookie value with key
request.getCookie(key,defaultValue)
```
URL Parameters:
```csharp
//get URL parameter with key
request.getArg(key,defaultValue)
```
Authorization:
```csharp
request.getAuthHeader() -> {"type","key"}
```
Body:
```csharp
//Receive body value as a string
request.body
```
# Response
Headers:
```csharp
//set header
response.header["key"]="value"
```
Cookies:
```csharp
//set cookie
response.cookie["key"]="value"
```
Content type and Status code:
```csharp
response.extensions["name"]
response.statusCode["code"]
```
Send response:
```csharp
response.send200Ok(Content , contentType)
response.send(Content , statusCode , contentType)
response.sendJson(Content , statusCode)
response.sendNotFound(Content , contentType)
```
Security:
```csharp
//Set xss protection header
response.setSecurityHeader()
//The text generates a secure response
response.safeResponse(response)
```
Redirect:
```csharp
//redirect to path
response.redirect("/path");
```
# Middleware
An example of setting up a middleware:
```csharp
public static (Requests,Response) AccessControllMiddleware(Requests requests, Response response)
{
    response.header["Access-Control-Allow-Origin"] = "*";
    response.header["Access-Control-Allow-Headers"] = "Content-Type, Content-Length, Accept-Encoding";
    return (requests, response);
}

...

server.Middlewares.Add(AccessControlMiddleware);
server.Start();
```
## License

Released under [MIT](/LICENSE) by [@xiosec](https://github.com/xiosec).

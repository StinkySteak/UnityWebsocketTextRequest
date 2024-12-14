# Websocket Text Transport
a HttpClient/Server-like for unity by utilizing UnityTransport (UTP)

This plugin is for unity game developer who wants to host a text server.

- If you are looking for UDP Version, check out [UDPTextRequest](https://github.com/StinkySteak/UnityUDPTextRequest)

## Features
- Simple
- Only supports text format (UTF-8)
- Max of 512 charactes can be sent (using `FixedString512`)

## Installation
#### Install via git URL (Package Manager)
```
https://github.com/StinkySteak/UnityWebsocketTextRequest.git
```

## Compatibility
| Name           	| Description    	|
|----------------	|----------------	|
| Unity Version  	| 2022 or later  	|
| Platform       	| All including WebGL |
| UnityTransport 	| 2.0.0 or later 	|
| HTTPS 	        | Not supported yet |

## Dependencies
- UnityTransport 2.0.0 or more

## Example Use case
- Retrieving session data from a running game server without connecting to the game
    - Players count
    - Match status

## Usage examples
### Server
```cs
public int ServerPort;
public string Content;
private WebSocketTextServer _httpServer;

private void Start()
{
    _httpServer = new WebSocketTextServer(ServerPort);
    _httpServer.Start();
    _httpServer.SetContent(Content);
}

private void Update()
{
    _httpServer.PollUpdate();
}

private void OnDestroy()
{
    _httpServer.Stop();
}
```

### Client
```cs
public int ServerPort;
public string Url;
public string Result;
private WebSocketTextClient _httpClient;

private IEnumerator Start()
{
    _httpClient = new WebSocketTextClient(Url, ServerPort);

    _httpClient.SendRequest();

    while (!_httpClient.IsDone)
    {
        _httpClient.PollUpdate();
        yield return null;
    }

    Result = _httpClient.Text;
    _httpClient.Dispose();
}
```

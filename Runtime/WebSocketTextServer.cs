using Unity.Networking.Transport;
using UnityEngine;

namespace StinkySteak.Networking
{
    public class WebSocketTextServer
    {
        private int _listenPort;
        private NetworkDriver _driver;
        private string _text;

        public WebSocketTextServer( int listenPort)
        {
            _listenPort = listenPort;
        }

        public void SetContent(string content)
        {
            _text = content;
        }

        public void Start()
        {
            _driver = NetworkDriver.Create(new WebSocketNetworkInterface());

            NetworkEndpoint endpoint = NetworkEndpoint.AnyIpv4.WithPort((ushort)_listenPort);

            if (_driver.Bind(endpoint) != 0)
            {
                Debug.LogError($"Failed to bind to port {_listenPort}");
                return;
            }

            _driver.Listen();
            Debug.Log($"[{nameof(WebSocketTextServer)}]: Server is listening at: {_listenPort}");
        }

        public void PollUpdate()
        {
            if (_driver.IsCreated && _driver.Listening)
            {
                _driver.ScheduleUpdate().Complete();

                NetworkConnection c;
                while ((c = _driver.Accept(out var payload)) != default)
                {
                    _driver.BeginSend(NetworkPipeline.Null, c, out var writer);
                    writer.WriteFixedString512(_text);
                    _driver.EndSend(writer);

                    _driver.Disconnect(c);
                }
            }
        }

        public void Stop()
        {
            _driver.Dispose();
        }
    }
}
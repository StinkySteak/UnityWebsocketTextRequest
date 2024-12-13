using Unity.Collections;
using Unity.Networking.Transport;

namespace StinkySteak.Networking
{
    public class WebSocketTextClient
    {
        private string _url;
        private int _serverPort;
        private NetworkDriver _driver;
        private NetworkConnection _serverConnection;
        private string _text;
        private bool _isDone;
        private bool _isError;
        public string Text => _text;
        public bool IsDone => _isDone;
        public bool IsError => _isError;

        public WebSocketTextClient(string url, int serverPort)
        {
            _url = url;
            _serverPort = serverPort;
        }

        public void SendRequest()
        {
            NetworkEndpoint endpoint = NetworkEndpoint.Parse(_url, (ushort)_serverPort);

            _driver = NetworkDriver.Create(new WebSocketNetworkInterface());

            _serverConnection = _driver.Connect(endpoint);
        }

        public void PollUpdate()
        {
            if (_driver.IsCreated)
            {
                _driver.ScheduleUpdate().Complete();

                NetworkEvent.Type cmd;

                while ((cmd = _driver.PopEventForConnection(_serverConnection, out DataStreamReader stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Connect)
                    {
                        
                    }

                    if (cmd == NetworkEvent.Type.Data)
                    {
                        _text = stream.ReadFixedString512().ToString();
                        _isDone = true;
                    }

                    if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        if (!_isDone)
                            _isError = true;

                        _isDone = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            _driver.Dispose();
        }
    }
}
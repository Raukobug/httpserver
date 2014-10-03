using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace httpserver
{
    public class HttpServer
    {
        readonly Config _config = new Config();
        private bool _listener = true;
        private readonly List<Task<int>> _tlist = new List<Task<int>>();

        readonly EventLog _myLog = new EventLog();
        private readonly TcpListener _serverSocket;
        private readonly TcpListener _stopSocket;

        public HttpServer()
        {
            _serverSocket = new TcpListener(IPAddress.Any, _config.ServerPort);
            _stopSocket = new TcpListener(IPAddress.Any, _config.ShutdownPort);
        }
        public void StartServer()
        {
            _myLog.Source = "MyServer";
            //creates a server socket/listner/welcome socket

            _serverSocket.Start();
            _myLog.WriteEntry("Server startup.",EventLogEntryType.Information, 1);

            //As long no key has been pressed keep the server runing for one more entry.
            while (_listener)
            {
                //creates a connectionSocket by accepting the connection request from the client
                var connectionSocket = _serverSocket.AcceptSocket();
                    var rr = new ReadingRequest(connectionSocket);

                    _tlist.Add(Task.Run(() => rr.SocketHandler()));   

                //network stream for the connected client; to read from or write to
            }

        }

        public void StopServer()
        {
            _stopSocket.Start();
            _stopSocket.AcceptSocket();
            _stopSocket.Stop();
            Stop();
        }

        public void Stop()
        {
// ReSharper disable CoVariantArrayConversion
            Task.WaitAll(_tlist.ToArray());
// ReSharper restore CoVariantArrayConversion
            _listener = false;
            var client = new TcpClient("localhost", _config.ServerPort);
            client.Close();
            _serverSocket.Stop();
            _myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);
        }
    }
}

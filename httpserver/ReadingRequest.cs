using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace httpserver
{
    class ReadingRequest
    {
        private readonly Socket _connectionSocket;
        readonly EventLog _myLog = new EventLog();
        public ReadingRequest(Socket connectionSocket)
        {
            _connectionSocket = connectionSocket;
            _myLog.Source = "MyServer";
        }

        public int SocketHandler()
        {
            var ns = new NetworkStream(_connectionSocket);
            var sr = new StreamReader(ns, Encoding.UTF8);

            //formats the input from the stream to a useful format
            string clientRequest = sr.ReadLine();
            _myLog.WriteEntry("Client request: " + clientRequest, EventLogEntryType.Information, 2);
            var hr = new HandlingRequest(clientRequest, ns, _connectionSocket);
            hr.Handling();
            return 0; //Returns 0 so that the Task.WaitAll in HttpServer knows when this thread is done
        }
    }
}

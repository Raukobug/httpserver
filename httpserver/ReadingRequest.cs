using System.IO;
using System.Net.Sockets;
using System.Text;

namespace httpserver
{
    class ReadingRequest
    {
        private readonly Socket _connectionSocket;

        public ReadingRequest(Socket connectionSocket)
        {
            _connectionSocket = connectionSocket;
        }

        public int SocketHandler()
        {
            var ns = new NetworkStream(_connectionSocket);
            var sr = new StreamReader(ns, Encoding.UTF8);

            //formates the input form the stream to a usefull formate
            string srtext = sr.ReadLine();
            //_myLog.WriteEntry("Client request: " + srtext, EventLogEntryType.Information, 2);
            var hr = new HandlingRequest(srtext, ns, _connectionSocket);
            hr.Handling();
            return 0;
        }
    }
}

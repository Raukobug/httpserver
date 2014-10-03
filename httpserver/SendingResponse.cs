using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    class SendingResponse
    {
        private readonly NetworkStream _ns;
        private readonly string _httpResponse;
        private readonly string _path;
        readonly EventLog _myLog = new EventLog();

        public SendingResponse(NetworkStream ns, string httpResponse, string path)
        {
            _ns = ns;
            _httpResponse = httpResponse;
            _path = path;
            _myLog.Source = "MyServer";
        }

        public void Response()
        {
            var sw = new StreamWriter(_ns) { AutoFlush = true };
            sw.Write(_httpResponse);

            using (FileStream file = File.OpenRead(_path))
            {
                file.CopyTo(_ns);
            }
            _myLog.WriteEntry("Server response: " + _httpResponse, EventLogEntryType.Information, 3);
        }
    }
}

using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    class SendingRespons
    {
        private readonly NetworkStream _ns;
        private readonly string _httpRespons;
        private readonly string _content;
        private readonly string _path;
        readonly EventLog _myLog = new EventLog();

        public SendingRespons(NetworkStream ns, string content, string httpRespons, string path)
        {
            _ns = ns;
            _content = content;
            _httpRespons = httpRespons;
            _path = path;
            _myLog.Source = "MyServer";
        }

        public void Respons()
        {
            var sw = new StreamWriter(_ns) { AutoFlush = true };

            sw.Write(_httpRespons);
            //sw.Write(_content);
            using (FileStream file = File.OpenRead(_path))
            {
                file.CopyTo(_ns);
            }
            _myLog.WriteEntry("Server respons: " + _httpRespons, EventLogEntryType.Information, 3);
        }
    }
}

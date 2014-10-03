using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    class SendingResponse
    {
        private readonly NetworkStream _ns;
        private readonly string _httpResponse;
        private readonly string _content;
        private readonly string _path;
        readonly EventLog _myLog = new EventLog();

        public SendingResponse(NetworkStream ns, string content, string httpResponse)
        {
            _ns = ns;
            _content = content;
            _httpResponse = httpResponse;
            _path = path;
            _myLog.Source = "MyServer";
        }

        public void Response()
        {
            var sw = new StreamWriter(_ns) { AutoFlush = true };
            sw.Write(_httpResponse);

            //sw.Write(_content);
            using (FileStream file = File.OpenRead(_path))
            {
                file.CopyTo(_ns);
            }
        }
    }
}

using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    class SendingRespons
    {
        private readonly NetworkStream _ns;
        private readonly string _httpRespons;
        private readonly string _content;

        public SendingRespons(NetworkStream ns, string content, string httpRespons)
        {
            _ns = ns;
            _content = content;
            _httpRespons = httpRespons;
        }

        public void Respons()
        {
            var sw = new StreamWriter(_ns) { AutoFlush = true };
            sw.Write(_httpRespons);
            sw.Write(_content);
        }
    }
}

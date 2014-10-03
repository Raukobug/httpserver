using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace httpserver
{
    class HandlingRequest
    {
        readonly Config _config = new Config();
        private readonly string _request;
        private static string _rootCatalog;
        private const string VersionHttp = "HTTP/1.0"; //Change to 1.1 during unit test
        private readonly NetworkStream _ns;
        private readonly Socket _connectionSocket;
        private string _path;
        private string _httpResponse;

        public HandlingRequest(string request, NetworkStream ns, Socket connectionSocket)
        {
            _request = request;
            _ns = ns;
            _connectionSocket = connectionSocket;
            _rootCatalog = _config.RootCatalog;
        }

        public void Handling()
        {
            if (_request != null)
            {
                string[] words = _request.Split(' ');
                string getFile = WebUtility.UrlDecode(words[1].Replace("/", "\\")); //Decodes ex. %20 to space
                _path = _rootCatalog + getFile;
                if (getFile == "\\")
                {
                    _path = _rootCatalog + "\\" + _config.WelcomeFile;
                }
            }
            string extension = Path.GetExtension(_path); //Saves the extension of the path
            var sh = new StatusHandler(_request, _path);
            var cth = new ContentTypeHandler(extension); //Gets the correct output for the HTTP response (ex .HTML = text/html)
            string content = ""; //Used to store content later.
            string fileLastEdit = Convert.ToString(File.GetLastWriteTime(_path));
            string timeRightNow = DateTime.Today.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(); //Gets Date and Time
            try
            {
                //If no get is set, look for the index.html.
                if (File.Exists(_path))
                {
                    //Opens the file from the path and puts each byte in the file into a new string.
                    using (FileStream fs = File.OpenRead(_path))
                    {
                        var b = new byte[fs.Length];
                        var temp = new UTF8Encoding(true);
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            content += temp.GetString(b); //Adds to the end of the string
                        }
                    }
                    _httpResponse = VersionHttp + " " + sh.ServerResponse() + "\r\n" + cth.ContentTypeLookUp() + "\r\n" + "Content-Lenght: " + content.Length + "\r\n\r\n";
                    var sendRespons = new SendingResponse(_ns, _httpResponse, _path);
                    sendRespons.Response();
                }
                else
                {
                    _httpResponse = VersionHttp + " " + sh.ServerResponse() + "\r\n" + cth.ContentTypeLookUp() + "\r\n" + "Content-Lenght: " + content.Length + "\r\n\r\n";
                    var sendRespons = new SendingResponse(_ns, _httpResponse, _path);
                    sendRespons.Response();
                }
            }
            finally
            {
                _ns.Close();
                _connectionSocket.Close();
                Console.Write(_httpResponse + "\nDate today: " + timeRightNow + "\nFile last change: " + fileLastEdit + "\n");
            }
        }
    }
}

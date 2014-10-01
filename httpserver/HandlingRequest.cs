using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace httpserver
{
    class HandlingRequest
    {
        private readonly string _request;
        private static readonly string RootCatalog = Directory.GetCurrentDirectory();
        private const string VersionHttp = "HTTP/1.1"; //Change during unit test
        private readonly NetworkStream _ns;
        private readonly Socket _connectionSocket;
        private string _path;

        public HandlingRequest(string request, NetworkStream ns, Socket connectionSocket)
        {
            _request = request;
            _ns = ns;
            _connectionSocket = connectionSocket;
        }

        public void Handling()
        {
            if (_request != null)
            {
                string[] words = _request.Split(' ');
                string getFile = WebUtility.UrlDecode(words[1].Replace("/", "\\"));
                _path = RootCatalog + getFile;
                if (getFile == "\\")
                {
                    _path = RootCatalog + "\\index.html";
                }
            }
                string extension = Path.GetExtension(_path); //Saves the extension of the path
                var sh = new StatusHandler(_request, _path);
                var cth = new ContentTypeHandler(extension); //Gets the correct output for the HTTP response (ex .HTML = text/html)
                string content = "";
                //If the file exists retun the file else return a error 404 Not Found
                string fileLastEdit = Convert.ToString(File.GetLastWriteTime(_path));
                string timeRightNow = DateTime.Today.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                try
                {
                    //If no get is set take look for the index.html.
                    if (File.Exists(_path))
                    {
                        //Opens the file from the path and take each byte in the file to a new string.
                        using (FileStream fs = File.OpenRead(_path))
                        {
                            var b = new byte[fs.Length];
                            var temp = new UTF8Encoding(true);
                            while (fs.Read(b, 0, b.Length) > 0)
                            {
                                content += temp.GetString(b);
                            }
                        }
                    }
                }
                finally
                {
                    var httpRespons = VersionHttp + " " + sh.ServerRespons() + "\n" + cth.ContentTypeLookUp() + "\n" + "Content-Lenght: " + content.Length;
                    var sendRespons = new SendingRespons(_ns, content, httpRespons);
                    //Console.Write(consoleText + "\n" + cth.ContentTypeLookUp() + "\n" + "Content-Lenght: " + text.Length);
                    sendRespons.Respons();
                    _ns.Close();
                    _connectionSocket.Close();
                    //Console.Write(srtext + "\n"); //Prints the message the server gets from the client
                    Console.Write(_path);
                    Console.Write(httpRespons +  "\nDate today: " + timeRightNow + "\nFile last change: " + fileLastEdit + "\n");
                }
            
        }

    }
}

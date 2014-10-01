using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace httpserver
{
    public class HttpServer
    {
        //Kommentar til at teste GitHub/Merging :)
        public static readonly int DefaultPort = 8080;
        private static readonly string RootCatalog = Directory.GetCurrentDirectory();
        private const string Version = "HTTP/1.0 "; //Change during unit test
        private Socket _connectionSocket;
        private Stream _ns;
        private bool _listener = true;

        readonly EventLog _myLog = new EventLog();
        readonly TcpListener _serverSocket = new TcpListener(IPAddress.Any, DefaultPort);
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
                _connectionSocket = _serverSocket.AcceptSocket();
                //network stream for the connected client; to read from or write to
                _ns = new NetworkStream(_connectionSocket);
                var sr = new StreamReader(_ns, Encoding.UTF8);
                var sw = new StreamWriter(_ns) { AutoFlush = true };
                //formates the input form the stream to a usefull formate
                string srtext = sr.ReadLine();
                _myLog.WriteEntry("Client request: " + srtext, EventLogEntryType.Information, 2);

                if (srtext != null)
                {
                    string[] words = srtext.Split(' ');
                    string getFile = words[1].Replace("/", "\\");
                    string path = RootCatalog + getFile;
                    if (getFile == "\\")
                    {
                        path = RootCatalog + "\\index.html";
                    }
                    if (path.Contains("%22"))
                    {
                        path = path.Replace("%22", "");
                    }
                    string extension = Path.GetExtension(path); //Saves the extension of the path
                    var sh = new StatusHandler(srtext, path);
                    var cth = new ContentTypeHandler(extension); //Gets the correct output for the HTTP response (ex .HTML = text/html)
                    string text = "";
                    string consoleText = sh.ServerRespons();
                    var hg = new HtmlGenerator(sh.ServerRespons(), Version);
                    //If the file exists retun the file else return a error 404 Not Found
                    string fileLastEdit = Convert.ToString(File.GetLastWriteTime(path));
                    try
                    {
                        //If no get is set take look for the index.html.
                        if (File.Exists(path))
                        {
                            consoleText = Version + sh.ServerRespons();
                            //Opens the file from the path and take each byte in the file to a new string.
                            using (FileStream fs = File.OpenRead(path))
                            {
                                var b = new byte[fs.Length];
                                var temp = new UTF8Encoding(true);
                                while (fs.Read(b, 0, b.Length) > 0)
                                {
                                    text += temp.GetString(b);
                                }
                            }
                        }
                        else
                        {
                            consoleText = Version + sh.ServerRespons();
                            text = hg.GetSite();

                        }

                    }
                    finally
                    {
                        sw.Write(consoleText + "\n" + cth.ContentTypeLookUp()+ "\n" + "Content-Lenght: " + text.Length);
                        string timeRightNow = DateTime.Today.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                        //sw.Write(consoleText + " " + cth.ContentTypeLookUp()); //UnitTest - Change depending on the unit test you want to run
                        sw.Write(text);
                        //Console.Write(srtext + "\n"); //Prints the message the server gets from the client
                        Console.Write(consoleText + "\n" + cth.ContentTypeLookUp() + "\nDate today: " + timeRightNow + "\nContent-Lenght: " + text.Length + "\nFile last change: " + fileLastEdit + "\n");
                        _ns.Close();
                        _connectionSocket.Close();
                        _myLog.WriteEntry("Server respons: " + sh.ServerRespons(), EventLogEntryType.Information, 3);
                    }
                }
            }

        }

        public void ServerStop()
        {
            while (Console.KeyAvailable == false)
            {
                
            }
            var client = new TcpClient("localhost", DefaultPort);
            _listener = false;
            _ns.Close();
            _connectionSocket.Close();
            client.Close();
            _serverSocket.Stop();
            _myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);
        }

        public void Stop()
        {
            _myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);
            var client = new TcpClient("localhost", DefaultPort);
            _listener = false;
            _ns.Close();
            _connectionSocket.Close();
            client.Close();
            _serverSocket.Stop();
        }
    }
}

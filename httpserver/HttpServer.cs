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
        private const string Version = "HTTP/1.0 ";

        readonly EventLog _myLog = new EventLog();
        public void StartServer()
        {
            _myLog.Source = "MyServer";
            //creates a server socket/listner/welcome socket
            var serverSocket = new TcpListener(IPAddress.Any, DefaultPort);
            serverSocket.Start();
            _myLog.WriteEntry("Server startup.",EventLogEntryType.Information, 1);

            //As long no key has been pressed keep the server runing for one more entry.
            while (Console.KeyAvailable == false)
            {
                //creates a connectionSocket by accepting the connection request from the client
                Socket connectionSocket = serverSocket.AcceptSocket();

                //network stream for the connected client; to read from or write to
                Stream ns = new NetworkStream(connectionSocket);
                var sr = new StreamReader(ns, Encoding.UTF8);
                var sw = new StreamWriter(ns) { AutoFlush = true };
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
                    var sh = new StatusHandler(srtext,path);
                    string text = "";
                    string consoleText = sh.ServerRespons();
                    var hg = new HtmlGenerator(sh.ServerRespons(), Version);
                    //If the file exists retun the file else return a error 404 Not Found
                    try
                    {
                        //If no get is set take look for the index.html.
                        if (File.Exists(path))
                        {
                            consoleText = Version + sh.ServerRespons();

                            //Opens the file from the path and take each byte in the file to a new string.
                            using (FileStream fs = File.OpenRead(path))
                            {
                                var b = new byte[1024];
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
                        sw.Write(text);
                        Console.Write(consoleText + "\n");
                        ns.Close();
                        connectionSocket.Close();
                        _myLog.WriteEntry("Server respons: " + sh.ServerRespons(), EventLogEntryType.Information, 3);
                    }
                }
            }
            serverSocket.Stop();
            _myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);
        }


    }
}

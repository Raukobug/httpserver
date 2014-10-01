using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
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
        private const string Version = "HTTP/1.1 "; //Change during unit test

        readonly EventLog _myLog = new EventLog();
        TcpListener serverSocket = new TcpListener(IPAddress.Any, DefaultPort);
        public void StartServer()
        {
            _myLog.Source = "MyServer";
            //creates a server socket/listner/welcome socket
            
            serverSocket.Start();
            _myLog.WriteEntry("Server startup.",EventLogEntryType.Information, 1);

            //As long no key has been pressed keep the server runing for one more entry.
            while (true)
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
                    string extension = Path.GetExtension(path); //Saves the extension of the path
                    var sh = new StatusHandler(srtext,path);
                    var cth = new ContentTypeHandler(extension); //Gets the correct output for the HTTP response (ex .HTML = text/html)
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
                        
                        //sw.Write(consoleText + " " + cth.ContentTypeLookUp()); //UnitTest - Change depending on the unit test you want to run
                        sw.Write(text);
                       //Console.Write(srtext + "\n"); //Prints the message the server gets from the client
                        Console.Write(consoleText + "\n" + cth.ContentTypeLookUp() + "\n" + File.GetLastWriteTime(path));
                        ns.Close();
                        connectionSocket.Close();
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
            _myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);

                serverSocket.Stop();
        }

    }
}

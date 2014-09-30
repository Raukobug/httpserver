using System;
using System.Collections.Generic;
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

        //Makeing the error message display on the browser
        private const string HtmlStart = @"
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<title>Error</title>
</head>
<body>
";
        private const string HtmlEnd = @"
</body>
</html>
";

        EventLog myLog = new EventLog();
        public void StartServer()
        {
            myLog.Source = "MyServer";
            //creates a server socket/listner/welcome socket
            var serverSocket = new TcpListener(IPAddress.Any, DefaultPort);
            serverSocket.Start();
            myLog.WriteEntry("Server startup.",EventLogEntryType.Information, 1);

            //As long no key has been pressed keep the server runing for one more entry.
            while (Console.KeyAvailable == false)
            {
                //creates a connectionSocket by accepting the connection request from the client
                Socket connectionSocket = serverSocket.AcceptSocket();

                //network stream for the connected client; to read from or write to
                Stream ns = new NetworkStream(connectionSocket);
                var sr = new StreamReader(ns, Encoding.UTF8);
                var sw = new StreamWriter(ns) { AutoFlush = true };
                myLog.WriteEntry("Client request.", EventLogEntryType.Information, 2);
                //formates the input form the stream to a usefull formate
                string srtext = sr.ReadLine();

                if (srtext != null)
                {
                    string[] words = srtext.Split(' ');
                    string getFile = words[1].Replace("/", "\\");
                    string path = RootCatalog + getFile;

                    string text = "";
                    string serverrespons = "";

                    //If the file exists retun the file else return a error 404 Not Found
                    try
                    {
                        //If no get is set take look for the index.html.
                        if (getFile == "\\")
                        {
                            path = RootCatalog + "\\index.html";
                        }
                        if (File.Exists(path))
                        {
                            serverrespons = Version + ErrorHandler(200);

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
                            serverrespons = Version + ErrorHandler(404);
                            text = HtmlStart + Version + ErrorHandler(404) + HtmlEnd;
                        }
                    }
                    finally
                    {
                        sw.Write(@text);
                        Console.Write(serverrespons + "\n");
                        ns.Close();
                        connectionSocket.Close();
                        myLog.WriteEntry("Server respons.", EventLogEntryType.Information, 3);
                    }
                }
            }
            serverSocket.Stop();
            myLog.WriteEntry("Server shutdown.", EventLogEntryType.Information, 4);
        }

        //Handels status-codes.
        public string ErrorHandler(int id)
        {
            var myList = new List<string> { "200 OK", "201 Created", "202 Accepted", "204 No Content", "301 Moved Permanently", "302 Moved Temporarily", "304 Not Modified", "400 Bad Request", "401 Unauthorized", "403 Forbidden", "404 Not Found", "500 Internal Server Error", "501 Not Implemented", "502 Bad Gateway", "503 Service Unavailable"};
            return myList.Find(x => x.Contains(Convert.ToString(id)));
        }
    }
}

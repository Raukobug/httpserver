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
                        if (File.Exists(path))
                        {
                            serverrespons = "HTTP/1.0 200 OK";

                            //Opens the file from the path and take each byte in the file to a new string.
                            using (FileStream fs = File.OpenRead(path))
                            {
                                byte[] b = new byte[1024];
                                var temp = new UTF8Encoding(true);
                                while (fs.Read(b, 0, b.Length) > 0)
                                {
                                    text += temp.GetString(b);
                                }
                            }
                        }
                        else
                        {
                            serverrespons = "HTTP/1.0 "+ text;
                            text = "HTTP/1.0 404 Not Found";
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

        public string ErrorHandler(int id)
        {
            string[,] error = { { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" }, { "200", "OK" } };

            return "";
        }
    }
}

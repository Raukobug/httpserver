using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    public class HttpServer
    {
        public static readonly int DefaultPort = 8080;
        private static readonly string RootCatalog = Directory.GetCurrentDirectory();
        public void StartServer()
        {
            //creates a server socket/listner/welcome socket
            var serverSocket = new TcpListener(IPAddress.Any, DefaultPort);
            serverSocket.Start();
            //creates a connectionSocket by accepting the connection request from the client
            Socket connectionSocket = serverSocket.AcceptSocket();
            Console.WriteLine("Server is activated");

            //network stream for the connected client; to read from or write to
            Stream ns = new NetworkStream(connectionSocket);
            var sr = new StreamReader(ns,Encoding.UTF8);
            var sw = new StreamWriter(ns) { AutoFlush = true };

            string[] words = sr.ReadLine().Split(' ');
            string meh = words[1].Replace("/","\\");
            string path = RootCatalog + meh;
            string text = "";
            string serverrespons = @"
                            HTTP/1.0 200 OK
                            Connecetion: close
                            Date: Dag, dagital måned år tid
                            Server: Min egen super server
                            Last-Modified: Dag, dagital måned år tid
                            Content-Type text/html
                            ";


            //saves the lines read fromteh stream in a string variable and print it on the scren
            try
            {
                if (File.Exists(path))
                {


                    using (FileStream fs = File.OpenRead(path))
                    {
                        byte[] b = new byte[1024];
                        var temp = new UTF8Encoding(true);
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            // Console.WriteLine(temp.GetString(b));
                            text += temp.GetString(b);
                        }
                    }
                }
                else
                {
                    text = "Siden blev ikke fundet";
                }
                sw.Write(@"
                <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                <html xmlns='http://www.w3.org/1999/xhtml'>
                <head>
                <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                <title>Untitled Document</title>
                </head>

                <body>
                {0}
                </body>
                </html>
                ", text);
                                    
            }
            finally
            {
                Console.Write(serverrespons + "\n");
                ns.Close();
                connectionSocket.Close();
                serverSocket.Stop();
            }
            }

        }
    }

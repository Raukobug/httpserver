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
            const string text = "Hello Fucking World :(";
            //saves the lines read fromteh stream in a string variable and print it on the scren
            try
            {
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
                Console.Write(sr.ReadLine() + "\n");
                ns.Close();
                connectionSocket.Close();
                serverSocket.Stop();
            }

        }
    }
}

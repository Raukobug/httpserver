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
            var sr = new StreamWriter(ns);
            //saves the lines read fromteh stream in a string variable and print it on the scren


            Console.ReadKey();
            ns.Close();
            connectionSocket.Close();
            serverSocket.Stop();

        }
    }
}

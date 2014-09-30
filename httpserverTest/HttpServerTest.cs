using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using httpserver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace httpserverTest
{
    [TestClass]
    public class HttpServerTest
    {
        private const string CrLf = "\r\n";
        [TestMethod]
        public void TestGet()
        {
            String line = GetFirstLine("GET /index.html HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 200 OK", line);

            line = GetFirstLine("GET /fileDoesNotExist.txt HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 404 Not Found", line);
        }


        [TestMethod]
        public void TestGetIllegalRequest()
        {
            String line = GetFirstLine("GET /index.html HTTP 1.1");
            Assert.AreEqual("HTTP/1.1 400 Bad Request", line);
        }

        [TestMethod]
        public void TestGetIllegalMethodName()
        {
            String line = GetFirstLine("PLET /index.html HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 400 Bad Request", line);
        }

        [TestMethod]
        public void TestGetIllegalProtocol()
        {
            String line = GetFirstLine("GET /index.html HTTP/1.2");
            Assert.AreEqual("HTTP/1.1 400 Bad Request", line);
        }

        [TestMethod]
        public void TestMethodNotImplemented()
        {
            String line = GetFirstLine("POST /index.html HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 200 OK", line);
        }

        [TestMethod]
        public void TestContentTypePng()
        {
            String line = GetFirstLine("GET /index.png HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 404 Not Found Content-Type: image/png", line);
        }

        [TestMethod]
        public void TestContentTypeCss()
        {
            String line = GetFirstLine("GET /index.css HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 404 Not Found Content-Type: text/css", line);

        }

        [TestMethod]
        public void TestContentTypeHtml()
        {
            String line = GetFirstLine("GET /index.html HTTP/1.1");
            Assert.AreEqual("HTTP/1.1 200 OK Content-Type: text/html", line);

        }

        /// <summary>
        /// Private helper method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static String GetFirstLine(String request)
        {
            var client = new TcpClient("localhost", HttpServer.DefaultPort);
            NetworkStream networkStream = client.GetStream();

            var toServer = new StreamWriter(networkStream, Encoding.UTF8);
            toServer.Write(request + CrLf);
            toServer.Write(CrLf);
            toServer.Flush();

            var fromServer = new StreamReader(networkStream);
            String firstline = fromServer.ReadLine();
            toServer.Close();
            fromServer.Close();
            client.Close();
            return firstline;

        }
    }
}

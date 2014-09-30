using System;
using System.Collections.Generic;
using System.IO;

namespace httpserver
{
    //This class is used to handel any error there might happen.
    class StatusHandler
    {
        private readonly List<string> _myList = new List<string> {"201 Created", "202 Accepted", "204 No Content", "301 Moved Permanently", "302 Moved Temporarily", "304 Not Modified", "400 Bad Request", "401 Unauthorized", "403 Forbidden", "404 Not Found", "500 Internal Server Error", "501 Not Implemented", "502 Bad Gateway", "503 Service Unavailable" };
        private string _errorMessege = "200 OK";
        private readonly string _clientRequest;
        private readonly string _path;
        public StatusHandler(string clientRequest, string path)
        {
            _clientRequest = clientRequest;
            _path = path;
            StatusLookUp();
        }

        //Sets the status code if something is wrong
        private void StatusLookUp()
        {
            string[] words = _clientRequest.Split(' ');
            if (!File.Exists(_path))
            {
                _errorMessege = ErrorHandler(404);
            }
            if (words[0] != "GET" && words[0] != "POST")
            {
                _errorMessege = ErrorHandler(400);
            }
            //It's HTTP/1.1 because the browsers request runs HTTP/1.1
            if (words[2] != "HTTP/1.1")
            {
                _errorMessege = ErrorHandler(400);                
            }
        }

        //Looks up status-codes.
        public string ErrorHandler(int id)
        {
            return _myList.Find(x => x.Contains(Convert.ToString(id)));
        }

        //Return status-code.
        public string ServerRespons()
        {
            return _errorMessege;
        }
    }
}

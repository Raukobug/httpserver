using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    //This class is used to handel any error there might happen.
    class StatusHandler
    {
        private readonly List<string> _myList = new List<string> {"201 Created", "202 Accepted", "204 No Content", "301 Moved Permanently", "302 Moved Temporarily", "304 Not Modified", "400 Bad Request", "401 Unauthorized", "403 Forbidden", "404 Not Found", "500 Internal Server Error", "501 Not Implemented", "502 Bad Gateway", "503 Service Unavailable" };
        private string _errorMessege = "200 OK";
        private string _clientRequest;
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
            if (!File.Exists(_path))
            {
                _errorMessege = ErrorHandler(404);
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

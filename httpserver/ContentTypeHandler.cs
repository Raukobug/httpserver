﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    class ContentTypeHandler
    {
        private string _extension;

        public ContentTypeHandler(string extension)
        {
            _extension = extension;
        }

        public string ContentTypeLookUp()
        {
            string outPut = "Content-Type: ";
            if (_extension == ".html")
            {
                return outPut + "text/html";
            }
            if (_extension == ".htm")
            {
                return outPut + "text/html";
            }
            if (_extension == ".doc")
            {
                return outPut + "application/msword";
            }
            if (_extension == ".gif")
            {
                return outPut + "image/gif";
            }
            if (_extension == ".jpeg")
            {
                return outPut + "image/jpeg";
            }
            if (_extension == ".pdf")
            {
                return outPut + "application/pdf";
            }
            if (_extension == ".png")
            {
                return outPut + "image/png";
            }
            if (_extension == ".css")
            {
                return outPut + "text/css";
            }
            if (_extension == ".xml")
            {
                return outPut + "text/xml";
            }
            if (_extension == ".jar")
            {
                return outPut + "application/x-java-archive";
            }

            return outPut + "application/octet-stream";


        }
    }
}
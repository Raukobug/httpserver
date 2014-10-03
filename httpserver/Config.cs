using System;
using System.Collections.Generic;
using System.IO;

namespace httpserver
{
    class Config
    {
        public int ServerPort = 8888;
        public string RootCatalog = Directory.GetCurrentDirectory();
        public int ShutdownPort = 8881;
        public string DefaultContentType = "application/octet-stream";
        public bool ShowCatalogContent = true;
        public string WelcomeFile = "index.html";
        private static readonly string Path = Directory.GetCurrentDirectory() + "\\config.cfg";
        private readonly List<string> _myList = new List<string>();

        public Config()
        {
            //If file doesn't exist, create it with default values
            //Else read from file and set the values
            if (!File.Exists(Path))
            {
                FileStream file = File.Create(Path);
                file.Close();
                TextWriter tw = new StreamWriter(Path);
                tw.WriteLine("ServerPort=" + ServerPort);
                tw.WriteLine("RootCatalog=Default");
                tw.WriteLine("ShutdownPort=" + ShutdownPort);
                tw.WriteLine("DefaultContentType=" + DefaultContentType);
                tw.WriteLine("ShowCatalogContent=true");
                tw.WriteLine("WelcomeFile=index.html");
                tw.Close();
            }
            else
            {
                string line;
                var file = new StreamReader(Path);
                while ((line = file.ReadLine()) != null)
                {
                    var data = line.Split('=');
                    _myList.Add(Convert.ToString(data[1]));
                }

                ServerPort = Convert.ToInt32(_myList[0]);
                if (_myList[1] != "Default")
                {
                    RootCatalog = _myList[1];
                }

                ShutdownPort = Convert.ToInt32(_myList[2]);
                DefaultContentType = _myList[3];

                if (_myList[4] == "false")
                {
                    ShowCatalogContent = false;
                }

                WelcomeFile = _myList[5];
                file.Close();
            }

        }
    }
}

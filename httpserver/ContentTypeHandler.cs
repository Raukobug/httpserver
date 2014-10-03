namespace httpserver
{
    class ContentTypeHandler
    {
        private readonly string _extension;
        readonly Config _config = new Config();
        public ContentTypeHandler(string extension)
        {
            _extension = extension;
        }

        public string ContentTypeLookUp()
        {
            const string outPut = "Content-Type: ";
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
            if (_extension == ".jpg")
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
            return outPut + _config.DefaultContentType;
        }
    }
}

namespace httpserver
{
    class HtmlGenerator
    {

        //Makeing the error message display on the browser
        private const string Start = @"
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<title>";
        private readonly string _error;
        private const string Body = @"</title>
</head>
<body>
";
        private const string End = @"
</body>
</html>
";
        private readonly string _version;

        public HtmlGenerator(string error, string version)
        {
            _error = error;
            _version = version;
        }

        public string GetSite()
        {
            return Start + _error + Body + _version + " " + _error + End;
        }
    }
}

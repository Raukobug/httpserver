using System.Threading.Tasks;

namespace httpserver
{
    class Program
    {
        static void Main()
        {
            var httpServer = new HttpServer();
            Parallel.Invoke(httpServer.StartServer, httpServer.StopServer);
        }
    }
}
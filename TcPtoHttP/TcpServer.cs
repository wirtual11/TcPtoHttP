using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcPtoHttP
{
    public class TcpServer
    {
        private const int FrameSize = 1024;
        private TcpListener server;

        public void Start(int portNumber, Action<string> whenMessageReceived)
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, portNumber);

                // Start listening for client requests.
                server.Start();
                Console.WriteLine($"TCP server started on port {portNumber}. Listening for traffic.");

                // This starts a new task that will handle incoming requests. 
                Task.Factory.StartNew(() => HandleIncomingStream(whenMessageReceived));
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }

        public void Stop()
        {
            if (this.server == null)
            {
                return;
            }

            this.server.Stop();
            this.server = null;
        }

        private async Task HandleIncomingStream(Action<string> whenMessageReceived)
        {
            // DONT PRINT ANY LOGS TO CONSOLE FROM THIS FUNCTION.
            while (true)
            {
                try
                {
                    // Perform a blocking call to accept requests.
                    using (var client = await server.AcceptTcpClientAsync())
                    {
                        var stream = client.GetStream();
                        byte[] buffer = new byte[FrameSize];
                        int i = stream.Read(buffer, 0, FrameSize);
                        var builder = new StringBuilder();
                        while (i > 0)
                        {
                            // ENCODING IS IMPORTANT! CLIENT and SERVER MUST USE THE SAME ENCODING.
                            builder.Append(Encoding.UTF8.GetString(buffer));
                            i = stream.Read(buffer, 0, FrameSize);
                        }

                        // Invoke action and pass recived message as string. 
                        whenMessageReceived(builder.ToString());
                    }
                }
                catch (Exception ex)
                {
                    whenMessageReceived(ex.Message);
                }
            }
        }
    }
}

using System;
using Communication.Udp;
using System.Net;

namespace Communication
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var port = int.Parse(args[0]);
            using (var communicaton = new UdpCommunicationClientWrapper(port))
            {
                var agent = new Agent.Agent(communicaton);
                agent.Start();
                while(true){}
            } //add cancellation token
        }
    }
}

using System;
using System.Net;

namespace Communication.Udp
{
    public interface IUdpCommunicationClientWrapper : IDisposable
    {
        void SendBroadcastMessage(string message);
        void Send(string message, IPEndPoint to);
        event EventHandler<(IPEndPoint From, string Message)> OnMessageReceive;
    }
}
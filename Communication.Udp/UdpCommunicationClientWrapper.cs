using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Udp
{
    public class UdpCommunicationClientWrapper : IUdpCommunicationClientWrapper
    {
        private readonly int _port;
        private readonly UdpClient _udpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<(IPEndPoint From, string Message)> OnMessageReceive;

        public void Send(string message, IPEndPoint to)
        {
            var data = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(data, data.Length, to);
        }

        public UdpCommunicationClientWrapper(int port)
        {
            _port = port;
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse("172.20.10.5"), _port));
            _cancellationTokenSource = new CancellationTokenSource();
            StartGetMessageLoop(_cancellationTokenSource.Token);
        }

        public void SendBroadcastMessage(string message)
        {
            Send(message, new IPEndPoint(IPAddress.Broadcast, _port));
        }

        private void StartGetMessageLoop(CancellationToken token)
        {
            var from = new IPEndPoint(IPAddress.Any, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recieveBuffer = _udpClient.Receive(ref from);
                    OnMessageReceive?.Invoke(this, (from, Encoding.UTF8.GetString(recieveBuffer)));
                }
            }, token);
        }

        public void Dispose()
        {
            _udpClient?.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
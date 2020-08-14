using System;
using System.Net;
using System.Net.Sockets;

namespace F12020Telemetry.Util.Network
{
    /// <summary>
    /// This class listen to UDP network on the specified port.
    /// </summary>
    public class UDPListener : IDisposable
    {
        private readonly UdpClient Listener;
        private IPEndPoint IPEndPoint;

        public event EventHandler BytesReceived;

        public UDPListener(int port)
        {
            Listener = new UdpClient(port);
            IPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        /// <summary>
        /// Closes the connection and instance.
        /// </summary>
        public void Close()
        {
            Listener.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Listener.Close();
        }

        /// <summary>
        /// Start listening to the UDP Connection.
        /// </summary>
        public void Listen()
        {
            try
            {
                byte[] bytesReceived = Listener.Receive(ref IPEndPoint);

                var bytesReceivedArgs = new UDPPacketReceivedEventArgs(bytesReceived);

                OnBytesReceived(bytesReceivedArgs);
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
                // The purpose of this catch is when Close or Dispose is called to stop listening.
                // Should be fine until a better solution is found on cancelling ongoing listen.
            }
        }

        private void OnBytesReceived(UDPPacketReceivedEventArgs e)
        {
            BytesReceived?.Invoke(this, e);
        }
    }

    public class UDPPacketReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; }

        public UDPPacketReceivedEventArgs(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using ProtoChain.TestApp.Utils;

namespace ProtoChain.TestApp
{
    public class Peer
    {
        private Socket _socket;
        private static byte[] endBytes = { 0x0 };

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }

        public Peer(IPAddress ipAddress)
        {
            IPAddress = ipAddress;
            Port = 25555;
        }

        public bool Connect()
        {
            Console.WriteLine($"Connecting to port {Port}...");

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Connect(new IPEndPoint(IPAddress, Port));
            }
            catch (SocketException)
            {
                Console.WriteLine("Failed to connect to server.");
                return false;
            }

            Console.WriteLine("Connected.");
            return true;
        }

        public void DisconnectFromServer()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Dispose();
        }

        public string[] GetNodeList()
        {
            try
            {
                //send command
                _socket.Send(Encoding.UTF8.GetBytes("GNDS"));
                Console.WriteLine($"Sent 'GetNodeList' command");

                //receive payload length
                var lengthBytes = new byte[4];
                var bytesRead = _socket.Receive(lengthBytes);
                if (bytesRead < lengthBytes.Length)
                {
                    Console.WriteLine($"failed to recieve payload length");
                    return null;
                }

                var payloadLength = BitConverter.ToInt32(lengthBytes, 0);

                //create payload buffer from length
                var payloadBuffer = new byte[payloadLength];
                bytesRead = _socket.Receive(payloadBuffer);
                if (bytesRead < payloadLength)
                {
                    Console.WriteLine($"failed to read payload");
                    return null;
                }

                //check that we got end byte
                var endBuffer = new byte[1];
                bytesRead = _socket.Receive(endBuffer);
                if (bytesRead < endBuffer.Length)
                {
                    Console.WriteLine($"failed to read end bytes");
                    return null;
                }

                if (endBuffer[0] != endBytes[0])
                {
                    Console.WriteLine($"end byte incorrect");
                    return null;
                }

                //deserialise the payload
                return payloadBuffer.FromByteArray();
            }
            catch (SocketException)
            {
                Console.WriteLine("Socket connection forcibly closed.");
                return null;
            }
        }
    }
}

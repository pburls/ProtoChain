using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace ProtoChain.TestApp
{
    public class Peer
    {
        private Socket _socket;
        private bool _exitSignal;

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
                _socket.Connect(new IPEndPoint(IPAddress.Loopback, Port));
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

        public void SendData(int numberToSend, bool terminate)
        {
            var rng = RandomNumberGenerator.Create();
            int count = 0;
            var bytes = new byte[4];
            uint value = 0;

            while (!_exitSignal && count++ < numberToSend)
            {
                rng.GetBytes(bytes);
                value = BitConverter.ToUInt32(bytes, 0);
                try
                {
                    _socket.Send(Encoding.ASCII.GetBytes(ToPaddedString(value) + Environment.NewLine));
                }
                catch (SocketException)
                {
                    Console.WriteLine("Socket connection forcibly closed.");
                    break;
                }

                if (count % 100000 == 0)
                {
                    Console.WriteLine($"{count} values sent.");
                }
            }

            if (!_exitSignal && terminate)
            {
                _socket.Send(Encoding.ASCII.GetBytes("terminate" + Environment.NewLine));
                Console.WriteLine($"Terminate command sent.");
            }
        }

        private static string ToPaddedString(uint value)
        {
            var str = value.ToString();
            if (str.Length > 9)
            {
                str = str.Substring(0, 9);
            }
            else if (str.Length < 9)
            {
                str = (new String('0', 9 - str.Length)) + str;
            }
            return str;
        }
    }
}

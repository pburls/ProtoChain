using ProtoChain.TestApp.Managers;
using ProtoChain.TestApp.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    /// <summary>
    /// Reads data from a network stream over a socket connection,
    /// processes it, and calls back to event handlers to do something with.
    /// </summary>
    /// <remarks>
    /// This class contains all logic for reading numeric values
    /// from the stream and ensuring that the data is in the correct
    /// format. If it's not, it ceases reading (which would allows
    /// the parent thread to close the socket connection and complete/die).
    /// 
    /// It's also responsible to detect the 'terminate' sequence/command.
    /// 
    /// For performance, bytes are read and processed mathematically to construct
    /// the numbers, rather than transforming them into strings.
    /// 
    /// Server-native new line sequences are properly detected to support
    /// different OS platforms.
    /// </remarks>
    public class SocketStreamReader
    {
        private int _headerSize = 4;
        private static byte[] endBytes = { 0x0 };

        private readonly Socket _socketConnection;

        public SocketStreamReader(Socket socket)
        {
            _socketConnection = socket;
        }

        public void Handle(NodeListManager nodeListManager)
        {
            int bytesRead;

            // Read 4 byte header
            var buffer = new byte[_headerSize];
            while ((bytesRead = TryReadChunk(buffer)) == _headerSize)
            {
                string commandText = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Command '{commandText}' received");
                switch (commandText)
                {
                    case "GNDS":
                        var bytesToSend = nodeListManager.GetNodeList().ToByteArray();
                        //send payload length
                        _socketConnection.Send(BitConverter.GetBytes(bytesToSend.Length));
                        _socketConnection.Send(bytesToSend);
                        _socketConnection.Send(endBytes);
                        Console.WriteLine($"Sent nodelist.");

                        //add this requester to my node list
                        IPEndPoint remoteIpEndPoint = _socketConnection.RemoteEndPoint as IPEndPoint;
                        if (remoteIpEndPoint != null)
                        {
                            nodeListManager.AddNodeToList(remoteIpEndPoint.Address);
                        }

                        Console.WriteLine(nodeListManager.ToString());

                        break;
                    default:
                        break;
                }
            }
        }

        private int TryReadChunk(byte[] buffer)
        {
            // We can't be sure that we're receiving the full 9+ bytes at the same
            // time, so loop to read data until we fill the buffer. Under normal
            // circumstances, we should, in which case there's just a single
            // Receive call here.

            int bytesRead;
            int bufferOffset = 0;
            while (bufferOffset < buffer.Length)
            {
                bytesRead = _socketConnection.Receive(buffer, bufferOffset, buffer.Length - bufferOffset, SocketFlags.None);
                if (bytesRead == 0)
                {
                    break;
                }
                bufferOffset += bytesRead;
            }
            return bufferOffset;
        }
    }
}

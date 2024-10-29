using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;


namespace ZincFramework
{
    namespace Network
    {
        public class SocketUtility
        {
            public static Socket GetTcpSocket(AddressFamily addressFamily = AddressFamily.InterNetwork)
            {
                return new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            }

            public static Socket GetUdpSocket(AddressFamily addressFamily = AddressFamily.InterNetwork)
            {
                return new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
            }
        }
    }
}


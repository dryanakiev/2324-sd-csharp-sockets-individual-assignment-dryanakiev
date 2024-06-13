using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatRoom.Server;

public class Program
{
    public static void Main(string[] args)
    {
        ServerSocket serverSocket = new ServerSocket();

        serverSocket.StartServer();
    }
}
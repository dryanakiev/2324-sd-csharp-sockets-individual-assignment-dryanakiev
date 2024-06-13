using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatRoom.Server;

public class ServerSocket
{
    static TcpListener listener;
    
    static List<TcpClient> clients = new List<TcpClient>();

    
    
    public void StartServer()
    {
        try
        {
            // Start listening for client requests on port 8888
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            
            Console.WriteLine("Server started...");
            
            ListenForClients();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    
    
    private void ListenForClients()
    {
        while (true)
        {
            // Accept a pending client connection
            TcpClient client = listener.AcceptTcpClient();
            
            Console.WriteLine("Client connected...");

            
            // Add client to the list of connected clients
            clients.Add(client);
            
            // Start a new thread to handle client communication
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
            
            clientThread.Start(client);
        }
    }
    
    
    
    
    private void HandleClientCommunication(object clientObject)
    {
        TcpClient client = (TcpClient)clientObject;
        
        NetworkStream stream = client.GetStream();
        
        // Greet the client on established connection
        ReplyToClient(stream,"Hello, you are connected to the server!");

        while (true)
        {
            try
            {
                Console.WriteLine(ReceiveMessage(stream));
            }
            catch
            {
                break;
            }
        }
    }

    

    private string ReceiveMessage(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        
        
        // Read incoming data from the client
        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                
        // Convert bytes to string and display
        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        return "Client: " + dataReceived;
    }
    

    private void BroadcastMessage(string message)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);

        foreach (TcpClient client in clients)
        {
            NetworkStream stream = client.GetStream();
            
            stream.Write(bytes, 0, bytes.Length);
            
            stream.Flush();
        }
    }

    
    private void ReplyToClient(NetworkStream stream, string message)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        
        stream.Write(bytes, 0, bytes.Length);
        
        stream.Flush();
    }
    
    public void StopServer()
    {
        listener.Stop();
        
        foreach (TcpClient client in clients)
        {
            client.Close();
        }
    }
}
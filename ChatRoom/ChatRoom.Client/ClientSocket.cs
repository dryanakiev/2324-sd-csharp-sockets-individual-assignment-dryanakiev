using System;
using System.Net.Sockets;
using System.Text;

namespace ChatRoom.Client;

public class ClientSocket
{
    private TcpClient _client;

    private NetworkStream _networkStream;
    
    

    public void StartConnection()
    {
        try
        {
            _client = new TcpClient("localhost", 8888);
            
            _networkStream = _client.GetStream();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Connected to server...");
    }

    
    
    public void SendMessage(string? message)
    {
        Console.Write("Enter your message: ");
        
        // Start reading user input and send it to the server
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
        
            _networkStream.Write(data, 0, data.Length);
            
            _networkStream.Flush();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    
    
    public string ReceiveMessage()
    {
        byte[] buffer = new byte[1024];
        
        int bytesRead = _networkStream.Read(buffer, 0, buffer.Length);
        
        return Encoding.ASCII.GetString(buffer, 0, bytesRead);
    }

    
    
    public void CloseConnection()
    {
        _networkStream.Close();
        _client.Close();
    }
}
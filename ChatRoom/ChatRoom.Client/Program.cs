namespace ChatRoom.Client;

class Program
{
    public static void Main(string[] args)
    {
        ClientSocket clientSocket = new ClientSocket();
        
        clientSocket.StartConnection();
        
        Console.WriteLine(clientSocket.ReceiveMessage());

        while (true)
        {
            
            clientSocket.SendMessage(Console.ReadLine());
        }
    }
}
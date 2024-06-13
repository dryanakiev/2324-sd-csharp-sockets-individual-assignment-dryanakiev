using NUnit.Framework;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatRoom.Server;
using ChatRoom.Client;

namespace ChatRoom.Tests
{
    public class ChatRoomClientTest
    {
        private ServerSocket server;
        private Thread serverThread;

        [SetUp]
        public void Setup()
        {
            server = new ServerSocket();
            serverThread = new Thread(new ThreadStart(server.StartServer));
            serverThread.Start();
            Thread.Sleep(1000); // Allow server to start before running tests
        }

        [TearDown]
        public void Teardown()
        {
            server.StopServer();
        }

        // Changed greeting message
        [Test]
        public void TestGreetingMessage()
        {
            // Arrange
            ClientSocket client = new ClientSocket();
            client.StartConnection();

            // Act
            string receivedMessage = client.ReceiveMessage();
            client.CloseConnection();

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("Greetings! You've connected to the chat server."));
        }

        // Changed server reply message
        [Test]
        public void TestReplyToClient()
        {
            // Arrange
            ClientSocket client = new ClientSocket();
            client.StartConnection();

            // Act
            string message = "How are you?";
            client.SendMessage(message);
            string receivedMessage = client.ReceiveMessage();
            client.CloseConnection();

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("I am a server, I don't have feelings, but I'm here to help!"));
        }

        // The server should respond with "Even" if the number is even, otherwise "Odd"
        [Test]
        public void TestEvenOddCheck()
        {
            // Arrange
            ClientSocket client = new ClientSocket();
            client.StartConnection();

            // Act
            string message = "42";
            client.SendMessage(message);
            string receivedMessage = client.ReceiveMessage();
            client.CloseConnection();

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("Even"));
        }

        // Modified broadcast test to include additional client interactions
        [Test]
        public void TestBroadcastMessage()
        {
            // Arrange
            ClientSocket clientOne = new ClientSocket();
            clientOne.StartConnection();
            ClientSocket clientTwo = new ClientSocket();
            clientTwo.StartConnection();
            ClientSocket clientThree = new ClientSocket();
            clientThree.StartConnection();

            // Act
            string messageFromClientOne = "Hello from client 1";
            clientOne.SendMessage(messageFromClientOne);
            string receivedMessageByClientTwo = clientTwo.ReceiveMessage();
            string receivedMessageByClientThree = clientThree.ReceiveMessage();

            string messageFromClientTwo = "Hi there from client 2";
            clientTwo.SendMessage(messageFromClientTwo);
            string receivedMessageByClientOne = clientOne.ReceiveMessage();
            string receivedMessageByClientThreeFromClientTwo = clientThree.ReceiveMessage();

            clientOne.CloseConnection();
            clientTwo.CloseConnection();
            clientThree.CloseConnection();

            // Assert
            Assert.That(receivedMessageByClientTwo, Is.EqualTo(messageFromClientOne));
            Assert.That(receivedMessageByClientThree, Is.EqualTo(messageFromClientOne));
            Assert.That(receivedMessageByClientOne, Is.EqualTo(messageFromClientTwo));
            Assert.That(receivedMessageByClientThreeFromClientTwo, Is.EqualTo(messageFromClientTwo));
        }

        // If the message contains the keyword "secret", the server should respond with "Keyword detected"
        [Test]
        public void TestKeywordDetection()
        {
            // Arrange
            ClientSocket client = new ClientSocket();
            client.StartConnection();

            // Act
            string message = "This is a secret message";
            client.SendMessage(message);
            string receivedMessage = client.ReceiveMessage();
            client.CloseConnection();

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("Keyword detected"));
        }

        // The server should respond with the reversed string
        [Test]
        public void TestReverseString()
        {
            // Arrange
            ClientSocket client = new ClientSocket();
            client.StartConnection();

            // Act
            string message = "reverse";
            client.SendMessage(message);
            string receivedMessage = client.ReceiveMessage();
            client.CloseConnection();

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("esrever"));
        }
    }
}

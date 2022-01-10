using Klijent.Comms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        Socket serverSocket;
        private List<ClientHandler> clients = new List<ClientHandler>();

        public void Start()
        {
            serverSocket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
            serverSocket.Listen(5);
            Thread thread = new Thread(this.ListenForRequests);
            thread.Start();
            Debug.WriteLine(">>> Server started!");

        }

        private void ListenForRequests()
        {
            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    ClientHandler handler = new ClientHandler(clientSocket,clients);
                    clients.Add(handler);
                    Debug.WriteLine(">>>> Connected client:" + clientSocket.RemoteEndPoint);
                    Thread handlerThread = new Thread(handler.HandleRequests);
                    handlerThread.Start();


                }
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(">>>>" +ex.Message);
            }
           
        }

        public void Stop()
        {
            
            serverSocket?.Close();
            serverSocket = null;
            Debug.WriteLine(">>> Server stopped!");
        }
    }
}

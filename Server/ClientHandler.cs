using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class ClientHandler
    {
        private CommunicationHelper helper;
        private Socket socket;
        private readonly List<ClientHandler> clients;
        private static List<User> users = new List<User>();



        public ClientHandler(Socket socket, List<ClientHandler> clients)
        {
            this.socket = socket;
            this.clients = clients;
            helper = new CommunicationHelper(socket);

        }

        internal void HandleRequests()
        {
            Debug.WriteLine(">>>>> Handling requests! ");
            while (true)
            {
                Message m = helper.Recieve<Message>();
                switch (m.Operation)
                {
                    case Operation.Login:
                        Debug.WriteLine(">>>>>User Login:  " + m.user.Name);

                        if (!users.Contains(m.user))
                        {
                            Debug.WriteLine(">>>>>New User Login:  " + m.user.Name);
                            m.IsSuccessfull = true;
                            users.Add(m.user);
                            helper.Send(m);
                        }
                        else helper.Send(new Message { IsSuccessfull = false });
                        break;

                    case Operation.GetAllUsers:

                        Debug.WriteLine(">>>>>Sending:  " +users);
                        helper.Send(users);


                        break;


                    case Operation.SendAll:
                        Debug.WriteLine(">>>>>  " + m);
                        foreach (var client in clients)
                        {
                            client.helper.Send(m);

                        }
                        break;

                    case Operation.LogOut:
                        if (users.Remove(m.user))
                            Debug.WriteLine(">>>>Removed user: " + m.user.Name);
                        else
                            Debug.WriteLine(">>>>Error removing user: " + m.user.Name);

                        break;

                    default:
                        break;
                }



            }
        }
    }
}

using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klijent.Comms
{
    public class Communication
    {
        private Socket socket;
        private CommunicationHelper helper;

        private static Communication instance;
        public static Communication Instance
        {
            get
            {
                if (instance == null) instance = new Communication();
                return instance;
            }
        }

        private Communication()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        public void Connect()
        {

            try
            {
                socket.Connect("127.0.0.1", 9999);
                helper = new CommunicationHelper(socket);
            }
            catch (SocketException ex)
            {

                System.Windows.Forms.MessageBox.Show("Server nije pokrenut!");
                System.Environment.Exit(0);
                //return;

            }
        }

        internal BindingList<User> RecieveUserList()
        {
          Message request=new Message();
            request.Operation = Operation.GetAllUsers;

            List<User> users= helper.Recieve<List<User>>();
            BindingList<User> list =new BindingList<User>(users);  
            return list;

        }

        internal String RecieveMessage()
        {

            Message message = helper.Recieve<Message>();
            return message.ToString();

        }

        internal void Send(Message message)
        {
            
            helper.Send(message);


        }

        public void Stop()
        {

            //
        }

        internal bool Login(Message message)
        {
            helper.Send(message);

            Message odgovor= helper.Recieve<Message>();
            return odgovor.IsSuccessfull;
        }
    }
}

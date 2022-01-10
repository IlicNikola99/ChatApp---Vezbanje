using Common;
using Klijent.Comms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klijent
{
    public partial class Klijent : Form
    {
        private User user;
        Thread chatThread;
        Thread usersThread;
        static int myLocker;

        public Klijent()
        {
            InitializeComponent();
            chatThread = new Thread(this.UpdateChatField);
            usersThread = new Thread(this.UpdateUsers);
            this.Text = "Klijent";
            Communication.Instance.Connect();
            if (user == null)
            {
                txtMessage.Enabled = false;
                btnSend.Enabled = false;
                rtbMessages.Enabled = false;
            }


        }

        private void UpdateChatField()
        {
            
            try
            {
                while (true)
                {
                    String message = Communication.Instance.RecieveMessage();
                    Invoke(new Action(() => { this.rtbMessages.AppendText(message + "\n"); }));
                    //Thread.Sleep(1000);
                   // Monitor.Exit(myLocker);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(">>>>>>>>>>> " + ex.Message);
            }

        }

        private void UpdateUsers()
        {
            
            //Monitor.Enter(myLocker);
            try
            {
                while (true)
                {
                    Common.Message message = new Common.Message();
                    message.Operation = Operation.GetAllUsers;
                    Communication.Instance.Send(message);

                    BindingList<User> usersList = Communication.Instance.RecieveUserList();
                    

                    Invoke(new Action(() => { this.dataGridView1.DataSource = usersList; }));
                   // Monitor.Exit(myLocker);
                   // Thread.Sleep(1000);
                    


                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(">>>>>>>>>>> " + ex.Message);
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Common.Message message = new Common.Message()
            {
                Operation = Common.Operation.SendAll,
                Text = txtMessage.Text,
                user = user
            };
            Communication.Instance.Send(message);

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtName.Text;
            if (username == "") return;
            User user1 = new User()
            {
                Name = username,
            };
            Common.Message message = new Common.Message()
            {
                Operation = Operation.Login,
                user = user1,
                Text = ""

            };

            bool succesful = Communication.Instance.Login(message);
            if (succesful)
            {
                user = user1;
                txtUserPlaceholder.Text = username;
                txtName.Enabled = false;
                btnLogin.Enabled = false;
                txtMessage.Enabled = true;
                btnSend.Enabled = true;
                rtbMessages.Enabled = true;

                //usersThread.Start();
                
                chatThread.Start();
            }
            else MessageBox.Show("That username is already taken!");
        }


        private void Klijent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (user == null) return;
            Common.Message message = new Common.Message();
            message.Operation = Operation.LogOut;
            message.user = user;
            Communication.Instance.Send(message);
            this.user = null;



        }



     
    }
}

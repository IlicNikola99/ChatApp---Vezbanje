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

namespace Server
{
    public partial class Form1 : Form
    {
        private Server server=new Server();
        private bool kraj = false;
        public Form1()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void btnStartTime_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(this.StartTimer);
            thread.Start(); 
        }

        private void StartTimer()
        {
            kraj = false;
            try
            {
                while (!kraj)
                {
                    this.Invoke(new Action(() =>
                    {
                        lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                    }));
                    Thread.Sleep(1000);
                }
            }
            catch (InvalidOperationException ex)
            {

                Debug.WriteLine(">>>>> "+ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStopTime_Click(object sender, EventArgs e)
        {
            kraj = true;
        }
    }
}

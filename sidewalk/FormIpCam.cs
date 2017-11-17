using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidewalk
{
    public partial class FormIpCam : Form
    {
        private bool rodando;
        private bool pausa;

        public FormIpCam()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {

            if (rodando)
            {
                rodando = false;
                buttonRun.Text = "Start";
            }
            else
            {
                rodando = true;
                buttonRun.Text = "Cancel";

                //this.rodar();
                Thread t = new Thread(new ThreadStart(this.getFrame));          
                t.Start();
            }
        }

        private void getFrame()
        {
            string sourceURL = "http://192.168.0.11:8080/shot.jpg";
            byte[] buffer = new byte[800 * 480];
            int read, total;

            while (rodando)
            {
                if (!pausa)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
                    WebResponse resp = req.GetResponse();

                    Stream stream = resp.GetResponseStream();

                    total = 0;
                    while ((read = stream.Read(buffer, total, 1000)) != 0)
                    {
                        total += read;
                    }

                    Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));

                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox1.Image = bmp;
                        pictureBox1.Refresh();
                    });
                }
                
            }

        }

        private void FormIpCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rodando)
            {
                buttonRun_Click(null, null);
                e.Cancel = true;
            }
        }

        private void buttonPausa_Click(object sender, EventArgs e)
        {

            if (pausa)
            {
                pausa = false;
                buttonPausa.Text = "Continue";
            }
            else
            {
                pausa = true;
                buttonPausa.Text = "Pause";
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidewalk
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void buttonLerVideo_Click(object sender, EventArgs e)
        {
            FormLerVideo x = new FormLerVideo();
            x.ShowDialog();
        }

        private void buttonCameraIP_Click(object sender, EventArgs e)
        {
            FormIpCam x = new FormIpCam();
            x.ShowDialog();
        }

        private void buttonBluetooth_Click(object sender, EventArgs e)
        {
            FormBluetooth x = new FormBluetooth();
            x.ShowDialog();
        }

        private void buttonFeatures_Click(object sender, EventArgs e)
        {
            FormFeatures x = new FormFeatures();
            x.ShowDialog();
        }
    }
}

namespace Sidewalk
{
    partial class FormIpCam
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new AForge.Controls.PictureBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonPausa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(21, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(710, 342);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(112, 17);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(484, 20);
            this.textBoxIP.TabIndex = 4;
            this.textBoxIP.Text = "192.168.0.11:8080";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(21, 15);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 3;
            this.buttonRun.Text = "Start";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonPausa
            // 
            this.buttonPausa.Location = new System.Drawing.Point(602, 14);
            this.buttonPausa.Name = "buttonPausa";
            this.buttonPausa.Size = new System.Drawing.Size(75, 23);
            this.buttonPausa.TabIndex = 6;
            this.buttonPausa.Text = "Pausa";
            this.buttonPausa.UseVisualStyleBackColor = true;
            this.buttonPausa.Click += new System.EventHandler(this.buttonPausa_Click);
            // 
            // FormIpCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 460);
            this.Controls.Add(this.buttonPausa);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonRun);
            this.Name = "FormIpCam";
            this.Text = "FormIpCam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIpCam_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonPausa;
    }
}
namespace Sidewalk
{
    partial class FormMenu
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
            this.buttonLerVideo = new System.Windows.Forms.Button();
            this.buttonCameraIP = new System.Windows.Forms.Button();
            this.buttonBluetooth = new System.Windows.Forms.Button();
            this.buttonFeatures = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLerVideo
            // 
            this.buttonLerVideo.Location = new System.Drawing.Point(13, 13);
            this.buttonLerVideo.Name = "buttonLerVideo";
            this.buttonLerVideo.Size = new System.Drawing.Size(191, 23);
            this.buttonLerVideo.TabIndex = 0;
            this.buttonLerVideo.Text = "Ler Video";
            this.buttonLerVideo.UseVisualStyleBackColor = true;
            this.buttonLerVideo.Click += new System.EventHandler(this.buttonLerVideo_Click);
            // 
            // buttonCameraIP
            // 
            this.buttonCameraIP.Location = new System.Drawing.Point(13, 43);
            this.buttonCameraIP.Name = "buttonCameraIP";
            this.buttonCameraIP.Size = new System.Drawing.Size(191, 23);
            this.buttonCameraIP.TabIndex = 1;
            this.buttonCameraIP.Text = "Camera IP";
            this.buttonCameraIP.UseVisualStyleBackColor = true;
            this.buttonCameraIP.Click += new System.EventHandler(this.buttonCameraIP_Click);
            // 
            // buttonBluetooth
            // 
            this.buttonBluetooth.Location = new System.Drawing.Point(13, 73);
            this.buttonBluetooth.Name = "buttonBluetooth";
            this.buttonBluetooth.Size = new System.Drawing.Size(191, 23);
            this.buttonBluetooth.TabIndex = 2;
            this.buttonBluetooth.Text = "Bluetooth por COM?";
            this.buttonBluetooth.UseVisualStyleBackColor = true;
            this.buttonBluetooth.Click += new System.EventHandler(this.buttonBluetooth_Click);
            // 
            // buttonFeatures
            // 
            this.buttonFeatures.Location = new System.Drawing.Point(13, 103);
            this.buttonFeatures.Name = "buttonFeatures";
            this.buttonFeatures.Size = new System.Drawing.Size(191, 23);
            this.buttonFeatures.TabIndex = 3;
            this.buttonFeatures.Text = "Features";
            this.buttonFeatures.UseVisualStyleBackColor = true;
            this.buttonFeatures.Click += new System.EventHandler(this.buttonFeatures_Click);
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 502);
            this.Controls.Add(this.buttonFeatures);
            this.Controls.Add(this.buttonBluetooth);
            this.Controls.Add(this.buttonCameraIP);
            this.Controls.Add(this.buttonLerVideo);
            this.Name = "FormMenu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLerVideo;
        private System.Windows.Forms.Button buttonCameraIP;
        private System.Windows.Forms.Button buttonBluetooth;
        private System.Windows.Forms.Button buttonFeatures;
    }
}


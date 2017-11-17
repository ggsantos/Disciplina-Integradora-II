namespace Sidewalk
{
    partial class FormLerVideo
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
            this.buttonRun = new System.Windows.Forms.Button();
            this.textBoxVideo = new System.Windows.Forms.TextBox();
            this.pictureBoxVideo = new AForge.Controls.PictureBox();
            this.buttonPausa = new System.Windows.Forms.Button();
            this.labelCount = new System.Windows.Forms.Label();
            this.buttonSalvar = new System.Windows.Forms.Button();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.pictureBoxResult = new AForge.Controls.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(26, 31);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(88, 23);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "Start";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxVideo
            // 
            this.textBoxVideo.Location = new System.Drawing.Point(26, 5);
            this.textBoxVideo.Name = "textBoxVideo";
            this.textBoxVideo.Size = new System.Drawing.Size(517, 20);
            this.textBoxVideo.TabIndex = 1;
            this.textBoxVideo.Text = "C:\\Users\\Dragleer\\Documents\\Side Walk\\Biblioteca\\video1.avi";
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.Image = null;
            this.pictureBoxVideo.Location = new System.Drawing.Point(26, 60);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(588, 365);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxVideo.TabIndex = 2;
            this.pictureBoxVideo.TabStop = false;
            // 
            // buttonPausa
            // 
            this.buttonPausa.Location = new System.Drawing.Point(120, 31);
            this.buttonPausa.Name = "buttonPausa";
            this.buttonPausa.Size = new System.Drawing.Size(88, 23);
            this.buttonPausa.TabIndex = 3;
            this.buttonPausa.Text = "Pause";
            this.buttonPausa.UseVisualStyleBackColor = true;
            this.buttonPausa.Click += new System.EventHandler(this.buttonPausa_Click);
            // 
            // labelCount
            // 
            this.labelCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCount.Location = new System.Drawing.Point(549, 5);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(65, 20);
            this.labelCount.TabIndex = 4;
            this.labelCount.Text = "0";
            this.labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSalvar
            // 
            this.buttonSalvar.Location = new System.Drawing.Point(214, 31);
            this.buttonSalvar.Name = "buttonSalvar";
            this.buttonSalvar.Size = new System.Drawing.Size(88, 23);
            this.buttonSalvar.TabIndex = 5;
            this.buttonSalvar.Text = "Salve";
            this.buttonSalvar.UseVisualStyleBackColor = true;
            this.buttonSalvar.Click += new System.EventHandler(this.buttonSalvar_Click);
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(439, 31);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(88, 23);
            this.buttonProcess.TabIndex = 6;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.Image = null;
            this.pictureBoxResult.Location = new System.Drawing.Point(629, 60);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(588, 365);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 7;
            this.pictureBoxResult.TabStop = false;
            // 
            // FormLerVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 482);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.buttonProcess);
            this.Controls.Add(this.buttonSalvar);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.buttonPausa);
            this.Controls.Add(this.pictureBoxVideo);
            this.Controls.Add(this.textBoxVideo);
            this.Controls.Add(this.buttonRun);
            this.Name = "FormLerVideo";
            this.Text = "FormLerVideo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLerVideo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textBoxVideo;
        private AForge.Controls.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Button buttonPausa;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Button buttonSalvar;
        private System.Windows.Forms.Button buttonProcess;
        private AForge.Controls.PictureBox pictureBoxResult;
    }
}
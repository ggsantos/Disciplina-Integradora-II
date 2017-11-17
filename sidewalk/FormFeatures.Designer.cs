namespace Sidewalk
{
    partial class FormFeatures
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
            this.textBoxPasta = new System.Windows.Forms.TextBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.buttonHarrisCornersDetector = new System.Windows.Forms.Button();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.buttonMCO = new System.Windows.Forms.Button();
            this.buttonLocalBinaryPattern = new System.Windows.Forms.Button();
            this.buttonHOG = new System.Windows.Forms.Button();
            this.buttonHaralick = new System.Windows.Forms.Button();
            this.buttonProcessa = new System.Windows.Forms.Button();
            this.buttonProcessaCor = new System.Windows.Forms.Button();
            this.txtFile2 = new System.Windows.Forms.TextBox();
            this.buttonLoadImageNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPasta
            // 
            this.textBoxPasta.Location = new System.Drawing.Point(13, 13);
            this.textBoxPasta.Name = "textBoxPasta";
            this.textBoxPasta.Size = new System.Drawing.Size(373, 20);
            this.textBoxPasta.TabIndex = 0;
            this.textBoxPasta.Text = "C:\\Users\\Dragleer\\Documents\\Side Walk\\Fotos";
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(610, 13);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(115, 23);
            this.buttonLoadImage.TabIndex = 1;
            this.buttonLoadImage.Text = "Load Next Image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(12, 48);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(584, 356);
            this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginal.TabIndex = 2;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // buttonHarrisCornersDetector
            // 
            this.buttonHarrisCornersDetector.Location = new System.Drawing.Point(12, 421);
            this.buttonHarrisCornersDetector.Name = "buttonHarrisCornersDetector";
            this.buttonHarrisCornersDetector.Size = new System.Drawing.Size(125, 23);
            this.buttonHarrisCornersDetector.TabIndex = 3;
            this.buttonHarrisCornersDetector.Text = "HarrisCornersDetector";
            this.buttonHarrisCornersDetector.UseVisualStyleBackColor = true;
            this.buttonHarrisCornersDetector.Click += new System.EventHandler(this.buttonHarrisCornersDetector_Click);
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.Location = new System.Drawing.Point(614, 48);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(576, 356);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 4;
            this.pictureBoxResult.TabStop = false;
            // 
            // buttonMCO
            // 
            this.buttonMCO.Location = new System.Drawing.Point(143, 421);
            this.buttonMCO.Name = "buttonMCO";
            this.buttonMCO.Size = new System.Drawing.Size(125, 23);
            this.buttonMCO.TabIndex = 3;
            this.buttonMCO.Text = "MCO";
            this.buttonMCO.UseVisualStyleBackColor = true;
            this.buttonMCO.Click += new System.EventHandler(this.buttonMCO_Click);
            // 
            // buttonLocalBinaryPattern
            // 
            this.buttonLocalBinaryPattern.Location = new System.Drawing.Point(274, 421);
            this.buttonLocalBinaryPattern.Name = "buttonLocalBinaryPattern";
            this.buttonLocalBinaryPattern.Size = new System.Drawing.Size(125, 23);
            this.buttonLocalBinaryPattern.TabIndex = 5;
            this.buttonLocalBinaryPattern.Text = "LocalBinaryPattern";
            this.buttonLocalBinaryPattern.UseVisualStyleBackColor = true;
            this.buttonLocalBinaryPattern.Click += new System.EventHandler(this.buttonLocalBinaryPattern_Click);
            // 
            // buttonHOG
            // 
            this.buttonHOG.Location = new System.Drawing.Point(144, 450);
            this.buttonHOG.Name = "buttonHOG";
            this.buttonHOG.Size = new System.Drawing.Size(125, 23);
            this.buttonHOG.TabIndex = 6;
            this.buttonHOG.Text = "HOG";
            this.buttonHOG.UseVisualStyleBackColor = true;
            this.buttonHOG.Click += new System.EventHandler(this.buttonHOG_Click);
            // 
            // buttonHaralick
            // 
            this.buttonHaralick.Location = new System.Drawing.Point(13, 450);
            this.buttonHaralick.Name = "buttonHaralick";
            this.buttonHaralick.Size = new System.Drawing.Size(125, 23);
            this.buttonHaralick.TabIndex = 7;
            this.buttonHaralick.Text = "Haralick";
            this.buttonHaralick.UseVisualStyleBackColor = true;
            this.buttonHaralick.Click += new System.EventHandler(this.buttonHaralick_Click);
            // 
            // buttonProcessa
            // 
            this.buttonProcessa.Location = new System.Drawing.Point(471, 421);
            this.buttonProcessa.Name = "buttonProcessa";
            this.buttonProcessa.Size = new System.Drawing.Size(125, 23);
            this.buttonProcessa.TabIndex = 8;
            this.buttonProcessa.Text = "Processa";
            this.buttonProcessa.UseVisualStyleBackColor = true;
            this.buttonProcessa.Click += new System.EventHandler(this.buttonProcessa_Click);
            // 
            // buttonProcessaCor
            // 
            this.buttonProcessaCor.Location = new System.Drawing.Point(471, 449);
            this.buttonProcessaCor.Name = "buttonProcessaCor";
            this.buttonProcessaCor.Size = new System.Drawing.Size(125, 23);
            this.buttonProcessaCor.TabIndex = 9;
            this.buttonProcessaCor.Text = "Processa Cor";
            this.buttonProcessaCor.UseVisualStyleBackColor = true;
            this.buttonProcessaCor.Click += new System.EventHandler(this.buttonProcessaCor_Click);
            // 
            // txtFile2
            // 
            this.txtFile2.Location = new System.Drawing.Point(392, 13);
            this.txtFile2.Name = "txtFile2";
            this.txtFile2.Size = new System.Drawing.Size(204, 20);
            this.txtFile2.TabIndex = 10;
            this.txtFile2.Text = "video1_1159.png";
            // 
            // buttonLoadImageNew
            // 
            this.buttonLoadImageNew.Location = new System.Drawing.Point(732, 13);
            this.buttonLoadImageNew.Name = "buttonLoadImageNew";
            this.buttonLoadImageNew.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadImageNew.TabIndex = 11;
            this.buttonLoadImageNew.Text = "Load";
            this.buttonLoadImageNew.UseVisualStyleBackColor = true;
            this.buttonLoadImageNew.Click += new System.EventHandler(this.buttonLoadImageNew_Click);
            // 
            // FormFeatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 495);
            this.Controls.Add(this.buttonLoadImageNew);
            this.Controls.Add(this.txtFile2);
            this.Controls.Add(this.buttonProcessaCor);
            this.Controls.Add(this.buttonProcessa);
            this.Controls.Add(this.buttonHaralick);
            this.Controls.Add(this.buttonHOG);
            this.Controls.Add(this.buttonLocalBinaryPattern);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.buttonMCO);
            this.Controls.Add(this.buttonHarrisCornersDetector);
            this.Controls.Add(this.pictureBoxOriginal);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.textBoxPasta);
            this.Name = "FormFeatures";
            this.Text = "FormFeatures";
            this.Load += new System.EventHandler(this.FormFeatures_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPasta;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.Button buttonHarrisCornersDetector;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.Button buttonMCO;
        private System.Windows.Forms.Button buttonLocalBinaryPattern;
        private System.Windows.Forms.Button buttonHOG;
        private System.Windows.Forms.Button buttonHaralick;
        private System.Windows.Forms.Button buttonProcessa;
        private System.Windows.Forms.Button buttonProcessaCor;
        private System.Windows.Forms.TextBox txtFile2;
        private System.Windows.Forms.Button buttonLoadImageNew;
    }
}
namespace Squamster
{
    partial class BrightnessContrastControl
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
            this.brightnessSlider = new System.Windows.Forms.TrackBar();
            this.brightnessValue = new System.Windows.Forms.TextBox();
            this.lbl_Brightness = new System.Windows.Forms.Label();
            this.contrastSlider = new System.Windows.Forms.TrackBar();
            this.contrastValue = new System.Windows.Forms.TextBox();
            this.lbl_Contrast = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Okay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // brightnessSlider
            // 
            this.brightnessSlider.Location = new System.Drawing.Point(12, 14);
            this.brightnessSlider.Maximum = 100;
            this.brightnessSlider.Minimum = -100;
            this.brightnessSlider.Name = "brightnessSlider";
            this.brightnessSlider.Size = new System.Drawing.Size(225, 45);
            this.brightnessSlider.TabIndex = 0;
            this.brightnessSlider.TickFrequency = 10;
            this.brightnessSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.brightnessSlider.Scroll += new System.EventHandler(this.slider_Scroll);
            this.brightnessSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.contrastSlider_MouseUp);
            // 
            // brightnessValue
            // 
            this.brightnessValue.AcceptsReturn = true;
            this.brightnessValue.AcceptsTab = true;
            this.brightnessValue.BackColor = System.Drawing.SystemColors.Control;
            this.brightnessValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.brightnessValue.Location = new System.Drawing.Point(243, 26);
            this.brightnessValue.MaxLength = 4;
            this.brightnessValue.Name = "brightnessValue";
            this.brightnessValue.Size = new System.Drawing.Size(39, 13);
            this.brightnessValue.TabIndex = 1;
            this.brightnessValue.Text = "0%";
            this.brightnessValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.brightnessValue.WordWrap = false;
            this.brightnessValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.value_KeyDown);
            this.brightnessValue.Leave += new System.EventHandler(this.value_Leave);
            // 
            // lbl_Brightness
            // 
            this.lbl_Brightness.AutoSize = true;
            this.lbl_Brightness.Location = new System.Drawing.Point(1, 3);
            this.lbl_Brightness.Name = "lbl_Brightness";
            this.lbl_Brightness.Size = new System.Drawing.Size(56, 13);
            this.lbl_Brightness.TabIndex = 2;
            this.lbl_Brightness.Text = "Brightness";
            // 
            // contrastSlider
            // 
            this.contrastSlider.Location = new System.Drawing.Point(12, 54);
            this.contrastSlider.Maximum = 100;
            this.contrastSlider.Minimum = -100;
            this.contrastSlider.Name = "contrastSlider";
            this.contrastSlider.Size = new System.Drawing.Size(225, 45);
            this.contrastSlider.TabIndex = 0;
            this.contrastSlider.TickFrequency = 10;
            this.contrastSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.contrastSlider.Scroll += new System.EventHandler(this.slider_Scroll);
            this.contrastSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.contrastSlider_MouseUp);
            // 
            // contrastValue
            // 
            this.contrastValue.AcceptsReturn = true;
            this.contrastValue.AcceptsTab = true;
            this.contrastValue.BackColor = System.Drawing.SystemColors.Control;
            this.contrastValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contrastValue.Location = new System.Drawing.Point(243, 66);
            this.contrastValue.MaxLength = 4;
            this.contrastValue.Name = "contrastValue";
            this.contrastValue.Size = new System.Drawing.Size(39, 13);
            this.contrastValue.TabIndex = 1;
            this.contrastValue.Text = "0%";
            this.contrastValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.contrastValue.WordWrap = false;
            this.contrastValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.value_KeyDown);
            this.contrastValue.Leave += new System.EventHandler(this.value_Leave);
            // 
            // lbl_Contrast
            // 
            this.lbl_Contrast.AutoSize = true;
            this.lbl_Contrast.Location = new System.Drawing.Point(1, 43);
            this.lbl_Contrast.Name = "lbl_Contrast";
            this.lbl_Contrast.Size = new System.Drawing.Size(46, 13);
            this.lbl_Contrast.TabIndex = 2;
            this.lbl_Contrast.Text = "Contrast";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(207, 99);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "&Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Okay
            // 
            this.btn_Okay.Location = new System.Drawing.Point(126, 99);
            this.btn_Okay.Name = "btn_Okay";
            this.btn_Okay.Size = new System.Drawing.Size(75, 23);
            this.btn_Okay.TabIndex = 3;
            this.btn_Okay.Text = "&OK";
            this.btn_Okay.UseVisualStyleBackColor = true;
            this.btn_Okay.Click += new System.EventHandler(this.btn_Okay_Click);
            // 
            // BrightnessContrastControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 134);
            this.Controls.Add(this.btn_Okay);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lbl_Contrast);
            this.Controls.Add(this.lbl_Brightness);
            this.Controls.Add(this.contrastValue);
            this.Controls.Add(this.brightnessValue);
            this.Controls.Add(this.contrastSlider);
            this.Controls.Add(this.brightnessSlider);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BrightnessContrastControl";
            this.Text = "BrightnessControl";
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar brightnessSlider;
        private System.Windows.Forms.TextBox brightnessValue;
        private System.Windows.Forms.Label lbl_Brightness;
        private System.Windows.Forms.TrackBar contrastSlider;
        private System.Windows.Forms.TextBox contrastValue;
        private System.Windows.Forms.Label lbl_Contrast;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Okay;
    }
}
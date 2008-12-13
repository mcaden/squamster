namespace Squamster
{
    partial class OgreForm : System.Windows.Forms.Form
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statsLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.texList = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.loopAnim = new System.Windows.Forms.CheckBox();
            this.Btn_Anim_Stop = new System.Windows.Forms.Button();
            this.Btn_Anim_Play = new System.Windows.Forms.Button();
            this.animBox = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Btn_Load = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.meshListBox = new System.Windows.Forms.ListBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.ForeColor = System.Drawing.Color.DimGray;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel1.Controls.Add(this.statsLabel);
            this.splitContainer1.Panel1.MouseLeave += new System.EventHandler(this.splitContainer1_Panel1_MouseLeave);
            this.splitContainer1.Panel1.MouseEnter += new System.EventHandler(this.splitContainer1_Panel1_MouseEnter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel2.BackgroundImage = global::Squamster.Properties.Resources.sidebar;
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.texList);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.loopAnim);
            this.splitContainer1.Panel2.Controls.Add(this.Btn_Anim_Stop);
            this.splitContainer1.Panel2.Controls.Add(this.Btn_Anim_Play);
            this.splitContainer1.Panel2.Controls.Add(this.animBox);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.Btn_Load);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.meshListBox);
            this.splitContainer1.Size = new System.Drawing.Size(934, 600);
            this.splitContainer1.SplitterDistance = 788;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // statsLabel
            // 
            this.statsLabel.AutoSize = true;
            this.statsLabel.BackColor = System.Drawing.Color.Transparent;
            this.statsLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsLabel.ForeColor = System.Drawing.Color.White;
            this.statsLabel.Location = new System.Drawing.Point(0, 0);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(97, 15);
            this.statsLabel.TabIndex = 0;
            this.statsLabel.Text = "No Mesh Selected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(72, 387);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Color:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(110, 387);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 15);
            this.panel1.TabIndex = 12;
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // texList
            // 
            this.texList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.texList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.texList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.texList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.texList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.texList.ForeColor = System.Drawing.Color.Black;
            this.texList.FormattingEnabled = true;
            this.texList.Location = new System.Drawing.Point(7, 408);
            this.texList.Name = "texList";
            this.texList.Size = new System.Drawing.Size(128, 21);
            this.texList.TabIndex = 11;
            this.texList.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBox1.BackgroundImage = global::Squamster.Properties.Resources.pictureBoxBG;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(7, 434);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // loopAnim
            // 
            this.loopAnim.AutoSize = true;
            this.loopAnim.BackColor = System.Drawing.Color.Transparent;
            this.loopAnim.Checked = true;
            this.loopAnim.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loopAnim.Enabled = false;
            this.loopAnim.FlatAppearance.BorderColor = System.Drawing.Color.Maroon;
            this.loopAnim.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.loopAnim.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.loopAnim.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.loopAnim.ForeColor = System.Drawing.Color.White;
            this.loopAnim.Location = new System.Drawing.Point(7, 251);
            this.loopAnim.Name = "loopAnim";
            this.loopAnim.Size = new System.Drawing.Size(99, 17);
            this.loopAnim.TabIndex = 7;
            this.loopAnim.Text = "Loop Animation";
            this.loopAnim.UseVisualStyleBackColor = false;
            this.loopAnim.MouseClick += new System.Windows.Forms.MouseEventHandler(this.loopAnim_MouseClick);
            // 
            // Btn_Anim_Stop
            // 
            this.Btn_Anim_Stop.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Anim_Stop.Enabled = false;
            this.Btn_Anim_Stop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Anim_Stop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_Anim_Stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Anim_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Anim_Stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Anim_Stop.ForeColor = System.Drawing.Color.White;
            this.Btn_Anim_Stop.Location = new System.Drawing.Point(83, 222);
            this.Btn_Anim_Stop.Name = "Btn_Anim_Stop";
            this.Btn_Anim_Stop.Size = new System.Drawing.Size(52, 23);
            this.Btn_Anim_Stop.TabIndex = 6;
            this.Btn_Anim_Stop.Text = "Stop";
            this.Btn_Anim_Stop.UseVisualStyleBackColor = false;
            this.Btn_Anim_Stop.Click += new System.EventHandler(this.Btn_Anim_Stop_Click);
            // 
            // Btn_Anim_Play
            // 
            this.Btn_Anim_Play.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Anim_Play.Enabled = false;
            this.Btn_Anim_Play.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Anim_Play.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_Anim_Play.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Anim_Play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Anim_Play.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Anim_Play.ForeColor = System.Drawing.Color.White;
            this.Btn_Anim_Play.Location = new System.Drawing.Point(7, 222);
            this.Btn_Anim_Play.Name = "Btn_Anim_Play";
            this.Btn_Anim_Play.Size = new System.Drawing.Size(70, 23);
            this.Btn_Anim_Play.TabIndex = 5;
            this.Btn_Anim_Play.Text = "Play";
            this.Btn_Anim_Play.UseVisualStyleBackColor = false;
            this.Btn_Anim_Play.Click += new System.EventHandler(this.Btn_Anim_Play_Click);
            // 
            // animBox
            // 
            this.animBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.animBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.animBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.animBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.animBox.DropDownWidth = 128;
            this.animBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.animBox.ForeColor = System.Drawing.Color.Black;
            this.animBox.FormattingEnabled = true;
            this.animBox.Location = new System.Drawing.Point(7, 195);
            this.animBox.Name = "animBox";
            this.animBox.Size = new System.Drawing.Size(128, 21);
            this.animBox.TabIndex = 4;
            this.animBox.SelectionChangeCommitted += new System.EventHandler(this.animBox_SelectionChangeCommitted);
            this.animBox.DropDownClosed += new System.EventHandler(this.animBox_SelectionChangeCommitted);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(71, 131);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Load All";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Btd_LoadAll_Click);
            // 
            // Btn_Load
            // 
            this.Btn_Load.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Load.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Btn_Load.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Load.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_Load.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Load.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Load.ForeColor = System.Drawing.Color.White;
            this.Btn_Load.Location = new System.Drawing.Point(7, 131);
            this.Btn_Load.Name = "Btn_Load";
            this.Btn_Load.Size = new System.Drawing.Size(58, 23);
            this.Btn_Load.TabIndex = 2;
            this.Btn_Load.Text = "Load...";
            this.Btn_Load.UseVisualStyleBackColor = false;
            this.Btn_Load.Click += new System.EventHandler(this.Btn_Load_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Meshes";
            // 
            // meshListBox
            // 
            this.meshListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.meshListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.meshListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meshListBox.ForeColor = System.Drawing.Color.Black;
            this.meshListBox.FormattingEnabled = true;
            this.meshListBox.Location = new System.Drawing.Point(7, 30);
            this.meshListBox.Name = "meshListBox";
            this.meshListBox.Size = new System.Drawing.Size(128, 93);
            this.meshListBox.TabIndex = 0;
            this.meshListBox.SelectedIndexChanged += new System.EventHandler(this.meshListBox_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // OgreForm
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(934, 571);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = global::Squamster.Properties.Resources.Squamster;
            this.Name = "OgreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Squamster";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel RenderWindowPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox meshListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Btn_Load;
        private System.Windows.Forms.ComboBox animBox;
        private System.Windows.Forms.Button Btn_Anim_Play;
        private System.Windows.Forms.Button Btn_Anim_Stop;
        private System.Windows.Forms.CheckBox loopAnim;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox texList;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label2;

    }
}
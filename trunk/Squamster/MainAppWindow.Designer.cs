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
            this.statsLabel = new System.Windows.Forms.Label();
            this.Lbl_Color = new System.Windows.Forms.Label();
            this.colorSelector = new System.Windows.Forms.Panel();
            this.texList = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.loopAnim = new System.Windows.Forms.CheckBox();
            this.Btn_Anim_Stop = new System.Windows.Forms.Button();
            this.Btn_Anim_Play = new System.Windows.Forms.Button();
            this.animBox = new System.Windows.Forms.ComboBox();
            this.Btn_Load_All = new System.Windows.Forms.Button();
            this.Btn_Load = new System.Windows.Forms.Button();
            this.Lbl_Mesh_List = new System.Windows.Forms.Label();
            this.meshListBox = new System.Windows.Forms.ListBox();
            this.texturePreviewList = new System.Windows.Forms.ImageList(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Btn_View = new System.Windows.Forms.Button();
            this.Btn_Paint = new System.Windows.Forms.Button();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.paintPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.brushOpacityControl = new System.Windows.Forms.NumericUpDown();
            this.brushScaleControl = new System.Windows.Forms.NumericUpDown();
            this.Lbl_Brushes = new System.Windows.Forms.Label();
            this.brushList = new System.Windows.Forms.ListView();
            this.brushPreviewList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_Paint = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_View = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToGrayscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.blurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gaussianBlurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sharpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.viewPanel.SuspendLayout();
            this.paintPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brushOpacityControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brushScaleControl)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statsLabel
            // 
            this.statsLabel.AutoSize = true;
            this.statsLabel.BackColor = System.Drawing.Color.Transparent;
            this.statsLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statsLabel.ForeColor = System.Drawing.Color.White;
            this.statsLabel.Location = new System.Drawing.Point(0, 0);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(97, 15);
            this.statsLabel.TabIndex = 0;
            this.statsLabel.Text = "No Mesh Selected";
            // 
            // Lbl_Color
            // 
            this.Lbl_Color.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Color.ForeColor = System.Drawing.Color.White;
            this.Lbl_Color.Location = new System.Drawing.Point(68, 389);
            this.Lbl_Color.Name = "Lbl_Color";
            this.Lbl_Color.Size = new System.Drawing.Size(34, 13);
            this.Lbl_Color.TabIndex = 2;
            this.Lbl_Color.Text = "Color:";
            // 
            // colorSelector
            // 
            this.colorSelector.BackColor = System.Drawing.Color.Black;
            this.colorSelector.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.colorSelector.ForeColor = System.Drawing.Color.White;
            this.colorSelector.Location = new System.Drawing.Point(110, 387);
            this.colorSelector.Name = "colorSelector";
            this.colorSelector.Size = new System.Drawing.Size(25, 15);
            this.colorSelector.TabIndex = 12;
            this.colorSelector.MouseUp += new System.Windows.Forms.MouseEventHandler(this.selectColor);
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
            this.texList.SelectedIndexChanged += new System.EventHandler(this.selectTexture);
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
            this.animBox.SelectionChangeCommitted += new System.EventHandler(this.selectAnim);
            this.animBox.DropDownClosed += new System.EventHandler(this.selectAnim);
            // 
            // Btn_Load_All
            // 
            this.Btn_Load_All.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Load_All.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Load_All.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_Load_All.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Load_All.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Load_All.ForeColor = System.Drawing.Color.White;
            this.Btn_Load_All.Location = new System.Drawing.Point(71, 131);
            this.Btn_Load_All.Name = "Btn_Load_All";
            this.Btn_Load_All.Size = new System.Drawing.Size(64, 23);
            this.Btn_Load_All.TabIndex = 3;
            this.Btn_Load_All.Text = "Load All";
            this.Btn_Load_All.UseVisualStyleBackColor = false;
            this.Btn_Load_All.Click += new System.EventHandler(this.Btn_LoadAll_Click);
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
            // Lbl_Mesh_List
            // 
            this.Lbl_Mesh_List.AutoSize = true;
            this.Lbl_Mesh_List.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Mesh_List.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Mesh_List.ForeColor = System.Drawing.Color.White;
            this.Lbl_Mesh_List.Location = new System.Drawing.Point(4, 12);
            this.Lbl_Mesh_List.Name = "Lbl_Mesh_List";
            this.Lbl_Mesh_List.Size = new System.Drawing.Size(106, 13);
            this.Lbl_Mesh_List.TabIndex = 1;
            this.Lbl_Mesh_List.Text = "Available Meshes";
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
            this.meshListBox.SelectedIndexChanged += new System.EventHandler(this.selectMesh);
            // 
            // texturePreviewList
            // 
            this.texturePreviewList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.texturePreviewList.ImageSize = new System.Drawing.Size(128, 128);
            this.texturePreviewList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // Btn_View
            // 
            this.Btn_View.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_View.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.Btn_View.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_View.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_View.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_View.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_View.ForeColor = System.Drawing.Color.White;
            this.Btn_View.Location = new System.Drawing.Point(784, 12);
            this.Btn_View.Name = "Btn_View";
            this.Btn_View.Size = new System.Drawing.Size(20, 81);
            this.Btn_View.TabIndex = 0;
            this.Btn_View.Text = "V\r\ni\r\ne\r\nw\r\n\r\n";
            this.Btn_View.UseVisualStyleBackColor = false;
            this.Btn_View.Click += new System.EventHandler(this.setViewMode);
            // 
            // Btn_Paint
            // 
            this.Btn_Paint.BackColor = System.Drawing.Color.Black;
            this.Btn_Paint.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.Btn_Paint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Btn_Paint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Paint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Paint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.Btn_Paint.ForeColor = System.Drawing.Color.White;
            this.Btn_Paint.Location = new System.Drawing.Point(784, 99);
            this.Btn_Paint.Name = "Btn_Paint";
            this.Btn_Paint.Size = new System.Drawing.Size(20, 81);
            this.Btn_Paint.TabIndex = 1;
            this.Btn_Paint.Text = "P\r\na\r\ni\r\nn\r\nt\r\n";
            this.Btn_Paint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Btn_Paint.UseVisualStyleBackColor = false;
            this.Btn_Paint.Click += new System.EventHandler(this.setPaintMode);
            // 
            // viewPanel
            // 
            this.viewPanel.BackgroundImage = global::Squamster.Properties.Resources.sidebar;
            this.viewPanel.Controls.Add(this.Lbl_Mesh_List);
            this.viewPanel.Controls.Add(this.meshListBox);
            this.viewPanel.Controls.Add(this.texList);
            this.viewPanel.Controls.Add(this.animBox);
            this.viewPanel.Controls.Add(this.Btn_Anim_Play);
            this.viewPanel.Controls.Add(this.Btn_Anim_Stop);
            this.viewPanel.Controls.Add(this.Btn_Load);
            this.viewPanel.Controls.Add(this.Btn_Load_All);
            this.viewPanel.Controls.Add(this.pictureBox1);
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(0, 0);
            this.viewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(143, 605);
            this.viewPanel.TabIndex = 1;
            // 
            // paintPanel
            // 
            this.paintPanel.BackColor = System.Drawing.Color.Silver;
            this.paintPanel.BackgroundImage = global::Squamster.Properties.Resources.sidebar;
            this.paintPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paintPanel.Controls.Add(this.label2);
            this.paintPanel.Controls.Add(this.label1);
            this.paintPanel.Controls.Add(this.brushOpacityControl);
            this.paintPanel.Controls.Add(this.brushScaleControl);
            this.paintPanel.Controls.Add(this.Lbl_Brushes);
            this.paintPanel.Controls.Add(this.brushList);
            this.paintPanel.Controls.Add(this.colorSelector);
            this.paintPanel.Controls.Add(this.Lbl_Color);
            this.paintPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paintPanel.Location = new System.Drawing.Point(0, 0);
            this.paintPanel.Margin = new System.Windows.Forms.Padding(0);
            this.paintPanel.Name = "paintPanel";
            this.paintPanel.Size = new System.Drawing.Size(143, 605);
            this.paintPanel.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Brush Opacity";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Brush Size";
            // 
            // brushOpacityControl
            // 
            this.brushOpacityControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.brushOpacityControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brushOpacityControl.DecimalPlaces = 2;
            this.brushOpacityControl.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.brushOpacityControl.Location = new System.Drawing.Point(83, 159);
            this.brushOpacityControl.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.brushOpacityControl.Name = "brushOpacityControl";
            this.brushOpacityControl.Size = new System.Drawing.Size(51, 20);
            this.brushOpacityControl.TabIndex = 14;
            this.brushOpacityControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.brushOpacityControl.ValueChanged += new System.EventHandler(this.brushOpacityControl_ValueChanged);
            // 
            // brushScaleControl
            // 
            this.brushScaleControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.brushScaleControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brushScaleControl.DecimalPlaces = 2;
            this.brushScaleControl.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.brushScaleControl.Location = new System.Drawing.Point(82, 133);
            this.brushScaleControl.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.brushScaleControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.brushScaleControl.Name = "brushScaleControl";
            this.brushScaleControl.Size = new System.Drawing.Size(52, 20);
            this.brushScaleControl.TabIndex = 13;
            this.brushScaleControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.brushScaleControl.ValueChanged += new System.EventHandler(this.brushScaleControl_ValueChanged);
            // 
            // Lbl_Brushes
            // 
            this.Lbl_Brushes.AutoSize = true;
            this.Lbl_Brushes.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Brushes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Brushes.ForeColor = System.Drawing.Color.White;
            this.Lbl_Brushes.Location = new System.Drawing.Point(4, 12);
            this.Lbl_Brushes.Name = "Lbl_Brushes";
            this.Lbl_Brushes.Size = new System.Drawing.Size(52, 13);
            this.Lbl_Brushes.TabIndex = 1;
            this.Lbl_Brushes.Text = "Brushes";
            // 
            // brushList
            // 
            this.brushList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.brushList.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.brushList.AutoArrange = false;
            this.brushList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.brushList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.brushList.LabelWrap = false;
            this.brushList.LargeImageList = this.brushPreviewList;
            this.brushList.Location = new System.Drawing.Point(7, 30);
            this.brushList.Margin = new System.Windows.Forms.Padding(0);
            this.brushList.MultiSelect = false;
            this.brushList.Name = "brushList";
            this.brushList.ShowGroups = false;
            this.brushList.Size = new System.Drawing.Size(128, 97);
            this.brushList.SmallImageList = this.brushPreviewList;
            this.brushList.StateImageList = this.brushPreviewList;
            this.brushList.TabIndex = 0;
            this.brushList.TileSize = new System.Drawing.Size(34, 34);
            this.brushList.UseCompatibleStateImageBehavior = false;
            this.brushList.View = System.Windows.Forms.View.List;
            this.brushList.SelectedIndexChanged += new System.EventHandler(this.brushList_SelectedIndexChanged);
            // 
            // brushPreviewList
            // 
            this.brushPreviewList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.brushPreviewList.ImageSize = new System.Drawing.Size(16, 16);
            this.brushPreviewList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Silver;
            this.menuStrip1.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.filtersToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(944, 19);
            this.menuStrip1.Stretch = false;
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(47, 18);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_View_Paint,
            this.Menu_View_View});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 18);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // Menu_View_Paint
            // 
            this.Menu_View_Paint.CheckOnClick = true;
            this.Menu_View_Paint.Name = "Menu_View_Paint";
            this.Menu_View_Paint.Size = new System.Drawing.Size(109, 22);
            this.Menu_View_Paint.Text = "&Paint";
            this.Menu_View_Paint.Click += new System.EventHandler(this.paintToolStripMenuItem_Click);
            // 
            // Menu_View_View
            // 
            this.Menu_View_View.CheckOnClick = true;
            this.Menu_View_View.Name = "Menu_View_View";
            this.Menu_View_View.Size = new System.Drawing.Size(109, 22);
            this.Menu_View_View.Text = "&View";
            this.Menu_View_View.Click += new System.EventHandler(this.viewToolStripMenuItem1_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invertColorsToolStripMenuItem,
            this.convertToGrayscaleToolStripMenuItem,
            this.brightnessToolStripMenuItem,
            this.blurToolStripMenuItem,
            this.gaussianBlurToolStripMenuItem,
            this.sharpenToolStripMenuItem});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(68, 18);
            this.filtersToolStripMenuItem.Text = "&Filters";
            // 
            // invertColorsToolStripMenuItem
            // 
            this.invertColorsToolStripMenuItem.Name = "invertColorsToolStripMenuItem";
            this.invertColorsToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.invertColorsToolStripMenuItem.Text = "&Invert Colors";
            this.invertColorsToolStripMenuItem.Click += new System.EventHandler(this.invertColorsToolStripMenuItem_Click);
            // 
            // convertToGrayscaleToolStripMenuItem
            // 
            this.convertToGrayscaleToolStripMenuItem.Name = "convertToGrayscaleToolStripMenuItem";
            this.convertToGrayscaleToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.convertToGrayscaleToolStripMenuItem.Text = "Convert to &Grayscale";
            this.convertToGrayscaleToolStripMenuItem.Click += new System.EventHandler(this.convertToGrayscaleToolStripMenuItem_Click);
            // 
            // brightnessToolStripMenuItem
            // 
            this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
            this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.brightnessToolStripMenuItem.Text = "Brightness && &Contrast";
            this.brightnessToolStripMenuItem.Click += new System.EventHandler(this.brightnessToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Gray;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 19);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel1.Controls.Add(this.Btn_Paint);
            this.splitContainer1.Panel1.Controls.Add(this.Btn_View);
            this.splitContainer1.Panel1.Controls.Add(this.statsLabel);
            this.splitContainer1.Panel1.MouseLeave += new System.EventHandler(this.splitContainer1_Panel1_MouseLeave_1);
            this.splitContainer1.Panel1.MouseEnter += new System.EventHandler(this.splitContainer1_Panel1_MouseEnter_1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.paintPanel);
            this.splitContainer1.Panel2.Controls.Add(this.viewPanel);
            this.splitContainer1.Size = new System.Drawing.Size(944, 605);
            this.splitContainer1.SplitterDistance = 800;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 4;
            // 
            // blurToolStripMenuItem
            // 
            this.blurToolStripMenuItem.Name = "blurToolStripMenuItem";
            this.blurToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.blurToolStripMenuItem.Text = "&Blur";
            this.blurToolStripMenuItem.Click += new System.EventHandler(this.blurToolStripMenuItem_Click);
            // 
            // gaussianBlurToolStripMenuItem
            // 
            this.gaussianBlurToolStripMenuItem.Name = "gaussianBlurToolStripMenuItem";
            this.gaussianBlurToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.gaussianBlurToolStripMenuItem.Text = "G&aussian Blur";
            this.gaussianBlurToolStripMenuItem.Click += new System.EventHandler(this.gaussianBlurToolStripMenuItem_Click);
            // 
            // sharpenToolStripMenuItem
            // 
            this.sharpenToolStripMenuItem.Name = "sharpenToolStripMenuItem";
            this.sharpenToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.sharpenToolStripMenuItem.Text = "&Sharpen";
            this.sharpenToolStripMenuItem.Click += new System.EventHandler(this.sharpenToolStripMenuItem_Click);
            // 
            // OgreForm
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(944, 624);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = global::Squamster.Properties.Resources.Squamster;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OgreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Squamster";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.viewPanel.ResumeLayout(false);
            this.viewPanel.PerformLayout();
            this.paintPanel.ResumeLayout(false);
            this.paintPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brushOpacityControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brushScaleControl)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel RenderWindowPanel;
        private System.Windows.Forms.ListBox meshListBox;
        private System.Windows.Forms.Label Lbl_Mesh_List;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.Button Btn_Load_All;
        private System.Windows.Forms.Button Btn_Load;
        private System.Windows.Forms.ComboBox animBox;
        private System.Windows.Forms.Button Btn_Anim_Play;
        private System.Windows.Forms.Button Btn_Anim_Stop;
        private System.Windows.Forms.CheckBox loopAnim;
        private System.Windows.Forms.ImageList texturePreviewList;
        private System.Windows.Forms.ComboBox texList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel colorSelector;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label Lbl_Color;
        private System.Windows.Forms.Button Btn_View;
        private System.Windows.Forms.Panel viewPanel;
        private System.Windows.Forms.Button Btn_Paint;
        private System.Windows.Forms.Panel paintPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Paint;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_View;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label Lbl_Brushes;
        private System.Windows.Forms.ListView brushList;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ImageList brushPreviewList;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown brushOpacityControl;
        private System.Windows.Forms.NumericUpDown brushScaleControl;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToGrayscaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blurToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gaussianBlurToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sharpenToolStripMenuItem;

    }
}
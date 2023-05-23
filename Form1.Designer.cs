namespace NetworkMapperForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            capturePacketsBtn=new Button();
            captureDevice=new ComboBox();
            refreshDevices=new Button();
            resultBox=new RichTextBox();
            stopCapBtn=new Button();
            exitBtn=new Button();
            panel1=new Panel();
            label1=new Label();
            gmap=new GMap.NET.WindowsForms.GMapControl();
            backgroundWorker1=new System.ComponentModel.BackgroundWorker();
            ColourKeyInfo=new Label();
            SelectedIp=new Label();
            BlockButton=new Button();
            UnblockButton=new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // capturePacketsBtn
            // 
            capturePacketsBtn.BackColor=Color.FromArgb(0, 192, 0);
            capturePacketsBtn.FlatAppearance.BorderColor=Color.White;
            capturePacketsBtn.FlatStyle=FlatStyle.Flat;
            capturePacketsBtn.Location=new Point(401, 95);
            capturePacketsBtn.Margin=new Padding(4);
            capturePacketsBtn.Name="capturePacketsBtn";
            capturePacketsBtn.Size=new Size(108, 36);
            capturePacketsBtn.TabIndex=0;
            capturePacketsBtn.Text="Capture Packets";
            capturePacketsBtn.UseVisualStyleBackColor=false;
            capturePacketsBtn.Click+=capturePacketsBtn_Click;
            // 
            // captureDevice
            // 
            captureDevice.FlatStyle=FlatStyle.Flat;
            captureDevice.FormattingEnabled=true;
            captureDevice.Location=new Point(14, 139);
            captureDevice.Margin=new Padding(4);
            captureDevice.Name="captureDevice";
            captureDevice.Size=new Size(495, 31);
            captureDevice.TabIndex=1;
            captureDevice.Tag="";
            captureDevice.Text="Capture Device";
            captureDevice.SelectedIndexChanged+=comboBox1_SelectedIndexChanged;
            // 
            // refreshDevices
            // 
            refreshDevices.BackColor=Color.FromArgb(64, 64, 64);
            refreshDevices.FlatStyle=FlatStyle.Flat;
            refreshDevices.ForeColor=Color.White;
            refreshDevices.Location=new Point(14, 95);
            refreshDevices.Margin=new Padding(4);
            refreshDevices.Name="refreshDevices";
            refreshDevices.Size=new Size(264, 36);
            refreshDevices.TabIndex=3;
            refreshDevices.Text="Refresh Devices";
            refreshDevices.UseVisualStyleBackColor=false;
            refreshDevices.Click+=refreshDevices_Click;
            // 
            // resultBox
            // 
            resultBox.BackColor=Color.DimGray;
            resultBox.BorderStyle=BorderStyle.None;
            resultBox.Font=new Font("Calibri", 11F, FontStyle.Regular, GraphicsUnit.Point);
            resultBox.ForeColor=SystemColors.Window;
            resultBox.HideSelection=false;
            resultBox.Location=new Point(13, 185);
            resultBox.Margin=new Padding(4);
            resultBox.Name="resultBox";
            resultBox.Size=new Size(680, 565);
            resultBox.TabIndex=4;
            resultBox.Text="";
            resultBox.TextChanged+=resultBox_TextChanged;
            // 
            // stopCapBtn
            // 
            stopCapBtn.BackColor=Color.FromArgb(192, 0, 0);
            stopCapBtn.FlatStyle=FlatStyle.Flat;
            stopCapBtn.ForeColor=Color.White;
            stopCapBtn.Location=new Point(285, 95);
            stopCapBtn.Margin=new Padding(4);
            stopCapBtn.Name="stopCapBtn";
            stopCapBtn.Size=new Size(108, 36);
            stopCapBtn.TabIndex=5;
            stopCapBtn.Text="Stop";
            stopCapBtn.UseVisualStyleBackColor=false;
            stopCapBtn.Click+=stopCapBtn_Click;
            // 
            // exitBtn
            // 
            exitBtn.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            exitBtn.AutoSize=true;
            exitBtn.BackColor=Color.FromArgb(64, 64, 64);
            exitBtn.FlatStyle=FlatStyle.Flat;
            exitBtn.ForeColor=Color.White;
            exitBtn.Location=new Point(1333, 95);
            exitBtn.Name="exitBtn";
            exitBtn.Size=new Size(75, 35);
            exitBtn.TabIndex=6;
            exitBtn.Text="Exit";
            exitBtn.UseVisualStyleBackColor=false;
            exitBtn.Click+=button1_Click;
            // 
            // panel1
            // 
            panel1.AutoSize=true;
            panel1.BackColor=Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(label1);
            panel1.ForeColor=Color.White;
            panel1.Location=new Point(0, 0);
            panel1.Name="panel1";
            panel1.Size=new Size(1904, 80);
            panel1.TabIndex=7;
            panel1.MouseDown+=panel1_MouseDown;
            panel1.MouseMove+=panel1_MouseMove;
            panel1.MouseUp+=panel1_MouseUp;
            // 
            // label1
            // 
            label1.Anchor=AnchorStyles.Top|AnchorStyles.Bottom;
            label1.AutoSize=true;
            label1.Font=new Font("Calibri", 48F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location=new Point(12, 0);
            label1.Name="label1";
            label1.Size=new Size(558, 78);
            label1.TabIndex=8;
            label1.Text="Connection Mapper";
            label1.Click+=label1_Click;
            // 
            // gmap
            // 
            gmap.Bearing=0F;
            gmap.CanDragMap=true;
            gmap.EmptyTileColor=Color.Navy;
            gmap.GrayScaleMode=false;
            gmap.HelperLineOption=GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            gmap.LevelsKeepInMemory=5;
            gmap.Location=new Point(700, 185);
            gmap.MarkersEnabled=true;
            gmap.MaxZoom=18;
            gmap.MinZoom=1;
            gmap.MouseWheelZoomEnabled=true;
            gmap.MouseWheelZoomType=GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gmap.Name="gmap";
            gmap.NegativeMode=false;
            gmap.PolygonsEnabled=true;
            gmap.RetryLoadTile=0;
            gmap.RoutesEnabled=true;
            gmap.ScaleMode=GMap.NET.WindowsForms.ScaleModes.Integer;
            gmap.SelectedAreaFillColor=Color.FromArgb(33, 65, 105, 225);
            gmap.ShowTileGridLines=false;
            gmap.Size=new Size(708, 566);
            gmap.TabIndex=8;
            gmap.Zoom=0D;
            gmap.Load+=gMapControl1_Load;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress=true;
            backgroundWorker1.WorkerSupportsCancellation=true;
            backgroundWorker1.DoWork+=backgroundWorker1_DoWork;
            // 
            // ColourKeyInfo
            // 
            ColourKeyInfo.AutoSize=true;
            ColourKeyInfo.Location=new Point(988, 159);
            ColourKeyInfo.Name="ColourKeyInfo";
            ColourKeyInfo.Size=new Size(420, 23);
            ColourKeyInfo.TabIndex=9;
            ColourKeyInfo.Text="Key:  Blue - Home, Red - Inbound, Green - Outbound";
            ColourKeyInfo.Click+=label2_Click;
            // 
            // SelectedIp
            // 
            SelectedIp.AutoSize=true;
            SelectedIp.Location=new Point(700, 159);
            SelectedIp.Name="SelectedIp";
            SelectedIp.Size=new Size(107, 23);
            SelectedIp.TabIndex=10;
            SelectedIp.Text="Selected IP: ";
            SelectedIp.Click+=label3_Click;
            // 
            // BlockButton
            // 
            BlockButton.BackColor=Color.FromArgb(0, 192, 0);
            BlockButton.FlatAppearance.BorderColor=Color.White;
            BlockButton.FlatStyle=FlatStyle.Flat;
            BlockButton.Location=new Point(699, 94);
            BlockButton.Margin=new Padding(4);
            BlockButton.Name="BlockButton";
            BlockButton.Size=new Size(108, 36);
            BlockButton.TabIndex=11;
            BlockButton.Text="Block";
            BlockButton.UseVisualStyleBackColor=false;
            BlockButton.Click+=BlockButton_Click;
            // 
            // UnblockButton
            // 
            UnblockButton.BackColor=Color.FromArgb(0, 192, 0);
            UnblockButton.FlatAppearance.BorderColor=Color.White;
            UnblockButton.FlatStyle=FlatStyle.Flat;
            UnblockButton.Location=new Point(815, 94);
            UnblockButton.Margin=new Padding(4);
            UnblockButton.Name="UnblockButton";
            UnblockButton.Size=new Size(126, 36);
            UnblockButton.TabIndex=12;
            UnblockButton.Text="Unblock All";
            UnblockButton.UseVisualStyleBackColor=false;
            UnblockButton.Click+=UnblockButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(10F, 23F);
            AutoScaleMode=AutoScaleMode.Font;
            BackColor=SystemColors.AppWorkspace;
            ClientSize=new Size(1421, 763);
            Controls.Add(UnblockButton);
            Controls.Add(BlockButton);
            Controls.Add(SelectedIp);
            Controls.Add(ColourKeyInfo);
            Controls.Add(gmap);
            Controls.Add(panel1);
            Controls.Add(exitBtn);
            Controls.Add(stopCapBtn);
            Controls.Add(resultBox);
            Controls.Add(refreshDevices);
            Controls.Add(captureDevice);
            Controls.Add(capturePacketsBtn);
            Font=new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            FormBorderStyle=FormBorderStyle.None;
            Margin=new Padding(4);
            Name="Form1";
            Text="Network Mapper";
            Load+=Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button capturePacketsBtn;
        private ComboBox captureDevice;
        private Button refreshDevices;
        private RichTextBox resultBox;
        private Button stopCapBtn;
        private Button exitBtn;
        private Panel panel1;
        private Label label1;
        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label ColourKeyInfo;
        private Label SelectedIp;
        private Button BlockButton;
        private Button UnblockButton;
    }
}
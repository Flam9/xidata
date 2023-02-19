
namespace FFXIBatchApp
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label5 = new System.Windows.Forms.Label();
			this.GithubLink = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.FFXIModdingDiscordLink = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.PathFFXI = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.StartNpcExtract = new System.Windows.Forms.Button();
			this.LogBox = new System.Windows.Forms.RichTextBox();
			this.LogTimer = new System.Windows.Forms.Timer(this.components);
			this.StopEverything = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(178, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(222, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "<-- Everything is open source!!";
			// 
			// GithubLink
			// 
			this.GithubLink.AutoSize = true;
			this.GithubLink.Location = new System.Drawing.Point(8, 25);
			this.GithubLink.Name = "GithubLink";
			this.GithubLink.Size = new System.Drawing.Size(158, 20);
			this.GithubLink.TabIndex = 3;
			this.GithubLink.TabStop = true;
			this.GithubLink.Text = "Github: vekien/xidata";
			this.GithubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLink_LinkClicked);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(178, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(402, 20);
			this.label4.TabIndex = 2;
			this.label4.Text = "<-- If you need help, join the discord and message Vekien";
			// 
			// FFXIModdingDiscordLink
			// 
			this.FFXIModdingDiscordLink.AutoSize = true;
			this.FFXIModdingDiscordLink.Location = new System.Drawing.Point(8, 56);
			this.FFXIModdingDiscordLink.Name = "FFXIModdingDiscordLink";
			this.FFXIModdingDiscordLink.Size = new System.Drawing.Size(158, 20);
			this.FFXIModdingDiscordLink.TabIndex = 1;
			this.FFXIModdingDiscordLink.TabStop = true;
			this.FFXIModdingDiscordLink.Text = "FFXI Modding Discord";
			this.FFXIModdingDiscordLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.FFXIModdingDiscordLink_LinkClicked);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(13, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(643, 57);
			this.label3.TabIndex = 0;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(337, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "FINAL FANTASY XI PATH   (click input to browse)";
			// 
			// PathFFXI
			// 
			this.PathFFXI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PathFFXI.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.PathFFXI.Location = new System.Drawing.Point(12, 48);
			this.PathFFXI.Name = "PathFFXI";
			this.PathFFXI.Size = new System.Drawing.Size(568, 25);
			this.PathFFXI.TabIndex = 0;
			this.PathFFXI.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PathFFXI_SelectPath);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label1.Location = new System.Drawing.Point(8, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(426, 50);
			this.label1.TabIndex = 1;
			this.label1.Text = "Will dump all the main story NPCs with their respective animations. This uses you" +
    "r keyboard.";
			// 
			// StartNpcExtract
			// 
			this.StartNpcExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartNpcExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartNpcExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartNpcExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartNpcExtract.Location = new System.Drawing.Point(459, 27);
			this.StartNpcExtract.Name = "StartNpcExtract";
			this.StartNpcExtract.Size = new System.Drawing.Size(167, 39);
			this.StartNpcExtract.TabIndex = 0;
			this.StartNpcExtract.Text = "Start NPC Exporting";
			this.StartNpcExtract.UseVisualStyleBackColor = false;
			this.StartNpcExtract.Click += new System.EventHandler(this.StartNpcExtract_Click);
			// 
			// LogBox
			// 
			this.LogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LogBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LogBox.Font = new System.Drawing.Font("Consolas", 9F);
			this.LogBox.ForeColor = System.Drawing.SystemColors.Menu;
			this.LogBox.Location = new System.Drawing.Point(688, 112);
			this.LogBox.Margin = new System.Windows.Forms.Padding(2);
			this.LogBox.Name = "LogBox";
			this.LogBox.Size = new System.Drawing.Size(644, 725);
			this.LogBox.TabIndex = 2;
			this.LogBox.Text = "";
			// 
			// LogTimer
			// 
			this.LogTimer.Enabled = true;
			this.LogTimer.Interval = 250;
			this.LogTimer.Tick += new System.EventHandler(this.LogTimer_Tick);
			// 
			// StopEverything
			// 
			this.StopEverything.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StopEverything.BackColor = System.Drawing.Color.Salmon;
			this.StopEverything.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StopEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StopEverything.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StopEverything.Location = new System.Drawing.Point(1188, 12);
			this.StopEverything.Name = "StopEverything";
			this.StopEverything.Size = new System.Drawing.Size(144, 72);
			this.StopEverything.TabIndex = 2;
			this.StopEverything.Text = "STOP";
			this.StopEverything.UseVisualStyleBackColor = false;
			this.StopEverything.Click += new System.EventHandler(this.StopEverything_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(-12, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(389, 106);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.panel1.Location = new System.Drawing.Point(12, 112);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10);
			this.panel1.Size = new System.Drawing.Size(669, 725);
			this.panel1.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.GithubLink);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.FFXIModdingDiscordLink);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(13, 78);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(639, 91);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Support";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.PathFFXI);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(13, 175);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox2.Size = new System.Drawing.Size(639, 91);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Settings";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.StartNpcExtract);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(13, 272);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox3.Size = new System.Drawing.Size(639, 82);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Export: NPC";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::FFXIBatchApp.Properties.Resources.bg4;
			this.ClientSize = new System.Drawing.Size(1344, 849);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.StopEverything);
			this.Controls.Add(this.LogBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1000, 640);
			this.Name = "Form1";
			this.Text = "FFXI BATCH EXPORT";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Unload);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.RichTextBox LogBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button StartNpcExtract;
		private System.Windows.Forms.Timer LogTimer;
		private System.Windows.Forms.Button StopEverything;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel FFXIModdingDiscordLink;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel GithubLink;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox PathFFXI;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
	}
}


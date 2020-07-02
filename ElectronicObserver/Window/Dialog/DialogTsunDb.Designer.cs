namespace ElectronicObserver.Window.Dialog
{
	partial class DialogTsunDb
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
            this.buttonEnable = new System.Windows.Forms.Button();
            this.buttonDisable = new System.Windows.Forms.Button();
            this.labelTsunDbText = new System.Windows.Forms.Label();
            this.labelTsunDbText2 = new System.Windows.Forms.Label();
            this.linkLabelTsunDbText = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonEnable
            // 
            this.buttonEnable.Location = new System.Drawing.Point(12, 57);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(131, 23);
            this.buttonEnable.TabIndex = 0;
            this.buttonEnable.Text = "Help the tsuns";
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.buttonEnable_Click);
            // 
            // buttonDisable
            // 
            this.buttonDisable.Location = new System.Drawing.Point(166, 57);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(164, 23);
            this.buttonDisable.TabIndex = 0;
            this.buttonDisable.Text = "Disable data submission";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.buttonDisable_Click);
            // 
            // labelTsunDbText
            // 
            this.labelTsunDbText.AutoSize = true;
            this.labelTsunDbText.Location = new System.Drawing.Point(12, 10);
            this.labelTsunDbText.Name = "labelTsunDbText";
            this.labelTsunDbText.Size = new System.Drawing.Size(285, 15);
            this.labelTsunDbText.TabIndex = 1;
            this.labelTsunDbText.Text = "You can help out by submitting your data to TsunDb ";
            // 
            // labelTsunDbText2
            // 
            this.labelTsunDbText2.AutoSize = true;
            this.labelTsunDbText2.Location = new System.Drawing.Point(12, 30);
            this.labelTsunDbText2.Name = "labelTsunDbText2";
            this.labelTsunDbText2.Size = new System.Drawing.Size(158, 15);
            this.labelTsunDbText2.TabIndex = 1;
            this.labelTsunDbText2.Text = "Check out our privacy policy";
            // 
            // linkLabelTsunDbText
            // 
            this.linkLabelTsunDbText.AutoSize = true;
            this.linkLabelTsunDbText.Location = new System.Drawing.Point(167, 30);
            this.linkLabelTsunDbText.Name = "linkLabelTsunDbText";
            this.linkLabelTsunDbText.Size = new System.Drawing.Size(30, 15);
            this.linkLabelTsunDbText.TabIndex = 2;
            this.linkLabelTsunDbText.TabStop = true;
            this.linkLabelTsunDbText.Text = "here";
            this.linkLabelTsunDbText.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelTsunDbText_LinkClicked);
            // 
            // DialogTsunDb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 89);
            this.Controls.Add(this.linkLabelTsunDbText);
            this.Controls.Add(this.labelTsunDbText2);
            this.Controls.Add(this.labelTsunDbText);
            this.Controls.Add(this.buttonDisable);
            this.Controls.Add(this.buttonEnable);
            this.Name = "DialogTsunDb";
            this.Text = "Enabling TsunDb data submission";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonEnable;
		private System.Windows.Forms.Button buttonDisable;
		private System.Windows.Forms.Label labelTsunDbText;
		private System.Windows.Forms.Label labelTsunDbText2;
		private System.Windows.Forms.LinkLabel linkLabelTsunDbText;
	}
}
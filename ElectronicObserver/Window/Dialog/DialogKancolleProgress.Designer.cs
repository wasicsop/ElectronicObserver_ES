using System.Windows.Forms.Integration;

namespace ElectronicObserver.Window.Dialog
{
	partial class DialogKancolleProgress
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
            this.SuspendLayout();
            // 
            // DialogKancolleProgress
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1370, 792);
            this.DoubleBuffered = true;
            this.Name = "DialogKancolleProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ship Progression List";
            this.Load += new System.EventHandler(this.DialogKancolleProgress_Load);
            this.ResumeLayout(false);

        }

        #endregion
	}
}
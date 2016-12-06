namespace ElectronicObserver.Window.Dialog {
	partial class DialogAntiAirDefense {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogAntiAirDefense));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.AnnihilationProbability = new System.Windows.Forms.TextBox();
			this.AdjustedFleetAA = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.ShowAll = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.AACutinKind = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Formation = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.EnemySlotCount = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.FleetID = new System.Windows.Forms.ComboBox();
			this.ResultView = new System.Windows.Forms.DataGridView();
			this.ResultView_ShipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_AntiAir = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_AdjustedAntiAir = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ProportionalAirDefense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_FixedAirDefense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownBoth = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownProportional = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownFixed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ResultView_ShootDownFailed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.EnemySlotCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultView)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.AnnihilationProbability);
			this.splitContainer1.Panel1.Controls.Add(this.AdjustedFleetAA);
			this.splitContainer1.Panel1.Controls.Add(this.label6);
			this.splitContainer1.Panel1.Controls.Add(this.ShowAll);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.AACutinKind);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.Formation);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.EnemySlotCount);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.FleetID);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ResultView);
			// 
			// AnnihilationProbability
			// 
			resources.ApplyResources(this.AnnihilationProbability, "AnnihilationProbability");
			this.AnnihilationProbability.Name = "AnnihilationProbability";
			this.AnnihilationProbability.ReadOnly = true;
			// 
			// AdjustedFleetAA
			// 
			resources.ApplyResources(this.AdjustedFleetAA, "AdjustedFleetAA");
			this.AdjustedFleetAA.Name = "AdjustedFleetAA";
			this.AdjustedFleetAA.ReadOnly = true;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// ShowAll
			// 
			resources.ApplyResources(this.ShowAll, "ShowAll");
			this.ShowAll.Name = "ShowAll";
			this.ToolTipInfo.SetToolTip(this.ShowAll, resources.GetString("ShowAll.ToolTip"));
			this.ShowAll.UseVisualStyleBackColor = true;
			this.ShowAll.CheckedChanged += new System.EventHandler(this.ShowAll_CheckedChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// AACutinKind
			// 
			this.AACutinKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AACutinKind.FormattingEnabled = true;
			resources.ApplyResources(this.AACutinKind, "AACutinKind");
			this.AACutinKind.Name = "AACutinKind";
			this.AACutinKind.SelectedIndexChanged += new System.EventHandler(this.AACutinKind_SelectedIndexChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// Formation
			// 
			this.Formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Formation.FormattingEnabled = true;
			this.Formation.Items.AddRange(new object[] {
            resources.GetString("Formation.Items"),
            resources.GetString("Formation.Items1"),
            resources.GetString("Formation.Items2")});
			resources.ApplyResources(this.Formation, "Formation");
			this.Formation.Name = "Formation";
			this.Formation.SelectedIndexChanged += new System.EventHandler(this.Formation_SelectedIndexChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// EnemySlotCount
			// 
			resources.ApplyResources(this.EnemySlotCount, "EnemySlotCount");
			this.EnemySlotCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.EnemySlotCount.Name = "EnemySlotCount";
			this.EnemySlotCount.Value = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.EnemySlotCount.ValueChanged += new System.EventHandler(this.EnemySlotCount_ValueChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// FleetID
			// 
			this.FleetID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FleetID.FormattingEnabled = true;
			this.FleetID.Items.AddRange(new object[] {
            resources.GetString("FleetID.Items"),
            resources.GetString("FleetID.Items1"),
            resources.GetString("FleetID.Items2"),
            resources.GetString("FleetID.Items3"),
            resources.GetString("FleetID.Items4")});
			resources.ApplyResources(this.FleetID, "FleetID");
			this.FleetID.Name = "FleetID";
			this.FleetID.SelectedIndexChanged += new System.EventHandler(this.FleetID_SelectedIndexChanged);
			// 
			// ResultView
			// 
			this.ResultView.AllowUserToAddRows = false;
			this.ResultView.AllowUserToDeleteRows = false;
			this.ResultView.AllowUserToResizeColumns = false;
			this.ResultView.AllowUserToResizeRows = false;
			this.ResultView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ResultView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResultView_ShipName,
            this.ResultView_AntiAir,
            this.ResultView_AdjustedAntiAir,
            this.ResultView_ProportionalAirDefense,
            this.ResultView_FixedAirDefense,
            this.ResultView_ShootDownBoth,
            this.ResultView_ShootDownProportional,
            this.ResultView_ShootDownFixed,
            this.ResultView_ShootDownFailed});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ResultView.DefaultCellStyle = dataGridViewCellStyle3;
			resources.ApplyResources(this.ResultView, "ResultView");
			this.ResultView.Name = "ResultView";
			this.ResultView.ReadOnly = true;
			this.ResultView.RowHeadersVisible = false;
			this.ResultView.RowTemplate.Height = 21;
			this.ResultView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ResultView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ResultView_CellFormatting);
			// 
			// ResultView_ShipName
			// 
			this.ResultView_ShipName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.ResultView_ShipName.DefaultCellStyle = dataGridViewCellStyle1;
			resources.ApplyResources(this.ResultView_ShipName, "ResultView_ShipName");
			this.ResultView_ShipName.Name = "ResultView_ShipName";
			this.ResultView_ShipName.ReadOnly = true;
			// 
			// ResultView_AntiAir
			// 
			this.ResultView_AntiAir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_AntiAir, "ResultView_AntiAir");
			this.ResultView_AntiAir.Name = "ResultView_AntiAir";
			this.ResultView_AntiAir.ReadOnly = true;
			// 
			// ResultView_AdjustedAntiAir
			// 
			this.ResultView_AdjustedAntiAir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_AdjustedAntiAir, "ResultView_AdjustedAntiAir");
			this.ResultView_AdjustedAntiAir.Name = "ResultView_AdjustedAntiAir";
			this.ResultView_AdjustedAntiAir.ReadOnly = true;
			// 
			// ResultView_ProportionalAirDefense
			// 
			this.ResultView_ProportionalAirDefense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle2.Format = "p2";
			this.ResultView_ProportionalAirDefense.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.ResultView_ProportionalAirDefense, "ResultView_ProportionalAirDefense");
			this.ResultView_ProportionalAirDefense.Name = "ResultView_ProportionalAirDefense";
			this.ResultView_ProportionalAirDefense.ReadOnly = true;
			// 
			// ResultView_FixedAirDefense
			// 
			this.ResultView_FixedAirDefense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_FixedAirDefense, "ResultView_FixedAirDefense");
			this.ResultView_FixedAirDefense.Name = "ResultView_FixedAirDefense";
			this.ResultView_FixedAirDefense.ReadOnly = true;
			// 
			// ResultView_ShootDownBoth
			// 
			this.ResultView_ShootDownBoth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_ShootDownBoth, "ResultView_ShootDownBoth");
			this.ResultView_ShootDownBoth.Name = "ResultView_ShootDownBoth";
			this.ResultView_ShootDownBoth.ReadOnly = true;
			// 
			// ResultView_ShootDownProportional
			// 
			this.ResultView_ShootDownProportional.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_ShootDownProportional, "ResultView_ShootDownProportional");
			this.ResultView_ShootDownProportional.Name = "ResultView_ShootDownProportional";
			this.ResultView_ShootDownProportional.ReadOnly = true;
			// 
			// ResultView_ShootDownFixed
			// 
			this.ResultView_ShootDownFixed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_ShootDownFixed, "ResultView_ShootDownFixed");
			this.ResultView_ShootDownFixed.Name = "ResultView_ShootDownFixed";
			this.ResultView_ShootDownFixed.ReadOnly = true;
			// 
			// ResultView_ShootDownFailed
			// 
			this.ResultView_ShootDownFailed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ResultView_ShootDownFailed, "ResultView_ShootDownFailed");
			this.ResultView_ShootDownFailed.Name = "ResultView_ShootDownFailed";
			this.ResultView_ShootDownFailed.ReadOnly = true;
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// DialogAntiAirDefense
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitContainer1);
			this.Name = "DialogAntiAirDefense";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogAntiAirDefense_FormClosed);
			this.Load += new System.EventHandler(this.DialogAntiAirDefense_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.EnemySlotCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox AACutinKind;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox Formation;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown EnemySlotCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox FleetID;
		private System.Windows.Forms.DataGridView ResultView;
		private System.Windows.Forms.CheckBox ShowAll;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShipName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_AntiAir;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_AdjustedAntiAir;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ProportionalAirDefense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_FixedAirDefense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownBoth;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownProportional;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownFixed;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResultView_ShootDownFailed;
		private System.Windows.Forms.TextBox AdjustedFleetAA;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox AnnihilationProbability;
	}
}
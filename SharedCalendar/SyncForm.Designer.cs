namespace SharedCalendar
{
  partial class SyncForm
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
      System.Windows.Forms.Label lbCalendar;
      System.Windows.Forms.Label label4;
      System.Windows.Forms.PictureBox bottomBorder;
      System.Windows.Forms.PictureBox topBorder;
      System.Windows.Forms.PictureBox leftBorder;
      System.Windows.Forms.PictureBox rightBorder;
      System.Windows.Forms.PictureBox separator1;
      this.lbSyncRelease = new System.Windows.Forms.Label();
      this.btnSync = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.cbCalendars = new System.Windows.Forms.ComboBox();
      this.closeImg = new System.Windows.Forms.PictureBox();
      lbCalendar = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      bottomBorder = new System.Windows.Forms.PictureBox();
      topBorder = new System.Windows.Forms.PictureBox();
      leftBorder = new System.Windows.Forms.PictureBox();
      rightBorder = new System.Windows.Forms.PictureBox();
      separator1 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.closeImg)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(bottomBorder)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(topBorder)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(leftBorder)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(rightBorder)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(separator1)).BeginInit();
      this.SuspendLayout();
      // 
      // lbSyncRelease
      // 
      this.lbSyncRelease.AutoSize = true;
      this.lbSyncRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
      this.lbSyncRelease.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
      this.lbSyncRelease.Location = new System.Drawing.Point(19, 61);
      this.lbSyncRelease.Name = "lbSyncRelease";
      this.lbSyncRelease.Size = new System.Drawing.Size(230, 18);
      this.lbSyncRelease.TabIndex = 0;
      this.lbSyncRelease.Text = "You are about to sync release: {0}";
      // 
      // lbCalendar
      // 
      lbCalendar.AutoSize = true;
      lbCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
      lbCalendar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
      lbCalendar.Location = new System.Drawing.Point(19, 96);
      lbCalendar.Name = "lbCalendar";
      lbCalendar.Size = new System.Drawing.Size(148, 18);
      lbCalendar.TabIndex = 1;
      lbCalendar.Text = "to Outllook Calendar:";
      // 
      // btnSync
      // 
      this.btnSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(169)))), ((int)(((byte)(130)))));
      this.btnSync.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSync.Enabled = false;
      this.btnSync.FlatAppearance.BorderSize = 0;
      this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
      this.btnSync.ForeColor = System.Drawing.Color.White;
      this.btnSync.Location = new System.Drawing.Point(302, 145);
      this.btnSync.Name = "btnSync";
      this.btnSync.Size = new System.Drawing.Size(75, 30);
      this.btnSync.TabIndex = 2;
      this.btnSync.Text = "Sync";
      this.btnSync.UseVisualStyleBackColor = false;
      // 
      // btnCancel
      // 
      this.btnCancel.BackColor = System.Drawing.Color.White;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
      this.btnCancel.ForeColor = System.Drawing.Color.DimGray;
      this.btnCancel.Location = new System.Drawing.Point(210, 145);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 30);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      // 
      // cbCalendars
      // 
      this.cbCalendars.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
      this.cbCalendars.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
      this.cbCalendars.FormattingEnabled = true;
      this.cbCalendars.Location = new System.Drawing.Point(173, 93);
      this.cbCalendars.Name = "cbCalendars";
      this.cbCalendars.Size = new System.Drawing.Size(204, 26);
      this.cbCalendars.TabIndex = 4;
      this.cbCalendars.SelectedIndexChanged += new System.EventHandler(this.cbCalendars_SelectedIndexChanged);
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
      label4.Location = new System.Drawing.Point(11, 8);
      label4.Margin = new System.Windows.Forms.Padding(0);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(96, 20);
      label4.TabIndex = 20;
      label4.Text = "Synchronize";
      // 
      // closeImg
      // 
      this.closeImg.Image = global::SharedCalendar.Properties.Resources.close;
      this.closeImg.Location = new System.Drawing.Point(379, 8);
      this.closeImg.Name = "closeImg";
      this.closeImg.Size = new System.Drawing.Size(18, 16);
      this.closeImg.TabIndex = 36;
      this.closeImg.TabStop = false;
      this.closeImg.Click += new System.EventHandler(this.closeImg_Click);
      // 
      // bottomBorder
      // 
      bottomBorder.BackColor = System.Drawing.SystemColors.ControlText;
      bottomBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
      bottomBorder.Location = new System.Drawing.Point(1, 199);
      bottomBorder.Name = "bottomBorder";
      bottomBorder.Size = new System.Drawing.Size(398, 1);
      bottomBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      bottomBorder.TabIndex = 35;
      bottomBorder.TabStop = false;
      // 
      // topBorder
      // 
      topBorder.BackColor = System.Drawing.SystemColors.ControlText;
      topBorder.Dock = System.Windows.Forms.DockStyle.Top;
      topBorder.Location = new System.Drawing.Point(1, 0);
      topBorder.Name = "topBorder";
      topBorder.Size = new System.Drawing.Size(398, 1);
      topBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      topBorder.TabIndex = 34;
      topBorder.TabStop = false;
      // 
      // leftBorder
      // 
      leftBorder.BackColor = System.Drawing.SystemColors.ControlText;
      leftBorder.Dock = System.Windows.Forms.DockStyle.Left;
      leftBorder.Location = new System.Drawing.Point(0, 0);
      leftBorder.Margin = new System.Windows.Forms.Padding(0);
      leftBorder.MaximumSize = new System.Drawing.Size(1, 0);
      leftBorder.Name = "leftBorder";
      leftBorder.Size = new System.Drawing.Size(1, 200);
      leftBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      leftBorder.TabIndex = 33;
      leftBorder.TabStop = false;
      // 
      // rightBorder
      // 
      rightBorder.BackColor = System.Drawing.SystemColors.ControlText;
      rightBorder.Dock = System.Windows.Forms.DockStyle.Right;
      rightBorder.Location = new System.Drawing.Point(399, 0);
      rightBorder.Margin = new System.Windows.Forms.Padding(0);
      rightBorder.MaximumSize = new System.Drawing.Size(1, 0);
      rightBorder.Name = "rightBorder";
      rightBorder.Size = new System.Drawing.Size(1, 200);
      rightBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      rightBorder.TabIndex = 32;
      rightBorder.TabStop = false;
      // 
      // separator1
      // 
      separator1.BackColor = System.Drawing.Color.Gainsboro;
      separator1.Location = new System.Drawing.Point(15, 35);
      separator1.Name = "separator1";
      separator1.Size = new System.Drawing.Size(360, 1);
      separator1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      separator1.TabIndex = 28;
      separator1.TabStop = false;
      // 
      // SyncForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(400, 200);
      this.Controls.Add(this.closeImg);
      this.Controls.Add(bottomBorder);
      this.Controls.Add(topBorder);
      this.Controls.Add(leftBorder);
      this.Controls.Add(rightBorder);
      this.Controls.Add(label4);
      this.Controls.Add(separator1);
      this.Controls.Add(this.cbCalendars);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnSync);
      this.Controls.Add(lbCalendar);
      this.Controls.Add(this.lbSyncRelease);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "SyncForm";
      this.Text = "Synchronize";
      ((System.ComponentModel.ISupportInitialize)(this.closeImg)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(bottomBorder)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(topBorder)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(leftBorder)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(rightBorder)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(separator1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lbSyncRelease;
    private System.Windows.Forms.Button btnSync;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ComboBox cbCalendars;
    private System.Windows.Forms.PictureBox closeImg;
  }
}
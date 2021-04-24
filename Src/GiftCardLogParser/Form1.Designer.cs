namespace GiftCardLogParser
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
            this.label1 = new System.Windows.Forms.Label();
            this._logFileTB = new System.Windows.Forms.TextBox();
            this._browseBtn = new System.Windows.Forms.Button();
            this._textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transaction Files:";
            // 
            // _logFileTB
            // 
            this._logFileTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._logFileTB.Location = new System.Drawing.Point(144, 17);
            this._logFileTB.Margin = new System.Windows.Forms.Padding(4);
            this._logFileTB.Name = "_logFileTB";
            this._logFileTB.ReadOnly = true;
            this._logFileTB.Size = new System.Drawing.Size(776, 22);
            this._logFileTB.TabIndex = 1;
            this._logFileTB.TabStop = false;
            // 
            // _browseBtn
            // 
            this._browseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browseBtn.Location = new System.Drawing.Point(929, 15);
            this._browseBtn.Margin = new System.Windows.Forms.Padding(4);
            this._browseBtn.Name = "_browseBtn";
            this._browseBtn.Size = new System.Drawing.Size(100, 28);
            this._browseBtn.TabIndex = 2;
            this._browseBtn.Text = "Select...";
            this._browseBtn.UseVisualStyleBackColor = true;
            this._browseBtn.Click += new System.EventHandler(this._browseBtn_Click);
            // 
            // _textBox
            // 
            this._textBox.AcceptsReturn = true;
            this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox.Location = new System.Drawing.Point(20, 49);
            this._textBox.Margin = new System.Windows.Forms.Padding(4);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.ReadOnly = true;
            this._textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox.Size = new System.Drawing.Size(1008, 627);
            this._textBox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 692);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._browseBtn);
            this.Controls.Add(this._logFileTB);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Gift Card Log Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _logFileTB;
		private System.Windows.Forms.Button _browseBtn;
		private System.Windows.Forms.TextBox _textBox;
	}
}


namespace PDF_Interpreter
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
            this.cmdGO = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.chkDoImageFiles = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdGO
            // 
            this.cmdGO.Location = new System.Drawing.Point(461, 12);
            this.cmdGO.Name = "cmdGO";
            this.cmdGO.Size = new System.Drawing.Size(34, 23);
            this.cmdGO.TabIndex = 2;
            this.cmdGO.Text = "Go";
            this.cmdGO.UseVisualStyleBackColor = true;
            this.cmdGO.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(25, 15);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(418, 20);
            this.txtFile.TabIndex = 3;
            this.txtFile.Text = "C:\\Temp\\13837922_with_OCR.pdf";
            // 
            // chkDoImageFiles
            // 
            this.chkDoImageFiles.AutoSize = true;
            this.chkDoImageFiles.Checked = true;
            this.chkDoImageFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoImageFiles.Location = new System.Drawing.Point(25, 41);
            this.chkDoImageFiles.Name = "chkDoImageFiles";
            this.chkDoImageFiles.Size = new System.Drawing.Size(190, 17);
            this.chkDoImageFiles.TabIndex = 4;
            this.chkDoImageFiles.Text = "Create Image files with GhostScript";
            this.chkDoImageFiles.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 66);
            this.Controls.Add(this.chkDoImageFiles);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.cmdGO);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cmdGO;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.CheckBox chkDoImageFiles;
    }
}


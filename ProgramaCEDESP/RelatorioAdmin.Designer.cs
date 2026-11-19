namespace ProgramaCEDESP
{
    partial class RelatorioAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelatorioAdmin));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imgRelatorio = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.imgRelatorio);
            this.groupBox1.Location = new System.Drawing.Point(22, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 188);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // imgRelatorio
            // 
            this.imgRelatorio.BackColor = System.Drawing.Color.Transparent;
            this.imgRelatorio.ForeColor = System.Drawing.Color.Transparent;
            this.imgRelatorio.Image = ((System.Drawing.Image)(resources.GetObject("imgRelatorio.Image")));
            this.imgRelatorio.Location = new System.Drawing.Point(18, 19);
            this.imgRelatorio.Name = "imgRelatorio";
            this.imgRelatorio.Size = new System.Drawing.Size(130, 150);
            this.imgRelatorio.TabIndex = 0;
            this.imgRelatorio.UseVisualStyleBackColor = false;
            this.imgRelatorio.Click += new System.EventHandler(this.imgRelatorio_Click);
            // 
            // RelatorioAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(214, 243);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RelatorioAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório final";
            this.Load += new System.EventHandler(this.RelatorioAdmin_Load);
            this.Click += new System.EventHandler(this.imgRelatorio_Click);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button imgRelatorio;
    }
}
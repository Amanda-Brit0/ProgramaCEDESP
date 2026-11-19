namespace ProgramaCEDESP
{
    partial class FreqAluno
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreqAluno));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.totalAulas = new System.Windows.Forms.Label();
            this.lblPorcentagemFrequencia = new System.Windows.Forms.Label();
            this.dataGridViewFrequencia = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFrequencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.totalAulas);
            this.groupBox1.Controls.Add(this.lblPorcentagemFrequencia);
            this.groupBox1.Controls.Add(this.dataGridViewFrequencia);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 449);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // totalAulas
            // 
            this.totalAulas.AutoSize = true;
            this.totalAulas.BackColor = System.Drawing.Color.Transparent;
            this.totalAulas.Location = new System.Drawing.Point(270, 136);
            this.totalAulas.Name = "totalAulas";
            this.totalAulas.Size = new System.Drawing.Size(35, 13);
            this.totalAulas.TabIndex = 4;
            this.totalAulas.Text = "label2";
            // 
            // lblPorcentagemFrequencia
            // 
            this.lblPorcentagemFrequencia.AutoSize = true;
            this.lblPorcentagemFrequencia.BackColor = System.Drawing.Color.Transparent;
            this.lblPorcentagemFrequencia.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPorcentagemFrequencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPorcentagemFrequencia.Location = new System.Drawing.Point(272, 190);
            this.lblPorcentagemFrequencia.Name = "lblPorcentagemFrequencia";
            this.lblPorcentagemFrequencia.Size = new System.Drawing.Size(54, 19);
            this.lblPorcentagemFrequencia.TabIndex = 3;
            this.lblPorcentagemFrequencia.Text = "label2";
            this.lblPorcentagemFrequencia.Click += new System.EventHandler(this.lblPorcentagemFrequencia_Click);
            // 
            // dataGridViewFrequencia
            // 
            this.dataGridViewFrequencia.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridViewFrequencia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFrequencia.Location = new System.Drawing.Point(24, 116);
            this.dataGridViewFrequencia.Name = "dataGridViewFrequencia";
            this.dataGridViewFrequencia.Size = new System.Drawing.Size(233, 307);
            this.dataGridViewFrequencia.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(244, 242);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(165, 181);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(119, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Faltas/Frequência";
            // 
            // FreqAluno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(461, 479);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FreqAluno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frequência";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFrequencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridViewFrequencia;
        private System.Windows.Forms.Label lblPorcentagemFrequencia;
        private System.Windows.Forms.Label totalAulas;
    }
}
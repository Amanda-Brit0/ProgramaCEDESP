namespace ProgramaCEDESP
{
    partial class TelaPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelaPrincipal));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MinhaPasta = new System.Windows.Forms.Label();
            this.material = new System.Windows.Forms.Label();
            this.btnLive2 = new System.Windows.Forms.PictureBox();
            this.btnSair = new System.Windows.Forms.Button();
            this.notas = new System.Windows.Forms.Label();
            this.frequencia = new System.Windows.Forms.Label();
            this.imgMinhaPasta = new System.Windows.Forms.PictureBox();
            this.imgMaterial = new System.Windows.Forms.PictureBox();
            this.imgNota = new System.Windows.Forms.PictureBox();
            this.imgFreq = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLive2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMinhaPasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgNota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.MinhaPasta);
            this.groupBox1.Controls.Add(this.material);
            this.groupBox1.Controls.Add(this.btnLive2);
            this.groupBox1.Controls.Add(this.btnSair);
            this.groupBox1.Controls.Add(this.notas);
            this.groupBox1.Controls.Add(this.frequencia);
            this.groupBox1.Controls.Add(this.imgMinhaPasta);
            this.groupBox1.Controls.Add(this.imgMaterial);
            this.groupBox1.Controls.Add(this.imgNota);
            this.groupBox1.Controls.Add(this.imgFreq);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 370);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // MinhaPasta
            // 
            this.MinhaPasta.AutoSize = true;
            this.MinhaPasta.BackColor = System.Drawing.Color.Transparent;
            this.MinhaPasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinhaPasta.Location = new System.Drawing.Point(67, 269);
            this.MinhaPasta.Name = "MinhaPasta";
            this.MinhaPasta.Size = new System.Drawing.Size(87, 15);
            this.MinhaPasta.TabIndex = 9;
            this.MinhaPasta.Text = "Minha Pasta";
            // 
            // material
            // 
            this.material.AutoSize = true;
            this.material.BackColor = System.Drawing.Color.Transparent;
            this.material.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.material.Location = new System.Drawing.Point(300, 159);
            this.material.Name = "material";
            this.material.Size = new System.Drawing.Size(121, 15);
            this.material.TabIndex = 8;
            this.material.Text = "Material do Curso";
            this.material.Click += new System.EventHandler(this.material_Click);
            // 
            // btnLive2
            // 
            this.btnLive2.BackColor = System.Drawing.Color.Transparent;
            this.btnLive2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLive2.BackgroundImage")));
            this.btnLive2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLive2.Location = new System.Drawing.Point(238, 185);
            this.btnLive2.Name = "btnLive2";
            this.btnLive2.Size = new System.Drawing.Size(129, 99);
            this.btnLive2.TabIndex = 11;
            this.btnLive2.TabStop = false;
            this.btnLive2.Click += new System.EventHandler(this.btnLive2_Click);
            this.btnLive2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.btnLive2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnSair.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSair.Location = new System.Drawing.Point(385, 338);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(55, 31);
            this.btnSair.TabIndex = 10;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            this.btnSair.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.btnSair.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // notas
            // 
            this.notas.AutoSize = true;
            this.notas.BackColor = System.Drawing.Color.Transparent;
            this.notas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notas.Location = new System.Drawing.Point(210, 155);
            this.notas.Name = "notas";
            this.notas.Size = new System.Drawing.Size(44, 15);
            this.notas.TabIndex = 7;
            this.notas.Text = "Notas";
            // 
            // frequencia
            // 
            this.frequencia.AutoSize = true;
            this.frequencia.BackColor = System.Drawing.Color.Transparent;
            this.frequencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencia.Location = new System.Drawing.Point(52, 157);
            this.frequencia.Name = "frequencia";
            this.frequencia.Size = new System.Drawing.Size(122, 15);
            this.frequencia.TabIndex = 6;
            this.frequencia.Text = "Faltas/Frequência";
            // 
            // imgMinhaPasta
            // 
            this.imgMinhaPasta.BackColor = System.Drawing.Color.Transparent;
            this.imgMinhaPasta.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgMinhaPasta.BackgroundImage")));
            this.imgMinhaPasta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgMinhaPasta.Location = new System.Drawing.Point(78, 200);
            this.imgMinhaPasta.Name = "imgMinhaPasta";
            this.imgMinhaPasta.Size = new System.Drawing.Size(67, 71);
            this.imgMinhaPasta.TabIndex = 4;
            this.imgMinhaPasta.TabStop = false;
            this.imgMinhaPasta.Click += new System.EventHandler(this.imgMinhaPasta_Click);
            this.imgMinhaPasta.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.imgMinhaPasta.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // imgMaterial
            // 
            this.imgMaterial.BackColor = System.Drawing.Color.Transparent;
            this.imgMaterial.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgMaterial.BackgroundImage")));
            this.imgMaterial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgMaterial.Location = new System.Drawing.Point(329, 90);
            this.imgMaterial.Name = "imgMaterial";
            this.imgMaterial.Size = new System.Drawing.Size(64, 66);
            this.imgMaterial.TabIndex = 3;
            this.imgMaterial.TabStop = false;
            this.imgMaterial.Click += new System.EventHandler(this.imgMaterial_Click);
            this.imgMaterial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.imgMaterial.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // imgNota
            // 
            this.imgNota.BackColor = System.Drawing.Color.Transparent;
            this.imgNota.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgNota.BackgroundImage")));
            this.imgNota.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgNota.Location = new System.Drawing.Point(205, 90);
            this.imgNota.Name = "imgNota";
            this.imgNota.Size = new System.Drawing.Size(65, 64);
            this.imgNota.TabIndex = 2;
            this.imgNota.TabStop = false;
            this.imgNota.Click += new System.EventHandler(this.imgNota_Click);
            this.imgNota.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.imgNota.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // imgFreq
            // 
            this.imgFreq.BackColor = System.Drawing.Color.Transparent;
            this.imgFreq.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgFreq.BackgroundImage")));
            this.imgFreq.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgFreq.Location = new System.Drawing.Point(81, 90);
            this.imgFreq.Name = "imgFreq";
            this.imgFreq.Size = new System.Drawing.Size(64, 64);
            this.imgFreq.TabIndex = 1;
            this.imgFreq.TabStop = false;
            this.imgFreq.Click += new System.EventHandler(this.imgFreq_Click);
            this.imgFreq.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseDown);
            this.imgFreq.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Imagens_MouseUp);
            // 
            // TelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(466, 395);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TelaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CEDESP - Página Principal";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLive2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMinhaPasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgNota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFreq)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox imgMinhaPasta;
        private System.Windows.Forms.PictureBox imgMaterial;
        private System.Windows.Forms.PictureBox imgNota;
        private System.Windows.Forms.PictureBox imgFreq;
        private System.Windows.Forms.Label notas;
        private System.Windows.Forms.Label frequencia;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Label MinhaPasta;
        private System.Windows.Forms.Label material;
        private System.Windows.Forms.PictureBox btnLive2;
    }
}
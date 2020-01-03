using Intech.EfdReinf.Transmissor.Controles;

namespace Intech.EfdReinf.Transmissor
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ButtonInicio = new System.Windows.Forms.Button();
            this.ButtonTransmitir = new System.Windows.Forms.Button();
            this.ButtonConsultar = new System.Windows.Forms.Button();
            this.ButtonUtilitarios = new System.Windows.Forms.Button();
            this.LabelVersao = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(660, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(131, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(657, 280);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // ButtonInicio
            // 
            this.ButtonInicio.Location = new System.Drawing.Point(13, 82);
            this.ButtonInicio.Name = "ButtonInicio";
            this.ButtonInicio.Size = new System.Drawing.Size(108, 23);
            this.ButtonInicio.TabIndex = 3;
            this.ButtonInicio.Text = "Início";
            this.ButtonInicio.UseVisualStyleBackColor = true;
            this.ButtonInicio.Click += new System.EventHandler(this.ButtonInicio_Click);
            // 
            // ButtonTransmitir
            // 
            this.ButtonTransmitir.Location = new System.Drawing.Point(13, 112);
            this.ButtonTransmitir.Name = "ButtonTransmitir";
            this.ButtonTransmitir.Size = new System.Drawing.Size(108, 23);
            this.ButtonTransmitir.TabIndex = 4;
            this.ButtonTransmitir.Text = "Transmitir";
            this.ButtonTransmitir.UseVisualStyleBackColor = true;
            this.ButtonTransmitir.Click += new System.EventHandler(this.ButtonTransmitir_Click);
            // 
            // ButtonConsultar
            // 
            this.ButtonConsultar.Location = new System.Drawing.Point(13, 142);
            this.ButtonConsultar.Name = "ButtonConsultar";
            this.ButtonConsultar.Size = new System.Drawing.Size(108, 23);
            this.ButtonConsultar.TabIndex = 5;
            this.ButtonConsultar.Text = "Consultar";
            this.ButtonConsultar.UseVisualStyleBackColor = true;
            this.ButtonConsultar.Click += new System.EventHandler(this.ButtonConsultar_Click);
            // 
            // ButtonUtilitarios
            // 
            this.ButtonUtilitarios.Location = new System.Drawing.Point(13, 172);
            this.ButtonUtilitarios.Name = "ButtonUtilitarios";
            this.ButtonUtilitarios.Size = new System.Drawing.Size(108, 23);
            this.ButtonUtilitarios.TabIndex = 6;
            this.ButtonUtilitarios.Text = "Utilitários";
            this.ButtonUtilitarios.UseVisualStyleBackColor = true;
            this.ButtonUtilitarios.Click += new System.EventHandler(this.ButtonUtilitarios_Click);
            // 
            // LabelVersao
            // 
            this.LabelVersao.AutoSize = true;
            this.LabelVersao.Location = new System.Drawing.Point(13, 333);
            this.LabelVersao.Name = "LabelVersao";
            this.LabelVersao.Size = new System.Drawing.Size(58, 13);
            this.LabelVersao.TabIndex = 7;
            this.LabelVersao.Text = "Versão 0.0";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 362);
            this.Controls.Add(this.LabelVersao);
            this.Controls.Add(this.ButtonUtilitarios);
            this.Controls.Add(this.ButtonConsultar);
            this.Controls.Add(this.ButtonTransmitir);
            this.Controls.Add(this.ButtonInicio);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intech Transmissor EFD-Reinf";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonInicio;
        private System.Windows.Forms.Button ButtonTransmitir;
        private System.Windows.Forms.Button ButtonConsultar;
        private System.Windows.Forms.Button ButtonUtilitarios;
        private System.Windows.Forms.Label LabelVersao;
    }
}


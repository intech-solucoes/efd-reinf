namespace Intech.EfdReinf.Transmissor.Controles
{
    partial class Transmitir
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonProcurar = new System.Windows.Forms.Button();
            this.TextBoxArquivo = new System.Windows.Forms.TextBox();
            this.ButtonTransmitir = new System.Windows.Forms.Button();
            this.ProgressBarPrimaria = new System.Windows.Forms.ProgressBar();
            this.ProgressBarSecundaria = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonSalvarLog = new System.Windows.Forms.Button();
            this.TextBoxLog = new System.Windows.Forms.TextBox();
            this.LabelProgressBarPrimaria = new System.Windows.Forms.Label();
            this.LabelProgressBarSecundaria = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ButtonProcurar);
            this.groupBox1.Controls.Add(this.TextBoxArquivo);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selecione o arquivo";
            // 
            // ButtonProcurar
            // 
            this.ButtonProcurar.Location = new System.Drawing.Point(554, 19);
            this.ButtonProcurar.Name = "ButtonProcurar";
            this.ButtonProcurar.Size = new System.Drawing.Size(90, 20);
            this.ButtonProcurar.TabIndex = 1;
            this.ButtonProcurar.Text = "Procurar";
            this.ButtonProcurar.UseVisualStyleBackColor = true;
            this.ButtonProcurar.Click += new System.EventHandler(this.ButtonProcurar_Click);
            // 
            // TextBoxArquivo
            // 
            this.TextBoxArquivo.Location = new System.Drawing.Point(6, 19);
            this.TextBoxArquivo.Name = "TextBoxArquivo";
            this.TextBoxArquivo.Size = new System.Drawing.Size(542, 20);
            this.TextBoxArquivo.TabIndex = 0;
            // 
            // ButtonTransmitir
            // 
            this.ButtonTransmitir.Location = new System.Drawing.Point(291, 56);
            this.ButtonTransmitir.Name = "ButtonTransmitir";
            this.ButtonTransmitir.Size = new System.Drawing.Size(75, 23);
            this.ButtonTransmitir.TabIndex = 1;
            this.ButtonTransmitir.Text = "Transmitir";
            this.ButtonTransmitir.UseVisualStyleBackColor = true;
            this.ButtonTransmitir.Click += new System.EventHandler(this.ButtonTransmitir_Click);
            // 
            // ProgressBarPrimaria
            // 
            this.ProgressBarPrimaria.Location = new System.Drawing.Point(10, 85);
            this.ProgressBarPrimaria.Name = "ProgressBarPrimaria";
            this.ProgressBarPrimaria.Size = new System.Drawing.Size(638, 14);
            this.ProgressBarPrimaria.TabIndex = 2;
            // 
            // ProgressBarSecundaria
            // 
            this.ProgressBarSecundaria.Location = new System.Drawing.Point(10, 128);
            this.ProgressBarSecundaria.Name = "ProgressBarSecundaria";
            this.ProgressBarSecundaria.Size = new System.Drawing.Size(638, 14);
            this.ProgressBarSecundaria.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TextBoxLog);
            this.groupBox2.Location = new System.Drawing.Point(10, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(511, 104);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // ButtonSalvarLog
            // 
            this.ButtonSalvarLog.Location = new System.Drawing.Point(527, 244);
            this.ButtonSalvarLog.Name = "ButtonSalvarLog";
            this.ButtonSalvarLog.Size = new System.Drawing.Size(121, 23);
            this.ButtonSalvarLog.TabIndex = 5;
            this.ButtonSalvarLog.Text = "Salvar Log";
            this.ButtonSalvarLog.UseVisualStyleBackColor = true;
            // 
            // TextBoxLog
            // 
            this.TextBoxLog.Location = new System.Drawing.Point(7, 19);
            this.TextBoxLog.Multiline = true;
            this.TextBoxLog.Name = "TextBoxLog";
            this.TextBoxLog.Size = new System.Drawing.Size(498, 79);
            this.TextBoxLog.TabIndex = 0;
            // 
            // LabelProgressBarPrimaria
            // 
            this.LabelProgressBarPrimaria.AutoSize = true;
            this.LabelProgressBarPrimaria.Location = new System.Drawing.Point(7, 102);
            this.LabelProgressBarPrimaria.Name = "LabelProgressBarPrimaria";
            this.LabelProgressBarPrimaria.Size = new System.Drawing.Size(35, 13);
            this.LabelProgressBarPrimaria.TabIndex = 6;
            this.LabelProgressBarPrimaria.Text = "label1";
            // 
            // LabelProgressBarSecundaria
            // 
            this.LabelProgressBarSecundaria.AutoSize = true;
            this.LabelProgressBarSecundaria.Location = new System.Drawing.Point(10, 145);
            this.LabelProgressBarSecundaria.Name = "LabelProgressBarSecundaria";
            this.LabelProgressBarSecundaria.Size = new System.Drawing.Size(35, 13);
            this.LabelProgressBarSecundaria.TabIndex = 7;
            this.LabelProgressBarSecundaria.Text = "label1";
            // 
            // Transmissor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelProgressBarSecundaria);
            this.Controls.Add(this.LabelProgressBarPrimaria);
            this.Controls.Add(this.ButtonSalvarLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ProgressBarSecundaria);
            this.Controls.Add(this.ProgressBarPrimaria);
            this.Controls.Add(this.ButtonTransmitir);
            this.Controls.Add(this.groupBox1);
            this.Name = "Transmissor";
            this.Size = new System.Drawing.Size(657, 280);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonProcurar;
        private System.Windows.Forms.TextBox TextBoxArquivo;
        private System.Windows.Forms.Button ButtonTransmitir;
        private System.Windows.Forms.ProgressBar ProgressBarPrimaria;
        private System.Windows.Forms.ProgressBar ProgressBarSecundaria;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ButtonSalvarLog;
        private System.Windows.Forms.TextBox TextBoxLog;
        private System.Windows.Forms.Label LabelProgressBarPrimaria;
        private System.Windows.Forms.Label LabelProgressBarSecundaria;
    }
}

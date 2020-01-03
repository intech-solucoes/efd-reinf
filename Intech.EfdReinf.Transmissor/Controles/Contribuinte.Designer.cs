namespace Intech.EfdReinf.Transmissor.Controles
{
    partial class Contribuinte
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
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBoxContribuinte = new System.Windows.Forms.ComboBox();
            this.ButtonTrocar = new System.Windows.Forms.Button();
            this.ButtonSair = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelUsuario = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contribuinte:";
            // 
            // ComboBoxContribuinte
            // 
            this.ComboBoxContribuinte.FormattingEnabled = true;
            this.ComboBoxContribuinte.Location = new System.Drawing.Point(8, 61);
            this.ComboBoxContribuinte.Name = "ComboBoxContribuinte";
            this.ComboBoxContribuinte.Size = new System.Drawing.Size(327, 21);
            this.ComboBoxContribuinte.TabIndex = 1;
            // 
            // ButtonTrocar
            // 
            this.ButtonTrocar.Location = new System.Drawing.Point(341, 61);
            this.ButtonTrocar.Name = "ButtonTrocar";
            this.ButtonTrocar.Size = new System.Drawing.Size(75, 21);
            this.ButtonTrocar.TabIndex = 2;
            this.ButtonTrocar.Text = "Trocar";
            this.ButtonTrocar.UseVisualStyleBackColor = true;
            this.ButtonTrocar.Click += new System.EventHandler(this.ButtonTrocar_Click);
            // 
            // ButtonSair
            // 
            this.ButtonSair.Location = new System.Drawing.Point(499, 61);
            this.ButtonSair.Name = "ButtonSair";
            this.ButtonSair.Size = new System.Drawing.Size(135, 21);
            this.ButtonSair.TabIndex = 3;
            this.ButtonSair.Text = "Sair da Conta";
            this.ButtonSair.UseVisualStyleBackColor = true;
            this.ButtonSair.Click += new System.EventHandler(this.ButtonSair_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Logado como:";
            // 
            // LabelUsuario
            // 
            this.LabelUsuario.AutoSize = true;
            this.LabelUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelUsuario.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.LabelUsuario.Location = new System.Drawing.Point(109, 8);
            this.LabelUsuario.Name = "LabelUsuario";
            this.LabelUsuario.Size = new System.Drawing.Size(92, 17);
            this.LabelUsuario.TabIndex = 5;
            this.LabelUsuario.Text = "LabelUsuario";
            // 
            // Contribuinte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelUsuario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ButtonSair);
            this.Controls.Add(this.ButtonTrocar);
            this.Controls.Add(this.ComboBoxContribuinte);
            this.Controls.Add(this.label1);
            this.Name = "Contribuinte";
            this.Size = new System.Drawing.Size(637, 87);
            this.Load += new System.EventHandler(this.Contribuinte_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBoxContribuinte;
        private System.Windows.Forms.Button ButtonTrocar;
        private System.Windows.Forms.Button ButtonSair;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabelUsuario;
    }
}

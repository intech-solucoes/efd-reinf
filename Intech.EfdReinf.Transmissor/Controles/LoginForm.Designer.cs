namespace Intech.EfdReinf.Transmissor.Controles
{
    partial class LoginForm
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
            this.TextBoxEmail = new System.Windows.Forms.TextBox();
            this.TextBoxSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonEntrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "E-mail:";
            // 
            // TextBoxEmail
            // 
            this.TextBoxEmail.Location = new System.Drawing.Point(8, 24);
            this.TextBoxEmail.Name = "TextBoxEmail";
            this.TextBoxEmail.Size = new System.Drawing.Size(239, 20);
            this.TextBoxEmail.TabIndex = 1;
            // 
            // TextBoxSenha
            // 
            this.TextBoxSenha.Location = new System.Drawing.Point(259, 24);
            this.TextBoxSenha.Name = "TextBoxSenha";
            this.TextBoxSenha.PasswordChar = '•';
            this.TextBoxSenha.Size = new System.Drawing.Size(239, 20);
            this.TextBoxSenha.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Senha:";
            // 
            // ButtonEntrar
            // 
            this.ButtonEntrar.Location = new System.Drawing.Point(8, 53);
            this.ButtonEntrar.Name = "ButtonEntrar";
            this.ButtonEntrar.Size = new System.Drawing.Size(75, 23);
            this.ButtonEntrar.TabIndex = 4;
            this.ButtonEntrar.Text = "Entrar";
            this.ButtonEntrar.UseVisualStyleBackColor = true;
            this.ButtonEntrar.Click += new System.EventHandler(this.ButtonEntrar_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonEntrar);
            this.Controls.Add(this.TextBoxSenha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxEmail);
            this.Controls.Add(this.label1);
            this.Name = "LoginForm";
            this.Size = new System.Drawing.Size(637, 87);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxEmail;
        private System.Windows.Forms.TextBox TextBoxSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonEntrar;
    }
}

namespace Intech.EfdReinf.Transmissor.Controles
{
    partial class Consultar
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
            this.ButtonConsultar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckBoxAlinharRecibos = new System.Windows.Forms.CheckBox();
            this.LabelSubPrestador = new System.Windows.Forms.Label();
            this.ComboBoxCNPJPrestador = new System.Windows.Forms.ComboBox();
            this.LabelPrestador = new System.Windows.Forms.Label();
            this.LabelSubPeriodo = new System.Windows.Forms.Label();
            this.ComboBoxPeriodo = new System.Windows.Forms.ComboBox();
            this.ComboBoxTipoConsulta = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LabelPeriodo = new System.Windows.Forms.Label();
            this.LabelSubCNPJ = new System.Windows.Forms.Label();
            this.TextBoxCNPJ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonConsultar
            // 
            this.ButtonConsultar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonConsultar.Location = new System.Drawing.Point(15, 239);
            this.ButtonConsultar.Name = "ButtonConsultar";
            this.ButtonConsultar.Size = new System.Drawing.Size(75, 23);
            this.ButtonConsultar.TabIndex = 0;
            this.ButtonConsultar.Text = "Consultar";
            this.ButtonConsultar.UseVisualStyleBackColor = true;
            this.ButtonConsultar.Click += new System.EventHandler(this.ButtonConsultar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CheckBoxAlinharRecibos);
            this.groupBox1.Controls.Add(this.LabelSubPrestador);
            this.groupBox1.Controls.Add(this.ComboBoxCNPJPrestador);
            this.groupBox1.Controls.Add(this.LabelPrestador);
            this.groupBox1.Controls.Add(this.LabelSubPeriodo);
            this.groupBox1.Controls.Add(this.ComboBoxPeriodo);
            this.groupBox1.Controls.Add(this.ComboBoxTipoConsulta);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.LabelPeriodo);
            this.groupBox1.Controls.Add(this.LabelSubCNPJ);
            this.groupBox1.Controls.Add(this.TextBoxCNPJ);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ButtonConsultar);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(649, 274);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parâmetros para Consulta";
            // 
            // CheckBoxAlinharRecibos
            // 
            this.CheckBoxAlinharRecibos.AutoSize = true;
            this.CheckBoxAlinharRecibos.Location = new System.Drawing.Point(15, 208);
            this.CheckBoxAlinharRecibos.Name = "CheckBoxAlinharRecibos";
            this.CheckBoxAlinharRecibos.Size = new System.Drawing.Size(100, 17);
            this.CheckBoxAlinharRecibos.TabIndex = 12;
            this.CheckBoxAlinharRecibos.Text = "Alinhar Recibos";
            this.CheckBoxAlinharRecibos.UseVisualStyleBackColor = true;
            // 
            // LabelSubPrestador
            // 
            this.LabelSubPrestador.AutoSize = true;
            this.LabelSubPrestador.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LabelSubPrestador.Location = new System.Drawing.Point(95, 161);
            this.LabelSubPrestador.Name = "LabelSubPrestador";
            this.LabelSubPrestador.Size = new System.Drawing.Size(91, 13);
            this.LabelSubPrestador.TabIndex = 11;
            this.LabelSubPrestador.Text = "(apenas números)";
            // 
            // ComboBoxCNPJPrestador
            // 
            this.ComboBoxCNPJPrestador.FormattingEnabled = true;
            this.ComboBoxCNPJPrestador.Location = new System.Drawing.Point(15, 177);
            this.ComboBoxCNPJPrestador.Name = "ComboBoxCNPJPrestador";
            this.ComboBoxCNPJPrestador.Size = new System.Drawing.Size(185, 21);
            this.ComboBoxCNPJPrestador.TabIndex = 10;
            // 
            // LabelPrestador
            // 
            this.LabelPrestador.AutoSize = true;
            this.LabelPrestador.Location = new System.Drawing.Point(12, 161);
            this.LabelPrestador.Name = "LabelPrestador";
            this.LabelPrestador.Size = new System.Drawing.Size(85, 13);
            this.LabelPrestador.TabIndex = 9;
            this.LabelPrestador.Text = "CNPJ Prestador:";
            // 
            // LabelSubPeriodo
            // 
            this.LabelSubPeriodo.AutoSize = true;
            this.LabelSubPeriodo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LabelSubPeriodo.Location = new System.Drawing.Point(56, 117);
            this.LabelSubPeriodo.Name = "LabelSubPeriodo";
            this.LabelSubPeriodo.Size = new System.Drawing.Size(94, 13);
            this.LabelSubPeriodo.TabIndex = 8;
            this.LabelSubPeriodo.Text = "(formato MM/yyyy)";
            // 
            // ComboBoxPeriodo
            // 
            this.ComboBoxPeriodo.FormattingEnabled = true;
            this.ComboBoxPeriodo.Location = new System.Drawing.Point(15, 133);
            this.ComboBoxPeriodo.Name = "ComboBoxPeriodo";
            this.ComboBoxPeriodo.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxPeriodo.TabIndex = 7;
            // 
            // ComboBoxTipoConsulta
            // 
            this.ComboBoxTipoConsulta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxTipoConsulta.FormattingEnabled = true;
            this.ComboBoxTipoConsulta.Items.AddRange(new object[] {
            "R-1000",
            "R-1070",
            "R-2010",
            "R-2098",
            "R-2099"});
            this.ComboBoxTipoConsulta.Location = new System.Drawing.Point(15, 87);
            this.ComboBoxTipoConsulta.Name = "ComboBoxTipoConsulta";
            this.ComboBoxTipoConsulta.Size = new System.Drawing.Size(117, 21);
            this.ComboBoxTipoConsulta.TabIndex = 6;
            this.ComboBoxTipoConsulta.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTipoConsulta_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tipo de Consulta:";
            // 
            // LabelPeriodo
            // 
            this.LabelPeriodo.AutoSize = true;
            this.LabelPeriodo.Location = new System.Drawing.Point(12, 117);
            this.LabelPeriodo.Name = "LabelPeriodo";
            this.LabelPeriodo.Size = new System.Drawing.Size(48, 13);
            this.LabelPeriodo.TabIndex = 4;
            this.LabelPeriodo.Text = "Período:";
            // 
            // LabelSubCNPJ
            // 
            this.LabelSubCNPJ.AutoSize = true;
            this.LabelSubCNPJ.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LabelSubCNPJ.Location = new System.Drawing.Point(109, 27);
            this.LabelSubCNPJ.Name = "LabelSubCNPJ";
            this.LabelSubCNPJ.Size = new System.Drawing.Size(91, 13);
            this.LabelSubCNPJ.TabIndex = 3;
            this.LabelSubCNPJ.Text = "(apenas números)";
            // 
            // TextBoxCNPJ
            // 
            this.TextBoxCNPJ.Location = new System.Drawing.Point(15, 43);
            this.TextBoxCNPJ.Name = "TextBoxCNPJ";
            this.TextBoxCNPJ.Size = new System.Drawing.Size(185, 20);
            this.TextBoxCNPJ.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "CNPJ Contribuinte:";
            // 
            // Consultar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "Consultar";
            this.Size = new System.Drawing.Size(657, 280);
            this.Load += new System.EventHandler(this.Consultar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonConsultar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxCNPJ;
        private System.Windows.Forms.Label LabelSubCNPJ;
        private System.Windows.Forms.ComboBox ComboBoxTipoConsulta;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LabelPeriodo;
        private System.Windows.Forms.ComboBox ComboBoxPeriodo;
        private System.Windows.Forms.Label LabelSubPeriodo;
        private System.Windows.Forms.Label LabelSubPrestador;
        private System.Windows.Forms.ComboBox ComboBoxCNPJPrestador;
        private System.Windows.Forms.Label LabelPrestador;
        private System.Windows.Forms.CheckBox CheckBoxAlinharRecibos;
    }
}

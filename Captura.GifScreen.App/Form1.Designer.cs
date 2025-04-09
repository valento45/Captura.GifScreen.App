namespace Captura.GifScreen.App
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            panel1 = new Panel();
            chkIniciarComWindows = new CheckBox();
            btSalvar = new Button();
            groupBox2 = new GroupBox();
            label3 = new Label();
            txtAtalhoGravacao = new TextBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            label2 = new Label();
            txtAtalhoCaptura = new TextBox();
            label1 = new Label();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Gif Screen App";
            notifyIcon1.Visible = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(chkIniciarComWindows);
            panel1.Controls.Add(btSalvar);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(541, 318);
            panel1.TabIndex = 0;
            // 
            // chkIniciarComWindows
            // 
            chkIniciarComWindows.AutoSize = true;
            chkIniciarComWindows.Location = new Point(3, 153);
            chkIniciarComWindows.Name = "chkIniciarComWindows";
            chkIniciarComWindows.Size = new Size(147, 19);
            chkIniciarComWindows.TabIndex = 3;
            chkIniciarComWindows.Text = "Iniciar com o Windows";
            chkIniciarComWindows.UseVisualStyleBackColor = true;
            // 
            // btSalvar
            // 
            btSalvar.Location = new Point(200, 282);
            btSalvar.Name = "btSalvar";
            btSalvar.Size = new Size(143, 31);
            btSalvar.TabIndex = 2;
            btSalvar.Text = "Salvar Configurações";
            btSalvar.UseVisualStyleBackColor = true;
            btSalvar.Click += btSalvar_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtAtalhoGravacao);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(289, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(247, 133);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Gravação";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(32, 65);
            label3.Name = "label3";
            label3.Size = new Size(188, 13);
            label3.TabIndex = 4;
            label3.Text = "*Teclas de atalhos, ex: CTRL + ALT + S";
            // 
            // txtAtalhoGravacao
            // 
            txtAtalhoGravacao.Location = new Point(83, 39);
            txtAtalhoGravacao.Name = "txtAtalhoGravacao";
            txtAtalhoGravacao.Size = new Size(138, 23);
            txtAtalhoGravacao.TabIndex = 2;
            txtAtalhoGravacao.KeyDown += txtAtalhoGravacao_KeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 42);
            label4.Name = "label4";
            label4.Size = new Size(45, 15);
            label4.TabIndex = 3;
            label4.Text = "Atalho:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtAtalhoCaptura);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(263, 133);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Captura";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(28, 70);
            label2.Name = "label2";
            label2.Size = new Size(171, 13);
            label2.TabIndex = 1;
            label2.Text = "*Teclas de atalhos, ex: CTRL + Prtn";
            // 
            // txtAtalhoCaptura
            // 
            txtAtalhoCaptura.Location = new Point(79, 44);
            txtAtalhoCaptura.Name = "txtAtalhoCaptura";
            txtAtalhoCaptura.Size = new Size(138, 23);
            txtAtalhoCaptura.TabIndex = 0;
            txtAtalhoCaptura.KeyDown += txtAtalhoCaptura_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 47);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Atalho:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Bisque;
            ClientSize = new Size(565, 342);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Gif Screen App";
            Shown += Form1_Shown;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private Panel panel1;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Button btSalvar;
        private Label label2;
        private TextBox txtAtalhoCaptura;
        private Label label1;
        private Label label3;
        private TextBox txtAtalhoGravacao;
        private Label label4;
        private CheckBox chkIniciarComWindows;
    }
}

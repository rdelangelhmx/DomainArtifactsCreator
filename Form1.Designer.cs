namespace WCF_MVC_Creator
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tDrivers = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tDataBase = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tServidor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tPuerto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tSchema = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tNameSpace = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pbProgreso = new System.Windows.Forms.ProgressBar();
            this.lProgreso = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tTabla = new System.Windows.Forms.ComboBox();
            this.tDatos = new System.Windows.Forms.TextBox();
            this.tAddition = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tService = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 445);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Conexión:";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox1.Location = new System.Drawing.Point(115, 445);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(437, 52);
            this.textBox1.TabIndex = 20;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(115, 411);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(408, 20);
            this.textBox3.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 411);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ruta Destino:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(437, 522);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 26);
            this.button1.TabIndex = 13;
            this.button1.Text = "Crear Clases";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tUser
            // 
            this.tUser.Location = new System.Drawing.Point(115, 112);
            this.tUser.Name = "tUser";
            this.tUser.Size = new System.Drawing.Size(240, 20);
            this.tUser.TabIndex = 3;
            this.tUser.TextChanged += new System.EventHandler(this.tUser_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Usuario:";
            // 
            // tDrivers
            // 
            this.tDrivers.FormattingEnabled = true;
            this.tDrivers.Items.AddRange(new object[] {
            "{SQL Server}",
            "{MySQL ODBC 8.0 ANSI Driver}",
            "{Microsoft ODBC Driver for Oracle}"});
            this.tDrivers.Location = new System.Drawing.Point(115, 12);
            this.tDrivers.Name = "tDrivers";
            this.tDrivers.Size = new System.Drawing.Size(240, 21);
            this.tDrivers.TabIndex = 0;
            this.tDrivers.SelectedIndexChanged += new System.EventHandler(this.tDrivers_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Driver ODBC:";
            // 
            // tPassword
            // 
            this.tPassword.Location = new System.Drawing.Point(115, 145);
            this.tPassword.Name = "tPassword";
            this.tPassword.Size = new System.Drawing.Size(240, 20);
            this.tPassword.TabIndex = 4;
            this.tPassword.TextChanged += new System.EventHandler(this.tPassword_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Contraseña:";
            // 
            // tDataBase
            // 
            this.tDataBase.FormattingEnabled = true;
            this.tDataBase.ItemHeight = 13;
            this.tDataBase.Location = new System.Drawing.Point(115, 178);
            this.tDataBase.Name = "tDataBase";
            this.tDataBase.Size = new System.Drawing.Size(240, 21);
            this.tDataBase.TabIndex = 5;
            this.tDataBase.DropDown += new System.EventHandler(this.tDataBase_DropDown);
            this.tDataBase.SelectedIndexChanged += new System.EventHandler(this.tDataBase_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Base De Datos:";
            // 
            // bFolder
            // 
            this.bFolder.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bFolder.Location = new System.Drawing.Point(529, 410);
            this.bFolder.Name = "bFolder";
            this.bFolder.Size = new System.Drawing.Size(23, 20);
            this.bFolder.TabIndex = 12;
            this.bFolder.Text = "...";
            this.bFolder.UseVisualStyleBackColor = false;
            this.bFolder.Click += new System.EventHandler(this.bFolder_Click);
            // 
            // tServidor
            // 
            this.tServidor.Location = new System.Drawing.Point(115, 46);
            this.tServidor.Name = "tServidor";
            this.tServidor.Size = new System.Drawing.Size(240, 20);
            this.tServidor.TabIndex = 1;
            this.tServidor.TextChanged += new System.EventHandler(this.tServidor_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Servidor:";
            // 
            // tPuerto
            // 
            this.tPuerto.Location = new System.Drawing.Point(115, 79);
            this.tPuerto.Name = "tPuerto";
            this.tPuerto.Size = new System.Drawing.Size(240, 20);
            this.tPuerto.TabIndex = 2;
            this.tPuerto.TextChanged += new System.EventHandler(this.tPuerto_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Puerto:";
            // 
            // tSchema
            // 
            this.tSchema.Location = new System.Drawing.Point(115, 245);
            this.tSchema.Name = "tSchema";
            this.tSchema.Size = new System.Drawing.Size(240, 20);
            this.tSchema.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Service Name:";
            // 
            // tNameSpace
            // 
            this.tNameSpace.Location = new System.Drawing.Point(115, 278);
            this.tNameSpace.Name = "tNameSpace";
            this.tNameSpace.Size = new System.Drawing.Size(240, 20);
            this.tNameSpace.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Name Space:";
            // 
            // pbProgreso
            // 
            this.pbProgreso.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.pbProgreso.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pbProgreso.Location = new System.Drawing.Point(32, 527);
            this.pbProgreso.Margin = new System.Windows.Forms.Padding(2);
            this.pbProgreso.Name = "pbProgreso";
            this.pbProgreso.Size = new System.Drawing.Size(350, 19);
            this.pbProgreso.Step = 5;
            this.pbProgreso.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProgreso.TabIndex = 34;
            // 
            // lProgreso
            // 
            this.lProgreso.BackColor = System.Drawing.Color.Transparent;
            this.lProgreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lProgreso.Location = new System.Drawing.Point(32, 506);
            this.lProgreso.Name = "lProgreso";
            this.lProgreso.Size = new System.Drawing.Size(350, 19);
            this.lProgreso.TabIndex = 35;
            this.lProgreso.Text = "Processing...";
            this.lProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lProgreso.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 311);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 37;
            this.label11.Text = "Table Name:";
            // 
            // tTabla
            // 
            this.tTabla.FormattingEnabled = true;
            this.tTabla.ItemHeight = 13;
            this.tTabla.Location = new System.Drawing.Point(115, 311);
            this.tTabla.Name = "tTabla";
            this.tTabla.Size = new System.Drawing.Size(240, 21);
            this.tTabla.TabIndex = 8;
            this.tTabla.DropDown += new System.EventHandler(this.tTabla_DropDown);
            // 
            // tDatos
            // 
            this.tDatos.Location = new System.Drawing.Point(115, 345);
            this.tDatos.Name = "tDatos";
            this.tDatos.Size = new System.Drawing.Size(350, 20);
            this.tDatos.TabIndex = 9;
            this.tDatos.Text = "Rodrigo del Angel <rdelangelhmx@gmail.com>";
            // 
            // tAddition
            // 
            this.tAddition.Location = new System.Drawing.Point(115, 378);
            this.tAddition.Name = "tAddition";
            this.tAddition.Size = new System.Drawing.Size(164, 20);
            this.tAddition.TabIndex = 10;
            this.tAddition.Text = "Azure";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 339);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "Autor:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(30, 378);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 13);
            this.label13.TabIndex = 39;
            this.label13.Text = "Repo Addition:";
            // 
            // tService
            // 
            this.tService.Location = new System.Drawing.Point(115, 212);
            this.tService.Name = "tService";
            this.tService.Size = new System.Drawing.Size(240, 20);
            this.tService.TabIndex = 40;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(30, 245);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Schema:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 582);
            this.Controls.Add(this.tService);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lProgreso);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tAddition);
            this.Controls.Add(this.tDatos);
            this.Controls.Add(this.tTabla);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tNameSpace);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tSchema);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tPuerto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tServidor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bFolder);
            this.Controls.Add(this.tDataBase);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tDrivers);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbProgreso);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Creación de DTO-DAO";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox tDrivers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox tDataBase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tServidor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tPuerto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tSchema;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tNameSpace;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ProgressBar pbProgreso;
        private System.Windows.Forms.Label lProgreso;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox tTabla;
        private System.Windows.Forms.TextBox tDatos;
        private System.Windows.Forms.TextBox tAddition;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tService;
        private System.Windows.Forms.Label label14;
    }
}


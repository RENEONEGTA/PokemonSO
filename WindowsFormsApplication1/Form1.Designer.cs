namespace WindowsFormsApplication1
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
            this.IP = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consultaPokedex = new System.Windows.Forms.RadioButton();
            this.Longitud = new System.Windows.Forms.RadioButton();
            this.Bonito = new System.Windows.Forms.RadioButton();
            this.SignIn = new System.Windows.Forms.Button();
            this.SignUp = new System.Windows.Forms.Button();
            this.textContraR = new System.Windows.Forms.TextBox();
            this.textUsuR = new System.Windows.Forms.TextBox();
            this.ContraseñaRegistrarse = new System.Windows.Forms.Label();
            this.UsuarioRegistrarse = new System.Windows.Forms.Label();
            this.textConRR = new System.Windows.Forms.TextBox();
            this.RepetirContraseña = new System.Windows.Forms.Label();
            this.usuarioBox = new CustomGroupBox();
            this.textUsu = new System.Windows.Forms.TextBox();
            this.contraseñaBox = new CustomGroupBox();
            this.textContra = new System.Windows.Forms.TextBox();
            this.iniciarSesionBox = new CustomGroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.circuloServidor = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.usuarioBox.SuspendLayout();
            this.contraseñaBox.SuspendLayout();
            this.iniciarSesionBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).BeginInit();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.IP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IP.ForeColor = System.Drawing.Color.White;
            this.IP.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.IP.Location = new System.Drawing.Point(100, 817);
            this.IP.Margin = new System.Windows.Forms.Padding(6);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(194, 27);
            this.IP.TabIndex = 2;
            this.IP.TabStop = false;
            this.IP.Text = "192.168.1.146";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 210);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 42);
            this.button2.TabIndex = 5;
            this.button2.Text = "Enviar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.consultaPokedex);
            this.groupBox1.Controls.Add(this.Longitud);
            this.groupBox1.Controls.Add(this.Bonito);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(41, 917);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(666, 374);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peticion";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // consultaPokedex
            // 
            this.consultaPokedex.AutoSize = true;
            this.consultaPokedex.Location = new System.Drawing.Point(56, 158);
            this.consultaPokedex.Margin = new System.Windows.Forms.Padding(6);
            this.consultaPokedex.Name = "consultaPokedex";
            this.consultaPokedex.Size = new System.Drawing.Size(335, 29);
            this.consultaPokedex.TabIndex = 9;
            this.consultaPokedex.TabStop = true;
            this.consultaPokedex.Text = "Dame informacion de Charmander";
            this.consultaPokedex.UseVisualStyleBackColor = true;
            // 
            // Longitud
            // 
            this.Longitud.AutoSize = true;
            this.Longitud.Location = new System.Drawing.Point(56, 116);
            this.Longitud.Margin = new System.Windows.Forms.Padding(6);
            this.Longitud.Name = "Longitud";
            this.Longitud.Size = new System.Drawing.Size(349, 29);
            this.Longitud.TabIndex = 7;
            this.Longitud.TabStop = true;
            this.Longitud.Text = "Dime la primera pratida que he echo";
            this.Longitud.UseVisualStyleBackColor = true;
            // 
            // Bonito
            // 
            this.Bonito.AutoSize = true;
            this.Bonito.Location = new System.Drawing.Point(56, 74);
            this.Bonito.Margin = new System.Windows.Forms.Padding(6);
            this.Bonito.Name = "Bonito";
            this.Bonito.Size = new System.Drawing.Size(306, 29);
            this.Bonito.TabIndex = 8;
            this.Bonito.TabStop = true;
            this.Bonito.Text = "Dime cuantos pokemons tengo";
            this.Bonito.UseVisualStyleBackColor = true;
            // 
            // SignIn
            // 
            this.SignIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SignIn.FlatAppearance.BorderSize = 0;
            this.SignIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignIn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignIn.ForeColor = System.Drawing.Color.White;
            this.SignIn.Location = new System.Drawing.Point(3, 10);
            this.SignIn.Margin = new System.Windows.Forms.Padding(0);
            this.SignIn.Name = "SignIn";
            this.SignIn.Size = new System.Drawing.Size(572, 52);
            this.SignIn.TabIndex = 11;
            this.SignIn.Text = "INICIAR SESION";
            this.SignIn.UseVisualStyleBackColor = false;
            this.SignIn.Click += new System.EventHandler(this.SignIn_Click);
            this.SignIn.MouseEnter += new System.EventHandler(this.SignIn_MouseEnter);
            this.SignIn.MouseLeave += new System.EventHandler(this.SignIn_MouseLeave);
            // 
            // SignUp
            // 
            this.SignUp.Location = new System.Drawing.Point(843, 1163);
            this.SignUp.Margin = new System.Windows.Forms.Padding(6);
            this.SignUp.Name = "SignUp";
            this.SignUp.Size = new System.Drawing.Size(154, 57);
            this.SignUp.TabIndex = 16;
            this.SignUp.Text = "Registrarse";
            this.SignUp.UseVisualStyleBackColor = true;
            this.SignUp.Click += new System.EventHandler(this.SignUp_Click);
            // 
            // textContraR
            // 
            this.textContraR.Location = new System.Drawing.Point(967, 1049);
            this.textContraR.Margin = new System.Windows.Forms.Padding(6);
            this.textContraR.Name = "textContraR";
            this.textContraR.Size = new System.Drawing.Size(235, 29);
            this.textContraR.TabIndex = 15;
            // 
            // textUsuR
            // 
            this.textUsuR.Location = new System.Drawing.Point(967, 988);
            this.textUsuR.Margin = new System.Windows.Forms.Padding(6);
            this.textUsuR.Name = "textUsuR";
            this.textUsuR.Size = new System.Drawing.Size(235, 29);
            this.textUsuR.TabIndex = 14;
            // 
            // ContraseñaRegistrarse
            // 
            this.ContraseñaRegistrarse.AutoSize = true;
            this.ContraseñaRegistrarse.Location = new System.Drawing.Point(839, 1049);
            this.ContraseñaRegistrarse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ContraseñaRegistrarse.Name = "ContraseñaRegistrarse";
            this.ContraseñaRegistrarse.Size = new System.Drawing.Size(114, 25);
            this.ContraseñaRegistrarse.TabIndex = 13;
            this.ContraseñaRegistrarse.Text = "Contraseña";
            // 
            // UsuarioRegistrarse
            // 
            this.UsuarioRegistrarse.AutoSize = true;
            this.UsuarioRegistrarse.Location = new System.Drawing.Point(839, 988);
            this.UsuarioRegistrarse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsuarioRegistrarse.Name = "UsuarioRegistrarse";
            this.UsuarioRegistrarse.Size = new System.Drawing.Size(79, 25);
            this.UsuarioRegistrarse.TabIndex = 12;
            this.UsuarioRegistrarse.Text = "Usuario";
            // 
            // textConRR
            // 
            this.textConRR.Location = new System.Drawing.Point(967, 1103);
            this.textConRR.Margin = new System.Windows.Forms.Padding(6);
            this.textConRR.Name = "textConRR";
            this.textConRR.Size = new System.Drawing.Size(235, 29);
            this.textConRR.TabIndex = 18;
            // 
            // RepetirContraseña
            // 
            this.RepetirContraseña.AutoSize = true;
            this.RepetirContraseña.Location = new System.Drawing.Point(839, 1103);
            this.RepetirContraseña.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RepetirContraseña.Name = "RepetirContraseña";
            this.RepetirContraseña.Size = new System.Drawing.Size(114, 50);
            this.RepetirContraseña.TabIndex = 17;
            this.RepetirContraseña.Text = "Repite la \r\nContraseña";
            // 
            // usuarioBox
            // 
            this.usuarioBox.BorderColor = System.Drawing.Color.White;
            this.usuarioBox.Controls.Add(this.textUsu);
            this.usuarioBox.ForeColor = System.Drawing.Color.White;
            this.usuarioBox.Location = new System.Drawing.Point(97, 319);
            this.usuarioBox.Name = "usuarioBox";
            this.usuarioBox.Size = new System.Drawing.Size(591, 86);
            this.usuarioBox.TabIndex = 20;
            this.usuarioBox.TabStop = false;
            // 
            // textUsu
            // 
            this.textUsu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textUsu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textUsu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textUsu.ForeColor = System.Drawing.Color.DimGray;
            this.textUsu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textUsu.Location = new System.Drawing.Point(19, 31);
            this.textUsu.Margin = new System.Windows.Forms.Padding(6);
            this.textUsu.Name = "textUsu";
            this.textUsu.Size = new System.Drawing.Size(549, 32);
            this.textUsu.TabIndex = 9;
            this.textUsu.Text = "Usuario";
            this.textUsu.TextChanged += new System.EventHandler(this.textUsu_TextChanged);
            this.textUsu.Enter += new System.EventHandler(this.textUsu_Enter);
            this.textUsu.Leave += new System.EventHandler(this.textUsu_Leave);
            // 
            // contraseñaBox
            // 
            this.contraseñaBox.BorderColor = System.Drawing.Color.White;
            this.contraseñaBox.Controls.Add(this.textContra);
            this.contraseñaBox.ForeColor = System.Drawing.Color.White;
            this.contraseñaBox.Location = new System.Drawing.Point(97, 443);
            this.contraseñaBox.Name = "contraseñaBox";
            this.contraseñaBox.Size = new System.Drawing.Size(591, 86);
            this.contraseñaBox.TabIndex = 21;
            this.contraseñaBox.TabStop = false;
            // 
            // textContra
            // 
            this.textContra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textContra.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textContra.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textContra.ForeColor = System.Drawing.Color.DimGray;
            this.textContra.Location = new System.Drawing.Point(19, 31);
            this.textContra.Margin = new System.Windows.Forms.Padding(6);
            this.textContra.Name = "textContra";
            this.textContra.Size = new System.Drawing.Size(543, 33);
            this.textContra.TabIndex = 10;
            this.textContra.Text = "Contraseña";
            this.textContra.Enter += new System.EventHandler(this.textContra_Enter);
            this.textContra.Leave += new System.EventHandler(this.textContra_Leave);
            // 
            // iniciarSesionBox
            // 
            this.iniciarSesionBox.BackColor = System.Drawing.Color.Transparent;
            this.iniciarSesionBox.BorderColor = System.Drawing.Color.White;
            this.iniciarSesionBox.Controls.Add(this.SignIn);
            this.iniciarSesionBox.Location = new System.Drawing.Point(97, 599);
            this.iniciarSesionBox.Name = "iniciarSesionBox";
            this.iniciarSesionBox.Size = new System.Drawing.Size(591, 65);
            this.iniciarSesionBox.TabIndex = 22;
            this.iniciarSesionBox.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsApplication1.Properties.Resources.PokemonSO1;
            this.pictureBox1.Location = new System.Drawing.Point(718, 141);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(762, 592);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // circuloServidor
            // 
            this.circuloServidor.Location = new System.Drawing.Point(273, 799);
            this.circuloServidor.Name = "circuloServidor";
            this.circuloServidor.Size = new System.Drawing.Size(73, 60);
            this.circuloServidor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.circuloServidor.TabIndex = 23;
            this.circuloServidor.TabStop = false;
            this.circuloServidor.Click += new System.EventHandler(this.circuloServidor_Click);
            this.circuloServidor.Paint += new System.Windows.Forms.PaintEventHandler(this.circuloServidor_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1549, 1300);
            this.Controls.Add(this.circuloServidor);
            this.Controls.Add(this.usuarioBox);
            this.Controls.Add(this.contraseñaBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textConRR);
            this.Controls.Add(this.RepetirContraseña);
            this.Controls.Add(this.SignUp);
            this.Controls.Add(this.textContraR);
            this.Controls.Add(this.textUsuR);
            this.Controls.Add(this.ContraseñaRegistrarse);
            this.Controls.Add(this.UsuarioRegistrarse);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.iniciarSesionBox);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.usuarioBox.ResumeLayout(false);
            this.usuarioBox.PerformLayout();
            this.contraseñaBox.ResumeLayout(false);
            this.contraseñaBox.PerformLayout();
            this.iniciarSesionBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox IP;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Longitud;
        private System.Windows.Forms.RadioButton Bonito;
        private System.Windows.Forms.TextBox textUsu;
        private System.Windows.Forms.TextBox textContra;
        private System.Windows.Forms.Button SignIn;
        private System.Windows.Forms.Button SignUp;
        private System.Windows.Forms.TextBox textContraR;
        private System.Windows.Forms.TextBox textUsuR;
        private System.Windows.Forms.Label ContraseñaRegistrarse;
        private System.Windows.Forms.Label UsuarioRegistrarse;
        private System.Windows.Forms.TextBox textConRR;
        private System.Windows.Forms.Label RepetirContraseña;
        private System.Windows.Forms.RadioButton consultaPokedex;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CustomGroupBox usuarioBox;
        public CustomGroupBox contraseñaBox;
        private CustomGroupBox iniciarSesionBox;
        private System.Windows.Forms.PictureBox circuloServidor;
    }
}


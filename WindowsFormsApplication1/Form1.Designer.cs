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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.IP = new System.Windows.Forms.TextBox();
            this.textContraR = new System.Windows.Forms.TextBox();
            this.textUsuR = new System.Windows.Forms.TextBox();
            this.ContraseñaRegistrarse = new System.Windows.Forms.Label();
            this.UsuarioRegistrarse = new System.Windows.Forms.Label();
            this.textConRR = new System.Windows.Forms.TextBox();
            this.RepetirContraseña = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.circuloServidor = new System.Windows.Forms.PictureBox();
            this.aunNoCuenta = new System.Windows.Forms.Label();
            this.fondoPokemon = new AxWMPLib.AxWindowsMediaPlayer();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consultaPokedex = new System.Windows.Forms.RadioButton();
            this.Longitud = new System.Windows.Forms.RadioButton();
            this.Bonito = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.salirJuegoBox = new CustomGroupBox();
            this.salirJuego = new System.Windows.Forms.Button();
            this.combatirBox = new CustomGroupBox();
            this.combatir = new System.Windows.Forms.Button();
            this.nuevaPartidaBox = new CustomGroupBox();
            this.nuevaPartida = new System.Windows.Forms.Button();
            this.cargarPartidaBox = new CustomGroupBox();
            this.cargarPartida = new System.Windows.Forms.Button();
            this.repiteContraBox = new CustomGroupBox();
            this.repiteContra = new System.Windows.Forms.TextBox();
            this.iniciarSesionBox = new CustomGroupBox();
            this.SignIn = new System.Windows.Forms.Button();
            this.usuarioBox = new CustomGroupBox();
            this.textUsu = new System.Windows.Forms.TextBox();
            this.contraseñaBox = new CustomGroupBox();
            this.textContra = new System.Windows.Forms.TextBox();
            this.registroBox = new CustomGroupBox();
            this.SignUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fondoPokemon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.salirJuegoBox.SuspendLayout();
            this.combatirBox.SuspendLayout();
            this.nuevaPartidaBox.SuspendLayout();
            this.cargarPartidaBox.SuspendLayout();
            this.repiteContraBox.SuspendLayout();
            this.iniciarSesionBox.SuspendLayout();
            this.usuarioBox.SuspendLayout();
            this.contraseñaBox.SuspendLayout();
            this.registroBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.IP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IP.ForeColor = System.Drawing.Color.White;
            this.IP.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.IP.Location = new System.Drawing.Point(100, 749);
            this.IP.Margin = new System.Windows.Forms.Padding(6);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(194, 19);
            this.IP.TabIndex = 2;
            this.IP.TabStop = false;
            this.IP.Text = "192.168.56.102";
            // 
            // textContraR
            // 
            this.textContraR.Location = new System.Drawing.Point(997, 1154);
            this.textContraR.Margin = new System.Windows.Forms.Padding(6);
            this.textContraR.Name = "textContraR";
            this.textContraR.Size = new System.Drawing.Size(237, 25);
            this.textContraR.TabIndex = 15;
            // 
            // textUsuR
            // 
            this.textUsuR.Location = new System.Drawing.Point(997, 1097);
            this.textUsuR.Margin = new System.Windows.Forms.Padding(6);
            this.textUsuR.Name = "textUsuR";
            this.textUsuR.Size = new System.Drawing.Size(237, 25);
            this.textUsuR.TabIndex = 14;
            // 
            // ContraseñaRegistrarse
            // 
            this.ContraseñaRegistrarse.AutoSize = true;
            this.ContraseñaRegistrarse.Location = new System.Drawing.Point(869, 1154);
            this.ContraseñaRegistrarse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ContraseñaRegistrarse.Name = "ContraseñaRegistrarse";
            this.ContraseñaRegistrarse.Size = new System.Drawing.Size(79, 19);
            this.ContraseñaRegistrarse.TabIndex = 13;
            this.ContraseñaRegistrarse.Text = "Contraseña";
            // 
            // UsuarioRegistrarse
            // 
            this.UsuarioRegistrarse.AutoSize = true;
            this.UsuarioRegistrarse.Location = new System.Drawing.Point(869, 1097);
            this.UsuarioRegistrarse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsuarioRegistrarse.Name = "UsuarioRegistrarse";
            this.UsuarioRegistrarse.Size = new System.Drawing.Size(56, 19);
            this.UsuarioRegistrarse.TabIndex = 12;
            this.UsuarioRegistrarse.Text = "Usuario";
            // 
            // textConRR
            // 
            this.textConRR.Location = new System.Drawing.Point(997, 1202);
            this.textConRR.Margin = new System.Windows.Forms.Padding(6);
            this.textConRR.Name = "textConRR";
            this.textConRR.Size = new System.Drawing.Size(237, 25);
            this.textConRR.TabIndex = 18;
            // 
            // RepetirContraseña
            // 
            this.RepetirContraseña.AutoSize = true;
            this.RepetirContraseña.Location = new System.Drawing.Point(869, 1202);
            this.RepetirContraseña.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RepetirContraseña.Name = "RepetirContraseña";
            this.RepetirContraseña.Size = new System.Drawing.Size(79, 38);
            this.RepetirContraseña.TabIndex = 17;
            this.RepetirContraseña.Text = "Repite la \r\nContraseña";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsApplication1.Properties.Resources.PokemonSO1;
            this.pictureBox1.Location = new System.Drawing.Point(717, 129);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(761, 542);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // circuloServidor
            // 
            this.circuloServidor.Location = new System.Drawing.Point(255, 732);
            this.circuloServidor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.circuloServidor.Name = "circuloServidor";
            this.circuloServidor.Size = new System.Drawing.Size(73, 55);
            this.circuloServidor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.circuloServidor.TabIndex = 23;
            this.circuloServidor.TabStop = false;
            this.circuloServidor.Click += new System.EventHandler(this.circuloServidor_Click);
            this.circuloServidor.Paint += new System.Windows.Forms.PaintEventHandler(this.circuloServidor_Paint);
            // 
            // aunNoCuenta
            // 
            this.aunNoCuenta.AutoSize = true;
            this.aunNoCuenta.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aunNoCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(209)))), ((int)(((byte)(255)))));
            this.aunNoCuenta.Location = new System.Drawing.Point(300, 620);
            this.aunNoCuenta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.aunNoCuenta.Name = "aunNoCuenta";
            this.aunNoCuenta.Size = new System.Drawing.Size(216, 23);
            this.aunNoCuenta.TabIndex = 24;
            this.aunNoCuenta.Text = "¿Aún no tienes cuenta?";
            this.aunNoCuenta.Click += new System.EventHandler(this.aunNoCuenta_Click);
            // 
            // fondoPokemon
            // 
            this.fondoPokemon.Enabled = true;
            this.fondoPokemon.Location = new System.Drawing.Point(942, 972);
            this.fondoPokemon.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fondoPokemon.Name = "fondoPokemon";
            this.fondoPokemon.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("fondoPokemon.OcxState")));
            this.fondoPokemon.Size = new System.Drawing.Size(558, 109);
            this.fondoPokemon.TabIndex = 25;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(255, 1054);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(466, 33);
            this.progressBar.TabIndex = 30;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.consultaPokedex);
            this.groupBox1.Controls.Add(this.Longitud);
            this.groupBox1.Controls.Add(this.Bonito);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(86, 1171);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(669, 342);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peticion";
            // 
            // consultaPokedex
            // 
            this.consultaPokedex.AutoSize = true;
            this.consultaPokedex.Location = new System.Drawing.Point(56, 144);
            this.consultaPokedex.Margin = new System.Windows.Forms.Padding(6);
            this.consultaPokedex.Name = "consultaPokedex";
            this.consultaPokedex.Size = new System.Drawing.Size(241, 23);
            this.consultaPokedex.TabIndex = 9;
            this.consultaPokedex.TabStop = true;
            this.consultaPokedex.Text = "Dame informacion de Charmander";
            this.consultaPokedex.UseVisualStyleBackColor = true;
            // 
            // Longitud
            // 
            this.Longitud.AutoSize = true;
            this.Longitud.Location = new System.Drawing.Point(56, 106);
            this.Longitud.Margin = new System.Windows.Forms.Padding(6);
            this.Longitud.Name = "Longitud";
            this.Longitud.Size = new System.Drawing.Size(253, 23);
            this.Longitud.TabIndex = 7;
            this.Longitud.TabStop = true;
            this.Longitud.Text = "Dime la primera pratida que he echo";
            this.Longitud.UseVisualStyleBackColor = true;
            // 
            // Bonito
            // 
            this.Bonito.AutoSize = true;
            this.Bonito.Location = new System.Drawing.Point(56, 68);
            this.Bonito.Margin = new System.Windows.Forms.Padding(6);
            this.Bonito.Name = "Bonito";
            this.Bonito.Size = new System.Drawing.Size(222, 23);
            this.Bonito.TabIndex = 8;
            this.Bonito.TabStop = true;
            this.Bonito.Text = "Dime cuantos pokemons tengo";
            this.Bonito.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 192);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 38);
            this.button2.TabIndex = 5;
            this.button2.Text = "Enviar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // salirJuegoBox
            // 
            this.salirJuegoBox.BackColor = System.Drawing.Color.Transparent;
            this.salirJuegoBox.BorderColor = System.Drawing.Color.White;
            this.salirJuegoBox.Controls.Add(this.salirJuego);
            this.salirJuegoBox.Location = new System.Drawing.Point(851, 1023);
            this.salirJuegoBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.salirJuegoBox.Name = "salirJuegoBox";
            this.salirJuegoBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.salirJuegoBox.Size = new System.Drawing.Size(592, 59);
            this.salirJuegoBox.TabIndex = 29;
            this.salirJuegoBox.TabStop = false;
            this.salirJuegoBox.Visible = false;
            // 
            // salirJuego
            // 
            this.salirJuego.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.salirJuego.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.salirJuego.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.salirJuego.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.salirJuego.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirJuego.ForeColor = System.Drawing.Color.White;
            this.salirJuego.Location = new System.Drawing.Point(7, 9);
            this.salirJuego.Margin = new System.Windows.Forms.Padding(6);
            this.salirJuego.Name = "salirJuego";
            this.salirJuego.Size = new System.Drawing.Size(574, 43);
            this.salirJuego.TabIndex = 16;
            this.salirJuego.Text = "SALIR DEL JUEGO";
            this.salirJuego.UseVisualStyleBackColor = false;
            this.salirJuego.MouseClick += new System.Windows.Forms.MouseEventHandler(this.salirJuego_MouseClick);
            this.salirJuego.MouseEnter += new System.EventHandler(this.salirJuego_MouseEnter);
            this.salirJuego.MouseLeave += new System.EventHandler(this.salirJuego_MouseLeave);
            // 
            // combatirBox
            // 
            this.combatirBox.BackColor = System.Drawing.Color.Transparent;
            this.combatirBox.BorderColor = System.Drawing.Color.White;
            this.combatirBox.Controls.Add(this.combatir);
            this.combatirBox.Location = new System.Drawing.Point(248, 939);
            this.combatirBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.combatirBox.Name = "combatirBox";
            this.combatirBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.combatirBox.Size = new System.Drawing.Size(592, 59);
            this.combatirBox.TabIndex = 28;
            this.combatirBox.TabStop = false;
            this.combatirBox.Visible = false;
            // 
            // combatir
            // 
            this.combatir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.combatir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.combatir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.combatir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combatir.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combatir.ForeColor = System.Drawing.Color.White;
            this.combatir.Location = new System.Drawing.Point(7, 9);
            this.combatir.Margin = new System.Windows.Forms.Padding(6);
            this.combatir.Name = "combatir";
            this.combatir.Size = new System.Drawing.Size(574, 43);
            this.combatir.TabIndex = 16;
            this.combatir.Text = "COMBATIR";
            this.combatir.UseVisualStyleBackColor = false;
            this.combatir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.combatir_MouseClick);
            this.combatir.MouseEnter += new System.EventHandler(this.combatir_MouseEnter);
            this.combatir.MouseLeave += new System.EventHandler(this.combatir_MouseLeave);
            // 
            // nuevaPartidaBox
            // 
            this.nuevaPartidaBox.BackColor = System.Drawing.Color.Transparent;
            this.nuevaPartidaBox.BorderColor = System.Drawing.Color.White;
            this.nuevaPartidaBox.Controls.Add(this.nuevaPartida);
            this.nuevaPartidaBox.Location = new System.Drawing.Point(86, 826);
            this.nuevaPartidaBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.nuevaPartidaBox.Name = "nuevaPartidaBox";
            this.nuevaPartidaBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.nuevaPartidaBox.Size = new System.Drawing.Size(592, 59);
            this.nuevaPartidaBox.TabIndex = 26;
            this.nuevaPartidaBox.TabStop = false;
            this.nuevaPartidaBox.Visible = false;
            // 
            // nuevaPartida
            // 
            this.nuevaPartida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.nuevaPartida.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.nuevaPartida.FlatAppearance.BorderSize = 0;
            this.nuevaPartida.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.nuevaPartida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nuevaPartida.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nuevaPartida.ForeColor = System.Drawing.Color.White;
            this.nuevaPartida.Location = new System.Drawing.Point(15, 10);
            this.nuevaPartida.Margin = new System.Windows.Forms.Padding(0);
            this.nuevaPartida.Name = "nuevaPartida";
            this.nuevaPartida.Size = new System.Drawing.Size(559, 37);
            this.nuevaPartida.TabIndex = 11;
            this.nuevaPartida.Text = "NUEVA PARTIDA";
            this.nuevaPartida.UseVisualStyleBackColor = false;
            this.nuevaPartida.Click += new System.EventHandler(this.nuevaPartida_Click);
            this.nuevaPartida.MouseEnter += new System.EventHandler(this.nuevaPartida_MouseEnter);
            this.nuevaPartida.MouseLeave += new System.EventHandler(this.nuevaPartida_MouseLeave);
            // 
            // cargarPartidaBox
            // 
            this.cargarPartidaBox.BackColor = System.Drawing.Color.Transparent;
            this.cargarPartidaBox.BorderColor = System.Drawing.Color.White;
            this.cargarPartidaBox.Controls.Add(this.cargarPartida);
            this.cargarPartidaBox.Location = new System.Drawing.Point(860, 939);
            this.cargarPartidaBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cargarPartidaBox.Name = "cargarPartidaBox";
            this.cargarPartidaBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cargarPartidaBox.Size = new System.Drawing.Size(592, 59);
            this.cargarPartidaBox.TabIndex = 27;
            this.cargarPartidaBox.TabStop = false;
            this.cargarPartidaBox.Visible = false;
            // 
            // cargarPartida
            // 
            this.cargarPartida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.cargarPartida.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cargarPartida.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.cargarPartida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cargarPartida.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cargarPartida.ForeColor = System.Drawing.Color.White;
            this.cargarPartida.Location = new System.Drawing.Point(7, 9);
            this.cargarPartida.Margin = new System.Windows.Forms.Padding(6);
            this.cargarPartida.Name = "cargarPartida";
            this.cargarPartida.Size = new System.Drawing.Size(574, 43);
            this.cargarPartida.TabIndex = 16;
            this.cargarPartida.Text = "CARGAR PARTIDA";
            this.cargarPartida.UseVisualStyleBackColor = false;
            this.cargarPartida.Click += new System.EventHandler(this.cargarPartida_Click);
            this.cargarPartida.MouseEnter += new System.EventHandler(this.cargarPartida_MouseEnter);
            this.cargarPartida.MouseLeave += new System.EventHandler(this.cargarPartida_MouseLeave);
            // 
            // repiteContraBox
            // 
            this.repiteContraBox.BorderColor = System.Drawing.Color.White;
            this.repiteContraBox.Controls.Add(this.repiteContra);
            this.repiteContraBox.ForeColor = System.Drawing.Color.White;
            this.repiteContraBox.Location = new System.Drawing.Point(97, 404);
            this.repiteContraBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.repiteContraBox.Name = "repiteContraBox";
            this.repiteContraBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.repiteContraBox.Size = new System.Drawing.Size(592, 78);
            this.repiteContraBox.TabIndex = 22;
            this.repiteContraBox.TabStop = false;
            this.repiteContraBox.Visible = false;
            // 
            // repiteContra
            // 
            this.repiteContra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.repiteContra.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.repiteContra.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repiteContra.ForeColor = System.Drawing.Color.DimGray;
            this.repiteContra.Location = new System.Drawing.Point(20, 29);
            this.repiteContra.Margin = new System.Windows.Forms.Padding(6);
            this.repiteContra.Name = "repiteContra";
            this.repiteContra.Size = new System.Drawing.Size(543, 23);
            this.repiteContra.TabIndex = 10;
            this.repiteContra.Text = "Repite la contraseña";
            this.repiteContra.Enter += new System.EventHandler(this.repiteContra_Enter);
            this.repiteContra.Leave += new System.EventHandler(this.repiteContra_Leave);
            // 
            // iniciarSesionBox
            // 
            this.iniciarSesionBox.BackColor = System.Drawing.Color.Transparent;
            this.iniciarSesionBox.BorderColor = System.Drawing.Color.White;
            this.iniciarSesionBox.Controls.Add(this.SignIn);
            this.iniciarSesionBox.Location = new System.Drawing.Point(97, 549);
            this.iniciarSesionBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.iniciarSesionBox.Name = "iniciarSesionBox";
            this.iniciarSesionBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.iniciarSesionBox.Size = new System.Drawing.Size(592, 59);
            this.iniciarSesionBox.TabIndex = 22;
            this.iniciarSesionBox.TabStop = false;
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
            this.SignIn.Location = new System.Drawing.Point(16, 10);
            this.SignIn.Margin = new System.Windows.Forms.Padding(0);
            this.SignIn.Name = "SignIn";
            this.SignIn.Size = new System.Drawing.Size(559, 48);
            this.SignIn.TabIndex = 11;
            this.SignIn.Text = "INICIAR SESION";
            this.SignIn.UseVisualStyleBackColor = false;
            this.SignIn.Click += new System.EventHandler(this.SignIn_Click);
            this.SignIn.MouseEnter += new System.EventHandler(this.SignIn_MouseEnter);
            this.SignIn.MouseLeave += new System.EventHandler(this.SignIn_MouseLeave);
            // 
            // usuarioBox
            // 
            this.usuarioBox.BorderColor = System.Drawing.Color.White;
            this.usuarioBox.Controls.Add(this.textUsu);
            this.usuarioBox.ForeColor = System.Drawing.Color.White;
            this.usuarioBox.Location = new System.Drawing.Point(97, 184);
            this.usuarioBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.usuarioBox.Name = "usuarioBox";
            this.usuarioBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.usuarioBox.Size = new System.Drawing.Size(592, 78);
            this.usuarioBox.TabIndex = 20;
            this.usuarioBox.TabStop = false;
            // 
            // textUsu
            // 
            this.textUsu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textUsu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textUsu.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textUsu.ForeColor = System.Drawing.Color.DimGray;
            this.textUsu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textUsu.Location = new System.Drawing.Point(20, 29);
            this.textUsu.Margin = new System.Windows.Forms.Padding(6);
            this.textUsu.Name = "textUsu";
            this.textUsu.Size = new System.Drawing.Size(549, 23);
            this.textUsu.TabIndex = 9;
            this.textUsu.Text = "Usuario";
            this.textUsu.Enter += new System.EventHandler(this.textUsu_Enter);
            this.textUsu.Leave += new System.EventHandler(this.textUsu_Leave);
            // 
            // contraseñaBox
            // 
            this.contraseñaBox.BorderColor = System.Drawing.Color.White;
            this.contraseñaBox.Controls.Add(this.textContra);
            this.contraseñaBox.ForeColor = System.Drawing.Color.White;
            this.contraseñaBox.Location = new System.Drawing.Point(97, 294);
            this.contraseñaBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contraseñaBox.Name = "contraseñaBox";
            this.contraseñaBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contraseñaBox.Size = new System.Drawing.Size(592, 78);
            this.contraseñaBox.TabIndex = 21;
            this.contraseñaBox.TabStop = false;
            // 
            // textContra
            // 
            this.textContra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textContra.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textContra.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textContra.ForeColor = System.Drawing.Color.DimGray;
            this.textContra.Location = new System.Drawing.Point(20, 29);
            this.textContra.Margin = new System.Windows.Forms.Padding(6);
            this.textContra.Name = "textContra";
            this.textContra.Size = new System.Drawing.Size(543, 23);
            this.textContra.TabIndex = 10;
            this.textContra.Text = "Contraseña";
            this.textContra.Enter += new System.EventHandler(this.textContra_Enter);
            this.textContra.Leave += new System.EventHandler(this.textContra_Leave);
            // 
            // registroBox
            // 
            this.registroBox.BackColor = System.Drawing.Color.Transparent;
            this.registroBox.BorderColor = System.Drawing.Color.White;
            this.registroBox.Controls.Add(this.SignUp);
            this.registroBox.Location = new System.Drawing.Point(97, 549);
            this.registroBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.registroBox.Name = "registroBox";
            this.registroBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.registroBox.Size = new System.Drawing.Size(592, 59);
            this.registroBox.TabIndex = 23;
            this.registroBox.TabStop = false;
            this.registroBox.Visible = false;
            // 
            // SignUp
            // 
            this.SignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SignUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignUp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUp.ForeColor = System.Drawing.Color.White;
            this.SignUp.Location = new System.Drawing.Point(7, 9);
            this.SignUp.Margin = new System.Windows.Forms.Padding(6);
            this.SignUp.Name = "SignUp";
            this.SignUp.Size = new System.Drawing.Size(574, 43);
            this.SignUp.TabIndex = 16;
            this.SignUp.Text = "REGISTRARSE";
            this.SignUp.UseVisualStyleBackColor = false;
            this.SignUp.Click += new System.EventHandler(this.SignUp_Click);
            this.SignUp.MouseEnter += new System.EventHandler(this.SignUp_MouseEnter);
            this.SignUp.MouseLeave += new System.EventHandler(this.SignUp_MouseLeave);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1522, 801);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.salirJuegoBox);
            this.Controls.Add(this.combatirBox);
            this.Controls.Add(this.nuevaPartidaBox);
            this.Controls.Add(this.cargarPartidaBox);
            this.Controls.Add(this.fondoPokemon);
            this.Controls.Add(this.repiteContraBox);
            this.Controls.Add(this.iniciarSesionBox);
            this.Controls.Add(this.aunNoCuenta);
            this.Controls.Add(this.circuloServidor);
            this.Controls.Add(this.usuarioBox);
            this.Controls.Add(this.contraseñaBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textConRR);
            this.Controls.Add(this.RepetirContraseña);
            this.Controls.Add(this.textContraR);
            this.Controls.Add(this.textUsuR);
            this.Controls.Add(this.ContraseñaRegistrarse);
            this.Controls.Add(this.UsuarioRegistrarse);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.registroBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Form1Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fondoPokemon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.salirJuegoBox.ResumeLayout(false);
            this.combatirBox.ResumeLayout(false);
            this.nuevaPartidaBox.ResumeLayout(false);
            this.cargarPartidaBox.ResumeLayout(false);
            this.repiteContraBox.ResumeLayout(false);
            this.repiteContraBox.PerformLayout();
            this.iniciarSesionBox.ResumeLayout(false);
            this.usuarioBox.ResumeLayout(false);
            this.usuarioBox.PerformLayout();
            this.contraseñaBox.ResumeLayout(false);
            this.contraseñaBox.PerformLayout();
            this.registroBox.ResumeLayout(false);
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
        private System.Windows.Forms.Label aunNoCuenta;
        private CustomGroupBox registroBox;
        public CustomGroupBox repiteContraBox;
        private System.Windows.Forms.TextBox repiteContra;
        private AxWMPLib.AxWindowsMediaPlayer fondoPokemon;
        private CustomGroupBox nuevaPartidaBox;
        private System.Windows.Forms.Button nuevaPartida;
        private CustomGroupBox cargarPartidaBox;
        private System.Windows.Forms.Button cargarPartida;
        private CustomGroupBox combatirBox;
        private System.Windows.Forms.Button combatir;
        private CustomGroupBox salirJuegoBox;
        private System.Windows.Forms.Button salirJuego;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}


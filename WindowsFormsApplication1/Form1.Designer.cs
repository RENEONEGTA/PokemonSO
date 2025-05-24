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
            this.aunNoCuenta = new System.Windows.Forms.Label();
            this.fondoPokemon = new AxWMPLib.AxWindowsMediaPlayer();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.circuloServidor = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Cerrar = new System.Windows.Forms.PictureBox();
            this.AbrirSobre = new System.Windows.Forms.Button();
            this.iniciarSesionBox = new CustomGroupBox();
            this.SignIn = new System.Windows.Forms.Button();
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
            this.usuarioBox = new CustomGroupBox();
            this.textUsu = new System.Windows.Forms.TextBox();
            this.contraseñaBox = new CustomGroupBox();
            this.textContra = new System.Windows.Forms.TextBox();
            this.registroBox = new CustomGroupBox();
            this.SignUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fondoPokemon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cerrar)).BeginInit();
            this.iniciarSesionBox.SuspendLayout();
            this.salirJuegoBox.SuspendLayout();
            this.combatirBox.SuspendLayout();
            this.nuevaPartidaBox.SuspendLayout();
            this.cargarPartidaBox.SuspendLayout();
            this.repiteContraBox.SuspendLayout();
            this.usuarioBox.SuspendLayout();
            this.contraseñaBox.SuspendLayout();
            this.registroBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.IP.Text = "192.168.56.101";  // shiva: "10.4.119.5"
            // 
            // aunNoCuenta
            // 
            this.aunNoCuenta.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.aunNoCuenta.AutoSize = true;
            this.aunNoCuenta.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aunNoCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(209)))), ((int)(((byte)(255)))));
            this.aunNoCuenta.Location = new System.Drawing.Point(349, 625);
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
            this.fondoPokemon.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(255, 1054);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(466, 33);
            this.progressBar.TabIndex = 30;
            this.progressBar.Visible = false;
            // 
            // circuloServidor
            // 
            this.circuloServidor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::WindowsFormsApplication1.Properties.Resources.PokemonSO1;
            this.pictureBox1.Location = new System.Drawing.Point(717, 129);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(761, 542);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(761, 542);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // Cerrar
            // 
            this.Cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cerrar.Image = global::WindowsFormsApplication1.Properties.Resources.cerrar;
            this.Cerrar.Location = new System.Drawing.Point(1391, 37);
            this.Cerrar.Name = "Cerrar";
            this.Cerrar.Size = new System.Drawing.Size(55, 51);
            this.Cerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Cerrar.TabIndex = 31;
            this.Cerrar.TabStop = false;
            this.Cerrar.Click += new System.EventHandler(this.Cerrar_Click_1);
            // 
            // AbrirSobre
            // 
            this.AbrirSobre.Location = new System.Drawing.Point(952, 60);
            this.AbrirSobre.Name = "AbrirSobre";
            this.AbrirSobre.Size = new System.Drawing.Size(158, 46);
            this.AbrirSobre.TabIndex = 32;
            this.AbrirSobre.Text = "Abrir Sobre";
            this.AbrirSobre.UseVisualStyleBackColor = true;
            this.AbrirSobre.Click += new System.EventHandler(this.AbrirSobre_Click);
            // 
            // iniciarSesionBox
            // 
            this.iniciarSesionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iniciarSesionBox.BackColor = System.Drawing.Color.Transparent;
            this.iniciarSesionBox.BorderColor = System.Drawing.Color.White;
            this.iniciarSesionBox.Controls.Add(this.SignIn);
            this.iniciarSesionBox.Location = new System.Drawing.Point(97, 549);
            this.iniciarSesionBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.iniciarSesionBox.MaximumSize = new System.Drawing.Size(800, 59);
            this.iniciarSesionBox.MinimumSize = new System.Drawing.Size(592, 59);
            this.iniciarSesionBox.Name = "iniciarSesionBox";
            this.iniciarSesionBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.iniciarSesionBox.Size = new System.Drawing.Size(592, 59);
            this.iniciarSesionBox.TabIndex = 22;
            this.iniciarSesionBox.TabStop = false;
            // 
            // SignIn
            // 
            this.SignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SignIn.FlatAppearance.BorderSize = 0;
            this.SignIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignIn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignIn.ForeColor = System.Drawing.Color.White;
            this.SignIn.Location = new System.Drawing.Point(16, 10);
            this.SignIn.Margin = new System.Windows.Forms.Padding(0);
            this.SignIn.MaximumSize = new System.Drawing.Size(750, 48);
            this.SignIn.MinimumSize = new System.Drawing.Size(559, 48);
            this.SignIn.Name = "SignIn";
            this.SignIn.Size = new System.Drawing.Size(559, 48);
            this.SignIn.TabIndex = 11;
            this.SignIn.Text = "INICIAR SESION";
            this.SignIn.UseVisualStyleBackColor = false;
            this.SignIn.Click += new System.EventHandler(this.SignIn_Click);
            this.SignIn.MouseEnter += new System.EventHandler(this.SignIn_MouseEnter);
            this.SignIn.MouseLeave += new System.EventHandler(this.SignIn_MouseLeave);
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
            this.repiteContraBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repiteContraBox.BorderColor = System.Drawing.Color.White;
            this.repiteContraBox.Controls.Add(this.repiteContra);
            this.repiteContraBox.ForeColor = System.Drawing.Color.White;
            this.repiteContraBox.Location = new System.Drawing.Point(97, 404);
            this.repiteContraBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.repiteContraBox.MaximumSize = new System.Drawing.Size(800, 78);
            this.repiteContraBox.MinimumSize = new System.Drawing.Size(592, 78);
            this.repiteContraBox.Name = "repiteContraBox";
            this.repiteContraBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.repiteContraBox.Size = new System.Drawing.Size(592, 78);
            this.repiteContraBox.TabIndex = 22;
            this.repiteContraBox.TabStop = false;
            this.repiteContraBox.Visible = false;
            // 
            // repiteContra
            // 
            this.repiteContra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repiteContra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.repiteContra.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.repiteContra.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repiteContra.ForeColor = System.Drawing.Color.DimGray;
            this.repiteContra.Location = new System.Drawing.Point(20, 29);
            this.repiteContra.Margin = new System.Windows.Forms.Padding(6);
            this.repiteContra.MaximumSize = new System.Drawing.Size(750, 33);
            this.repiteContra.MinimumSize = new System.Drawing.Size(549, 33);
            this.repiteContra.Name = "repiteContra";
            this.repiteContra.Size = new System.Drawing.Size(549, 33);
            this.repiteContra.TabIndex = 10;
            this.repiteContra.Text = "Repite la contraseña";
            this.repiteContra.Enter += new System.EventHandler(this.repiteContra_Enter);
            this.repiteContra.Leave += new System.EventHandler(this.repiteContra_Leave);
            // 
            // usuarioBox
            // 
            this.usuarioBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usuarioBox.BorderColor = System.Drawing.Color.White;
            this.usuarioBox.Controls.Add(this.textUsu);
            this.usuarioBox.ForeColor = System.Drawing.Color.White;
            this.usuarioBox.Location = new System.Drawing.Point(97, 184);
            this.usuarioBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.usuarioBox.MaximumSize = new System.Drawing.Size(800, 78);
            this.usuarioBox.MinimumSize = new System.Drawing.Size(592, 78);
            this.usuarioBox.Name = "usuarioBox";
            this.usuarioBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.usuarioBox.Size = new System.Drawing.Size(592, 78);
            this.usuarioBox.TabIndex = 20;
            this.usuarioBox.TabStop = false;
            // 
            // textUsu
            // 
            this.textUsu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUsu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textUsu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textUsu.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textUsu.ForeColor = System.Drawing.Color.DimGray;
            this.textUsu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textUsu.Location = new System.Drawing.Point(20, 29);
            this.textUsu.Margin = new System.Windows.Forms.Padding(6);
            this.textUsu.MaximumSize = new System.Drawing.Size(750, 33);
            this.textUsu.MinimumSize = new System.Drawing.Size(549, 33);
            this.textUsu.Name = "textUsu";
            this.textUsu.Size = new System.Drawing.Size(549, 33);
            this.textUsu.TabIndex = 9;
            this.textUsu.Text = "Usuario";
            this.textUsu.Enter += new System.EventHandler(this.textUsu_Enter);
            this.textUsu.Leave += new System.EventHandler(this.textUsu_Leave);
            // 
            // contraseñaBox
            // 
            this.contraseñaBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contraseñaBox.BorderColor = System.Drawing.Color.White;
            this.contraseñaBox.Controls.Add(this.textContra);
            this.contraseñaBox.ForeColor = System.Drawing.Color.White;
            this.contraseñaBox.Location = new System.Drawing.Point(97, 294);
            this.contraseñaBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contraseñaBox.MaximumSize = new System.Drawing.Size(800, 78);
            this.contraseñaBox.MinimumSize = new System.Drawing.Size(592, 78);
            this.contraseñaBox.Name = "contraseñaBox";
            this.contraseñaBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contraseñaBox.Size = new System.Drawing.Size(592, 78);
            this.contraseñaBox.TabIndex = 21;
            this.contraseñaBox.TabStop = false;
            // 
            // textContra
            // 
            this.textContra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textContra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.textContra.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textContra.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textContra.ForeColor = System.Drawing.Color.DimGray;
            this.textContra.Location = new System.Drawing.Point(20, 29);
            this.textContra.Margin = new System.Windows.Forms.Padding(6);
            this.textContra.MaximumSize = new System.Drawing.Size(750, 33);
            this.textContra.MinimumSize = new System.Drawing.Size(549, 33);
            this.textContra.Name = "textContra";
            this.textContra.Size = new System.Drawing.Size(549, 33);
            this.textContra.TabIndex = 10;
            this.textContra.Text = "Contraseña";
            this.textContra.Enter += new System.EventHandler(this.textContra_Enter);
            this.textContra.Leave += new System.EventHandler(this.textContra_Leave);
            // 
            // registroBox
            // 
            this.registroBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.registroBox.BackColor = System.Drawing.Color.Transparent;
            this.registroBox.BorderColor = System.Drawing.Color.White;
            this.registroBox.Controls.Add(this.SignUp);
            this.registroBox.Location = new System.Drawing.Point(97, 549);
            this.registroBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.registroBox.MaximumSize = new System.Drawing.Size(800, 59);
            this.registroBox.MinimumSize = new System.Drawing.Size(592, 59);
            this.registroBox.Name = "registroBox";
            this.registroBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.registroBox.Size = new System.Drawing.Size(592, 59);
            this.registroBox.TabIndex = 23;
            this.registroBox.TabStop = false;
            // 
            // SignUp
            // 
            this.SignUp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SignUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignUp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUp.ForeColor = System.Drawing.Color.White;
            this.SignUp.Location = new System.Drawing.Point(7, 9);
            this.SignUp.Margin = new System.Windows.Forms.Padding(6);
            this.SignUp.MaximumSize = new System.Drawing.Size(750, 48);
            this.SignUp.MinimumSize = new System.Drawing.Size(559, 48);
            this.SignUp.Name = "SignUp";
            this.SignUp.Size = new System.Drawing.Size(559, 48);
            this.SignUp.TabIndex = 11;
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
            this.ClientSize = new System.Drawing.Size(1490, 799);
            this.Controls.Add(this.AbrirSobre);
            this.Controls.Add(this.iniciarSesionBox);
            this.Controls.Add(this.Cerrar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.salirJuegoBox);
            this.Controls.Add(this.combatirBox);
            this.Controls.Add(this.nuevaPartidaBox);
            this.Controls.Add(this.cargarPartidaBox);
            this.Controls.Add(this.fondoPokemon);
            this.Controls.Add(this.repiteContraBox);
            this.Controls.Add(this.aunNoCuenta);
            this.Controls.Add(this.circuloServidor);
            this.Controls.Add(this.usuarioBox);
            this.Controls.Add(this.contraseñaBox);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.registroBox);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Form1Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.fondoPokemon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circuloServidor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cerrar)).EndInit();
            this.iniciarSesionBox.ResumeLayout(false);
            this.salirJuegoBox.ResumeLayout(false);
            this.combatirBox.ResumeLayout(false);
            this.nuevaPartidaBox.ResumeLayout(false);
            this.cargarPartidaBox.ResumeLayout(false);
            this.repiteContraBox.ResumeLayout(false);
            this.repiteContraBox.PerformLayout();
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
        private System.Windows.Forms.TextBox textUsu;
        private System.Windows.Forms.TextBox textContra;
        private System.Windows.Forms.Button SignIn;
        private System.Windows.Forms.Button SignUp;
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
        private System.Windows.Forms.PictureBox Cerrar;
        private System.Windows.Forms.Button AbrirSobre;
    }
}


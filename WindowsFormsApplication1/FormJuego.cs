using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsApplication1
{
    public partial class FormJuego : Form
    {
       
        System.Windows.Forms.Timer gameLoop = new System.Windows.Forms.Timer();
        Conectados conectados = new Conectados();
        List<Conectados> listaConectados = new List<Conectados>();
        
        Mapa mapa;
        Mapa minimapa;
        Jugador jugador;
        public Menu menu = new Menu();
  
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        bool up, down, left, right;
        float camX = 0, camY = 0;
        int tileSize = 256;
        int miniTileSize = 6;
        int vistaAncho = 5;
        int vistaAlto = 4;
        private float lastPosX = -1;
        private float lastPosY = -1;
        TextBox mensajeChat = new TextBox();
        RichTextBox historialMensajes = new RichTextBox();
        PanelDobleBuffer panelChat = new PanelDobleBuffer();
        PanelDobleBuffer botonCerrarSesion = new PanelDobleBuffer();

        PanelDobleBuffer panelMapa = new PanelDobleBuffer();
        PanelDobleBuffer panelMinimapa = new PanelDobleBuffer();
        PictureBox btnMenu = new PictureBox();
        PanelDobleBuffer contenedorMenu = new PanelDobleBuffer();
        PanelDobleBuffer panelMenu = new PanelDobleBuffer();
        PanelDobleBuffer panelAmigos = new PanelDobleBuffer();

        Dictionary<int, Jugador> jugadoresRemotos = new Dictionary<int, Jugador>();

        public string user { get; set; }
        public Socket server { get; set; }
        public int idPartida { get; set; }
        public int userId { get; set; }




        public FormJuego()
        {
            this.Text = "Mapa con cámara";
            this.ClientSize = new Size(vistaAncho * tileSize, vistaAlto * tileSize);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;               
            this.MinimizeBox = true;                            

            this.SizeChanged += FormJuego_SizeChanged;
            this.FormClosing += new FormClosingEventHandler(FormJuego_FormClosing);
            this.Shown += FormJuego_Shown;


            panelMapa.Dock = DockStyle.Fill;
            panelMapa.Paint += PanelMapa_Paint;
            this.Controls.Add(panelMapa);

            mapa = new Mapa(tileSize);
            
            minimapa = new Mapa(miniTileSize);
            jugador = new Jugador(256, 256); // posición inicial en pixeles

            // Ajusta el tamaño del panel al contenido real
            panelMinimapa.Width = mapa.ancho * miniTileSize;
            panelMinimapa.Height = mapa.alto * miniTileSize;

            panelMinimapa.BackColor = Color.FromArgb(22, 22, 22);
            panelMinimapa.Location = new Point(10, this.ClientSize.Height - panelMinimapa.Height - 10);
            panelMinimapa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panelMinimapa.Paint += PanelMinimapa_Paint;
            

            redondearPanel(panelMinimapa, 10);  
            this.Controls.Add(panelMinimapa);
            panelMinimapa.BringToFront();
           

            this.KeyDown += FormJuego_KeyDown;
            this.KeyUp += FormJuego_KeyUp;

            // Para optimizar el renderizado y evitar parpadeos
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            gameLoop.Interval = 16;
            gameLoop.Tick += GameLoop_Tick;
            gameLoop.Start();

            

            IniciarEnvioPeriodico();
            




            //PictureBox salir = new PictureBox
            //{
            //    Size = new Size(40, 40), // Ajusta el tamaño según sea necesario
            //    Location = new Point(1450, 45),
            //    SizeMode = PictureBoxSizeMode.StretchImage // Opcional: para ajustar la imagen al tamaño del control
            //};
            //string salir_image = Path.Combine(directorioBase, "Resources", "cerrar.png");
            //salir.Image = Image.FromFile(salir_image);
            //this.Controls.Add(salir);
            //salir.BringToFront();
            //salir.Visible = true;
            //salir.Click += salir_Click;

            //this.FormBorderStyle = FormBorderStyle.None; // Quitar la barra de título y botones
            //this.WindowState = FormWindowState.Maximized; // Maximizar el formulario
            //this.ControlBox = false; //Quitar los controles 
            //this.StartPosition = FormStartPosition.CenterScreen; //Centrar el formulario
            //this.ShowInTaskbar = false; // Esconder la taskbar
            //this.FormBorderStyle = FormBorderStyle.None; //Quitar el borderstyle


            //PictureBox max_min = new PictureBox //maximizar minimizar
            //{
            //    Size = new Size(40, 40), // Ajusta el tamaño según sea necesario
            //    Location = new Point(1450, 45),
            //    SizeMode = PictureBoxSizeMode.StretchImage // Opcional: para ajustar la imagen al tamaño del control
            //};
            //string salir_image = Path.Combine(directorioBase, "Resources", "cerrar.png");
            //salir.Image = Image.FromFile(salir_image);
            //this.Controls.Add(salir);
            //salir.BringToFront();
            //salir.Visible = true;
            //salir.Click += salir_Click;

            //this.FormBorderStyle = FormBorderStyle.None; // Quitar la barra de título y botones
            //this.WindowState = FormWindowState.Maximized; // Maximizar el formulario
            //this.ControlBox = false; //Quitar los controles 
            //this.StartPosition = FormStartPosition.CenterScreen; //Centrar el formulario
            //this.ShowInTaskbar = false; // Esconder la taskbar
            //this.FormBorderStyle = FormBorderStyle.None; //Quitar el borderstyle
        }
        private void FormJuego_Shown(object sender, EventArgs e)
        {
            menu.CrearMenu(true, this, server, user);
        }


        public void ActualizarJugadorRemoto(int idJugador, float x, float y)
        {
            // Si es el jugador local, no actualizamos nada
            if (idJugador == userId)
                return;

            if (jugadoresRemotos.ContainsKey(idJugador))
            {
                // Actualiza el movimiento remoto del jugador
                jugadoresRemotos[idJugador].ActualizarMovimientoRemoto(x, y);

                // Actualiza la posición del jugador remoto
                jugadoresRemotos[idJugador].x = x;
                jugadoresRemotos[idJugador].y = y;
                
            }
            else
            {
                Jugador nuevo = new Jugador(x, y); 
                jugadoresRemotos.Add(idJugador, nuevo);
            }

           
        }

        private void redondearPanel(Panel panel, int radio)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(panel.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(panel.Width - radio, panel.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, panel.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            panel.Region = new Region(path);
        }

        public void ActualizarListaConectados(List<Conectados> listaconectados)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => ActualizarListaConectados(listaconectados)));
                return;
            }
            conectados.DibujarConectadosEnLista(listaconectados, panelAmigos, this, user, server, panelAmigos.Width, panelAmigos.Height, idPartida);
            // Actualiza la lista de conectados
            listaConectados.Clear();
            listaConectados = listaconectados;


        }

        private void FormJuego_SizeChanged(object sender, EventArgs e)
        {
            AjustarTamaño();
        }

        private void AjustarTamaño()
        {
            menu.AjustarPanelMenu();
            if (this.WindowState == FormWindowState.Maximized)
            {
                vistaAlto = 5;
                vistaAncho = 9;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                vistaAlto = 4;
                vistaAncho = 5;
            }
        }

        private void FormJuego_FormClosing(object sender, EventArgs e)
        {
            
            DetenerEnvio();

        }
        void DetenerEnvio()
        {
            if (timerEnviarCoords != null)
            {
                timerEnviarCoords.Stop();
                timerEnviarCoords.Dispose();
                timerEnviarCoords = null;
            }
        }
        private void FormJuego_FormJuegoClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Juego cerrado incorrectamente");
        }



        private void FormJuego_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) up = true;
            if (e.KeyCode == Keys.S) down = true;
            if (e.KeyCode == Keys.A) left = true;
            if (e.KeyCode == Keys.D) right = true;
        }

        private void FormJuego_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) up = false;
            if (e.KeyCode == Keys.S) down = false;
            if (e.KeyCode == Keys.A) left = false;
            if (e.KeyCode == Keys.D) right = false;
        }

        private void GameLoop_Tick(object sender, EventArgs e)
        {
            float dx = 0, dy = 0;
            if (up) dy -= 1;
            if (down) dy += 1;
            if (left) dx -= 1;
            if (right) dx += 1;

            jugador.Mover(dx, dy, mapa);
            foreach (var jugadorRemoto in jugadoresRemotos.Values)
            {
                jugadorRemoto.ComprobarEstadoMovimiento();
            }
            ActualizarCamara();
            panelMapa.Invalidate();
            panelMinimapa.Invalidate();

            //93/idJugador/idPartida/posX/posY
        }

        System.Timers.Timer timerEnviarCoords;

        void IniciarEnvioPeriodico()
        {
            timerEnviarCoords = new System.Timers.Timer(100); // Cada 0.1 segundos
            timerEnviarCoords.Elapsed += EnviarCoordenadas;
            timerEnviarCoords.AutoReset = true;
            timerEnviarCoords.Enabled = true;
        }

        void EnviarCoordenadas(object sender, ElapsedEventArgs e)
        {
            float deltaX = Math.Abs(jugador.x - lastPosX);
            float deltaY = Math.Abs(jugador.y - lastPosY);

            if (deltaX > 0.1f || deltaY > 0.1f)
            {
                string mensaje = $"93/{idPartida}/{jugador.x}/{jugador.y}";

                try
                {
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    // Actualiza la última posición enviada
                    lastPosX = jugador.x;
                    lastPosY = jugador.y;
                }
                catch (Exception ex)
                {
                    // Opcional: log o manejo de error
                }
            }
            // Si no cambia, no haces nada (no envías nada)
        }


        private void ActualizarCamara()
        {
            camX = jugador.x - (vistaAncho * tileSize) / 2;
            camY = jugador.y - (vistaAlto * tileSize) / 2;

            // Redondea a números enteros
            camX = (float)Math.Round(Math.Max(0, Math.Min(camX, mapa.ancho * tileSize - vistaAncho * tileSize)));
            camY = (float)Math.Round(Math.Max(0, Math.Min(camY, mapa.alto * tileSize - vistaAlto * tileSize)));
        }

        private void PanelMapa_Paint(object sender, PaintEventArgs e)
        {
            mapa.Dibujar(e.Graphics, camX, camY, tileSize, vistaAncho, vistaAlto);
            jugador.Dibujar(e.Graphics, camX, camY, tileSize);

            foreach (var jugadorRemoto in jugadoresRemotos.Values)
            {
                jugadorRemoto.Dibujar(e.Graphics, camX, camY, tileSize, Brushes.Blue); // dibuja con otro color

            }
        }
        private void PanelMinimapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(22, 22, 22)); // mismo color que tu fondo


            // Dibujar todo el mapa a escala
            minimapa.Dibujar(g, 0, 0, miniTileSize, panelMinimapa.Width, panelMinimapa.Height);
            

            // Dibujar jugador en el minimapa (escala directa)
            int jugadorMiniX = (int)(jugador.x / tileSize * miniTileSize); // Conversión explícita de float a int
            int jugadorMiniY = (int)(jugador.y / tileSize * miniTileSize); // Conversión explícita de float a int
            jugador.DibujarEnMinimapa(g, jugadorMiniX, jugadorMiniY, miniTileSize);

            // Dibujar rectángulo de vista del mapa grande en el minimapa
            int vistaMiniX = (int)(camX / tileSize * miniTileSize); // Conversión explícita de float a int
            int vistaMiniY = (int)(camY / tileSize * miniTileSize); // Conversión explícita de float a int
            int vistaMiniAncho = vistaAncho * miniTileSize;
            int vistaMiniAlto = vistaAlto * miniTileSize;

            using (Pen pen = new Pen(Color.Red, 2))
            {
                g.DrawRectangle(pen, vistaMiniX, vistaMiniY, vistaMiniAncho, vistaMiniAlto);
            }

            foreach (var jugadorRemoto in jugadoresRemotos.Values)
            {
                int remX = (int)(jugadorRemoto.x / tileSize * miniTileSize);
                int remY = (int)(jugadorRemoto.y / tileSize * miniTileSize);
                jugador.DibujarEnMinimapa(g, remX, remY, miniTileSize, Brushes.Blue);
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (mapa != null) mapa.Dispose();
            base.OnFormClosing(e);
        }
    }
}

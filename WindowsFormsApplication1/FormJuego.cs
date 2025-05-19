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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsApplication1
{
    public partial class FormJuego : Form
    {
        Timer gameLoop = new Timer();
        Conectados conectados = new Conectados();
        List<Conectados> listaConectados = new List<Conectados>();
        string user;
        Socket server;
        Mapa mapa;
        Mapa minimapa;
        Jugador jugador;
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        bool up, down, left, right;
        float camX = 0, camY = 0;
        int tileSize = 256;
        int miniTileSize = 6;
        int vistaAncho = 5;
        int vistaAlto = 4;

        PanelDobleBuffer panelMapa = new PanelDobleBuffer();
        PanelDobleBuffer panelMinimapa = new PanelDobleBuffer();
        PictureBox invitar = new PictureBox();
        PanelDobleBuffer panelInvitar = new PanelDobleBuffer();
        PanelDobleBuffer panelAmigos = new PanelDobleBuffer();   


        public FormJuego(string user, Socket server)
        {
            this.Text = "Mapa con cámara";
            this.ClientSize = new Size(vistaAncho * tileSize, vistaAlto * tileSize);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;               
            this.MinimizeBox = true;                            

            this.Resize += new EventHandler(FormJuego_Resize);
            this.user = user;
            this.server = server;
            


            panelInvitar.Size = new Size(48, 48);
            panelInvitar.Location = new Point(this.ClientSize.Width - panelInvitar.Width-10, 10);
            panelInvitar.BackColor = Color.FromArgb(22,22,22);
            panelInvitar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            redondearPanel(panelInvitar, 10);


            invitar.Size = new Size(48, 48);
            invitar.Image = Image.FromFile(directorioBase + "/Resources/images/menuIcono.png");
            invitar.SizeMode = PictureBoxSizeMode.Zoom;
            invitar.BackColor = Color.Transparent;

            panelInvitar.Controls.Add(invitar);
            panelInvitar.BringToFront();
            this.Controls.Add(panelInvitar);
            AsignarEventoClick(panelInvitar);
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
            panelMinimapa.Location = new Point(this.ClientSize.Width - panelMinimapa.Width-10, this.ClientSize.Height - panelMinimapa.Height - 10);
            panelMinimapa.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelMinimapa.Paint += PanelMinimapa_Paint;
            

            redondearPanel(panelMinimapa, 10);  
            this.Controls.Add(panelMinimapa);
            panelMinimapa.BringToFront();

            
            

            this.KeyDown += FormJuego_KeyDown;
            this.KeyUp += FormJuego_KeyUp;
            this.DoubleBuffered = true;

            
            
            gameLoop.Interval = 33;
            gameLoop.Tick += GameLoop_Tick;
            gameLoop.Start();

            panelAmigos.Size = new Size(200, 300);
            panelAmigos.Location = new Point(this.ClientSize.Width - panelAmigos.Width - 10, panelInvitar.Bottom + 10);
            panelAmigos.BackColor = Color.FromArgb(22, 22, 22);
            redondearPanel(panelAmigos, 10);
            panelAmigos.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.Controls.Add(panelAmigos); 
            panelAmigos.BringToFront();
            panelAmigos.Visible = false;




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
            conectados.DibujarConectadosEnLista(listaconectados, panelAmigos, this, user, server, panelAmigos.Width, panelAmigos.Height);
            // Actualiza la lista de conectados
            listaConectados.Clear();
            listaConectados = listaconectados;


        }

        private void FormJuego_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                // Redibuja o escala los controles cuando se maximiza
                vistaAlto = 5;
                vistaAncho = 9;
                this.BeginInvoke((MethodInvoker)delegate {
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                });
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                // Redibuja o escala los controles cuando se restaura
                vistaAlto = 4;
                vistaAncho = 5;


                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                });
            }
        }

        void AsignarEventoClick(Control contenedor)
        {
            contenedor.Click += InvitarClick;

            foreach (Control hijo in contenedor.Controls)
            {
                AsignarEventoClick(hijo); // Aplica recursivamente
            }
        }
        private void InvitarClick(object sender, EventArgs e)
        {
            if (panelAmigos.Visible == true)
            {
                panelAmigos.Visible = false;
            }
            else
            {
                panelAmigos.Visible = true;
            }
        }


        private void salir_Click(object sender, EventArgs e)
        {
            this.Close();
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
            ActualizarCamara();
            panelMapa.Invalidate();
            panelMinimapa.Invalidate();

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
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (mapa != null) mapa.Dispose();
            base.OnFormClosing(e);
        }
    }
}

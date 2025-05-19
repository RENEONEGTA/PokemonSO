using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormJuego : Form
    {
        Timer gameLoop = new Timer();
        Mapa mapa;
        Mapa minimapa;
        Jugador jugador;
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        bool up, down, left, right;
        float camX = 0, camY = 0;
        int tileSize = 256;
        int miniTileSize = 16;
        int vistaAncho = 5;
        int vistaAlto = 4;


        int minimapaAncho = 256;
        int minimapaAlto = 256;
       


        PanelDobleBuffer panelMapa = new PanelDobleBuffer();
        PanelDobleBuffer panelMinimapa = new PanelDobleBuffer();



        public FormJuego()
        {
            this.Text = "Mapa con cámara";
            this.ClientSize = new Size(vistaAncho * tileSize, vistaAlto * tileSize);

            panelMapa.Dock = DockStyle.Fill;
            panelMapa.Paint += PanelMapa_Paint;
            this.Controls.Add(panelMapa);
            
            panelMinimapa.Width = minimapaAncho;
            panelMinimapa.Height = minimapaAlto;
            panelMinimapa.Location = new Point(this.Width - panelMinimapa.Width-50, this.Height - panelMinimapa.Height - 50);
            panelMinimapa.Paint += PanelMinimapa_Paint;
            this.Controls.Add(panelMinimapa);
            panelMinimapa.BringToFront();

            
            

            this.KeyDown += FormJuego_KeyDown;
            this.KeyUp += FormJuego_KeyUp;
            this.DoubleBuffered = true;

            mapa = new Mapa(tileSize);
            miniTileSize = minimapaAlto / mapa.alto;
            minimapa = new Mapa(miniTileSize);
            jugador = new Jugador(256, 256); // posición inicial en pixeles
            
            gameLoop.Interval = 33;
            gameLoop.Tick += GameLoop_Tick;
            gameLoop.Start();

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

            // Dibujar todo el mapa a escala
            minimapa.Dibujar(g, 0, 0, miniTileSize, minimapaAncho, minimapaAlto);
            

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

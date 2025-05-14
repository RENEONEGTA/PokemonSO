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
        Jugador jugador;
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        bool up, down, left, right;
        float camX = 0, camY = 0;
        int tileSize = 256;
        int vistaAncho = 5;
        int vistaAlto = 4;

        PanelDobleBuffer panelMapa = new PanelDobleBuffer();
        
        

        public FormJuego()
        {
            this.Text = "Mapa con cámara";
            this.ClientSize = new Size(vistaAncho * tileSize, vistaAlto * tileSize);

            panelMapa.Dock = DockStyle.Fill;
            panelMapa.Paint += PanelMapa_Paint;
            this.Controls.Add(panelMapa);

            this.KeyDown += FormJuego_KeyDown;
            this.KeyUp += FormJuego_KeyUp;
            this.DoubleBuffered = true;

            mapa = new Mapa();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (mapa != null) mapa.Dispose();
            base.OnFormClosing(e);
        }
    }
}

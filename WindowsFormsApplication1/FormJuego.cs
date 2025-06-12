using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public class PokemonEnMapa
        {
            public int PokedexId { get; set; }
            public float X { get; set; } // Posición actual
            public float Y { get; set; } // Posición actual
            public float TargetX { get; set; } // Posición de destino
            public float TargetY { get; set; } // Posición de destino
            public Image Sprite { get; set; }
        }

        Dictionary<int, PokemonEnMapa> pokemonesEnElMapa = new Dictionary<int, PokemonEnMapa>();

        System.Windows.Forms.Timer gameLoop = new System.Windows.Forms.Timer();
        Conectados conectados = new Conectados();
        List<Conectados> listaConectados = new List<Conectados>();
        public List<Pokemon> listaPokedex { get; set; }

        Mapa mapa;
        Mapa minimapa;
        Jugador jugador;
        GestorCratas gestorCartas = new GestorCratas();
        public Menu menu = new Menu();
  
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        bool up, down, left, right;
        float camX = 0, camY = 0;
        int tileSize = 256;
        int miniTileSize = 6;
        int vistaAncho = 5;
        int vistaAlto = 4;
        bool isIluminated = false; // Variable para controlar si el botón está iluminado o no
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

        PanelDobleBuffer panelInferior = new PanelDobleBuffer();
        PanelDobleBuffer btnPokedex = new PanelDobleBuffer();
        PanelDobleBuffer btnMochila = new PanelDobleBuffer();
        PanelDobleBuffer panelElegirPokemon = new PanelDobleBuffer();
        PictureBox btnMisPokemon = new PictureBox();
        public PanelDobleBuffer panelCartas = new PanelDobleBuffer();
        int panelCartasTop;
        bool panelCreado = false;

        
        private bool panelPokedexCreado = false;
        private bool pokedexVisible = false;
        

        Dictionary<int, Jugador> jugadoresRemotos = new Dictionary<int, Jugador>();

        public string user { get; set; }
        public Socket server { get; set; }
        public int idPartida { get; set; }
        public int userId { get; set; }
        public Socket server2 { get; set; }




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
            CrearBotonesInferiores();
        }
        private void FormJuego_Shown(object sender, EventArgs e)
        {
            menu.CrearMenu(true, this, server, user);
            
            // AÑADE la suscripción a los eventos del menú
            menu.AnimationStarted += Menu_AnimationStarted;
            menu.AnimationFinished += Menu_AnimationFinished;
        }

        private void Menu_AnimationStarted()
        {
            gameLoop.Stop(); // Pausa el bucle del juego
        }

        private void Menu_AnimationFinished()
        {
            gameLoop.Start(); // Reanuda el bucle del juego
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
            conectados.DibujarConectadosEnLista(listaconectados, menu.panelAmigos, this, user, server, panelAmigos.Width, panelAmigos.Height, idPartida);
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
            // --- Interaccion para capturar pokemons ---
            if (e.KeyCode == Keys.E)
            {
                PokemonEnMapa pokemonMasCercano = null;
                float distanciaMinima = float.MaxValue;

                // Buscamos el Pokémon más cercano al jugador
                foreach (var pokemonEntry in pokemonesEnElMapa)
                {
                    float dx = pokemonEntry.Value.X - jugador.x;
                    float dy = pokemonEntry.Value.Y - jugador.y;
                    float distancia = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        pokemonMasCercano = pokemonEntry.Value;
                    }
                }

                // Comprobamos si el Pokémon más cercano está dentro del rango de interacción
                float rangoDeInteraccion = 50.0f; // 50 píxeles de distancia
                if (pokemonMasCercano != null && distanciaMinima <= rangoDeInteraccion)
                {
                    Console.WriteLine($"Intentando interactuar con Pokémon ID de instancia: {pokemonMasCercano.PokedexId}"); // Usamos PokedexId para el log

                    // Si está en rango, notificamos al servidor
                    NotificarInteraccionAlServidor(pokemonMasCercano);
                }
            }
        }
        private void NotificarInteraccionAlServidor(PokemonEnMapa pokemon)
        {
            if (server2 == null || !server2.Connected) return;

            // Buscamos la clave (instancia_id) del pokmon en el diccionario
            var entry = pokemonesEnElMapa.FirstOrDefault(kvp => kvp.Value == pokemon);
            if (entry.Key == 0) return; // No se encontró la clave

            int instanciaId = entry.Key;

            // Enviamos el nuevo mensaje 98 con el ID del Pokemon
            string mensaje = $"98/{instanciaId}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server2.Send(msg);
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

            foreach (var pokemon in pokemonesEnElMapa.Values)
            {
                // Calcula la distancia vectorial hacia el punto de destino
                float dxp = pokemon.TargetX - pokemon.X;
                float dyp = pokemon.TargetY - pokemon.Y;
                float distancia = (float)Math.Sqrt(dxp * dxp + dyp * dyp);
                float velocidadPokemon = 1.0f;

                if (velocidadPokemon < distancia)
                {
                    pokemon.X += (dxp / distancia) * velocidadPokemon;
                    pokemon.Y += (dyp / distancia) * velocidadPokemon;
                }
                else
                {
                    // Si la velocidad es mayor que la distancia, nos colocamos directamente en el destino
                    // para evitar pasarnos o vibrar.
                    pokemon.X = pokemon.TargetX;
                    pokemon.Y = pokemon.TargetY;
                }
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
                    server2.Send(msg);

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

            // DIBUJAR LOS POKÉMON DEL MAPA
            foreach (var pokemonEntry in pokemonesEnElMapa.Values)
            {
                float pantallaX = pokemonEntry.X - camX;
                float pantallaY = pokemonEntry.Y - camY;

                // Dibuja el sprite
                e.Graphics.DrawImage(pokemonEntry.Sprite, pantallaX, pantallaY, 48, 48);
            }

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

        private void CrearBotonesInferiores()
        {
            int anchoPanel = 400;
            int altoPanel = 100;

            panelInferior = new PanelDobleBuffer
            {
                Anchor = AnchorStyles.Bottom,
                Size = new Size(anchoPanel, altoPanel),
                Location = new Point((this.Width-anchoPanel)/2, this.ClientSize.Height - altoPanel -10), // Ajusta la posición del panel inferior
                BackColor = Color.FromArgb(22, 22, 22)
            };

            redondearPanel(panelInferior, 10); // Redondear bordes del panel inferior

            int altoBoton = 50;
            int anchoBoton = 100;
            btnMochila = new PanelDobleBuffer
            {
                Size = new Size(anchoBoton, altoBoton),
                Location = new Point(10, (panelInferior.Height - altoBoton) / 2),
                BackColor = Color.FromArgb(30, 170, 240)
            };
            Label lblMochila = new Label
            {
                Text = "Mochila",
                ForeColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = false, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleCenter, 
                BackColor = Color.Transparent

            };
            // Reenviamos los eventos de la etiqueta al panel que la contiene.
            
            lblMochila.MouseEnter += (sender, e) => {
                IluminarControl(btnMochila, e); 
            };
           
            lblMochila.MouseLeave += (sender, e) => {
                RestaurarControl(btnMochila, e); 
            };

            lblMochila.Click += (sender, e) => {
                BtnMochila_Click(btnMochila, e);
            };
            btnMochila.Controls.Add(lblMochila);

            redondearPanel(btnMochila, 10); 
            panelInferior.Controls.Add(btnMochila); // Añadir el botón Mochila al panel inferior

            btnPokedex = new PanelDobleBuffer
            {
                Size = new Size(anchoBoton, altoBoton),
                Location = new Point(panelInferior.Width-anchoBoton-10, (panelInferior.Height - altoBoton) / 2),
                BackColor = Color.FromArgb(30, 170, 240)
            };
            Label lblPokedex = new Label
            {
                Text = "Pokédex",
                ForeColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Reenviamos los eventos de la etiqueta al panel que la contiene.
            lblPokedex.MouseEnter += (sender, e) =>{
                IluminarControl(btnPokedex, e);
            };
            lblPokedex.MouseLeave += (sender, e) =>{
                RestaurarControl(btnPokedex, e);
            };
            lblPokedex.Click += (sender, e) =>{
                BtnPokedex_Click(btnPokedex, e);
            };

            btnPokedex.Controls.Add(lblPokedex); // Añadir etiqueta al botón Pokédex

            redondearPanel(btnPokedex, 10); // Redondear bordes del botón Mochila
            
            btnPokedex.MouseEnter += IluminarControl;
            btnPokedex.MouseLeave += RestaurarControl;
            btnPokedex.Click += BtnPokedex_Click;
            panelInferior.Controls.Add(btnPokedex); // Añadir el botón Pokédex al panel inferior

            btnMisPokemon = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point((panelInferior.Width - 100) / 2, (panelInferior.Height-100)/2),
                BackColor = Color.Transparent,
                Image = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "pokeball.png")),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            
            
            panelInferior.Controls.Add(btnMisPokemon);
            btnMisPokemon.BringToFront();
            btnMisPokemon.Visible = true;
            btnMisPokemon.Click += PokedexBox_Click;
            this.Controls.Add(panelInferior); // Añadir el panel inferior al formulario
            panelInferior.BringToFront(); // Asegurarse de que el panel inferior esté al frente

        }


        private void BtnPokedex_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrir Pokédex");
        }

        private void BtnMochila_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrir Mochila");
        }
        private void IluminarControl(object sender, EventArgs e)
        {
            // Convertimos 'sender' al tipo 'Control' para poder acceder a sus propiedades
            if (sender is Control control)
            {
                // Si es la primera vez que pasamos el ratón, guardamos el color original
                if (control.Tag == null)
                {
                    control.Tag = control.BackColor;
                }

                Color colorOriginal = control.BackColor;

                // Aumentamos el valor de cada componente de color (R, G, B) para aclararlo.
                // Usamos Math.Min para asegurarnos de que ningún valor supere 255.
                int r = Math.Min(colorOriginal.R + 30, 255);
                int g = Math.Min(colorOriginal.G + 30, 255);
                int b = Math.Min(colorOriginal.B + 30, 255);

                control.BackColor = Color.FromArgb(colorOriginal.A, r, g, b); 
            }
        }
        private void RestaurarControl(object sender, EventArgs e)
        {
            // Convertimos 'sender' al tipo 'Control'
            if (sender is Control control)
            {
                // Si guardamos un color original en la propiedad Tag, lo restauramos
                if (control.Tag is Color colorOriginal)
                {
                    control.BackColor = colorOriginal;
                }
            }
        }


        // Evento para desplazar los controles dentro del panel
        private void PanelCartas_MouseWheel(object sender, MouseEventArgs e)
        {
            int desplazamiento = 15; // Velocidad de desplazamiento

            // Usar BeginUpdate y EndUpdate para evitar parpadeos
            panelCartas.SuspendLayout();

            // Mover todos los controles dentro de panelCartas
            foreach (Control ctrl in panelCartas.Controls)
            {
                // Excluir los botones específicos del desplazamiento
                if (ctrl is RoundButton || ctrl is RombeButton)
                {
                    continue;
                }

                // Desplazar hacia abajo solo hasta el punto de partida
                if (e.Delta > 0 && ctrl.Top < panelCartasTop)
                {
                    ctrl.Top += desplazamiento;
                    if (ctrl.Top > panelCartasTop)
                    {
                        ctrl.Top = panelCartasTop;
                    }
                }
                // Desplazar hacia arriba
                else if (e.Delta < 0)
                {
                    ctrl.Top -= desplazamiento;
                }
            }
            panelCartas.ResumeLayout();
            panelCartas.Update(); // Forzar la redibujación del panel
        }
        public void PrimerPokemon()
        {
            bool escogerPokemon = true;
            int ancho = 770;
            int alto = 410;

            panelElegirPokemon = new PanelDobleBuffer
            {
                Size = new Size(ancho, alto),
                Location = new Point(this.Width / 2 - (ancho / 2), this.Height / 2 - (alto / 2)),
                BackColor = Color.FromArgb(44, 44, 44),
                Padding = new Padding(5)
            };
            redondearPanel(panelElegirPokemon, 20); // Método para redondear
            this.Controls.Add(panelElegirPokemon);
            panelElegirPokemon.BringToFront();
            panelElegirPokemon.Visible = true;

            List<CartaPokemon> pokemonsIniciales = new List<CartaPokemon>();
            pokemonsIniciales.Add(new CartaPokemon("Charmander", 100, "Fuego", "images/Charmander.png", new List<(string, int)> { ("Llamarada", 20) }));
            pokemonsIniciales.Add(new CartaPokemon("Squirtle", 100, "Agua", "images/Squirtle.png", new List<(string, int)> { ("Chorro de Agua", 20) }));
            pokemonsIniciales.Add(new CartaPokemon("Bulbasaur", 100, "Planta", "images/Bulbasaur.png", new List<(string, int)> { ("Hoja Afilada", 20) }));

            gestorCartas.DibujarCartas(pokemonsIniciales, panelElegirPokemon, false, escogerPokemon);

            System.Windows.Forms.Label labelElegirPokemon = new System.Windows.Forms.Label
            {
                Text = "Elige tu Pokemon Inicial",
                ForeColor = Color.FromArgb(255, 255, 255),
                Location = new Point(panelElegirPokemon.Width / 2 - 100, panelElegirPokemon.Height - 10),
                AutoSize = true,
                Font = new Font("Arial", 16, FontStyle.Bold)
            };
            panelElegirPokemon.Controls.Add(labelElegirPokemon);
            labelElegirPokemon.Location = new Point(panelElegirPokemon.Width / 2 - labelElegirPokemon.Width / 2, panelElegirPokemon.Height - labelElegirPokemon.Height - 10);
        }

        private void PokedexBox_Click(object sender, EventArgs e)
        {
            // Si el panel ya está visible, simplemente lo ocultamos
            if (pokedexVisible)
            {
                panelCartas.Visible = false;
                pokedexVisible = false;
                return;
            }

            // Si el panel no se ha creado nunca, lo creamos
            if (!panelPokedexCreado)
            {
                int PanelSizeX = 780; // Ancho del panel
                int PanelSizeY = 600; // Alto del panel
                panelCartas = new PanelDobleBuffer
                {
                    Size = new Size(PanelSizeX, PanelSizeY),
                    Location = new Point(panelInferior.Left + panelInferior.Width/2 - (PanelSizeX / 2) , panelInferior.Top - PanelSizeY - 20),
                    BackColor = Color.FromArgb(22, 22, 22),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Visible = true
                };
                // Evento para manejar el desplazamiento
                panelCartas.MouseWheel += PanelCartas_MouseWheel;
                redondearPanel(panelCartas, 20); 

                this.Controls.Add(panelCartas);
                panelCartas.BringToFront();
                panelPokedexCreado = true;
            }

            // Enviamos la petición al servidor para obtener la lista de Pokémon
            string mensaje = "3/" + user;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        
        public void MostrarPokedex(string datosPokemon)
        {
            if (panelCartas == null || this.IsDisposed) return;

            // Usamos Invoke para asegurarnos de que la actualización se hace en el hilo de la UI
            this.Invoke((MethodInvoker)delegate
            {
                List<Pokemon> listaPokemon = new List<Pokemon>();
                
                listaPokemon = Pokemon.ParsearDatos(datosPokemon, listaPokemon);

                if (listaPokemon.Count > 0)
                {
                    List<CartaPokemon> cartas = new List<CartaPokemon>();
                    foreach (Pokemon pokemon in listaPokemon)
                    {
                        cartas.Add(new CartaPokemon(
                            pokemon.Nombre,
                            pokemon.Vida,
                            pokemon.Elemento,
                            "images/" + pokemon.Nombre + ".png",
                            new List<(string, int)> { (pokemon.Ataque, pokemon.Daño) }
                        ));
                    }

                    panelCartas.Controls.Clear();
                    gestorCartas.DibujarCartas(cartas, panelCartas, true, false); 
                    panelCartas.Visible = true;
                    pokedexVisible = true;
                    panelCartas.BringToFront();
                    btnMisPokemon.BringToFront();
                }
                else
                {
                    MessageBox.Show("No tienes ningún Pokémon en tu Pokedex.");
                }
            });
        }
        public void SincronizarPokemonesEnMapa(string datosCompletos)
        {
            // Aseguramos que la operación se ejecute en el hilo de la UI
            this.Invoke((MethodInvoker)delegate
            {
                pokemonesEnElMapa.Clear(); // Limpiamos la lista por si acaso

                // Separamos cada Pokémon usando el carácter '#'
                string[] pokemonesData = datosCompletos.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pokemonData in pokemonesData)
                {
                    // Separamos los campos de cada Pokémon por '/'
                    string[] partes = pokemonData.Split('/');
                    if (partes.Length < 4) continue;

                    int instanciaId = int.Parse(partes[0]);
                    int pokedexId = int.Parse(partes[1]);
                    float x = float.Parse(partes[2]);
                    float y = float.Parse(partes[3]);

                    // Buscamos la info del pokémon en nuestra lista de Pokedex para obtener el nombre y cargar la imagen
                    Pokemon infoPokemon = listaPokedex.FirstOrDefault(p => p.Id == pokedexId);
                    if (infoPokemon == null) continue;

                    var nuevoPokemon = new PokemonEnMapa
                    {
                        PokedexId = pokedexId,
                        X = x, // Posición inicial
                        Y = y, // Posición inicial
                        TargetX = x, // El destino inicial es la misma posición
                        TargetY = y, // El destino inicial es la misma posición
                        Sprite = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", infoPokemon.Nombre + ".png"))
                    };
                    pokemonesEnElMapa[instanciaId] = nuevoPokemon;
                }
            });
        }
        public void ActualizarDestinoPokemon(string datos)
        {
            // Formato esperado: <instancia_id>/<nueva_x>/<nueva_y>
            string[] partes = datos.Split('/');
            if (partes.Length < 3) return;

            int instanciaId = int.Parse(partes[0]);
            float nuevaX = float.Parse(partes[1]);
            float nuevaY = float.Parse(partes[2]);

            this.Invoke((MethodInvoker)delegate {
                // Buscamos el pokémon en nuestro diccionario
                if (pokemonesEnElMapa.TryGetValue(instanciaId, out PokemonEnMapa pokemon))
                {
                    // Actualizamos su DESTINO
                    pokemon.TargetX = nuevaX;
                    pokemon.TargetY = nuevaY;
                }
            });
        }
        public void EliminarPokemonDelMapa(string instanciaIdStr)
        {
            try
            {
                int instanciaId = int.Parse(instanciaIdStr.Trim());

                this.Invoke((MethodInvoker)delegate {
                    if (pokemonesEnElMapa.ContainsKey(instanciaId))
                    {
                        pokemonesEnElMapa.Remove(instanciaId);
                        Console.WriteLine($"Se ha eliminado el Pokémon con ID de instancia: {instanciaId}");
                    }
                });
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error al parsear el ID de instancia para eliminar: " + ex.Message);
            }
        }
        public void AnadirPokemonAlMapa(string datos)
        {
            // Usamos Invoke para garantizar que cualquier cambio en los datos del formulario
            // se haga de forma segura desde el hilo principal.
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    string[] partes = datos.Split('/');
                    if (partes.Length < 4) return;

                    int instanciaId = int.Parse(partes[0]);
                    int pokedexId = int.Parse(partes[1]);
                    float x = float.Parse(partes[2]);
                    float y = float.Parse(partes[3]);

                    // Comprobamos si el pokémon ya existe en el diccionario, por si acaso
                    if (pokemonesEnElMapa.ContainsKey(instanciaId)) return;

                    // Buscamos la información del Pokémon en la Pokedex para obtener su nombre
                    // (Es crucial que 'listaPokedex' tenga datos)
                    Pokemon infoPokemon = listaPokedex.FirstOrDefault(p => p.Id == pokedexId);
                    if (infoPokemon == null)
                    {
                        Console.WriteLine($"Error: No se encontró Pokémon en la Pokedex con ID: {pokedexId}");
                        return;
                    }

                    // Construimos la ruta de la imagen y verificamos que existe
                    string imagePath = Path.Combine(directorioBase, "Resources", "images", infoPokemon.Nombre + ".png");
                    if (!File.Exists(imagePath))
                    {
                        Console.WriteLine($"Error: No se encontró la imagen en la ruta: {imagePath}");
                        return;
                    }

                    // Creamos el nuevo objeto para el Pokémon en el mapa
                    var nuevoPokemon = new PokemonEnMapa
                    {
                        PokedexId = pokedexId,
                        X = x,
                        Y = y,
                        TargetX = x, // El destino inicial es su propia posición de aparición
                        TargetY = y,
                        Sprite = Image.FromFile(imagePath)
                    };

                    // Finalmente, lo añadimos al diccionario para que el gameLoop y el Paint lo usen
                    pokemonesEnElMapa[instanciaId] = nuevoPokemon;
                    Console.WriteLine($"Se ha añadido a {infoPokemon.Nombre} (ID de instancia: {instanciaId}) al mapa.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error procesando AnadirPokemonAlMapa: " + ex.Message);
                }
            });
        }


        // En FormJuego.cs

        public void IniciarCombate(string datosOponente)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    // Pausamos el juego del mapa por rendimiento
                    gameLoop.Stop();

                    // buscamos al oponente usando el ID que nos dio el servidor
                    string[] partes = datosOponente.Split('/');
                    if (partes.Length < 2) return;

                    int instanciaId = int.Parse(partes[0]);
                    int pokedexId = int.Parse(partes[1]);
                    Pokemon oponente = listaPokedex.FirstOrDefault(p => p.Id == pokedexId);

                    // Buscamos el pokemon que nos dio el server y usamos elk primer pokemon en la lista del jugador(mas adelante se podria cambiar para seleccionar el pokemon a usar)
                    Pokemon jugadorPokemon = listaPokedex.FirstOrDefault();

                    //vrificamos que hemos encontrado a ambos luchadores
                    if (oponente != null && jugadorPokemon != null)
                    {
                        Combate.pantallaCombate(this, jugadorPokemon, oponente, instanciaId);
                    }
                    else
                    {
                        MessageBox.Show("Error: No se encontraron los datos de los Pokémon para el combate.");
                        gameLoop.Start(); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al iniciar el combate: " + ex.Message);
                    gameLoop.Start(); 
                }
            });
        }

        public void ResultadoDeCaptura(string resultado)
        {
            Combate.ProcesarResultadoCaptura(resultado);
        }
        public void TerminarCombate()
        {
            // Buscamos el panel de combate por su nombre
            Control panelCombate = this.Controls.Find("panelCombate", true).FirstOrDefault();
            if (panelCombate != null)
            {
                // Lo eliminamos y liberamos sus recursos
                this.Controls.Remove(panelCombate);
                panelCombate.Dispose();
            }

            // ¡La línea clave! Reanudamos el bucle del juego.
            gameLoop.Start();
        }
    }
}

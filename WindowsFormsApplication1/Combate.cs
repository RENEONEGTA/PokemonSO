using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Combate
    {
        static string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
        static GestorCratas gestorCartas = new GestorCratas();

        // --- VARIABLES ESTÁTICAS PARA GESTIONAR EL ESTADO DEL COMBATE ---
        private static Pokemon pokemonJugador;
        private static Pokemon pokemonOponente;

        private static ProgressBar vidaJugadorBarra;
        private static ProgressBar vidaOponenteBarra;
        private static Random generadorAleatorio = new Random();

        private static Label vidaJugadorLabel;
        private static Label vidaOponenteLabel;

        private static FormJuego formJuegoActivo;
        private static int instanciaIdOponente;

        private static PanelDobleBuffer panelAtaquesJugador;
        // --- FIN DE VARIABLES ---

        /// Punto de entrada para iniciar un combate contra un Pokémon salvaje.

        public static void pantallaCombate(FormJuego ventana, Pokemon jugador, Pokemon oponente, int idInstancia )
        {
            // Guardamos las referencias en nuestras variables estáticas para usarlas durante el combate
            formJuegoActivo = ventana;
            pokemonJugador = jugador;
            pokemonOponente = oponente;
            instanciaIdOponente = idInstancia;

            PanelDobleBuffer panelCombate = new PanelDobleBuffer
            {
                Name = "panelCombate",
                Size = ventana.ClientSize,
                Location = new Point(0, 0),
                BackgroundImage = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "fondo_planta.jpeg")),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            ventana.Controls.Add(panelCombate);
            panelCombate.BringToFront();

            crearUICombate(panelCombate, ventana);
        }

        /// Ejecuta el ataque seleccionado por el jugador, aplica el daño y cede el turno.
        private static void EjecutarTurnoJugador(int daño)
        {
            if (panelAtaquesJugador == null) return;

            panelAtaquesJugador.Enabled = false; // Desactivamos los botones para que no pueda atacar de nuevo

            vidaOponenteBarra.Value = Math.Max(0, vidaOponenteBarra.Value - daño);
            vidaOponenteLabel.Text = $"{vidaOponenteBarra.Value} / {vidaOponenteBarra.Maximum}";

            if (vidaOponenteBarra.Value <= 0)
            {
                FinalizarCombate(true); // El jugador ha ganado
            }
            else
            {
                // Esperamos un momento antes de que el oponente ataque
                System.Windows.Forms.Timer timerOponente = new System.Windows.Forms.Timer();
                timerOponente.Interval = 1500; // 1.5 segundos
                timerOponente.Tick += (s, e) => {
                    timerOponente.Stop();
                    EjecutarTurnoOponente();
                };
                timerOponente.Start();
            }
        }


        private static void EjecutarTurnoOponente()
        {
            if (pokemonOponente.Ataques == null || pokemonOponente.Ataques.Count == 0)
            {
                // Si el oponente no tiene ataques, no hace nada y nos devuelve el turno
                panelAtaquesJugador.Enabled = true;
                return;
            }

            // IA muy simple: elije aliatoriamente el ataque que va a hacer

            int numeroDeAtaques = pokemonOponente.Ataques.Count;
            int indiceAtaqueElegido = generadorAleatorio.Next(0, numeroDeAtaques);
            var ataqueElegido = pokemonOponente.Ataques[indiceAtaqueElegido];
            int dañoOponente = ataqueElegido.Daño;
            vidaJugadorBarra.Value = Math.Max(0, vidaJugadorBarra.Value - dañoOponente);
            vidaJugadorLabel.Text = $"{vidaJugadorBarra.Value} / {vidaJugadorBarra.Maximum}";

            if (vidaJugadorBarra.Value <= 0)
            {
                FinalizarCombate(false); // El jugador ha perdido
            }
            else
            {
                // Es el turno del jugador de nuevo
                panelAtaquesJugador.Enabled = true;
            }
        }


        //Muestra el resultado final del combate y vuelve al mapa.
        private static void FinalizarCombate(bool victoria)
        {
            string mensaje = victoria ? "¡Has ganado el combate!" : "¡Tu Pokémon ha sido derrotado!";
            MessageBox.Show(mensaje);

            formJuegoActivo.TerminarCombate();
        }

        //Construye todos los elementos visuales de la pantalla de combate.
        private static void crearUICombate(PanelDobleBuffer panelCombate, Form ventana)
        {
            // --- Panel del Jugador ---
            Panel panelJugador = new Panel { Size = new Size(500, 160), Location = new Point(20, 20), BackColor = Color.Transparent, BorderStyle = BorderStyle.None };
            panelCombate.Controls.Add(panelJugador);
            RedondearControlConEstilo(panelJugador, 10, Color.Yellow, Color.Orange, 3, true, Color.LightYellow, Color.Yellow);

            Label lblJugador = new Label { Text = pokemonJugador.Nombre, Font = new Font("Arial", 20, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true, BackColor = Color.Transparent };
            panelJugador.Controls.Add(lblJugador);

            vidaJugadorBarra = new ProgressBar { Location = new Point(200, lblJugador.Top + lblJugador.Height / 2 - 5), Size = new Size(250, 10), Maximum = pokemonJugador.Vida, Value = pokemonJugador.Vida, ForeColor = Color.Red };
            panelJugador.Controls.Add(vidaJugadorBarra);
            redondearControl(vidaJugadorBarra, 10);

            vidaJugadorLabel = new Label
            {
                Text = $"{pokemonJugador.Vida} / {pokemonJugador.Vida}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(vidaJugadorBarra.Right - 80, vidaJugadorBarra.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            panelJugador.Controls.Add(vidaJugadorLabel);

            string jugadorIconoPath = Path.Combine(directorioBase, "Resources", "images", pokemonJugador.Nombre + ".png");
            PictureBox fotoPokemonJugador = new PictureBox { Size = new Size(80, 80), Location = new Point(20, 70), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Transparent };
            if (File.Exists(jugadorIconoPath)) fotoPokemonJugador.Image = Image.FromFile(jugadorIconoPath);
            panelJugador.Controls.Add(fotoPokemonJugador);

            string jugadorSpritePath = Path.Combine(directorioBase, "Resources", "images", pokemonJugador.Nombre + "_Jugador.png");
            PictureBox pokemonJugadorSprite = new PictureBox { Size = new Size(500, 500), Location = new Point(panelCombate.Width / 2 - 500 - 150, panelCombate.Height / 2), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Transparent };
            if (File.Exists(jugadorSpritePath)) pokemonJugadorSprite.Image = Image.FromFile(jugadorSpritePath);
            panelCombate.Controls.Add(pokemonJugadorSprite);

            // --- Panel del Enemigo ---
            Panel panelEnemigo = new Panel { Size = new Size(500, 160), Location = new Point(ventana.ClientSize.Width - 520, 20), BackColor = Color.Transparent, BorderStyle = BorderStyle.None };
            panelCombate.Controls.Add(panelEnemigo);
            RedondearControlConEstilo(panelEnemigo, 10, Color.Red, Color.DarkRed, 3, true, Color.LightSalmon, Color.Red);

            Label lblEnemigo = new Label { Text = pokemonOponente.Nombre, Font = new Font("Arial", 20, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true, BackColor = Color.Transparent };
            panelEnemigo.Controls.Add(lblEnemigo);

            vidaOponenteBarra = new ProgressBar { Location = new Point(200, lblEnemigo.Top + lblEnemigo.Height / 2 - 5), Size = new Size(250, 10), Maximum = pokemonOponente.Vida, Value = pokemonOponente.Vida, ForeColor = Color.Red };
            panelEnemigo.Controls.Add(vidaOponenteBarra);
            redondearControl(vidaOponenteBarra, 10);

            vidaOponenteLabel = new Label
            {
                Text = $"{pokemonOponente.Vida} / {pokemonOponente.Vida}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(vidaOponenteBarra.Right - 80, vidaOponenteBarra.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            panelEnemigo.Controls.Add(vidaOponenteLabel);

            string enemigoIconoPath = Path.Combine(directorioBase, "Resources", "images", pokemonOponente.Nombre + ".png");
            PictureBox fotoPokemonEnemigo = new PictureBox { Size = new Size(80, 80), Location = new Point(panelEnemigo.Width - 20 - 80, 70), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Transparent };
            if (File.Exists(enemigoIconoPath)) fotoPokemonEnemigo.Image = Image.FromFile(enemigoIconoPath);
            panelEnemigo.Controls.Add(fotoPokemonEnemigo);

            string enemigoSpritePath = Path.Combine(directorioBase, "Resources", "images", pokemonOponente.Nombre + ".png");
            PictureBox pokemonEnemigoSprite = new PictureBox { Size = new Size(350, 350), Location = new Point(panelCombate.Width / 2 + 150, panelCombate.Height / 2 - 200), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Transparent };
            if (File.Exists(enemigoSpritePath)) pokemonEnemigoSprite.Image = Image.FromFile(enemigoSpritePath);
            panelCombate.Controls.Add(pokemonEnemigoSprite);

            // --- Panel de la Carta de Ataques del Jugador ---
            panelAtaquesJugador = new PanelDobleBuffer { Size = new Size(250, 350), Location = new Point((panelCombate.Width - 240) / 2, panelCombate.Height - 80 - 350), BackColor = Color.Transparent, BorderStyle = BorderStyle.None };
            CartaPokemon cartaJugador = new CartaPokemon(pokemonJugador.Nombre, pokemonJugador.Vida, pokemonJugador.Elemento, "images/" + pokemonJugador.Nombre + ".png", new List<(string, int)> { (pokemonJugador.Ataque, pokemonJugador.Daño), ("Ataque Secundario", 10) });

            gestorCartas.DibujarCartas(new List<CartaPokemon> { cartaJugador }, panelAtaquesJugador, false, false);

            int ataqueY = 175; // Posición Y inicial para los botones de ataque
            foreach (var ataque in cartaJugador.Ataques)
            {
                PanelDobleBuffer botonAtaque = new PanelDobleBuffer { Size = new Size(panelAtaquesJugador.Width - 40, 40), Location = new Point(20, ataqueY), BackColor = Color.FromArgb(150, 255, 255, 255), Cursor = Cursors.Hand };
                Label lblAtaque = new Label { Text = $"{ataque.Nombre} ({ataque.Daño})", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Bold), BackColor = Color.Transparent };

                botonAtaque.Click += (s, e) => { EjecutarTurnoJugador(ataque.Daño); };
                lblAtaque.Click += (s, e) => { EjecutarTurnoJugador(ataque.Daño); }; // También en la etiqueta por si acaso

                botonAtaque.Controls.Add(lblAtaque);
                panelAtaquesJugador.Controls.Add(botonAtaque);
                botonAtaque.BringToFront();
                ataqueY += 50;
            }
            panelCombate.Controls.Add(panelAtaquesJugador);
            panelAtaquesJugador.BringToFront();

            // --- Panel inferior con botones de acción ---
            Panel panelBotones = new Panel { Size = new Size(430, 70), Location = new Point((panelCombate.Width - 430) / 2, panelCombate.Height - 80), BorderStyle = BorderStyle.FixedSingle };
            panelCombate.Controls.Add(panelBotones);
            RedondearControlConEstilo(panelBotones, 10, Color.Blue, Color.DarkBlue, 3, true, Color.LightSkyBlue, Color.White);
            panelBotones.BringToFront();

            CrearBoton(panelBotones, "Pokémon", 10, 10, Color.LightPink, (s, e) => MessageBox.Show("Cambiar de Pokémon (no implementado)"));
            CrearBoton(panelBotones, "Capturar", 150, 10, Color.Thistle, (s, e) => IntentarCaptura());
            CrearBoton(panelBotones, "Huir", 290, 10, Color.MediumAquamarine, (s, e) => {
                if (ventana is FormJuego formJuego) { formJuego.TerminarCombate(); }
            });
        }
        public static void ProcesarResultadoCaptura(string resultado)
        {
            int exito = int.Parse(resultado);

            if (exito == 1)
            {
                MessageBox.Show($"¡Genial! ¡{pokemonOponente.Nombre} ha sido capturado!");
                FinalizarCombate(true); // Finalizamos el combate como una victoria
            }
            else
            {
                MessageBox.Show($"¡Oh, no! ¡{pokemonOponente.Nombre} se ha escapado!");
                // Si falla, es el turno del oponente
                EjecutarTurnoOponente();
            }
        }
        private static void IntentarCaptura()
        {
            // Para que el jugador no pueda hacer otra acción mientras se lanza la bola
            panelAtaquesJugador.Enabled = false;

            MessageBox.Show("¡Lanzas una Poké Ball!");

            // Obtenemos la vida actual del oponente desde su barra de vida
            int vidaActualOponente = vidaOponenteBarra.Value;

            // Enviamos el nuevo mensaje 99 al servidor con los datos necesarios
            string mensaje = $"99/{instanciaIdOponente}/{vidaActualOponente}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            formJuegoActivo.server2.Send(msg);
        }
        public static void redondearControl(Control control, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radio, radio), 180, 90);
            path.AddArc(new Rectangle(control.Width - radio, 0, radio, radio), 270, 90);
            path.AddArc(new Rectangle(control.Width - radio, control.Height - radio, radio, radio), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radio, radio, radio), 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }



        private static void CrearBoton(Panel contenedor, string texto, int x, int y, Color color, EventHandler alHacerClick)
        {
            BotonRedondeado boton = new BotonRedondeado
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(120, 50),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ColorHover = Color.LightBlue,
                ColorBorde = Color.Black,
                RadioBorde = 20,
                ColorFondoPersonalizado = color
            };

            // Asignar evento de clic
            boton.Click += alHacerClick;

            contenedor.Controls.Add(boton);
        }


        public static void RedondearControlConEstilo(Control control, int radio, Color colorBorde1, Color colorBorde2, int grosorBorde = 2, bool usarSombra = true, Color? colorFondo = null, Color? colorFondo2 = null)
        {
            control.Resize += (s, e) => control.Invalidate(); // Redibuja en cada cambio de tamaño

            control.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = control.ClientRectangle;
                GraphicsPath path = CrearPathRedondeado(rect, radio);
                control.Region = new Region(path);

                //Pintar fondo interior (degradado o sólido)
                if (colorFondo != null)
                {
                    if (colorFondo2 != null)
                    {
                        using (LinearGradientBrush fondo = new LinearGradientBrush(rect, colorFondo.Value, colorFondo2.Value, LinearGradientMode.Vertical))
                        {
                            e.Graphics.FillPath(fondo, path);
                        }
                    }
                    else
                    {
                        using (SolidBrush fondo = new SolidBrush(colorFondo.Value))
                        {
                            e.Graphics.FillPath(fondo, path);
                        }
                    }
                }

                //sombra exterior (opcional)
                if (usarSombra)
                {
                    using (GraphicsPath sombra = CrearPathRedondeado(new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4), radio))
                    using (Pen penSombra = new Pen(Color.FromArgb(50, 0, 0, 0), grosorBorde + 2))
                    {
                        e.Graphics.DrawPath(penSombra, sombra);
                    }
                }

                // borde exterior
                using (LinearGradientBrush gradiente = new LinearGradientBrush(rect, colorBorde1, colorBorde2, LinearGradientMode.ForwardDiagonal))
                using (Pen pen = new Pen(gradiente, grosorBorde))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };
        }

        private static GraphicsPath CrearPathRedondeado(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radio * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

    }
}

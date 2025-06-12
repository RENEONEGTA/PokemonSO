using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Combate
    {
        static string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
        static GestorCratas gestorCartas = new GestorCratas();
        public static void pantallaCombate(Form ventana, Conectados conectado)
        {
            // Crear panel principal con doble buffer
            PanelDobleBuffer panelCombate = new PanelDobleBuffer
            {
                Name = "panelCombate",
                Size = ventana.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.Aqua,
                BackgroundImage = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "fondo_planta.jpeg")), // Cambia la ruta a la imagen de fondo
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            // Añadir el panel a la ventana
            ventana.Controls.Add(panelCombate);
            panelCombate.BringToFront();

            // Crear UI de combate
            crearUICombate(panelCombate, ventana);
        }

        private static void crearUICombate(PanelDobleBuffer panelCombate, Form ventana)
        {
            // Panel del jugador ------------------------------------------------------------------------------------------------------
            Panel panelJugador = new Panel
            {
                Size = new Size(500, 160),
                Location = new Point(20, 20),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            
            panelCombate.Controls.Add(panelJugador);
            RedondearControlConEstilo(
                panelJugador,
                radio: 10,
                colorBorde1: Color.Yellow,
                colorBorde2: Color.Orange,
                grosorBorde: 3,
                usarSombra: true,
                colorFondo: Color.LightYellow,
                colorFondo2: Color.Yellow
            );

            // Nombre del jugador
            Label lblJugador = new Label
            {
                Text = "Pikachu",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
               
            };
            panelJugador.Controls.Add(lblJugador);

            // Vida del jugador
            int alturaVida = 10;
            ProgressBar vidaJugador = new ProgressBar
            {
                Location = new Point(200, lblJugador.Top + lblJugador.Height/2 - alturaVida/2),
                Size = new Size(250, alturaVida),
                Value = 70,
                ForeColor = Color.Red
            };
            panelJugador.Controls.Add(vidaJugador);
            redondearControl(vidaJugador, 10);

            // Nivel jugador
            int tamanoNivel = 30;
            Label nivelJugador = new Label
            {
                Text = "10",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(tamanoNivel, tamanoNivel),
                Location = new Point(455, vidaJugador.Top + vidaJugador.Height/2 - tamanoNivel/2),
                BackColor = Color.Yellow,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false 
            };
            panelJugador.Controls.Add(nivelJugador);
            redondearControl(nivelJugador, 10);

            //Imagen Pokemon Jugador
            string jugadorPath = Path.Combine(directorioBase, "Resources", "images", "Pikachu.png");
            PictureBox fotoPokemonJugador = new PictureBox
            {
                Size = new Size(80,80),
                Location = new Point(20, 70),
                Image = Image.FromFile(jugadorPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelJugador.Controls.Add(fotoPokemonJugador);

            string PokemonJugadorPath = Path.Combine(directorioBase, "Resources", "images", "Pikachu_Jugador.png");
            PictureBox pokemonJugador = new PictureBox
            {
                Size = new Size(500, 500),
                Location = new Point(panelCombate.Width/2 - 500 - 150, panelCombate.Height/2 ),
                Image = Image.FromFile(PokemonJugadorPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelCombate.Controls.Add(pokemonJugador);

            // Caja del enemigo -----------------------------------------------------------------------------------------------------
            Panel panelEnemigo = new Panel
            {
                Size = new Size(500, 160),
                Location = new Point(ventana.ClientSize.Width - 520, 20),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            panelCombate.Controls.Add(panelEnemigo);
            RedondearControlConEstilo(
                panelEnemigo,
                radio: 10,
                colorBorde1: Color.Red,
                colorBorde2: Color.DarkRed,
                grosorBorde: 3,
                usarSombra: true,
                colorFondo: Color.LightSalmon,
                colorFondo2: Color.Red
            );

            // Nombre del enemigo
            Label lblEnemigo = new Label
            {
                Text = "Charmander",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panelEnemigo.Controls.Add(lblEnemigo);

            // Vida enemigo
            int alturaVidaEnemigo = 10;
            ProgressBar vidaEnemigo = new ProgressBar
            {
                Location = new Point(200, lblEnemigo.Top + lblEnemigo.Height / 2 - alturaVidaEnemigo / 2),
                Size = new Size(250, alturaVidaEnemigo),
                Value = 65,
                ForeColor = Color.Red
            };
            panelEnemigo.Controls.Add(vidaEnemigo);
            redondearControl(vidaEnemigo, 10);

            // Nivel enemigo
            int tamanoNivelEnemigo = 30;
            Label nivelEnemigo = new Label
            {
                Text = "12",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(tamanoNivelEnemigo, tamanoNivelEnemigo),
                Location = new Point(455, vidaEnemigo.Top + vidaEnemigo.Height / 2 - tamanoNivelEnemigo / 2),
                BackColor = Color.OrangeRed,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            panelEnemigo.Controls.Add(nivelEnemigo);
            redondearControl(nivelEnemigo, 10);

            //Imagen Pokemon
            string enemigoPath = Path.Combine(directorioBase, "Resources", "images", "Charmander.png");
            int tamanoPokemon = 80;
            PictureBox fotoPokemonEnemigo = new PictureBox
            {
                Size = new Size(tamanoPokemon, tamanoPokemon),
                Location = new Point(panelEnemigo.Width - 20 - tamanoPokemon, 70),
                Image = Image.FromFile(enemigoPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelEnemigo.Controls.Add(fotoPokemonEnemigo);

            string PokemonEnemigoPath = Path.Combine(directorioBase, "Resources", "images", "Charmander_Enemigo.png");
            PictureBox pokemonEnemigo = new PictureBox
            {
                Size = new Size(350, 350),
                Location = new Point(panelCombate.Width / 2  + 150, panelCombate.Height / 2 - 200),
                Image = Image.FromFile(PokemonEnemigoPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelCombate.Controls.Add(pokemonEnemigo);

            // Otros del Panel de combate -------------------------------------------------------------------------------------------------

            string pokeballPath = Path.Combine(directorioBase, "Resources", "images", "pokeball.png");
            int tamanoPokeball = 25;
            // Pokemon disponibles del Jugador
            for (int i = 0; i < 5; i++)
            {
                PictureBox pokeballsJugador = new PictureBox
                {
                    Size = new Size(tamanoPokeball, tamanoPokeball),
                    Location = new Point(panelJugador.Right + 20, panelJugador.Top + i * 30),
                    Image = Image.FromFile(pokeballPath), // Cambia la ruta a la imagen del Pokémon
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                panelCombate.Controls.Add(pokeballsJugador);
                pokeballsJugador.BringToFront();
            }

            
            // Pokemon disponibles del Enemigo
            for (int i = 0; i < 5; i++)
            {
                PictureBox pokeballsEnemigo = new PictureBox
                {
                    Size = new Size(tamanoPokeball, tamanoPokeball),
                    Location = new Point(panelEnemigo.Left - 20 - tamanoPokeball, panelEnemigo.Top + i * 30),
                    Image = Image.FromFile(pokeballPath), // Cambia la ruta a la imagen del Pokémon
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                panelCombate.Controls.Add(pokeballsEnemigo);
                pokeballsEnemigo.BringToFront();
            }

            // Panel inferior con botones
            Panel panelBotones = new Panel
            {
                Size = new Size(430, 70),
                Location = new Point((panelCombate.Width - 430) / 2, panelCombate.Height - 80),
                BackColor = Color.FromArgb(180, 240, 255),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelCombate.Controls.Add(panelBotones);
            RedondearControlConEstilo(
                panelBotones,
                radio: 10,
                colorBorde1: Color.Blue,
                colorBorde2: Color.DarkBlue,
                grosorBorde: 3,
                usarSombra: true,
                colorFondo: Color.LightSkyBlue,
                colorFondo2: Color.White
            );
            panelBotones.BringToFront();

            List<CartaPokemon> cartas = new List<CartaPokemon>();



            //Crear la carta correctamente y añadirla al panel
            PanelDobleBuffer panelCarta = new PanelDobleBuffer
            {
                Size = new Size(250, 350),
                Location = new Point((panelCombate.Width - 240) / 2, panelCombate.Height - 80 - 350),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            panelCombate.Controls.Add(panelCarta);
            CartaPokemon carta = new CartaPokemon(
                "Pikachu",
                39,
                "rayo",
                "images/" + "Pikachu" + ".png",
                new List<(string, int)>
                    {
                        ("Ataque Boltio", 30),
                        ("Rugido", 10)
                    }
            );
            bool escogerPokemon = false;
            cartas.Add(carta); // Agregar la carta a la lista
            gestorCartas.DibujarCartas(cartas, panelCarta, false, escogerPokemon );
            panelCarta.BringToFront();



            // Botones de combate
            CrearBoton(panelBotones, "Pokemon", 10, 10, Color.LightPink, (s, e) =>
            {
                MessageBox.Show("Has hecho clic en Pokémon");
            });

            CrearBoton(panelBotones, "Mochila", 150, 10, Color.Thistle, (s, e) =>
            {
                MessageBox.Show("Has hecho clic en Mochila");
            });

            CrearBoton(panelBotones, "Huir", 290, 10, Color.MediumAquamarine, (s, e) =>
            {
                MessageBox.Show("Has huido del combate");
                panelCombate.Visible = false; // Ocultar el panel de combate
                panelCombate.Controls.Clear(); // Borrar los controles del panel
            });

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



        
        //Nueva version de pantallaCombate que acepta un Pokemn
        public static void pantallaCombate(Form ventana, Pokemon oponenteSalvaje)
        {
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

            // Llamamos a la nueva version de crearUICombate
            crearUICombate(panelCombate, ventana, oponenteSalvaje);
        }


        //Nueva version de crearUICombate para el oponente salvaje
        private static void crearUICombate(PanelDobleBuffer panelCombate, Form ventana, Pokemon oponente)
        {
            // Panel del jugador ------------------------------------------------------------------------------------------------------
            Panel panelJugador = new Panel
            {
                Size = new Size(500, 160),
                Location = new Point(20, 20),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };

            panelCombate.Controls.Add(panelJugador);
            RedondearControlConEstilo(
                panelJugador,
                radio: 10,
                colorBorde1: Color.Yellow,
                colorBorde2: Color.Orange,
                grosorBorde: 3,
                usarSombra: true,
                colorFondo: Color.LightYellow,
                colorFondo2: Color.Yellow
            );

            // Nombre del jugador
            Label lblJugador = new Label
            {
                Text = "Pikachu",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true

            };
            panelJugador.Controls.Add(lblJugador);

            // Vida del jugador
            int alturaVida = 10;
            ProgressBar vidaJugador = new ProgressBar
            {
                Location = new Point(200, lblJugador.Top + lblJugador.Height / 2 - alturaVida / 2),
                Size = new Size(250, alturaVida),
                Value = 70,
                ForeColor = Color.Red
            };
            panelJugador.Controls.Add(vidaJugador);
            redondearControl(vidaJugador, 10);

            // Nivel jugador
            int tamanoNivel = 30;
            Label nivelJugador = new Label
            {
                Text = "10",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(tamanoNivel, tamanoNivel),
                Location = new Point(455, vidaJugador.Top + vidaJugador.Height / 2 - tamanoNivel / 2),
                BackColor = Color.Yellow,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            panelJugador.Controls.Add(nivelJugador);
            redondearControl(nivelJugador, 10);

            //Imagen Pokemon Jugador
            string jugadorPath = Path.Combine(directorioBase, "Resources", "images", "Pikachu.png");
            PictureBox fotoPokemonJugador = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(20, 70),
                Image = Image.FromFile(jugadorPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelJugador.Controls.Add(fotoPokemonJugador);

            string PokemonJugadorPath = Path.Combine(directorioBase, "Resources", "images", "Pikachu_Jugador.png");
            PictureBox pokemonJugador = new PictureBox
            {
                Size = new Size(500, 500),
                Location = new Point(panelCombate.Width / 2 - 500 - 150, panelCombate.Height / 2),
                Image = Image.FromFile(PokemonJugadorPath), // Cambia la ruta a la imagen del Pokémon
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelCombate.Controls.Add(pokemonJugador);


            // --- Panel del Enemigo Pokemon salvaje -----------------------------------------------------------------------------
            Panel panelEnemigo = new Panel
            {
                Size = new Size(500, 160),
                Location = new Point(ventana.ClientSize.Width - 520, 20),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            panelCombate.Controls.Add(panelEnemigo);
            RedondearControlConEstilo(panelEnemigo, 10, Color.Red, Color.DarkRed, 3, true, Color.LightSalmon, Color.Red);

            
            Label lblEnemigo = new Label
            {
                Text = oponente.Nombre, // OPONENTE
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panelEnemigo.Controls.Add(lblEnemigo);

            ProgressBar vidaEnemigo = new ProgressBar
            {
                Location = new Point(200, lblEnemigo.Top + lblEnemigo.Height / 2 - 5),
                Size = new Size(250, 10),
                Value = oponente.Vida, 
                Maximum = oponente.Vida, 
                ForeColor = Color.Red
            };

            panelEnemigo.Controls.Add(vidaEnemigo);
            redondearControl(vidaEnemigo, 10);
            // Nivel enemigo
            int tamanoNivelEnemigo = 30;
            Label nivelEnemigo = new Label
            {
                Text = "12",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(tamanoNivelEnemigo, tamanoNivelEnemigo),
                Location = new Point(455, vidaEnemigo.Top + vidaEnemigo.Height / 2 - tamanoNivelEnemigo / 2),
                BackColor = Color.OrangeRed,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            panelEnemigo.Controls.Add(nivelEnemigo);
            redondearControl(nivelEnemigo, 10);

            // icono pokoemon enemigo panel superior
            string enemigoIconoPath = Path.Combine(directorioBase, "Resources", "images", oponente.Nombre + ".png");
            PictureBox fotoPokemonEnemigo = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(panelEnemigo.Width - 20 - 80, 70),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            if (File.Exists(enemigoIconoPath))
            {
                fotoPokemonEnemigo.Image = Image.FromFile(enemigoIconoPath);
            }

            panelEnemigo.Controls.Add(fotoPokemonEnemigo);

            // Imagen del Pokemon enemigo grande
            string enemigoSpritePath = Path.Combine(directorioBase, "Resources", "images", oponente.Nombre + ".png");
            PictureBox pokemonEnemigo = new PictureBox
            {
                Size = new Size(350, 350),
                Location = new Point(panelCombate.Width / 2 + 150, panelCombate.Height / 2 - 200),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            if (File.Exists(enemigoSpritePath))
            {
                pokemonEnemigo.Image = Image.FromFile(enemigoSpritePath);
            }
            panelCombate.Controls.Add(pokemonEnemigo);

            // Otros del Panel de combate -------------------------------------------------------------------------------------------------

            string pokeballPath = Path.Combine(directorioBase, "Resources", "images", "pokeball.png");
            int tamanoPokeball = 25;
            // Pokemon disponibles del Jugador
            for (int i = 0; i < 5; i++)
            {
                PictureBox pokeballsJugador = new PictureBox
                {
                    Size = new Size(tamanoPokeball, tamanoPokeball),
                    Location = new Point(panelJugador.Right + 20, panelJugador.Top + i * 30),
                    Image = Image.FromFile(pokeballPath), // Cambia la ruta a la imagen del Pokémon
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                panelCombate.Controls.Add(pokeballsJugador);
                pokeballsJugador.BringToFront();
            }


            // Pokemon disponibles del Enemigo
            for (int i = 0; i < 5; i++)
            {
                PictureBox pokeballsEnemigo = new PictureBox
                {
                    Size = new Size(tamanoPokeball, tamanoPokeball),
                    Location = new Point(panelEnemigo.Left - 20 - tamanoPokeball, panelEnemigo.Top + i * 30),
                    Image = Image.FromFile(pokeballPath), // Cambia la ruta a la imagen del Pokémon
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                panelCombate.Controls.Add(pokeballsEnemigo);
                pokeballsEnemigo.BringToFront();
            }

            // Panel inferior con botones
            Panel panelBotones = new Panel
            {
                Size = new Size(430, 70),
                Location = new Point((panelCombate.Width - 430) / 2, panelCombate.Height - 80),
                BackColor = Color.FromArgb(180, 240, 255),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelCombate.Controls.Add(panelBotones);
            RedondearControlConEstilo(
                panelBotones,
                radio: 10,
                colorBorde1: Color.Blue,
                colorBorde2: Color.DarkBlue,
                grosorBorde: 3,
                usarSombra: true,
                colorFondo: Color.LightSkyBlue,
                colorFondo2: Color.White
            );
            panelBotones.BringToFront();

            List<CartaPokemon> cartas = new List<CartaPokemon>();



            //Crear la carta correctamente y añadirla al panel
            PanelDobleBuffer panelCarta = new PanelDobleBuffer
            {
                Size = new Size(250, 350),
                Location = new Point((panelCombate.Width - 240) / 2, panelCombate.Height - 80 - 350),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            panelCombate.Controls.Add(panelCarta);
            CartaPokemon carta = new CartaPokemon(
                "Pikachu",
                39,
                "rayo",
                "images/" + "Pikachu" + ".png",
                new List<(string, int)>
                    {
                        ("Ataque Boltio", 30),
                        ("Rugido", 10)
                    }
            );
            bool escogerPokemon = false;
            cartas.Add(carta); // Agregar la carta a la lista
            gestorCartas.DibujarCartas(cartas, panelCarta, false, escogerPokemon);
            panelCarta.BringToFront();



            // Botones de combate-----------------------------------------------------------
            CrearBoton(panelBotones, "Pokemon", 10, 10, Color.LightPink, (s, e) =>
            {
                MessageBox.Show("Has hecho clic en Pokémon");
            });

            CrearBoton(panelBotones, "Mochila", 150, 10, Color.Thistle, (s, e) =>
            {
                MessageBox.Show("Has hecho clic en Mochila");
            });

            CrearBoton(panelBotones, "Huir", 290, 10, Color.MediumAquamarine, (s, e) =>
            {
                if (ventana is FormJuego formJuego)
                {
                    // Llamamos al nuevo método para terminar el combate
                    formJuego.TerminarCombate();
                }
            });
        }

    }
}

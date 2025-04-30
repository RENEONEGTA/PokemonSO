using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics.Contracts;
using System.Reflection.Emit;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;

namespace WindowsFormsApplication1
{
    //Tiempos de video de fondo: 00:00:00 - 00:01:58,  00:02:00 - 00:02:52, 00:02:53 - 00:04:07, 00:04:09 - 00:06:54, 00:06:57 - 00:08:35, 00:08:36 - 00:10:59, 00:11:01 - 00:12:16

    public partial class Form1 : Form
    {
        Socket server;
        private int puertoServidor = 9010; // Puerto del servidor
        private Timer parpadeoTimer = new Timer();
        private bool serverRun = false;
        private bool colorAzul = true;
        private bool registro = false;
        private GestorCratas gestorCartas = new GestorCratas();
        private Pokemon Pokemon = new Pokemon();
        private Partida Partida = new Partida();
        private Conectados Conectados = new Conectados();
        private Combate Combate = new Combate();
        private bool combate = false;
        private PanelDobleBuffer panelCartas;
        private PanelDobleBuffer panelCargarPartida;
        private PanelDobleBuffer panelCargarCombate;
        private bool boolPanelCargarPartida = true;
        private bool boolPanelCargarCombate = true;
        private bool panelCreado = false;
        int PokedexLocationX = 700; // Posición X de la Pokeball
        int PokedexLocationY = 700; // Posición Y de la Pokeball
        int panelCartasTop = 0; // Posición Top del panel de cartas
        private bool isDownloading = false;
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
        string user;
        string contra;
        bool iniciado = false;    
        List<Conectados> listaConectadosGlobal = new List<Conectados>();
        RichTextBox historialMensajes = new RichTextBox();
        System.Windows.Forms.TextBox textBoxMensaje = new System.Windows.Forms.TextBox();
        RoundButton enviarMensaje = new RoundButton();
        PanelDobleBuffer panelChat = new PanelDobleBuffer();


        public Form1()
        {
            InitializeComponent();
            parpadeoTimer.Interval = 500; // Parpadeo cada 500 ms
            parpadeoTimer.Tick += ParpadeoTimer_Tick;
        }

        // Actualiza el progreso de la barra
        public void UpdateProgress(int percent)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(UpdateProgress), percent);
            }
            else
            {
                progressBar.Value = percent;
            }
        }

        // Configura el estilo de la barra de progreso
        public void SetProgressBarStyle(ProgressBarStyle style)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<ProgressBarStyle>(SetProgressBarStyle), style);
            }
            else
            {
                progressBar.Style = style;
            }
        }

        // Establece el valor máximo de la barra
        public void SetMaxValue(int max)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(SetMaxValue), max);
            }
            else
            {
                progressBar.Maximum = max;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            ////Inicio Sesion
            //Usuario.Visible = false;
            //textUsu.Visible = false;
            //Contraseña.Visible = false;
            //textContra.Visible = false;
            //SignIn.Visible = false;

            ////Resitrarse
            //UsuarioRegistrarse.Visible = false;
            //textUsuR.Visible = false;
            //ContraseñaRegistrarse.Visible = false;
            //textContraR.Visible = false;
            //RepetirContraseña.Visible = false;
            //textConRR.Visible = false;
            //SignUp.Visible = false;

            ////Consultas 
            //groupBox1.Visible = false;
            ChangeCircleColor(Color.Blue);
            ConectarServidor();
            AtenderServidor();
        }

        private async Task crearFondoAsync()
        {

            if (isDownloading) // Si ya hay una descarga en curso, no hacer nada
                return;

            isDownloading = true;

            string videoPath = Path.Combine(directorioBase, "Resources", "videos", "FondoPokemon.mp4");
            string directorio = Path.Combine(directorioBase, "Resources", "videos");
            string videoUrl = "https://www.dropbox.com/scl/fi/ztvmhlz5238yno38g4lju/FondoPokemon.mp4?rlkey=q0tle4yqr378txm938bivfayo&st=dexsrxui&dl=1";

            //Verificar que el directorio existe
            if (!Directory.Exists(directorio))
            {
                DirectoryInfo di = Directory.CreateDirectory(directorio);
            }

            // Verificar si el archivo de video ya existe
            if (File.Exists(videoPath))
            {
                // El video ya existe, omitir la descarga y mostrarlo directamente
                Invoke((MethodInvoker)delegate
                {
                    MostrarVideoYElementos();
                });
                isDownloading = false; // Permitir futuras descargas (aunque no se realicen)
                return;
            }



            // Mostrar la barra de progreso en la UI
            Invoke((MethodInvoker)delegate
            { 
                progressBar.Visible = true;
                progressBar.Size = new Size(this.ClientSize.Width / 2, 30);
                progressBar.Location = new Point(this.ClientSize.Width / 2 - progressBar.Width / 2, this.ClientSize.Height - 60);
            });


            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Obtener las cabeceras primero para conocer el tamaño del contenido
                    HttpResponseMessage response = await client.GetAsync(videoUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;
                    var progressHandler = new Progress<int>(percent =>
                    {
                        // Actualiza la barra de progreso en la UI
                        Invoke((MethodInvoker)delegate
                        {
                            UpdateProgress(percent);
                        });
                    });

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(videoPath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        long totalBytesRead = 0;

                        // Configurar ProgressForm según si se conoce el tamaño total
                        if (totalBytes.HasValue)
                        {
                            SetProgressBarStyle(ProgressBarStyle.Continuous);
                            SetMaxValue(100);
                        }
                        else
                        {
                            SetProgressBarStyle(ProgressBarStyle.Marquee);
                        }

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            if (totalBytes.HasValue)
                            {
                                int percent = (int)((totalBytesRead * 100) / totalBytes.Value);
                                (progressHandler as IProgress<int>).Report(percent);
                            }
                        }
                    }
                }


                // Mostrar video y otros elementos UI después de la descarga
                Invoke((MethodInvoker)delegate
                {
                    MostrarVideoYElementos();
                    
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar los archivos del juego: {ex.Message}");
            }
            finally
            {
                progressBar.Visible = false;
                isDownloading = false; // Permite nuevas descargas
            }
        }

        private void crearPanelCombate() {

            
                int PanelSizeX = 500;
                int PanelSizeY = 300;

                panelCargarCombate = new PanelDobleBuffer
                {
                    Size = new Size(PanelSizeX, PanelSizeY), // Tamaño ajustado
                    Location = new Point(combatirBox.Left + combatirBox.Width + 10, combatirBox.Top + (combatirBox.Height / 2) - (PanelSizeY / 2)),
                    BackColor = Color.Black,
                };
                this.Controls.Add(panelCargarCombate);
                panelCargarCombate.Visible = false;
                panelCargarCombate.BringToFront();
                


                // Configurar el evento Paint para aplicar bordes redondeados y degradado
                panelCargarCombate.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
                {
                    PanelDobleBuffer panel = (PanelDobleBuffer)senderPanel;
                    int radio = 20; // Radio para las esquinas redondeadas
                    Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

                    // Crear la ruta con esquinas redondeadas
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
                        path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
                        path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
                        path.CloseFigure();

                        // Asigna la región redondeada al panel
                        panelCargarCombate.Region = new Region(path);

                        // Configurar el suavizado
                        ePanel.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Crear un pincel de degradado: de blanco a un gris muy claro
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            rect,
                            Color.Black,
                            Color.FromArgb(22, 22, 22),
                            LinearGradientMode.Vertical))
                        {
                            // Rellenar la ruta con el degradado
                            ePanel.Graphics.FillPath(brush, path);
                        }

                        // Dibujar un borde:
                        using (Pen pen = new Pen(Color.FromArgb(38, 209, 255), 4))
                        {
                            ePanel.Graphics.DrawPath(pen, path);
                        }


                    }
                });

                }
        private void MostrarVideoYElementos()
        {
            
            //this.FormBorderStyle = FormBorderStyle.None; // Oculta los bordes y la barra de título
            //this.WindowState = FormWindowState.Maximized; // Maximiza para ocupar toda la pantalla
            
            // Obtén la ruta del directorio bin/debug
            string videoPath = Path.Combine(directorioBase, "Resources", "videos", "FondoPokemon.mp4");
            fondoPokemon.Visible = true;
            fondoPokemon.uiMode = "none"; // Ocultar controles de reproducción


            // Tiempos de video (tiempo de inicio, tiempo de fin) en segundos
            List<Tuple<int, int>> tiempos = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(0, 118),    // 00:00:00 - 00:01:58
                new Tuple<int, int>(120, 172), // 00:02:00 - 00:02:52
                new Tuple<int, int>(173, 247), // 00:02:53 - 00:04:07
                new Tuple<int, int>(249, 414), // 00:04:09 - 00:06:54
                new Tuple<int, int>(417, 515), // 00:06:57 - 00:08:35
                new Tuple<int, int>(516, 659), // 00:08:36 - 00:10:59
                new Tuple<int, int>(661, 736)    // 00:11:01 - 00:12:16
            };

            // Selección aleatoria de un índice entre 0 y 6
            Random rand = new Random();
            int indiceSeleccionado = rand.Next(tiempos.Count); // Genera un número entre 0 y 6

            var segmentoSeleccionado = tiempos[indiceSeleccionado];

            // Configurar el video
            fondoPokemon.URL = videoPath;
            fondoPokemon.settings.autoStart = true;
            fondoPokemon.settings.setMode("loop", false); // No en bucle
            fondoPokemon.SendToBack(); // Enviar al fondo

            // Ajustar el video al tamaño de la ventana
            fondoPokemon.Dock = DockStyle.Fill;  // Esto hace que el video ocupe todo el formulario

            fondoPokemon.Ctlcontrols.currentPosition = segmentoSeleccionado.Item1; // Establecer la posición de inicio
            fondoPokemon.Ctlcontrols.play(); // Iniciar la reproducción

            // Configurar la pausa al final del segmento
            Timer timer = new Timer();
            timer.Interval = (segmentoSeleccionado.Item2 - segmentoSeleccionado.Item1) * 1000; // Duración en milisegundos
            timer.Tick += (s, e) =>
            {
                fondoPokemon.Ctlcontrols.pause(); // Pausar el video
                fondoPokemon.Ctlcontrols.currentPosition = segmentoSeleccionado.Item2; // Pausar al final
                timer.Stop(); // Detener el temporizador
            };
            timer.Start();


            nuevaPartidaBox.Visible = true;
            nuevaPartidaBox.Location = new Point(32, 128);
            cargarPartidaBox.Visible = true;
            cargarPartidaBox.Location = new Point(32, 192);
            combatirBox.Visible = true;
            combatirBox.Location = new Point(32, 256);
            salirJuegoBox.Visible = true;
            salirJuegoBox.Location = new Point(32, 320);

            crearPanelCombate();
            crearChat();




            PictureBox pokedexBox = new PictureBox
            {
                Size = new Size(100, 100), // Ajusta el tamaño según sea necesario
                Location = new Point(PokedexLocationX, PokedexLocationY),
                SizeMode = PictureBoxSizeMode.StretchImage // Opcional: para ajustar la imagen al tamaño del control
            };
            string pokeballPath = Path.Combine(directorioBase, "Resources", "images", "pokeball.png");
            pokedexBox.Image = Image.FromFile(pokeballPath);
            this.Controls.Add(pokedexBox);
            pokedexBox.BringToFront();
            pokedexBox.Visible = true;
            pokedexBox.Click += pokedexBox_Click;
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




        private void pokedexBox_Click(object sender, EventArgs e)
        {

            if (!panelCreado)
            {
                int PanelSizeX = 780;
                int PanelSizeY = 600;

                panelCartas = new PanelDobleBuffer
                {
                    Size = new Size(PanelSizeX, PanelSizeY), // Tamaño ajustado
                    Location = new Point(PokedexLocationX - (PanelSizeX / 2) + 50, PokedexLocationY - PanelSizeY - 20),
                    BackColor = Color.Black,
                };
                this.Controls.Add(panelCartas);
                panelCartas.Visible = false;
                panelCreado = true;

                // Evento para manejar el desplazamiento
                panelCartas.MouseWheel += PanelCartas_MouseWheel;

                // Configurar el evento Paint para aplicar bordes redondeados y degradado
                panelCartas.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
                {
                    PanelDobleBuffer panel = (PanelDobleBuffer)senderPanel;
                    int radio = 20; // Radio para las esquinas redondeadas
                    Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

                    // Crear la ruta con esquinas redondeadas
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
                        path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
                        path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
                        path.CloseFigure();

                        // Asigna la región redondeada al panel
                        panelCartas.Region = new Region(path);

                        // Configurar el suavizado
                        ePanel.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Crear un pincel de degradado: de blanco a un gris muy claro
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            rect,
                            Color.Black,
                            Color.DarkGray,
                            LinearGradientMode.Vertical))
                        {
                            // Rellenar la ruta con el degradado
                            ePanel.Graphics.FillPath(brush, path);
                        }

                        // Si deseas dibujar un borde:
                        using (Pen pen = new Pen(Color.FromArgb(38, 209, 255), 4))
                        {
                            ePanel.Graphics.DrawPath(pen, path);
                        }

                        panelCartasTop = panelCartas.Top - 50;
                    }
                });
            }

            if (!combate)
            {
                if (serverRun == true)
                {
                    if (user != null)
                    {
                        string mensaje = "3/" + user;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        ////Recibimos la respuesta del servidor
                        //byte[] msg2 = new byte[1000];
                        //server.Receive(msg2);
                        //mensaje = Encoding.ASCII.GetString(msg2);


                        //MessageBox.Show(mensaje);
                        //List<Pokemon> listaPokemon = new List<Pokemon>();

                        //listaPokemon = Pokemon.ParsearDatos(mensaje, listaPokemon);

                        //if (listaPokemon.Count > 0)
                        //{            
                        //    List<CartaPokemon> cartas = new List<CartaPokemon>();

                        //    foreach (Pokemon pokemon in listaPokemon)
                        //    {
                        //        // Crear la carta correctamente y añadirla a la lista
                        //        CartaPokemon carta = new CartaPokemon(
                        //            pokemon.Nombre,
                        //            pokemon.Vida,
                        //            pokemon.Elemento,
                        //            "images/" + pokemon.Nombre + ".png",
                        //            new List<(string, int)>
                        //            {
                        //                (pokemon.Ataque, pokemon.Daño),
                        //                ("Rugido", 10)
                        //            }
                        //        );
                        //        cartas.Add(carta); // Agregar la carta a la lista
                        //    }

                        //    panelCartas.BringToFront();
                        //    combate = true;
                        //    gestorCartas.DibujarCartas(cartas, panelCartas, true);
                        //    panelCartas.Visible = true;
                        //}
                        //else
                        //{
                        //    MessageBox.Show("No tienes pokemons");

                        //}
                    }
                    else
                    {
                        MessageBox.Show("Ha habido un error al buscar los pokemons del jugador " + user);
                    }
                }
                else
                {
                    MessageBox.Show("No tienes conexion con el servidor");
                }


            }
            else
            {
                panelCartas.Visible = false;
                combate = false;


            }

        }

        private async void ConectarServidor()
        {
            parpadeoTimer.Start(); // Iniciar el parpadeo
            await Task.Run(() =>
            {

                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(IP.Text);
                IPEndPoint ipep = new IPEndPoint(direc, puertoServidor);


                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket



                    Invoke((MethodInvoker)delegate
                    {
                        ChangeCircleColor(Color.Green);
                        parpadeoTimer.Stop(); // Detener el parpadeo
                        serverRun = true;
                        Task.CompletedTask.Wait();

                    });

                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    Invoke((MethodInvoker)delegate
                    {
                        ChangeCircleColor(Color.Red);
                        parpadeoTimer.Stop(); // Detener el parpadeo
                        MessageBox.Show("Error de conexión con el servidor: " + ex.Message);
                        serverRun = false;
                        Task.CompletedTask.Wait();

                    });
                    return;
                }

            });
        }
        private async void AtenderServidor()
        {
            await Task.Run(() =>
            {
                while (true)
                {

                    if (serverRun == true)
                    {
                        //recivimos mensake del servidor
                        byte[] msg2 = new byte[2048];
                        server.Receive(msg2);
                        try
                        {

                            string datos = Encoding.ASCII.GetString(msg2);
                            string[] mensajeSucio = datos.Split('\n'); // separa por línea

                            foreach (string mensaje1 in mensajeSucio)
                            {
                                if (mensaje1.Contains("~$"))
                                {
                                    string[] partes = mensaje1.Split(new string[] { "~$" }, StringSplitOptions.None);
                                    int codigo = Convert.ToInt32(partes[0]);
                                    string mensaje = partes[1].Trim();

                                    switch (codigo)
                                    {
                                        case 1: //Creado Cuenta

                                            MessageBox.Show(mensaje);
                                            cambiarInicioRegistro();

                                            break;

                                        case 2: //Iniciado Sesion

                                            // Mostramos el mensaje recibido


                                            // Comparamos correctamente
                                            //MessageBox.Show(mensaje);
                                            if (Convert.ToInt32(mensaje) == 1)
                                            {

                                                Invoke((MethodInvoker)delegate
                                                {
                                                    iniciado = true;
                                                    MessageBox.Show("Se ha iniciado la sesion con exito");

                                                    foreach (Control control in this.Controls)
                                                    {
                                                        control.Visible = false;
                                                    }

                                                });

                                                _ = Task.Run(async () =>
                                                    {
                                                        await crearFondoAsync();
                                                    });
                                            }
                                            else
                                            {
                                                MessageBox.Show("No se ha podido Iniciar Sesion");
                                            }

                                            break;

                                        case 3: //Pokedex

                                            MessageBox.Show(mensaje);
                                            Invoke((MethodInvoker)delegate
                                            {
                                                List<Pokemon> listaPokemon = new List<Pokemon>();

                                                listaPokemon = Pokemon.ParsearDatos(mensaje, listaPokemon);

                                                if (listaPokemon.Count > 0)
                                                {
                                                    List<CartaPokemon> cartas = new List<CartaPokemon>();

                                                    foreach (Pokemon pokemon in listaPokemon)
                                                    {
                                                        // Crear la carta correctamente y añadirla a la lista
                                                        CartaPokemon carta = new CartaPokemon(
                                                            pokemon.Nombre,
                                                            pokemon.Vida,
                                                            pokemon.Elemento,
                                                            "images/" + pokemon.Nombre + ".png",
                                                            new List<(string, int)>
                                                            {
                                                        (pokemon.Ataque, pokemon.Daño),
                                                        ("Rugido", 10)
                                                            }
                                                        );
                                                        cartas.Add(carta); // Agregar la carta a la lista
                                                    }

                                                    panelCartas.BringToFront();
                                                    combate = true;
                                                    gestorCartas.DibujarCartas(cartas, panelCartas, true);
                                                    panelCartas.Visible = true;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No tienes pokemons");

                                                }
                                            });

                                            break;

                                        case 4: //Lista de Partidas Donde esta el Jugador

                                            MessageBox.Show(mensaje);

                                            Invoke((MethodInvoker)delegate
                                            {
                                                List<Partida> listaPartidas = new List<Partida>();
                                                listaPartidas = Partida.ParsearRespuesta(mensaje, listaPartidas);
                                                if (listaPartidas.Count > 0)
                                                {
                                                    Partida.DibujarPartidas(listaPartidas, panelCargarPartida);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No tienes partidas");
                                                }
                                            });

                                            break;
                                        case 5: //La gente que tiene el pokemon selecionado

                                            MessageBox.Show(mensaje);

                                            break;
                                        case 6: //Lista conectados Combates 

                                            //MessageBox.Show(mensaje);


                                            //Invoke((MethodInvoker)delegate
                                            //{


                                            //    //listaConectadosGlobal = Conectados.ParsearDatos(mensaje, listaConectadosGlobal);
                                            //    if (listaConectadosGlobal.Count > 0)
                                            //    {
                                            //        Conectados.DibujarConectados(listaConectadosGlobal, panelCargarCombate, this, user, server);
                                            //    }
                                            //    else
                                            //    {
                                            //        MessageBox.Show("No tienes partidas");
                                            //    }
                                            //});

                                            break;
                                        case 100: //Lista Conectados Notificacion
                                            //MessageBox.Show(mensaje);
                                            Invoke((MethodInvoker)delegate
                                            {
                                                listaConectadosGlobal.Clear();
                                                
                                                listaConectadosGlobal = Conectados.ParsearDatos(mensaje, listaConectadosGlobal);
                                                if(boolPanelCargarCombate==false)
                                                {
                                                    Conectados.DibujarConectados(listaConectadosGlobal, panelCargarCombate, this, user, server);
                                                }

                                            });
                                            break;

                                        case 101: //Recivimos Notificacion del Chat
                                           
                                            Invoke((MethodInvoker)delegate
                                            {
                                                HistorialMensajes(mensaje.ToString());
                                               
                                            });
                                            break;

                                        case 102: //Invitacion Combate


                                            int idInvitador;
                                            if (int.TryParse(mensaje, out idInvitador))
                                            {
                                                // Buscar al jugador en la lista global
                                                Conectados jugadorInvitador = listaConectadosGlobal
                                                    .FirstOrDefault(c => c.Id == idInvitador);

                                                Invoke((MethodInvoker)delegate
                                                {
                                                    if (jugadorInvitador != null)
                                                    {
                                                        Conectados.RecibirInvitacion(jugadorInvitador, this, server);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("El jugador que te ha invitado no está en tu lista de conectados.");
                                                    }
                                                });


                                            }
                                            else
                                            {
                                                MessageBox.Show("Error al interpretar el ID del jugador invitador.");
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error al recibir el mensaje del servidor: " + ex.Message);
                            
                        }
                        
                    }                   
                }
            });
        }
        private void DesconectarServidor()
        {
            if (serverRun == true && iniciado == true) //Nos desconectamos
            {
                string mensaje = "0/" + user + "/" + contra;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (serverRun == true)
            {
                string mensaje = "0/" + user + "/" + contra;
                // Enviamos al servidor el nombre 0
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }


        }

        private void Form1_Form1Closing(object sender, FormClosingEventArgs e)
        {
            DesconectarServidor();
        }

        private void ParpadeoTimer_Tick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate {
                ChangeCircleColor(colorAzul ? Color.Blue : Color.Green);
                colorAzul = !colorAzul;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Longitud.Checked) //Consulta Primera partida que he echo
            {
                string mensaje = "3/" + textUsu.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
                MessageBox.Show("La Primera partida que hize fue" + mensaje);
            }
            else if (consultaPokedex.Checked) //Consulta que pokemon tiene mayo vida.
            {
                string mensaje = "4/" + textUsu.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                MessageBox.Show("El pokemon con mas vida es el " + mensaje);

            }
            else if (Bonito.Checked) //Consulta Cuantos pokemos tengo.
            {
                string mensaje = "5/" + textUsu.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
                MessageBox.Show("Tengo " + mensaje + " pokemons");
            }
            else
                MessageBox.Show("Seleciona la estadistica que quieras consultar");

            // Se terminó el servicio. 
            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            if (serverRun == true)
            {
                if (repiteContra.Text == textContra.Text && textContra.Text.Length != 0 && repiteContra.Text.Length != 0 && textUsu.Text.Length != 0)
                {
                    string mensaje = "1/" + textUsu.Text + "/" + textContra.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    //byte[] msg2 = new byte[80];
                    //server.Receive(msg2);
                    //mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];


                    //MessageBox.Show(mensaje);
                    //cambiarInicioRegistro();

                }
                else
                {
                    MessageBox.Show("Las contraseñas son diferentes, mira si hay un error Ortografico. O rellena los campos vacios");
                }
            }
            else
            {
                MessageBox.Show("No tienes conexion con el servidor");
            }
        }

        private void SignIn_Click(object sender, EventArgs e)
        {
            if (serverRun == true)
            {
                if (textUsu.Text.Length != 0 && textContra.Text.Length != 0)
                {
                    user = textUsu.Text;
                    contra = textContra.Text;
                    string mensaje = "2/" + textUsu.Text + "/" + textContra.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    ////Recibimos la respuesta del servidor
                    //byte[] msg2 = new byte[80];
                    //int bytesRecibidos = server.Receive(msg2); // Número real de bytes recibidos

                    //// Convertimos solo los bytes útiles y eliminamos espacios extra
                    //string mensajeRecibido = Encoding.ASCII.GetString(msg2, 0, bytesRecibidos).Trim();

                    //// Mostramos el mensaje recibido
                    //MessageBox.Show(mensajeRecibido);

                    //// Comparamos correctamente
                    //if (mensajeRecibido.Equals("Sesion Iniciada exitosamente", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    iniciado = true;

                    //    foreach (Control control in this.Controls)
                    //    {
                    //        control.Visible = false;

                    //    }
                    //    crearFondoAsync();
                    //    //this.FormBorderStyle = FormBorderStyle.None;
                    //}

                }
                else
                    MessageBox.Show("Rellena los campos que estan vacios");
            }
            else
            {
                MessageBox.Show("No tienes conexion con el servidor");
            }


        }

        private void textUsu_TextChanged(object sender, EventArgs e)
        {

        }

        private void textContra_Enter(object sender, EventArgs e)
        {
            contraseñaBox.ForeColor = Color.FromArgb(38, 209, 255);
            contraseñaBox.BorderColor = Color.FromArgb(38, 209, 255);
            contraseñaBox.Text = "Contraseña";
            if (textContra.Text == "Contraseña")
            {
                textContra.Text = "";

            }
        }

        private void textContra_Leave(object sender, EventArgs e)
        {
            contraseñaBox.ForeColor = Color.FromArgb(255, 255, 255);
            contraseñaBox.BorderColor = Color.FromArgb(255, 255, 255);

            if (textContra.Text.Length == 0)
            {
                textContra.Text = "Contraseña";
                contraseñaBox.Text = "";

            }


        }

        private void textUsu_Enter(object sender, EventArgs e)
        {
            usuarioBox.ForeColor = Color.FromArgb(38, 209, 255);
            usuarioBox.BorderColor = Color.FromArgb(38, 209, 255);
            usuarioBox.Text = "Usuario";
            if (textUsu.Text == "Usuario")
            {
                textUsu.Text = "";
            }
        }

        private void textUsu_Leave(object sender, EventArgs e)
        {
            usuarioBox.ForeColor = Color.FromArgb(255, 255, 255);
            usuarioBox.BorderColor = Color.FromArgb(255, 255, 255);


            if (textUsu.Text.Length == 0)
            {
                usuarioBox.Text = "";
                textUsu.Text = "Usuario";
            }

        }
        private void repiteContra_Enter(object sender, EventArgs e)
        {
            repiteContraBox.ForeColor = Color.FromArgb(38, 209, 255);
            repiteContraBox.BorderColor = Color.FromArgb(38, 209, 255);
            repiteContraBox.Text = "Repite la contraseña";
            if (repiteContra.Text == "Repite la contraseña")
            {
                repiteContra.Text = "";
            }
        }

        private void repiteContra_Leave(object sender, EventArgs e)
        {
            repiteContraBox.ForeColor = Color.FromArgb(255, 255, 255);
            repiteContraBox.BorderColor = Color.FromArgb(255, 255, 255);


            if (repiteContra.Text.Length == 0)
            {
                repiteContraBox.Text = "";
                repiteContra.Text = "Repite la contraseña";
            }
        }

        private void SignIn_MouseEnter(object sender, EventArgs e)
        {
            iniciarSesionBox.BorderColor = Color.FromArgb(38, 209, 255);
            SignIn.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void SignIn_MouseLeave(object sender, EventArgs e)
        {
            iniciarSesionBox.BorderColor = Color.FromArgb(255, 255, 255);
            SignIn.ForeColor = Color.FromArgb(255, 255, 255);
        }


        private Color circleColor = Color.Blue;

        private void circuloServidor_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int diameter = Math.Min(circuloServidor.Width, circuloServidor.Height) - 10;
            int x = (circuloServidor.Width - diameter) / 2;
            int y = (circuloServidor.Height - diameter) / 2;

            g.FillEllipse(new SolidBrush(circleColor), x, y, diameter, diameter);
            g.DrawEllipse(Pens.Black, x, y, diameter, diameter);
        }

        public void ChangeCircleColor(Color newColor)
        {
            circleColor = newColor;
            circuloServidor.Invalidate();
        }

        private void circuloServidor_Click(object sender, EventArgs e)
        {
            ChangeCircleColor(Color.Blue);
            ConectarServidor();
        }

        private void cambiarInicioRegistro()
        {
            textUsu.Text = "Usuario";
            textContra.Text = "Contraseña";
            repiteContra.Text = "Repite la contraseña";
            this.ActiveControl = null;
            usuarioBox.Text = "";
            contraseñaBox.Text = "";
            repiteContraBox.Text = "";

            if (!registro)
            {

                iniciarSesionBox.Visible = false;
                registroBox.Visible = true;
                registro = true;
                repiteContraBox.Visible = true;
                aunNoCuenta.Text = "¿Ya tienes cuenta?";

            }
            else
            {
                iniciarSesionBox.Visible = true;
                registroBox.Visible = false;
                repiteContraBox.Visible = false;
                registro = false;
                aunNoCuenta.Text = "¿Aún no tienes cuenta?";

            }

        }

        private void aunNoCuenta_Click(object sender, EventArgs e)
        {
            cambiarInicioRegistro();

        }


        private void SignUp_MouseEnter(object sender, EventArgs e)
        {
            registroBox.BorderColor = Color.FromArgb(38, 209, 255);
            SignUp.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void SignUp_MouseLeave(object sender, EventArgs e)
        {
            registroBox.BorderColor = Color.FromArgb(255, 255, 255);
            SignUp.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void nuevaPartida_MouseEnter(object sender, EventArgs e)
        {
            nuevaPartidaBox.BorderColor = Color.FromArgb(38, 209, 255);
            nuevaPartida.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void nuevaPartida_MouseLeave(object sender, EventArgs e)
        {
            nuevaPartidaBox.BorderColor = Color.FromArgb(255, 255, 255);
            nuevaPartida.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void cargarPartida_MouseEnter(object sender, EventArgs e)
        {
            cargarPartidaBox.BorderColor = Color.FromArgb(38, 209, 255);
            cargarPartida.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void cargarPartida_MouseLeave(object sender, EventArgs e)
        {
            cargarPartidaBox.BorderColor = Color.FromArgb(255, 255, 255);
            cargarPartida.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void combatir_MouseEnter(object sender, EventArgs e)
        {
            combatirBox.BorderColor = Color.FromArgb(38, 209, 255);
            combatir.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void combatir_MouseLeave(object sender, EventArgs e)
        {
            combatirBox.BorderColor = Color.FromArgb(255, 255, 255);
            combatir.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void salirJuego_MouseEnter(object sender, EventArgs e)
        {
            salirJuegoBox.BorderColor = Color.FromArgb(38, 209, 255);
            salirJuego.ForeColor = Color.FromArgb(38, 209, 255);
        }

        private void salirJuego_MouseLeave(object sender, EventArgs e)
        {
            salirJuegoBox.BorderColor = Color.FromArgb(255, 255, 255);
            salirJuego.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void salirJuego_MouseClick(object sender, MouseEventArgs e)
        {
            DesconectarServidor();
            Application.Exit();
        }

        private void combatir_MouseClick(object sender, MouseEventArgs e)
        {

            if (boolPanelCargarCombate==true)
            {

                if (user != null)
                {
                    panelCargarCombate.Visible = true;
                    boolPanelCargarCombate = false;
                    Conectados.DibujarConectados(listaConectadosGlobal, panelCargarCombate, this, user, server);
                }
                else
                {
                    MessageBox.Show("Ha habido un error al buscar las partidas del jugador " + user);
                }
            }
            else
            {
                boolPanelCargarCombate = true;
                panelCargarCombate.Visible = false;
            }

        }

        public static void ManejarClickCarta(CartaPokemon carta)
        {
            string mensaje = "5/" + carta.Nombre;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            // Cambiar 'server' a 'Form1.server' y hacerlo accesible
            Form1 form = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            form?.server.Send(msg);

            ////Recibimos la respuesta del servidor
            //byte[] msg2 = new byte[1000];
            //form?.server.Receive(msg2);
            //mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

            //MessageBox.Show(mensaje);
        }



        private void cargarPartida_Click(object sender, EventArgs e)
        {
            if (boolPanelCargarPartida)
            {
                int PanelSizeX = 500;
                int PanelSizeY = 300;

                panelCargarPartida = new PanelDobleBuffer
                {
                    Size = new Size(PanelSizeX, PanelSizeY), // Tamaño ajustado
                    Location = new Point(cargarPartidaBox.Left + cargarPartidaBox.Width + 10, cargarPartidaBox.Top + (cargarPartidaBox.Height / 2) - (PanelSizeY / 2)),
                    BackColor = Color.Black,
                };
                this.Controls.Add(panelCargarPartida);
                panelCargarPartida.Visible = true;
                panelCargarPartida.BringToFront();
                boolPanelCargarPartida = false;


                // Configurar el evento Paint para aplicar bordes redondeados y degradado
                panelCargarPartida.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
                {
                    PanelDobleBuffer panel = (PanelDobleBuffer)senderPanel;
                    int radio = 20; // Radio para las esquinas redondeadas
                    Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

                    // Crear la ruta con esquinas redondeadas
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
                        path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
                        path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
                        path.CloseFigure();

                        // Asigna la región redondeada al panel
                        panelCargarPartida.Region = new Region(path);

                        // Configurar el suavizado
                        ePanel.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Crear un pincel de degradado: de blanco a un gris muy claro
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            rect,
                            Color.Black,
                            Color.FromArgb(22, 22, 22),
                            LinearGradientMode.Vertical))
                        {
                            // Rellenar la ruta con el degradado
                            ePanel.Graphics.FillPath(brush, path);
                        }

                        // Dibujar un borde:
                        using (Pen pen = new Pen(Color.FromArgb(38, 209, 255), 4))
                        {
                            ePanel.Graphics.DrawPath(pen, path);
                        }


                    }
                });

                if (user != null)
                {
                    string mensaje = "4/" + user;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    ////Recibimos la respuesta del servidor
                    //byte[] msg2 = new byte[1000];
                    //server.Receive(msg2);
                    //mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                    //MessageBox.Show(mensaje);

                    //List<Partida> listaPartidas = new List<Partida>();
                    //listaPartidas = Partida.ParsearRespuesta(mensaje, listaPartidas);
                    //if (listaPartidas.Count > 0)
                    //{
                    //    Partida.DibujarPartidas(listaPartidas, panelCargarPartida);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("No tienes partidas");
                    //}
                }
                else
                {
                    MessageBox.Show("Ha habido un error al buscar las partidas del jugador " + user);
                }
            }
            else
            {
                boolPanelCargarPartida = true;
                panelCargarPartida.Visible = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serverRun == true)
            {
                string mensaje = "7/";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[1000];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                MessageBox.Show(mensaje);
            }
            else
            {
                MessageBox.Show("Sin conexion al servidor");
            }
        }

        private void crearChat()
        {
            int padding = 20;
            int buttonSize = 50;
            int textBoxWidth = 300;
            int textBoxHeight = 50;
            int historyHeight = 300;

            // Botón de enviar (redondeado con clase personalizada RoundButton)
            enviarMensaje = new RoundButton
            {
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(this.Width - padding - buttonSize, this.Height - padding - buttonSize),
                Image = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "enviar.png")),
                BackColor = Color.FromArgb(88, 88, 88)
            };

            // TextBox de mensaje con borde redondeado usando Panel
            var panelTextBox = new Panel
            {
                Size = new Size(textBoxWidth, textBoxHeight),
                Location = new Point(enviarMensaje.Left - textBoxWidth - 10, enviarMensaje.Top),
                BackColor = Color.FromArgb(66, 66, 66),
                Padding = new Padding(5)
            };

            textBoxMensaje = new System.Windows.Forms.TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(66, 66, 66),
                ForeColor = Color.White,
                Multiline = true,
                MaxLength = 100,
                Dock = DockStyle.Fill
            };

            panelTextBox.Controls.Add(textBoxMensaje);
            redondearPanel(panelTextBox, 10); // Método para redondear

            // RichTextBox historial (envuelto también en panel redondeado)
            panelChat = new PanelDobleBuffer
            {
                Size = new Size(textBoxWidth, historyHeight),
                Location = new Point(panelTextBox.Left, panelTextBox.Top - historyHeight - 20),
                BackColor = Color.FromArgb(44, 44, 44),
                Padding = new Padding(5)
            };

            historialMensajes = new RichTextBox
            {
                Size = new Size(panelChat.Width - 10, panelChat.Height - 10),
                Location = new Point(5, 5),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(44, 44, 44),
                ForeColor = Color.White,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                WordWrap = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            panelChat.Controls.Add(historialMensajes);
            redondearPanel(panelChat, 10);

            // Agregar controles
            this.Controls.Add(panelChat);
            this.Controls.Add(panelTextBox);
            this.Controls.Add(enviarMensaje);

            // Evento de enviar mensaje
            enviarMensaje.Click += enviarMensaje_Click;

            historialMensajes.Visible = true;
            enviarMensaje.Visible = true;
            textBoxMensaje.Visible = true;

            panelChat.BringToFront();
            enviarMensaje.BringToFront();
            panelTextBox.BringToFront();
            historialMensajes.ReadOnly = true;
            historialMensajes.HideSelection = false; // Permite seleccionar el texto en el RichTextBox
            historialMensajes.ScrollBars = RichTextBoxScrollBars.Vertical; // Agrega una barra de desplazamiento vertical
            historialMensajes.WordWrap = true; // Permite el ajuste de línea

            // Mostrar mensaje de prueba
            MessageBox.Show("Chat creado");
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


        //Metodos para el xat
        private void enviarMensaje_Click(object sender, EventArgs e)
        {
            //Puede ser que textUsu cuando se estconde ya no se puede aceder a su contenido
            string mensaje = $"7/{textUsu.Text}/{textBoxMensaje.Text}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBoxMensaje.Clear(); // Limpiar el TextBox después de enviar el mensaje
        }
        void HistorialMensajes(string nuevoMensaje)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
            string entrada = $"[{timeStamp}] {nuevoMensaje}{Environment.NewLine}";
            historialMensajes.AppendText(entrada);
            historialMensajes.SelectionStart = historialMensajes.Text.Length;
            historialMensajes.ScrollToCaret();
            panelChat.Update();
        }
    }
}

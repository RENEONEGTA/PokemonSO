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
using System.Runtime.CompilerServices;

namespace WindowsFormsApplication1
{
    //Tiempos de video de fondo: 00:00:00 - 00:01:58,  00:02:00 - 00:02:52, 00:02:53 - 00:04:07, 00:04:09 - 00:06:54, 00:06:57 - 00:08:35, 00:08:36 - 00:10:59, 00:11:01 - 00:12:16

    public partial class Form1 : Form
    {
        Socket server;
        Socket server2;
        FormJuego formJuego = new FormJuego();
        private int puertoServidor = 9020; // Puerto del servidor
        private int puertoServidor2 = 9040;
        private Timer parpadeoTimer = new Timer();
        private bool serverRun = false;
        private bool serverRun2 = false;
        private bool colorAzul = true;
        private bool registro = false;
        private GestorCratas gestorCartas = new GestorCratas();
        private Pokemon Pokemon = new Pokemon();
        private Partida Partida = new Partida();
        private Conectados Conectados = new Conectados();
        private Combate Combate = new Combate();
        private bool combate = false;

        Menu menu = new Menu();
        private PanelDobleBuffer panelElegirPokemon;
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
        bool JugadorNuevo = false;
        bool Pokedex =true;
        public string NuevoPokemon;
        List<Conectados> listaConectadosGlobal = new List<Conectados>();
        RichTextBox historialMensajes = new RichTextBox();
        System.Windows.Forms.TextBox textBoxMensaje = new System.Windows.Forms.TextBox();
        RoundButton enviarMensaje = new RoundButton();
        PanelDobleBuffer panelChat = new PanelDobleBuffer();
        RoundButton Charmander = new RoundButton();
        RoundButton Squirtel = new RoundButton();   
        RoundButton Bulbasaur = new RoundButton();
        bool escogerPokemon = false;
        public delegate void ConectadosActualizadosHandler(List<Conectados> conectados);
        public event ConectadosActualizadosHandler ConectadosActualizados;
        Dictionary<int, JugadorInfo> jugadoresEnPartida = new Dictionary<int, JugadorInfo>();
        int userId = 0; // ID del jugador local
        List<Pokemon> listaPokedex = new List<Pokemon>();
        PanelDobleBuffer PanelSobre = new PanelDobleBuffer();

        Mapa mapa;
        Jugador jugador;
        PanelDobleBuffer panelMapa;


        public Form1()
        {
            InitializeComponent();
            parpadeoTimer.Interval = 500; // Parpadeo cada 500 ms
            parpadeoTimer.Tick += ParpadeoTimer_Tick;

            //this.FormBorderStyle = FormBorderStyle.None; // Quitar la barra de título y botones
            //this.WindowState = FormWindowState.Maximized; // Maximizar el formulario
            //this.ControlBox = false; //Quitar los controles 
            //this.StartPosition = FormStartPosition.CenterScreen; //Centrar el formulario
            //this.ShowInTaskbar = false; // Esconder la taskbar
            //this.FormBorderStyle = FormBorderStyle.None; //Quitar el borderstyle

            AbrirSobre.Visible = false;

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ChangeCircleColor(Color.Blue);
            ConectarServidor();
            ConectarServidor2();
            AtenderServidorPrincipal();
            
            this.SizeChanged += Form1_SizeChanged;

            // Ejecutar la función después de que el formulario se muestre
            this.BeginInvoke((MethodInvoker)delegate {
                EscalarControles(); // o Refresh general
            });
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (iniciado == true)
            {
                AjustarTamaño();
            }
        }

        private void AjustarTamaño()
        {
            menu.AjustarPanelMenu();
          
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

        
        private void EscalarControles()
        {
            aunNoCuenta.Location = new Point(iniciarSesionBox.Left + (iniciarSesionBox.Width-aunNoCuenta.Width)/2, iniciarSesionBox.Bottom + 30);
            this.Invalidate();
            this.Update();
            this.Refresh();
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

        private void crearPanelCombate()
        {
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
            Cerrar.Visible = false;
            AbrirSobre.Visible = true;
            menu.contenedorMenu.Visible = true; // Mostrar el menú


            crearPanelCombate();

            
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

        private void Inicio()
        {
            Invoke((MethodInvoker)delegate
            {
                iniciado = true;
                //MessageBox.Show("Se ha iniciado la sesion con exito");

                //Creacion de menu al obtener el socket para poder usar el chat

                menu.CrearMenu(false, this, server, user);
                menu.contenedorMenu.Visible = false; // Ocultar el menú al inicio
                AjustarTamaño();

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

        private async void AtenderServidorPrincipal()
        {
            await Task.Run(() =>
            {
                while (true)
                {

                    if (serverRun == true)
                    {
                        //recivimos mensake del servidor
                        try
                        {
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

                                            case 2: // Iniciado Sesión

                                                // El mensaje que llega es del tipo "1/42"
                                                string[] partesInicio = mensaje.Split('/');

                                                // Verificamos que tiene al menos 2 partes: el estado y el userId
                                                if (partesInicio.Length >= 2)
                                                {
                                                    int estado, idJugador;

                                                    // Intentamos convertir ambas partes a enteros
                                                    if (int.TryParse(partesInicio[0], out estado) && int.TryParse(partesInicio[1], out idJugador))
                                                    {
                                                        if (estado == 1)
                                                        {
                                                            // Guardamos el ID del jugador local
                                                            userId = idJugador;
                                                            this.user = textUsu.Text; // Actualizamos el TextBox con el usuario

                                                            // Llamamos a la función de inicio
                                                            Inicio();

                                                            CargarPokedex();
                                                            // Solo enviamos el mensaje DESPUÉS de conectar con éxito.
                                                            string mensajeservidor = "0/" + user + "/" + userId;
                                                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensajeservidor);
                                                            server2.Send(msg);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("No se ha podido iniciar sesión.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Error al interpretar los datos del servidor.");
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Formato de mensaje inválido.");
                                                }

                                                break;


                                            case 3: //Pokedex

                                               
                                                if (formJuego != null && !formJuego.IsDisposed)
                                                {
                                                    // Si el formulario del juego está abierto, le pasamos los datos a él
                                                    formJuego.MostrarPokedex(mensaje);
                                                }
                                                

                                                break;

                                            case 30:

                                                MessageBox.Show("Error al importar la Pokedex");
                                                break;

                                            case 31:

                                                Invoke((MethodInvoker)delegate
                                                {
                                                    listaPokedex = Pokemon.ParsearDatos(mensaje, listaPokedex);
                                                });
                                                break;


                                            case 4: //Lista de Partidas Donde esta el Jugador

                                                //MessageBox.Show(mensaje);

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
                                            case 7:
                                                break;
                                            case 8:
                                                break;
                                            case 9:
                                                break;

                                            case 10: //Abrir Sobre
                                            
                                                Invoke((MethodInvoker)delegate
                                                {
                                                    MostrarSobre(Convert.ToInt32(mensaje));
                                                });
                                                break;                                            

                                            case 100: //Lista Conectados Notificacion
                                                //MessageBox.Show(mensaje);
                                                Invoke((MethodInvoker)delegate
                                                {
                                                    listaConectadosGlobal.Clear();

                                                    listaConectadosGlobal = Conectados.ParsearDatos(mensaje, listaConectadosGlobal);
                                                    if (boolPanelCargarCombate == false)
                                                    {
                                                        Conectados.DibujarConectados(listaConectadosGlobal, panelCargarCombate, this, user, server);
                                                    }
                                                    Conectados.DibujarConectadosEnLista(listaConectadosGlobal, menu.panelAmigos, this, user, server, menu.panelAmigos.Width, menu.panelAmigos.Height, 0);
                                                    ConectadosActualizados?.Invoke(listaConectadosGlobal);
                                                });
                                                break;

                                            case 101: //Recivimos Notificacion del Chat

                                                Invoke((MethodInvoker)delegate
                                                {
                                                    string texto = mensaje.ToString();                                     

                                                    // Mostrar mensaje en el formulario de Partidas si está abierto
                                                    if (formJuego != null && !formJuego.IsDisposed && formJuego.menu != null)
                                                        formJuego.menu.HistorialMensajes(texto);

                                                    // Mostrar mensaje en el formulario de Principal si está abierto
                                                    if (this != null && !this.IsDisposed && this.menu != null)
                                                        this.menu.HistorialMensajes(texto);
                                                });
                                                break;

                                            case 102: //Invitacion Combate


                                                string[] invitarPartes = mensaje.Split('/');

                                                if (invitarPartes.Length >= 3 &&
                                                    int.TryParse(invitarPartes[0], out int idInvitador) &&
                                                    int.TryParse(invitarPartes[2], out int idPartida))
                                                {
                                                    string nombreInvitador = invitarPartes[1];

                                                    // Buscar al jugador en la lista global
                                                    Conectados jugadorInvitador = listaConectadosGlobal
                                                        .FirstOrDefault(c => c.Id == idInvitador);

                                                    Invoke((MethodInvoker)delegate
                                                    {
                                                        if (jugadorInvitador == null)
                                                        {
                                                            // Si no está, lo creamos manualmente con los datos recibidos
                                                            jugadorInvitador = new Conectados
                                                            {
                                                                Id = idInvitador,
                                                                Nombre = nombreInvitador
                                                            };
                                                        }

                                                        Conectados.RecibirInvitacionMundo(jugadorInvitador, this, server2, idPartida);
                                                    });
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Error al interpretar la invitación: formato incorrecto.");
                                                }


                                                break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al recibir el mensaje del servidor: " + ex.Message);

                            }
                        }
                        catch (SocketException ex)
                        {
                            // Si hay un error de socket, mostramos un mensaje y salimos del bucle
                            Invoke((MethodInvoker)delegate
                            {
                                MessageBox.Show("Error de conexión con el servidor: " + ex.Message);
                                serverRun = false;
                                Task.CompletedTask.Wait();
                            });
                            break; // Salir del bucle si hay un error de conexión
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
                serverRun = false; // Cambiamos el estado de la conexión
            }
            else if (serverRun == true)
            {
                string mensaje = "0/" + user + "/" + contra;
                // Enviamos al servidor el nombre 0
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                serverRun = false; // Cambiamos el estado de la conexión
            }
        }

        private void Form1_Form1Closing(object sender, FormClosingEventArgs e)
        {
            DesconectarServidor();
        }

        private void ParpadeoTimer_Tick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                ChangeCircleColor(colorAzul ? Color.Blue : Color.Green);
                colorAzul = !colorAzul;
            });
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
                    JugadorNuevo = true;

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
            if (JugadorNuevo == true)
            {
                MessageBox.Show("No tienes pokemons");
                PrimerPokemon();
                JugadorNuevo = false;
            }
            else
            {
                if (boolPanelCargarCombate == true)
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
        }

        public static void MostrarInformacionCarta(CartaPokemon carta)
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
            if (JugadorNuevo == true)
            {
                MessageBox.Show("No tienes pokemons");
                PrimerPokemon();
                JugadorNuevo = false;
            }
            else
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

        private void redondearPanelDobleBuffer(PanelDobleBuffer panel, int radio)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(panel.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(panel.Width - radio, panel.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, panel.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            panel.Region = new Region(path);
        }


        public static void AgregarPokemon(CartaPokemon cartaPokemon)
        {   
            // Cambiar 'server' a 'Form1.server' y hacerlo accesible
            Form1 form = Application.OpenForms.OfType<Form1>().FirstOrDefault();

            //Enviamos que pokemon queremos agregar y a quien 
            string mensaje = $"8/" + cartaPokemon.Nombre + "/" + form?.user;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            
            form?.server.Send(msg);
            form?.Controls.Remove(form?.panelElegirPokemon);
            form?.panelElegirPokemon.Dispose();
        }

        private void PrimerPokemon()
        {
            escogerPokemon = true;
            int ancho = 770;
            int alto = 410;

            panelElegirPokemon = new PanelDobleBuffer
            {
                Size = new Size(ancho, alto),
                Location = new Point(this.Width / 2 - (ancho/2), this.Height / 2 - (alto/2)),
                BackColor = Color.FromArgb(44, 44, 44),
                Padding = new Padding(5)
            };
            redondearPanelDobleBuffer(panelElegirPokemon, 20); // Método para redondear
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

        private void nuevaPartida_Click(object sender, EventArgs e)
        {
            
            if(serverRun2 == true)
            {
                //Enviamos para crear partida
                string mensaje = "91/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server2.Send(msg);
            }
            else
            {
                MessageBox.Show("No se ha establecido conexion con el servidor de partidas");
            }
        }

        public void CrearPartida(int id)
        {
            if (formJuego == null || formJuego.IsDisposed)
            {
                formJuego = new FormJuego(); 
            }
            if (JugadorNuevo == true)
            {
                MessageBox.Show("No tienes pokemons");
                PrimerPokemon();
                JugadorNuevo = false;
            }
            else
            {
                if (server2 == null || !server2.Connected)
                {
                    // Este método ya lo tienes, se encarga de conectar.
                    ConectarServidor2();
                }

                // Enviamos un mensaje para notificar que nos hemos unido a la instancia de la partida.
                // Usamos el nuevo código 89.
                string mensaje = $"90/{id}";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                // Usamos un bloque try-catch por si la conexión aún no está lista.
                try
                {
                    server2.Send(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo notificar la unión a la partida: " + ex.Message);
                }

                formJuego.user = user;
                formJuego.server = server;
                formJuego.server2 = server2;
                formJuego.idPartida = id;
                formJuego.userId = userId;
                formJuego.listaPokedex = this.listaPokedex;



                // Enlazar al evento utilizando una instancia del formulario  
                this.ConectadosActualizados += formJuego.ActualizarListaConectados;
                // Enviarle la lista actual si ya tiene datos
                if (listaConectadosGlobal.Count > 0)
                {
                    formJuego.ActualizarListaConectados(listaConectadosGlobal);
                }
                formJuego.Show();
                
                //MessageBox.Show(Convert.ToString(id));
            }
        }

        private void Atacar(Pokemon pokemon)
        {
            //Enviamos el ataque utilizado
            string mensaje = $"9/" + Convert.ToString(pokemon.Daño);
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private int RecibirAtaque()
        {
            string mensaje;
            byte[] msg2 = new byte[1000];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2);
            int dano = Convert.ToInt32(mensaje);
            return dano;
        }

        private void Cerrar_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Metodo para Abrir un Sobre de Pokemons
        private void AbrirSobre_Click(object sender, EventArgs e)
        {
            string mensaje = "10/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void CargarPokedex()
        {
            string mensaje = "30/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void MostrarSobre(int idP)
        {
            int ancho = 300;
            int alto = 400;
            List<CartaPokemon> lista = new List<CartaPokemon>();
            PanelSobre = new PanelDobleBuffer
            {
                Size = new Size(ancho, alto),
                Location = new Point(this.Width / 2 - (ancho / 2), this.Height / 2 - (alto / 2)),
                BackColor = Color.FromArgb(44, 44, 44),
                Padding = new Padding(5)
            };
            redondearPanelDobleBuffer(PanelSobre, 20); // Método para redondear
            this.Controls.Add(PanelSobre);
            PanelSobre.BringToFront();
            PanelSobre.Visible = true;

            int Cancho = 0;
            int Calto = 0;
            PanelDobleBuffer panelcarta = new PanelDobleBuffer
            {
                Size = new Size(Cancho,Calto),
                Location = new Point(PanelSobre.Width / 2 - (Cancho / 2), PanelSobre.Height / 2 - (Calto / 2)),
                BackColor = Color.FromArgb(44,44,44)
            };
            PanelSobre.Controls.Add(panelcarta);
            PanelSobre.BringToFront(); 
            PanelSobre.Visible = true;

            List<CartaPokemon> sobre = new List<CartaPokemon>();

            foreach (Pokemon pokemon in listaPokedex)
            {
                if (pokemon.Id == idP)
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
                    sobre.Add(carta); // Agregar la carta a la lista
                }
            }

            gestorCartas.DibujarCartas(sobre, panelcarta, true, false);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            EscalarControles();
        }

        public void MostrarIniciarSesion()
        {
            DesconectarServidor();
            //volver todos los constroles invisibles
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }
            // Mostrar el panel de inicio de sesión
            iniciarSesionBox.Visible = true;
            SignIn.Visible = true;

            usuarioBox.Visible = true;
            textUsu.Visible = true;
            textUsu.Text = "Usuario";

            contraseñaBox.Visible = true;
            textContra.Visible = true;
            textContra.Text = "Contraseña";

            aunNoCuenta.Visible = true;
            circuloServidor.Visible = true;
            IP.Visible = true;
            pictureBox1.Visible = true;
            Cerrar.Visible = true;  
            this.WindowState = FormWindowState.Normal;
            ConectarServidor();
            AtenderServidorPrincipal();

            registro = false;
            iniciado = false;

        }

        private async void ConectarServidor2()
        {
            parpadeoTimer.Start(); // Iniciar el parpadeo
            await Task.Run(() =>
            {

                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(IP.Text);
                IPEndPoint ipep = new IPEndPoint(direc, puertoServidor2);


                //Creamos el socket 
                server2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server2.Connect(ipep);//Intentamos conectar el socket

                    Invoke((MethodInvoker)delegate
                    {
                        serverRun2 = true;

                        AtenderServidorDePartidas();

                    });
                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    Invoke((MethodInvoker)delegate
                    {
                        ChangeCircleColor(Color.Red);
                        parpadeoTimer.Stop(); // Detener el parpadeo
                        MessageBox.Show("Error de conexión con el segundo servidor: " + ex.Message);
                        serverRun2 = false;
                        Task.CompletedTask.Wait();

                    });
                    return;
                }
            });

        }
        // En Form1.cs, puedes añadir este método después de AtenderServidorPrincipal

        private async void AtenderServidorDePartidas()
        {
            // Búfer para acumular los datos que llegan
            string bufferAcumulado = "";
            string delimitador = "<EOM>";

            await Task.Run(() =>
            {
                while (true)
                {
                    // Escuchamos solo si la conexión con el servidor 2 está activa
                    if (serverRun2 == true && server2 != null && server2.Connected)
                    {
                        try
                        {
                            byte[] msg2 = new byte[4096];
                            // La clave: Recibimos mensajes del socket 'server2'
                            int bytesRecibidos = server2.Receive(msg2);


                            if (bytesRecibidos > 0)
                            {
                                bufferAcumulado += Encoding.ASCII.GetString(msg2, 0, bytesRecibidos);
                                // Procesamos todos los mensajes completos que haya en el búfer
                                while (bufferAcumulado.Contains(delimitador))
                                {
                                    // Encontramos la posición del primer delimitador
                                    int indiceDelimitador = bufferAcumulado.IndexOf(delimitador);

                                    // Extraemos el mensaje completo desde el inicio hasta el delimitador
                                    string mensajeCompleto = bufferAcumulado.Substring(0, indiceDelimitador);
                                    bufferAcumulado = bufferAcumulado.Substring(indiceDelimitador + delimitador.Length);

                                   
                                    string[] partes = mensajeCompleto.Split(new string[] { "~$" }, StringSplitOptions.None);
                                    if (partes.Length < 2) continue;

                                    int codigo = Convert.ToInt32(partes[0].Trim());
                                    string mensaje = partes[1].Trim();

                                    // Este switch solo manejará códigos del servidor de partidas
                                    switch (codigo)
                                    {
                                        case 90: // Sincronizar la lista inicial de Pokémon en el mapa

                                            if (formJuego != null && !formJuego.IsDisposed)
                                            {
                                                Invoke((MethodInvoker)delegate
                                                {
                                                    formJuego.SincronizarPokemonesEnMapa(mensaje);
                                                });
                                            }
                                            break;

                                        case 91: // Respuesta de Crear Partida (ahora desde server2)
                                            CrearPartida(Convert.ToInt32(mensaje));
                                            break;

                                        case 94: // Actualización de coordenadas de otros jugadores
                                            string[] partesJugadores = mensaje.Split('/');
                                            Invoke((MethodInvoker)delegate
                                            {
                                                foreach (string entrada in partesJugadores)
                                                {
                                                    if (string.IsNullOrWhiteSpace(entrada)) continue;
                                                    string[] datosPos = entrada.Split(':');
                                                    if (datosPos.Length != 3) continue;

                                                    int id = int.Parse(datosPos[0]);
                                                    int x = int.Parse(datosPos[1]);
                                                    int y = int.Parse(datosPos[2]);

                                                    if (Application.OpenForms.OfType<FormJuego>().FirstOrDefault() is FormJuego fj)
                                                    {
                                                        fj.ActualizarJugadorRemoto(id, x, y);
                                                    }
                                                }
                                            });

                                            break;

                                        case 95: // Spawn de un nuevo Pokémon

                                            if (formJuego != null && !formJuego.IsDisposed)
                                            {
                                                Invoke((MethodInvoker)delegate
                                                {

                                                    formJuego.AnadirPokemonAlMapa(mensaje);
                                                });

                                            }
                                            break;

                                        case 96: // Despawn de un Pokémon
                                            if (formJuego != null && !formJuego.IsDisposed)
                                            {
                                                Invoke((MethodInvoker)delegate
                                                {
                                                    formJuego.EliminarPokemonDelMapa(mensaje);
                                                });

                                            }
                                            break;

                                        case 97: // Un Pokémon se ha movido
                                            if (formJuego != null && !formJuego.IsDisposed)
                                            {
                                                Invoke((MethodInvoker)delegate
                                                {
                                                    formJuego.ActualizarDestinoPokemon(mensaje);
                                                });

                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        catch (SocketException)
                        {
                            // Si hay un error, el socket probablemente se cerró.
                            serverRun2 = false;
                        }
                        catch (Exception)
                        {
                            // Otra excepción, podríamos querer registrarla
                        }
                    }
                    else
                    {
                        // Pequeña pausa para no consumir 100% de CPU si el server no está conectado
                        System.Threading.Thread.Sleep(100);
                    }
                }
            });
        }
    }
}


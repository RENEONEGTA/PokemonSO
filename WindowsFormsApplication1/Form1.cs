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

namespace WindowsFormsApplication1
{
    //Tiempos de video de fondo: 00:00:00 - 00:01:58,  00:02:00 - 00:02:52, 00:02:53 - 00:04:07, 00:04:09 - 00:06:54, 00:06:57 - 00:08:35, 00:08:36 - 00:10:59, 00:11:01 - 00:12:16

    public partial class Form1 : Form
    {
        Socket server;
        private Timer parpadeoTimer = new Timer();
        private bool serverRun = false;
        private bool colorAzul = true;
        private bool registro = false;
        private GestorCratas gestorCartas = new GestorCratas();
        private bool combate = false;
        private PanelDobleBuffer panelCartas;
        private bool panelCreado = false;
        int PokedexLocationX = 700; // Posición X de la Pokeball
        int PokedexLocationY = 700; // Posición Y de la Pokeball
        int panelCartasTop = 0; // Posición Top del panel de cartas


        public Form1()
        {
            InitializeComponent();
            parpadeoTimer.Interval = 500; // Parpadeo cada 500 ms
            parpadeoTimer.Tick += ParpadeoTimer_Tick;
            

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


        }

        private void crearFondo()
        {
            fondoPokemon.Visible = true;
            fondoPokemon.uiMode = "none"; // Ocultar controles de reproducción
            // Rutas del video
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", "FondoPokemon.mp4");

            // Tiempos de video (tiempo de inicio, tiempo de fin) en segundos
            List<Tuple<int, int>> tiempos = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(0, 118),   // 00:00:00 - 00:01:58
                new Tuple<int, int>(120, 172), // 00:02:00 - 00:02:52
                new Tuple<int, int>(173, 247), // 00:02:53 - 00:04:07
                new Tuple<int, int>(249, 414), // 00:04:09 - 00:06:54
                new Tuple<int, int>(417, 515), // 00:06:57 - 00:08:35
                new Tuple<int, int>(516, 659), // 00:08:36 - 00:10:59
                new Tuple<int, int>(661, 736)  // 00:11:01 - 00:12:16
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

            PictureBox pokedexBox = new PictureBox
            {
                Size = new Size(100, 100), // Ajusta el tamaño según sea necesario
                Location = new Point(PokedexLocationX, PokedexLocationY),
                SizeMode = PictureBoxSizeMode.StretchImage // Opcional: para ajustar la imagen al tamaño del control
            };
            string pokeballPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "pokeball.png");
            pokedexBox.Image = Image.FromFile(pokeballPath);
            this.Controls.Add(pokedexBox);
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
                    Location = new Point(PokedexLocationX-(PanelSizeX/2)+50, PokedexLocationY-PanelSizeY-20),
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

                        panelCartasTop = panelCartas.Top-50;
                    }
                });
            }

            



            if (!combate)
            {
                List<CartaPokemon> cartas = new List<CartaPokemon>
                {
                    new CartaPokemon("Charmander", 120, "Fuego", "images/Charmander.png", new List<(string, int)> { ("Garrazo", 30), ("Rugido", 10) }),
                    new CartaPokemon("Squirtle", 100, "Agua", "images/Squirtle.png", new List<(string, int)> { ("Mordisco", 40), ("Chapoteo", 15) }),
                    new CartaPokemon("Bulbasaur", 90, "Planta", "images/Bulbasaur.png", new List<(string, int)> { ("Picotazo", 20), ("Remolino", 25) })
                };



                panelCartas.BringToFront();


                combate = true;

                gestorCartas.DibujarCartas(cartas, panelCartas);
                panelCartas.Visible = true;
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
                IPEndPoint ipep = new IPEndPoint(direc, 9050);
            

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

                    });

                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    Invoke((MethodInvoker)delegate
                    {
                        ChangeCircleColor(Color.Red);
                        parpadeoTimer.Stop(); // Detener el parpadeo
                        serverRun = false;

                    });
                    return;
                }
                
            });

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
                    string mensaje = "1/" + textUsu.Text + "," + textContra.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                    
                    MessageBox.Show(mensaje);
                    cambiarInicioRegistro();

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
                    string mensaje = "2/" + textUsu.Text + "," + textContra.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    int bytesRecibidos = server.Receive(msg2); // Número real de bytes recibidos

                    // Convertimos solo los bytes útiles y eliminamos espacios extra
                    string mensajeRecibido = Encoding.ASCII.GetString(msg2, 0, bytesRecibidos).Trim();

                    // Mostramos el mensaje recibido
                    MessageBox.Show(mensajeRecibido);

                    // Comparamos correctamente
                    if (mensajeRecibido.Equals("Sesion Iniciada exitosamente", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (Control control in this.Controls)
                        {
                            control.Visible = false;
                            crearFondo();
                            //this.FormBorderStyle = FormBorderStyle.None;
                        }
                    }

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
            contraseñaBox.ForeColor = Color.FromArgb(38,209,255);
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
            usuarioBox.BorderColor = Color.FromArgb(255 ,255, 255);
           

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
            this.Close();
        }

        private void combatir_MouseClick(object sender, MouseEventArgs e)
        {

            
        }
    }
}

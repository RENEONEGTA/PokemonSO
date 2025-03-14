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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        private Timer parpadeoTimer = new Timer();
        private bool serverRun = false;
        private bool colorAzul = true;

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
                if (textContraR == textConRR && textConRR.Text.Length != 0 && textContraR.Text.Length != 0 && textUsuR.Text.Length != 0)
                {
                    string mensaje = "1/" + textUsuR.Text + "," + textContraR.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                    
                    MessageBox.Show(mensaje);
    
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
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
        
                    MessageBox.Show(mensaje);

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
            contraseñaBox.Text = "";
            if (textContra.Text.Length == 0)
            { 
                textContra.Text = "Contraseña";
                
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
            usuarioBox.Text = "";

            if (textUsu.Text.Length == 0)
            {
                textUsu.Text = "Usuario";
            }
        }

        private void SignIn_MouseEnter(object sender, EventArgs e)
        {
            iniciarSesionBox.BorderColor = Color.FromArgb(38, 209, 255);
        }

        private void SignIn_MouseLeave(object sender, EventArgs e)
        {
            iniciarSesionBox.BorderColor = Color.FromArgb(255, 255, 255);
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
    }
}

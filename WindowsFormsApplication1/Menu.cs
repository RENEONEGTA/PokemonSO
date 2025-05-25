using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Menu
    {
        string user;
        Socket server;
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
        Form form;
        int padding = 10;

        TextBox mensajeChat = new TextBox();
        RichTextBox historialMensajes = new RichTextBox();
        PanelDobleBuffer panelChat = new PanelDobleBuffer();
        PanelDobleBuffer botonCerrarSesion = new PanelDobleBuffer();
        PictureBox btnMenu = new PictureBox();
        public PanelDobleBuffer contenedorMenu = new PanelDobleBuffer();
        public PanelDobleBuffer panelMenu = new PanelDobleBuffer();
        PanelDobleBuffer panelAmigos = new PanelDobleBuffer();
        Label tituloChat = new Label();
        Label tituloAmigos = new Label();
        PanelDobleBuffer panelTextBox = new PanelDobleBuffer();


        public void CrearMenu(bool partida, Form form, Socket server, string user)
        {
            this.form = form;
            this.server = server;
            this.user = user;
            contenedorMenu.Size = new Size(48, 48);
            contenedorMenu.Location = new Point(form.ClientSize.Width - contenedorMenu.Width - 10, 10);
            contenedorMenu.BackColor = Color.FromArgb(22, 22, 22);
            contenedorMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            redondearPanel(contenedorMenu, 10);

            int anchurabotones = 280;
            int alturabotones = 40;

            btnMenu.Size = new Size(48, 48);
            btnMenu.Image = Image.FromFile(directorioBase + "/Resources/images/menuIcono.png");
            btnMenu.SizeMode = PictureBoxSizeMode.Zoom;
            btnMenu.BackColor = Color.Transparent;

            contenedorMenu.Controls.Add(btnMenu);
            btnMenu.BringToFront();
            form.Controls.Add(contenedorMenu);
            contenedorMenu.BringToFront();
            AsignarEventoClick(contenedorMenu);

            panelMenu.Size = new Size(300, form.ClientSize.Height);

            panelMenu.Location = new Point(form.ClientSize.Width, 0);
            panelMenu.BackColor = Color.FromArgb(22, 22, 22);
            redondearPanel(panelMenu, 10);
            panelMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            form.Controls.Add(panelMenu);
            panelMenu.BringToFront();
            panelMenu.Visible = false;

            //Titulo del menú
            Label tituloMenu = new Label
            {
                Text = "Menú",
                ForeColor = Color.White,
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(10, 20),
                AutoSize = true
            };
            panelMenu.Controls.Add(tituloMenu);
            tituloMenu.BringToFront();

            // Zona de amigos
            tituloAmigos = new Label
            {
                Text = "Conectados",
                ForeColor = Color.White,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(10, 80),
                AutoSize = true
            };
            panelMenu.Controls.Add(tituloAmigos);
            tituloAmigos.BringToFront();
            //Panel de amigos

            panelAmigos.Size = new Size(panelMenu.Width - 20, 300);
            panelAmigos.Location = new Point(10, tituloAmigos.Top + tituloAmigos.Height + 10);
            panelAmigos.BackColor = Color.FromArgb(44, 44, 44);
            redondearPanel(panelAmigos, 10);
            panelAmigos.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            panelMenu.Controls.Add(panelAmigos);
            panelAmigos.BringToFront();

            // Boton de eliminar cuenta

            PanelDobleBuffer botonEliminarCuenta = new PanelDobleBuffer
            {
                Size = new Size(anchurabotones, alturabotones),
                Location = new Point(10, form.ClientSize.Height - alturabotones - 30),
                BackColor = Color.FromArgb(220, 38, 38)
            };
            redondearPanel(botonEliminarCuenta, 10);
            panelMenu.Controls.Add(botonEliminarCuenta);
            botonEliminarCuenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            string textoEliminarCuenta;
            string textoCerrarSesion;
            if (partida)// si es una partida
            {
                textoEliminarCuenta = "Abandonar Partida";
                textoCerrarSesion = "Reanudar la Partida";
            }
            else// si no es una partida
            {
                textoEliminarCuenta = "Eliminar Cuenta";
                textoCerrarSesion = "Cerrar Sesión";
            }

            Label eliminarCuentaLbl = new Label
            {
                Text = textoEliminarCuenta,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            botonEliminarCuenta.Controls.Add(eliminarCuentaLbl);
            // Centramos el label dentro del botón
            eliminarCuentaLbl.Location = new Point(
                (botonEliminarCuenta.Width - eliminarCuentaLbl.Width) / 2,
                (botonEliminarCuenta.Height - eliminarCuentaLbl.Height) / 2
            );

            // Botón de cerrar sesion
            botonCerrarSesion = new PanelDobleBuffer
            {
                Size = new Size(anchurabotones, alturabotones),
                Location = new Point(10, botonEliminarCuenta.Top - alturabotones - 20),
                BackColor = Color.FromArgb(44, 44, 44)
            };
            redondearPanel(botonCerrarSesion, 10);
            panelMenu.Controls.Add(botonCerrarSesion);
            botonCerrarSesion.BringToFront();
            botonCerrarSesion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            Label cerrarSesionLbl = new Label
            {
                Text = textoCerrarSesion,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            botonCerrarSesion.Controls.Add(cerrarSesionLbl);
            // Centramos el label dentro del botón
            cerrarSesionLbl.Location = new Point(
                (botonCerrarSesion.Width - cerrarSesionLbl.Width) / 2,
                (botonCerrarSesion.Height - cerrarSesionLbl.Height) / 2
            );

            botonEliminarCuenta.Click += (s, e) =>
            {
                if (partida)
                {
                    // lógica para abandonar la partida
                    form.Close();
                }
                else
                {
                    // lógica para eliminar la cuenta
                    MessageBox.Show("Eliminar Cuenta");
                    form.Close();
                }

            };
            botonCerrarSesion.Click += (s, e) =>
            {
                if (partida)
                {
                    // lógica para reanudar la partida
                    MenuClick(s, e); // Cierra el menú si está abierto
                }
                else
                {
                    // lógica para cerrar sesión
                    MessageBox.Show("Cerrar Sesión");
                    form.Close();
                }
            };

            // Chat
            CrearChat();
            AjustarPanelMenu(); // Ajusta el tamaño del panel del menú y los paneles de chat y amigos
        }

        private void CrearChat()
        {            
            int textBoxHeight = 40;
            int textBoxWidth = panelMenu.Width - textBoxHeight - padding;

            int historyHeight = 300;

            // TextBox de mensaje con borde redondeado usando Panel
            panelTextBox = new PanelDobleBuffer
            {
                Size = new Size(textBoxWidth - padding * 2, textBoxHeight),
                Location = new Point(padding, botonCerrarSesion.Top - textBoxHeight - padding),
                BackColor = Color.FromArgb(66, 66, 66),
                Padding = new Padding(5)
            };

            string hintText = "Escribe un mensaje...";
            bool mostrandoHint = true;

            mensajeChat = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(66, 66, 66),
                ForeColor = Color.Gray, // Color del hint
                Multiline = true,
                MaxLength = 100,
                Dock = DockStyle.Fill,
                Text = hintText
            };

            // Al entrar en el TextBox, quitamos el hint si está presente
            mensajeChat.GotFocus += (s, e) =>
            {
                if (mostrandoHint)
                {
                    mensajeChat.Text = "";
                    mensajeChat.ForeColor = Color.White;
                    mostrandoHint = false;
                }
            };

            // Al salir, si está vacío, volvemos a mostrar el hint
            mensajeChat.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(mensajeChat.Text))
                {
                    mensajeChat.Text = hintText;
                    mensajeChat.ForeColor = Color.Gray;
                    mostrandoHint = true;
                }
            };

            panelTextBox.Controls.Add(mensajeChat);
            redondearPanel(panelTextBox, 10); // Método para redondear

            
            PanelDobleBuffer enviarMensaje = new PanelDobleBuffer
            {
                Size = new Size(textBoxHeight, textBoxHeight),
                Location = new Point(panelTextBox.Right + 10, panelTextBox.Top),
                BackColor = Color.FromArgb(88, 88, 88)
            };
            redondearPanel(enviarMensaje, 10); // Redondear el panel del botón
            PictureBox enviarIcono = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "enviar.png")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };

            // RichTextBox historial (envuelto también en panel redondeado)
            panelChat = new PanelDobleBuffer
            {
                Size = new Size(panelMenu.Width - padding * 2, historyHeight),
                Location = new Point(padding, panelTextBox.Top - historyHeight - padding),
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

            // Título del Chat
            tituloChat = new Label
            {
                Text = "Chat",
                ForeColor = Color.White,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true
            };

            // Primero lo agregamos para poder medirlo correctamente
            panelMenu.Controls.Add(tituloChat);

            tituloChat.Location = new Point(padding, panelChat.Top - tituloChat.Height - padding);
            tituloChat.BringToFront();


            panelChat.Controls.Add(historialMensajes);
            redondearPanel(panelChat, 10);

            // Agregar controles
            panelMenu.Controls.Add(panelChat);
            panelMenu.Controls.Add(panelTextBox);
            panelMenu.Controls.Add(enviarMensaje);
            tituloChat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelChat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            enviarMensaje.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Evento de enviar mensaje
            enviarMensaje.Click += enviarMensaje_Click;

            historialMensajes.Visible = true;
            enviarMensaje.Visible = true;
            mensajeChat.Visible = true;

            panelChat.BringToFront();
            enviarMensaje.BringToFront();
            panelTextBox.BringToFront();
            historialMensajes.ReadOnly = true;
            historialMensajes.HideSelection = false; // Permite seleccionar el texto en el RichTextBox
            historialMensajes.ScrollBars = RichTextBoxScrollBars.Vertical; // Agrega una barra de desplazamiento vertical
            historialMensajes.WordWrap = true; // Permite el ajuste de línea

        }
        private void enviarMensaje_Click(object sender, EventArgs e)
        {
            //Puede ser que textUsu cuando se estconde ya no se puede aceder a su contenido
            string mensaje = $"7/{user}/{mensajeChat.Text}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            mensajeChat.Clear(); // Limpiar el TextBox después de enviar el mensaje
        }
        public void HistorialMensajes(string nuevoMensaje)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
            string entrada = $"[{timeStamp}] {nuevoMensaje}{Environment.NewLine}";
            historialMensajes.AppendText(entrada);
            historialMensajes.SelectionStart = historialMensajes.Text.Length;
            historialMensajes.ScrollToCaret();
            panelChat.Update();
        }

        void AsignarEventoClick(Control contenedor)
        {
            contenedor.Click += MenuClick;

            foreach (Control hijo in contenedor.Controls)
            {
                AsignarEventoClick(hijo); // Aplica recursivamente
            }
        }
        private void MenuClick(object sender, EventArgs e)
        {
            if (panelMenu.Left == form.ClientSize.Width - panelMenu.Width) // abrir
            {
                ButtonAnimator.AnimatePanel(panelMenu, new Point(panelMenu.Left, panelMenu.Top), new Point(panelMenu.Left + 300, panelMenu.Top), ButtonAnimator.AnimationDirection.Right, false);
                contenedorMenu.BringToFront(); // Asegura que el contenedor del botón esté al frente               
            }
            else if (panelMenu.Left == form.ClientSize.Width) // cerrar
            {
                panelMenu.Visible = true; // Asegura que el panel sea visible antes de animar             
                ButtonAnimator.AnimatePanel(panelMenu, new Point(panelMenu.Left, panelMenu.Top), new Point(panelMenu.Left - 300, panelMenu.Top), ButtonAnimator.AnimationDirection.Left, true);
                contenedorMenu.BringToFront();
            }
        }
        public void redondearPanel(Panel panel, int radio)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(panel.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(panel.Width - radio, panel.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, panel.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            panel.Region = new Region(path);
            
            panel.Update();      // Fuerza el repintado inmediato

        }
        public void AjustarPanelMenu()
        {
            // Ajusta el alto del menú al alto del cliente del formulario
            panelMenu.Height = form.ClientSize.Height;
            
            int altoMin = tituloAmigos.Bottom + padding;
            int bajoMin = panelTextBox.Top - padding;
            int tituloChatEspacio = tituloChat.Height + padding*2;
            int espacioDisponible = bajoMin - altoMin - tituloChatEspacio;

            int alturaPanelAmigos = (int)(espacioDisponible*0.4f);
            int alturaPanelChat = (int)(espacioDisponible * 0.6f);
            panelChat.Height = alturaPanelChat;
            panelAmigos.Height = alturaPanelAmigos;

            // Ajusta las posiciones de los paneles
            panelChat.Location = new Point(padding, bajoMin - alturaPanelChat);
            panelAmigos.Location = new Point(padding, altoMin);
            tituloChat.Location = new Point(padding, panelChat.Top - tituloChat.Height - padding);

            redondearPanel(panelMenu, 10);
            redondearPanel(panelAmigos, 10);
            redondearPanel(panelChat, 10);
        }
    }
}

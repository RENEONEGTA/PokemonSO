using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApplication1;
using System.Diagnostics.Contracts;
using System.Net.Sockets;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;

public class NuevoFormulario : Form
{
    public NuevoFormulario()
    {
        this.Text = "Nuevo Formulario";
        this.Size = new Size(300, 200);
    }
    

}

public class Conectados
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Victorias { get; set; }
    public int Derrotas { get; set; }

    string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
    int panelConectadoTop = 0;
    
    // Guarda los formularios de invitación por ID del jugador que invita
    Dictionary<int, Form> formulariosInvitacionAbiertos = new Dictionary<int, Form>();
    public static List<Conectados> ParsearDatos(string datos, List<Conectados> lista)
    {
        string[] registros = datos.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string registro in registros)
        {
            string[] campos = registro.Split('/');

            // Si tiene menos de 4 campos, agregamos valores vacíos
            if (campos.Length < 4)
            {
                return lista;

            }

            int.TryParse(campos[0], out int id);
            int.TryParse(campos[4], out int victorias);
            int.TryParse(campos[5], out int derrotas);
            
            

            //Si no existe, lo añadimos
            Conectados conectados = new Conectados
             {
                Id = id,
                Nombre = campos[1],
                Victorias = victorias,
                Derrotas = derrotas
             };

            lista.Add(conectados);



        }
        return lista;
    }

    public void DibujarConectados(List<Conectados> conectados, PanelDobleBuffer panelConectados, Form ventana, string nombre, Socket server)
    {
        int y = 10;
        int x = 10;

        // Limpiar el panel antes de dibujar
        panelConectados.Controls.Clear();

        foreach (var conectado in conectados)
        {
            if (conectado.Nombre != null && conectado.Nombre != nombre)
            {
                PanelDobleBuffer panelConectado = new PanelDobleBuffer
                {
                    Size = new Size(150, 170),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightBlue
                };
                // Configurar el evento Paint para aplicar bordes redondeados y degradado
                panelConectado.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
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
                        panelConectado.Region = new Region(path);

                        // Configurar el suavizado
                        ePanel.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Crear un pincel de degradado: 
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            rect,
                            Color.FromArgb(33, 33, 33),
                            Color.FromArgb(44, 44, 44),
                            LinearGradientMode.Vertical))
                        {
                            // Rellenar la ruta con el degradado
                            ePanel.Graphics.FillPath(brush, path);
                        }
                    }
                });

                Label lblNombre = new Label
                {
                    Text = "Nombre: " + conectado.Nombre,
                    Location = new Point(10, 10),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };

                string imparPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chico.png");
                string parPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chica.png");
                string imagePath = (conectado.Id % 2 == 0) ? parPath : imparPath;

                PanelDobleBuffer panelFondo = new PanelDobleBuffer
                {
                    Size = new Size(130, 90),
                    Location = new Point(10, 30),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackgroundImage = Image.FromFile(imagePath),
                    BackgroundImageLayout = ImageLayout.Zoom
                };

                Label lblVictorias = new Label
                {
                    Text = "Victorias: " + string.Join(", ", conectado.Victorias),
                    Location = new Point(10, 130),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };
                Label lblDerrotas = new Label
                {
                    Text = "Derrotas: " + string.Join(", ", conectado.Derrotas),
                    Location = new Point(10, 150),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };



                panelConectado.Controls.Add(lblNombre);
                panelConectado.Controls.Add(panelFondo);
                panelConectado.Controls.Add(lblVictorias);
                panelConectado.Controls.Add(lblDerrotas);

                panelConectados.Controls.Add(panelConectado);


                // Mover la posición para la siguiente carta
                x += 160;
                if (x > panelConectados.Width - 160) // Si no cabe en la fila, bajar
                {
                    x = 10;
                    y += 190;
                }

                panelConectado.Click += (esender, e1) =>
                {


                    NuevoFormulario nuevoForm = new NuevoFormulario();
                    nuevoForm.StartPosition = FormStartPosition.Manual;
                    nuevoForm.Location = new Point(ventana.Location.X + 50, ventana.Location.Y + 50);

                    Label mensaje = new Label();

                    mensaje.Text = "Esperando a que " + conectado.Nombre + " acepte el combate";
                    mensaje.Location = new Point((nuevoForm.Width - mensaje.Width) / 2, nuevoForm.Height / 2);
                    mensaje.AutoSize = true;

                    nuevoForm.Controls.Add(mensaje);



                    // Crear el botón de cancelar
                    Button botonCancelar = new Button();
                    botonCancelar.Text = "Cancelar";
                    botonCancelar.Location = new Point((nuevoForm.Width - botonCancelar.Width) / 2, mensaje.Top + mensaje.Height + 10);
                    botonCancelar.AutoSize = true;
                    botonCancelar.Click += (s, args) =>
                    {

                        MessageBox.Show("Combate cancelado con " + conectado.Nombre);
                        //nuevoForm.Close();
                    };
                    nuevoForm.Controls.Add(botonCancelar);

                    string invitacion = "7/" + conectado.Id;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(invitacion);
                    server.Send(msg);


                    nuevoForm.Show();

                    //Combate.pantallaCombate(nuevoForm, conectado);
                };
            }
           
        }
        
    }

    public void DibujarConectadosEnLista(List<Conectados> conectados, PanelDobleBuffer panelConectados, Form ventana, string nombre, Socket server, int ancho, int alto, int idPartidaInvitada)
    {
        int y = 2;
        int x = 2;

        // Limpiar el panel antes de dibujar
        panelConectados.Controls.Clear();

        foreach (var conectado in conectados)
        {
            if (conectado.Nombre != null && conectado.Nombre != nombre)
            {
                PanelDobleBuffer panelConectado = new PanelDobleBuffer
                {
                    Size = new Size(ancho-x-x, 50),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightBlue
                };
                // Configurar el evento Paint para aplicar bordes redondeados y degradado
                panelConectado.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
                {
                    PanelDobleBuffer panel = (PanelDobleBuffer)senderPanel;
                    int radio = 10; // Radio para las esquinas redondeadas
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
                        panelConectado.Region = new Region(path);

                        // Configurar el suavizado
                        ePanel.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Crear un pincel de degradado
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            rect,
                            Color.FromArgb(33, 33, 33),
                            Color.FromArgb(44, 44, 44),
                            LinearGradientMode.Vertical))
                        {
                            // Rellenar la ruta con el degradado
                            ePanel.Graphics.FillPath(brush, path);
                        }
                    }
                });

                Label lblNombre = new Label
                {
                    Text = conectado.Nombre,
                    Location = new Point(50, 5),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };

                string imparPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chico.png");
                string parPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chica.png");
                string imagePath = (conectado.Id % 2 == 0) ? parPath : imparPath;

                PanelDobleBuffer panelFondo = new PanelDobleBuffer
                {
                    Size = new Size(40, 40),
                    Location = new Point(5, 5),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackgroundImage = Image.FromFile(imagePath),
                    BackgroundImageLayout = ImageLayout.Zoom
                };
                
                redondearPanel(panelFondo, 10);

                Label lblVictorias = new Label
                {
                    Text = "Victorias: " + string.Join(", ", conectado.Victorias),
                    Location = new Point(50, 20),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };
                Label lblDerrotas = new Label
                {
                    Text = "Derrotas: " + string.Join(", ", conectado.Derrotas),
                    Location = new Point(50, 35),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    AutoSize = true
                };



                panelConectado.Controls.Add(lblNombre);
                panelConectado.Controls.Add(panelFondo);
                panelConectado.Controls.Add(lblVictorias);
                panelConectado.Controls.Add(lblDerrotas);

                panelConectados.Controls.Add(panelConectado);


                // Mover la posición para la siguiente carta
                y += 52;


                panelConectado.Click += (sender, e1) =>
                {
                    string invitacion = "9/" + conectado.Id + "/" + idPartidaInvitada;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(invitacion);
                    server.Send(msg);

                    // Creamos el label de invitación
                    Label labelInvitacion = new Label();
                    labelInvitacion.Text = "¡Invitado!";
                    labelInvitacion.ForeColor = Color.LimeGreen;
                    labelInvitacion.BackColor = Color.Transparent;
                    labelInvitacion.Font = new Font("Arial", 10, FontStyle.Bold);
                    labelInvitacion.AutoSize = true;
                    labelInvitacion.Location = new Point(
                        (panelConectado.Width - labelInvitacion.PreferredWidth) / 2,
                        (panelConectado.Height - labelInvitacion.PreferredHeight) / 2
                    );
                    labelInvitacion.BringToFront();
                    panelConectado.Enabled = false;
                    // Añadir el label al panel
                    panelConectado.Controls.Add(labelInvitacion);
                    lblDerrotas.Visible = false;
                    lblVictorias.Visible = false;
                    lblNombre.Visible = false;  

                    // Crear un temporizador para eliminar el mensaje a los 10 segundos
                    Timer temporizador = new Timer();
                    temporizador.Interval = 10000; // 10 segundos
                    temporizador.Tick += (s, args) =>
                    {
                        if (panelConectado.Controls.Contains(labelInvitacion))
                            panelConectado.Controls.Remove(labelInvitacion);
                        panelConectado.Enabled = true;
                        lblDerrotas.Visible = true;
                        lblVictorias.Visible = true;
                        lblNombre.Visible = true;
                        labelInvitacion.Dispose();
                        temporizador.Stop();
                        temporizador.Dispose();
                    };
                    temporizador.Start();
                };

            }
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


    public void RecibirInvitacion(Conectados conectado, Form ventana, Socket server)
    {
        // Verificar si ya hay un formulario abierto para este jugador
        if (formulariosInvitacionAbiertos.ContainsKey(conectado.Id))
            return; // Ya se ha abierto uno, ignoramos la nueva invitación

        // Crear nuevo formulario
        NuevoFormulario nuevoForm = new NuevoFormulario();
        formulariosInvitacionAbiertos[conectado.Id] = nuevoForm; // Guardar el formulario

        nuevoForm.FormClosed += (s, e) =>
        {
            // Cuando se cierre el form, lo quitamos del diccionario
            formulariosInvitacionAbiertos.Remove(conectado.Id);
        };
 
        nuevoForm.StartPosition = FormStartPosition.Manual;
        nuevoForm.Location = new Point(ventana.Location.X + 50, ventana.Location.Y + 50);

        Label mensaje = new Label();

        mensaje.Text = conectado.Nombre + " te ha invitado a un combate";
        mensaje.Location = new Point((nuevoForm.Width - mensaje.Width) / 2, nuevoForm.Height / 2);
        mensaje.AutoSize = true;

        nuevoForm.Controls.Add(mensaje);

        // Crear el botón de aceptar
        Button botonAceptar = new Button();
        botonAceptar.Text = "Aceptar";
        botonAceptar.Location = new Point((nuevoForm.Left + (botonAceptar.Width/2)), mensaje.Top + mensaje.Height + 10);
        botonAceptar.AutoSize = true;

        botonAceptar.Click += (s, args) =>
        {
            
            MessageBox.Show("Combate aceptado con " + conectado.Nombre);
            nuevoForm.Close();
        };
        nuevoForm.Controls.Add(botonAceptar);

        // Crear el botón de cancelar
        Button botonCancelar = new Button();
        botonCancelar.Text = "Cancelar";
        botonCancelar.Location = new Point((nuevoForm.Right-(botonCancelar.Width/2)), mensaje.Top + mensaje.Height + 10);
        botonCancelar.AutoSize = true;
        botonCancelar.Click += (s, args) =>
        {

            MessageBox.Show("Combate cancelado con " + conectado.Nombre);
            //nuevoForm.Close();
        };
        nuevoForm.Controls.Add(botonCancelar);

        string invitacion = "7/" + conectado.Id;
        // Enviamos al servidor el nombre tecleado
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(invitacion);
        server.Send(msg);


        nuevoForm.Show();

    }

    public void RecibirInvitacionMundo(Conectados jugadorInvitador, Form formulario, Socket server2, int idPartidaInvitada)
    {
        // Crear panel principal de la invitación
        Panel panelInvitacion = new Panel();
        panelInvitacion.Size = new Size(300, 80);
        panelInvitacion.BackColor = Color.FromArgb(100, 100, 100);
        panelInvitacion.BorderStyle = BorderStyle.None;

        // Posicion
        panelInvitacion.Location = new Point(20, 20); 
        redondearPanel(panelInvitacion, 10);


        // Imagen del usuario a la izquierda

        string imparPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chico.png");
        string parPath = Path.Combine(directorioBase, "Resources", "images", "personaje_chica.png");
        string imagePath = (jugadorInvitador.Id % 2 == 0) ? parPath : imparPath;

        PanelDobleBuffer avatar = new PanelDobleBuffer
        {
            Size = new Size(60, 60),
            Location = new Point(10, 10),
            BorderStyle = BorderStyle.Fixed3D,
            BackgroundImage = Image.FromFile(imagePath),
            BackgroundImageLayout = ImageLayout.Zoom
        };

        redondearPanel(avatar, 10);

        // Label con el nombre del jugador
        Label nombre = new Label();
        nombre.Text = jugadorInvitador.Nombre;
        nombre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        nombre.Location = new Point(80, 10);
        nombre.AutoSize = true;

        // Label con descripción
        Label descripcion = new Label();
        descripcion.Text = "te ha invitado a su partida";
        descripcion.Font = new Font("Segoe UI", 9);
        descripcion.ForeColor = Color.DarkGray;
        descripcion.Location = new Point(80, 30);
        descripcion.AutoSize = true;

        // Botón aceptar
        PictureBox btnAceptar = new PictureBox();
        btnAceptar.Size = new Size(24, 24);
        btnAceptar.Location = new Point(220, 50);
        btnAceptar.SizeMode = PictureBoxSizeMode.Zoom;
        btnAceptar.Cursor = Cursors.Hand;
        string aceptarPath = Path.Combine(directorioBase, "Resources", "images", "aceptar.png");
        btnAceptar.Image = Image.FromFile(aceptarPath);

        // Botón rechazar
        PictureBox btnRechazar = new PictureBox();
        btnRechazar.Size = new Size(24, 24);
        btnRechazar.Location = new Point(250, 50);
        btnRechazar.SizeMode = PictureBoxSizeMode.Zoom;
        btnRechazar.Cursor = Cursors.Hand;
        string cancelarPath = Path.Combine(directorioBase, "Resources", "images", "cancelar.png");
        btnRechazar.Image = Image.FromFile(cancelarPath);

        // Acción de aceptar
        btnAceptar.Click += (s, e) =>
        {
            string mensajeAceptar = "92/" + idPartidaInvitada;
            byte[] datos = Encoding.ASCII.GetBytes(mensajeAceptar);
            server2.Send(datos);

            formulario.Controls.Remove(panelInvitacion);
        };

        // Acción de rechazar
        btnRechazar.Click += (s, e) =>
        {

            formulario.Controls.Remove(panelInvitacion);
        };

        // Agregar controles al panel
        panelInvitacion.Controls.Add(avatar);
        panelInvitacion.Controls.Add(nombre);
        panelInvitacion.Controls.Add(descripcion);
        panelInvitacion.Controls.Add(btnAceptar);
        panelInvitacion.Controls.Add(btnRechazar);

        // Mostrar el panel en el formulario
        formulario.Controls.Add(panelInvitacion);
        panelInvitacion.BringToFront();

    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApplication1;

public class Conectados
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Victorias { get; set; }
    public int Derrotas { get; set; }

    string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
    int panelConectadoTop = 0;

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

            int.TryParse(campos[1], out int id);
            int.TryParse(campos[3], out int victorias);
            int.TryParse(campos[4], out int derrotas);
            Conectados conectados = new Conectados
            {
                Id = id,
                Nombre = campos[2],
                Victorias = victorias,
                Derrotas = derrotas

            };
            lista.Add(conectados);
        }
        return lista;
    }

    public void DibujarConectados(List<Conectados> conectados, PanelDobleBuffer panelConectados, Form ventana)
    {
        int y = 10;
        int x = 10;
        foreach (var conectado in conectados)
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

                    // Crear un pincel de degradado: de blanco a un gris muy claro
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

            panelConectado.Click += (sender, e) => {

                Combate.pantallaCombate(ventana, conectado);
            };
        }
    }


}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;
using WindowsFormsApplication1;

public class Partida
{
    public int Id { get; set; }
    public string Fecha { get; set; }
    public List<string> Jugadores { get; set; }
    public string Ganador { get; set; }

    string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
    int panelPartidaTop = 0;
    public static List<Partida> ParsearRespuesta(string respuesta, List<Partida> lista)
    {
        string[] registros = respuesta.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
        

        foreach (string registro in registros)
        {

            string[] partes = registro.Trim('/').Split('/');

            if (partes.Length >= 7) // Línea ~24?
            {
                string id = partes[0];
                string fecha = partes[1];
                List<string> jugadores = new List<string>
                {
                    partes[2],
                    partes[3],
                    partes[4],
                    partes[5]
                };
                string ganador = partes[6];
                Partida Partida = new Partida
                {
                    Id = int.Parse(id),
                    Fecha = fecha,
                    Jugadores = jugadores,
                    Ganador = ganador
                };
                lista.Add(Partida);
            }
        }
        return lista;
    }
    

    public void DibujarPartidas(List<Partida> partidas, PanelDobleBuffer panelPartidas)
    {
        int y = 10;
        foreach (var partida in partidas)
        {
            PanelDobleBuffer panelPartida = new PanelDobleBuffer
            {
                Size = new Size(200, 150),
                Location = new Point(10, y),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue
            };
            // Configurar el evento Paint para aplicar bordes redondeados y degradado
            panelPartida.Paint += new PaintEventHandler((object senderPanel, PaintEventArgs ePanel) =>
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
                    panelPartida.Region = new Region(path);

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

            Label lblFecha = new Label
            {
                Text = "Fecha: " + partida.Fecha,
                Location = new Point(10, 10),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };

            string imparPath = Path.Combine(directorioBase, "Resources", "images", "fondo_partida1.jpeg");
            string parPath = Path.Combine(directorioBase, "Resources", "images", "fondo_partida2.jpeg");
            string imagePath = (partida.Id % 2 == 0) ? parPath : imparPath;

            PanelDobleBuffer panelFondo = new PanelDobleBuffer
            {
                Size = new Size(180, 90),
                Location = new Point(10, 30),
                BorderStyle = BorderStyle.Fixed3D,
                BackgroundImage = Image.FromFile(imagePath), 
                BackgroundImageLayout = ImageLayout.Stretch
            };

            Label lblJugadores = new Label
            {
                Text = "Jugadores: " + string.Join(", ", partida.Jugadores),
                Location = new Point(10, 130),
                ForeColor = Color.White, 
                BackColor = Color.Transparent,
                AutoSize = true
            };

            

            panelPartida.Controls.Add(lblFecha);
            panelPartida.Controls.Add(panelFondo);
            panelPartida.Controls.Add(lblJugadores);

            panelPartidas.Controls.Add(panelPartida);

            y += 160;
            panelPartidaTop = panelPartida.Top - 50;
        }
    }
}

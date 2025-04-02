using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public class CartaPokemon
    {
        public string Nombre { get; set; }
        public int Vida { get; set; }
        public string Elemento { get; set; }
        public string ImagenRuta { get; set; }
        public List<(string Nombre, int Daño)> Ataques { get; set; }

        public CartaPokemon(string nombre, int vida, string elemento, string imagenRuta, List<(string, int)> ataques)
        {
            Nombre = nombre;
            Vida = vida;
            Elemento = elemento;
            ImagenRuta = imagenRuta;
            Ataques = ataques;
        }
    }

    public class PanelDobleBuffer : Panel
    {
        public PanelDobleBuffer()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
    }


    public class GestorCratas
    {
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;
        public void DibujarCartas(List<CartaPokemon> cartas, PanelDobleBuffer panelCartas)
        {

            

            panelCartas.Controls.Clear(); // Limpiar cartas anteriores

            int x = 10; // Posición inicial
            int y = 10;

            foreach (var carta in cartas)
            {
                Color colorFondo = ObtenerColorElemento(carta.Elemento);
                Color colorBorde = Color.LightGray; // Color del borde grueso
                Color colorInicio = ObtenerColorElemento(carta.Elemento); // Color base
                Color colorFin = ControlPaint.Light(colorInicio); // Un tono más claro para el degradado

                // Panel principal de la carta con bordes redondeados
                PanelDobleBuffer panelCarta = new PanelDobleBuffer
                {
                    Size = new Size(240, 340), // Tamaño ajustado
                    Location = new Point(x, y),
                    BackColor = colorFondo
                };
                panelCarta.Visible=false;

                // Aplicar bordes redondeados, degradado y borde grueso
                panelCarta.Paint += (sender, e) =>
                {
                    int radio = 15; // Radio de las esquinas redondeadas
                    using (GraphicsPath path = CrearRectanguloRedondeado(new Rectangle(0, 0, panelCarta.Width, panelCarta.Height), radio))
                    {
                        panelCarta.Region = new Region(path); // Aplica la máscara de recorte

                        // Dibujar el degradado
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            new Rectangle(0, 0, panelCarta.Width, panelCarta.Height),
                            colorInicio,
                            colorFin,
                            LinearGradientMode.Vertical))
                        {
                            e.Graphics.FillPath(brush, path); // Rellena el fondo con el degradado
                        }

                        // Dibujar el borde grueso personalizado
                        using (Pen pen = new Pen(colorBorde, 10))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.DrawPath(pen, path); // Dibuja el borde siguiendo la misma forma redondeada
                        }
                    }
                };

                // Nombre del Pokemon (arriba izquierda)
                Label lblNombre = new Label
                {
                    Text = carta.Nombre,
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent // Fondo transparente para que se vea el degradado
                };

                // Tamaño del círculo
                int tamanoCirculo = 30;
                int grosorBorde = 2; // Grosor del borde blanco

                // Crear un Panel circular que será el fondo del icono
                PanelDobleBuffer panelCirculo = new PanelDobleBuffer
                {
                    Size = new Size(tamanoCirculo, tamanoCirculo),
                    Location = new Point(panelCarta.Width - tamanoCirculo - 10, 5), // Pegado a la derecha
                    BackColor = ControlPaint.Dark(ObtenerColorElemento(carta.Elemento)), // Color basado en el elemento
                };

                // Convertirlo en un círculo con borde blanco para efecto 3D
                panelCirculo.Paint += (sender, e) =>
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        // Dibuja el borde externo (blanco) para crear un efecto 3D
                        path.AddEllipse(0, 0, tamanoCirculo, tamanoCirculo);
                        panelCirculo.Region = new Region(path);

                        // Borde blanco centrado
                        using (Pen pen = new Pen(Color.White, grosorBorde))
                        {
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            // Dibujar el borde centrado
                            e.Graphics.DrawEllipse(pen, grosorBorde / 2, grosorBorde / 2, tamanoCirculo - grosorBorde, tamanoCirculo - grosorBorde);
                        }

                        // Rellenar el fondo con el color del elemento (más pequeño que el borde)
                        using (Brush brush = new SolidBrush(ControlPaint.Light(ObtenerColorElemento(carta.Elemento))))
                        {
                            // Rellenar con el color del elemento dejando un espacio para el borde
                            e.Graphics.FillEllipse(brush, 0, 0, tamanoCirculo - 5, tamanoCirculo - 5);
                        }
                    }
                };


                // Crear el PictureBox del icono del elemento
                PictureBox pbElemento = new PictureBox
                {
                    Size = new Size(20, 20),
                    Location = new Point((tamanoCirculo - 20) / 2, (tamanoCirculo - 20) / 2), // Centrado en el panel
                    Image = ObtenerIconoElemento(carta.Elemento),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };

                // Agregar el icono dentro del panel circular
                panelCirculo.Controls.Add(pbElemento);

                // Agregar el panel a la carta
                panelCarta.Controls.Add(panelCirculo);


                // Vida (con el texto HP) y el icono del elemento a la derecha
                Label lblVida = new Label
                {
                    Text = carta.Vida.ToString(),
                    Location = new Point(panelCirculo.Left - 35, 15), // Ajustado un poco más hacia abajo
                    AutoSize = true,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent // Fondo transparente para que se vea el degradado
                };
                
                // Texto HP
                Label lblHP = new Label
                {
                    Text = "HP: ",
                    Location = new Point(lblVida.Left - 18, 20), // Ajustado un poco más hacia abajo
                    AutoSize = true,
                    Font = new Font("Arial", 6, FontStyle.Bold),
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent // Fondo transparente para que se vea el degradado
                };

                // Obtener la imagen de fondo según el elemento
                Image fondoElemento = ObtenerImagenFondo(carta.Elemento);

                // Crear un Panel que servirá como fondo de la imagen del Pokémon
                PanelDobleBuffer panelFondo = new PanelDobleBuffer
                {
                    Size = new Size(195, 120),
                    Location = new Point(25, 40),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackgroundImage = fondoElemento, // Imagen de fondo según el elemento
                    BackgroundImageLayout = ImageLayout.Stretch
                };

                // Imagen del Pokémon con bordes redondeados
                int tamanoPokemon = 80;
                PictureBox pbImagen = new PictureBox
                {
                    
                    Size = new Size(tamanoPokemon, tamanoPokemon),
                    Location = new Point(panelFondo.Width/2 - tamanoPokemon/2, panelFondo.Height/2 - tamanoPokemon/2), // Se coloca dentro del panel en el centro
                    Image = Image.FromFile(directorioBase + "/Resources/"+carta.ImagenRuta),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent // Para que se vea el fondo del panel
                };

                // Agregar el PictureBox al panel
                panelFondo.Controls.Add(pbImagen);

                // Agregar el panel a la carta
                panelCarta.Controls.Add(panelFondo);

                // Contenedor para los ataques
                PanelDobleBuffer panelAtaques = new PanelDobleBuffer
                {
                    Size = new Size(210, 100),
                    Location = new Point(15, 170),
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent // Fondo transparente para que se vea el degradado
                };

                int ataqueY = 10;
                foreach (var ataque in carta.Ataques)
                {
                    Label lblAtaque = new Label
                    {
                        Text = $"{ataque.Nombre}",
                        Location = new Point(10, ataqueY),
                        AutoSize = true,
                        MaximumSize = new Size(140, 0),
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        ForeColor = Color.Black
                    };
                    Label lblDaño = new Label
                    {
                        Text = $"{ataque.Daño}",
                        Location = new Point(180, ataqueY),
                        AutoSize = true,
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        ForeColor = Color.Black
                    };
                    panelAtaques.Controls.Add(lblAtaque);
                    panelAtaques.Controls.Add(lblDaño);
                    ataqueY += 40;
                }

                // Agregar controles al panel de la carta
                panelCarta.Controls.Add(lblNombre);
                panelCarta.Controls.Add(lblHP);
                panelCarta.Controls.Add(lblVida);
                panelCarta.Controls.Add(panelAtaques);

                // Agregar la carta al panel contenedor
                panelCartas.Controls.Add(panelCarta);

                panelCarta.Visible = true;

                // Mover la posición para la siguiente carta
                x += 250;
                if (x > panelCartas.Width - 250) // Si no cabe en la fila, bajar
                {
                    x = 10;
                    y += 360;
                }

                panelCarta.Click += (sender, e) => {
                    Form1.ManejarClickCarta(carta);
                };

            }

            string filterPath = Path.Combine(directorioBase, "Resources", "images", "filter.png");
            string fuegoPath = Path.Combine(directorioBase, "Resources", "images", "icono_fuego.png");
            string aguaPath = Path.Combine(directorioBase, "Resources", "images", "icono_agua.png");
            string plantaPath = Path.Combine(directorioBase, "Resources", "images", "icono_planta.png");
            string rayoPath = Path.Combine(directorioBase, "Resources", "images", "icono_rayo.png");
            // Crear un botón redondo
            RombeButton pokedexFilterButton = new RombeButton
            {
                Size = new Size(50, 50), // Tamaño del botón
                BackColor = Color.White, // Color de fondo
                ForeColor = Color.Gray, // Color del borde
                Image = Image.FromFile(filterPath), // Ruta del icono
                Location = new Point(panelCartas.Width - 60, panelCartas.Height - 60) // Ubicación del botón en el panel
            };

            // Agregar el botón al panel
            panelCartas.Controls.Add(pokedexFilterButton);
            pokedexFilterButton.Visible = true;
            pokedexFilterButton.BringToFront(); // Traer al frente

            


            RoundButton elementFireButon = new RoundButton
            {
                Size = new Size(50, 50), // Tamaño del botón
                BackColor = Color.White, // Color de fondo
                ForeColor = Color.Gray, // Color del borde
                Image = Image.FromFile(fuegoPath), // Ruta del icono
                Location = new Point(pokedexFilterButton.Left, pokedexFilterButton.Bottom) // Ubicación del botón en el panel
            };

            RoundButton elementWaterButon = new RoundButton
            {
                Size = new Size(50, 50), // Tamaño del botón
                BackColor = Color.White, // Color de fondo
                ForeColor = Color.Gray, // Color del borde
                Image = Image.FromFile(aguaPath), // Ruta del icono
                Location = new Point(pokedexFilterButton.Left, pokedexFilterButton.Bottom) // Ubicación del botón en el panel
            };

            RoundButton elementPlantButon = new RoundButton
            {
                Size = new Size(50, 50), // Tamaño del botón
                BackColor = Color.White, // Color de fondo
                ForeColor = Color.Gray, // Color del borde
                Image = Image.FromFile(plantaPath), // Ruta del icono
                Location = new Point(pokedexFilterButton.Left, pokedexFilterButton.Bottom) // Ubicación del botón en el panel
            };

            RoundButton elementRayoButon = new RoundButton
            {
                Size = new Size(50, 50), // Tamaño del botón
                BackColor = Color.White, // Color de fondo
                ForeColor = Color.Gray, // Color del borde
                Image = Image.FromFile(rayoPath), // Ruta del icono
                Location = new Point(pokedexFilterButton.Left, pokedexFilterButton.Bottom) // Ubicación del botón en el panel
            };


            pokedexFilterButton.Click += (sender, e) =>
            {
                if (!elementFireButon.Visible)
                {
                    // Agregar el botón al panel
                    panelCartas.Controls.Add(elementFireButon);
                    elementFireButon.Visible = true;
                    elementFireButon.BringToFront(); // Traer al frente

                    panelCartas.Controls.Add(elementWaterButon);
                    elementWaterButon.Visible = true;
                    elementWaterButon.BringToFront(); // Traer al frente

                    panelCartas.Controls.Add(elementPlantButon);
                    elementPlantButon.Visible = true;
                    elementPlantButon.BringToFront(); // Traer al frente
                    pokedexFilterButton.BringToFront(); // Traer al frente

                    panelCartas.Controls.Add(elementRayoButon);
                    elementPlantButon.Visible = true;
                    elementPlantButon.BringToFront(); // Traer al frente
                    pokedexFilterButton.BringToFront(); // Traer al frente

                    // Animar el botón para que aparezca
                    ButtonAnimator.AnimateButton(elementFireButon, new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), new Point(pokedexFilterButton.Left - 70, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, true);
                    // Animar el botón para que aparezca
                    ButtonAnimator.AnimateButton(elementWaterButon, new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), new Point(pokedexFilterButton.Left - 130, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, true);
                    // Animar el botón para que aparezca
                    ButtonAnimator.AnimateButton(elementPlantButon, new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), new Point(pokedexFilterButton.Left - 190, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, true);
                    // Animar el botón para que aparezca
                    ButtonAnimator.AnimateButton(elementRayoButon, new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), new Point(pokedexFilterButton.Left - 190, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, true);

                }
                else
                {
                    // Animar el botón para que desaparezca
                    ButtonAnimator.AnimateButton(elementFireButon, new Point(elementFireButon.Left, elementFireButon.Top), new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, false);
                    ButtonAnimator.AnimateButton(elementWaterButon, new Point(elementWaterButon.Left, elementFireButon.Top), new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, false);
                    ButtonAnimator.AnimateButton(elementPlantButon, new Point(elementPlantButon.Left, elementFireButon.Top), new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, false);
                    ButtonAnimator.AnimateButton(elementRayoButon, new Point(elementRayoButon.Left, elementFireButon.Top), new Point(pokedexFilterButton.Left, pokedexFilterButton.Top), ButtonAnimator.AnimationDirection.Left, false);

                }
            };








        }

        // Función para crear un rectángulo con esquinas redondeadas
        private GraphicsPath CrearRectanguloRedondeado(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            int diametro = radio * 2;

            // Esquinas redondeadas
            path.AddArc(rect.Left, rect.Top, diametro, diametro, 180, 90);
            path.AddArc(rect.Right - diametro, rect.Top, diametro, diametro, 270, 90);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diametro, diametro, diametro, 90, 90);
            path.CloseFigure();

            return path;
        }
       
        // Método para obtener el icono del elemento
        private Image ObtenerIconoElemento(string elemento)
        {
            switch (elemento.ToLower())
            {
                case "fuego": return Image.FromFile(directorioBase + "/Resources/images/icono_fuego.png");
                case "agua": return Image.FromFile(directorioBase + "/Resources/images/icono_agua.png");
                case "planta": return Image.FromFile(directorioBase + "/Resources/images/icono_planta.png");
                case "rayo": return Image.FromFile(directorioBase + "/Resources/images/icono_rayo.png");
                case "tierra": return Image.FromFile(directorioBase + "/Resources/images/icono_tierra.png");
                case "hielo": return Image.FromFile(directorioBase + "/Resources/images/icono_hielo.png");
                case "dragón": return Image.FromFile(directorioBase + "/Resources/images/icono_dragon.png");
                default: return Image.FromFile(directorioBase + "/Resources/images/default.png"); // Icono por defecto
            }
        }


        // Método para obtener un color basado en el elemento de la carta
        private Color ObtenerColorElemento(string elemento)
        {
            switch (elemento.ToLower())
            {
                case "fuego": return Color.OrangeRed;
                case "agua": return Color.SteelBlue;
                case "planta": return Color.ForestGreen;
                case "rayo": return Color.Yellow;
                case "tierra": return Color.SaddleBrown;
                case "hielo": return Color.Cyan;
                case "dragón": return Color.Purple;
                default: return Color.Gray;
            }
        }
        Image ObtenerImagenFondo(string elemento)
        {
            switch (elemento.ToLower()) 
            {
                case "fuego":
                    return Image.FromFile(directorioBase + "/Resources/images/fondo_fuego.jpeg");
                case "agua":
                    return Image.FromFile(directorioBase + "/Resources/images/fondo_agua.jpeg");
                case "tierra":
                    return Image.FromFile(directorioBase + "/Resources/images/fondo_tierra.jpeg");
                case "planta":
                    return Image.FromFile(directorioBase + "/Resources/images/fondo_planta.jpeg");
                case "rayo":
                    return Image.FromFile(directorioBase + "/Resources/images/fondo_rayo.jpeg");
                default:
                    return Image.FromFile(directorioBase + "/Resources/images/default.png"); // Fondo genérico
            }
        }
        


    }



}

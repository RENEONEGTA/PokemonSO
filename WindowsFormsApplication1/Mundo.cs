using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Mundo
    {
    }
    public class Mapa
    {
        public int[,] tiles;
        public int ancho => tiles.GetLength(1);
        public int alto => tiles.GetLength(0);
        
        private static Dictionary<int, Image> imagenesTiles;
        private Bitmap preRenderedMap; // Mapa pre-renderizado
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        public Mapa(int tileSize)
        {
            
            

            tiles = new int[40, 40]
            {
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,0,0,0,0,0,0,0,0,5,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,5,0,0,0,0,0,0,0,0,5},
                {5,0,3,3,0,0,3,3,0,0,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,0,0,3,3,0,0,3,3,0,5},
                {5,0,3,3,0,0,3,3,0,0,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,0,3,3,0,0,0,3,3,0,0,3,3,0,5},
                {5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,5},
                {5,5,5,5,0,0,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,0,0,5,5,5,5},
                {5,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,5},
                {5,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,5},
                {5,5,5,5,0,0,5,5,5,5,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,5,5,5,5,0,0,5,5,5,5},
                {2,2,2,2,0,0,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,0,0,2,2,2,2},
                {2,2,2,2,0,0,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,0,0,2,2,2,2},
                {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5},
                {5,0,0,0,0,0,0,0,0,5,0,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,0,5,0,0,0,0,0,0,0,0,5},
                {5,0,3,3,0,0,3,3,0,5,0,3,3,0,0,3,3,1,1,0,0,1,1,3,3,0,0,3,3,0,5,0,3,3,0,0,3,3,0,5},
                {5,0,3,3,0,0,3,3,0,5,0,3,3,0,0,3,3,1,1,0,0,1,1,3,3,0,0,3,3,0,5,0,3,3,0,0,3,3,0,5},
                {5,0,0,0,0,0,0,0,0,5,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,0,5},
                {5,5,5,5,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,5,5,5,5},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}


            };


            if (imagenesTiles == null)
            {
                CargarTexturas(tileSize);
            }

            PreRenderMapa(tileSize);

        }

        private void CargarTexturas(int tileSize)
        {
            imagenesTiles = new Dictionary<int, Image>();
            

            // Cargar y redimensionar imágenes
            imagenesTiles[0] = ResizeImage(Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "suelo.png")), tileSize);
            imagenesTiles[1] = ResizeImage(Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "hierba.png")), tileSize);
            imagenesTiles[2] = ResizeImage(Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "agua.png")), tileSize);
            imagenesTiles[3] = ResizeImage(Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "pared.png")), tileSize);
            imagenesTiles[5] = ResizeImage(Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "bosque.png")), tileSize);
        }

        private Image ResizeImage(Image image, int tileSize)
        {
            Bitmap resized = new Bitmap(tileSize, tileSize);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half; 
                g.DrawImage(image, 0, 0, tileSize, tileSize);
            }
            return resized;
        }

        private void PreRenderMapa(int tileSize)
        {
            
            preRenderedMap = new Bitmap(ancho * tileSize, alto * tileSize);
            using (Graphics g = Graphics.FromImage(preRenderedMap))
            {
                for (int y = 0; y < alto; y++)
                {
                    for (int x = 0; x < ancho; x++)
                    {
                        int tipo = tiles[y, x];
                        if (imagenesTiles.ContainsKey(tipo))
                        {
                            g.DrawImage(imagenesTiles[tipo], x * tileSize, y * tileSize);
                        }
                        else
                        {
                            using (Brush brush = new SolidBrush(Color.Magenta))
                                g.FillRectangle(brush, x * tileSize, y * tileSize, tileSize, tileSize);
                        }
                    }
                }
            }
        }

        public bool EsTransitable(int x, int y)
        {
            if (x < 0 || y < 0 || x >= ancho || y >= alto)
                return false;
            return tiles[y, x] == 0 || tiles[y, x] == 1;
        }



        public void Dibujar(Graphics g, float camX, float camY, int tileSize, int vistaAncho, int vistaAlto)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor; // ✅
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half; // ✅

            Rectangle srcRect = new Rectangle(
                (int)Math.Round(camX), // Redondea para evitar decimales
                (int)Math.Round(camY),
                vistaAncho * tileSize,
                vistaAlto * tileSize
            );

            g.DrawImage(
                preRenderedMap,
                new Rectangle(0, 0, vistaAncho * tileSize, vistaAlto * tileSize),
                srcRect,
                GraphicsUnit.Pixel
            );
        }

        public void Dispose()
        {
            if (preRenderedMap != null)
                preRenderedMap.Dispose();
        }
    }

    class JugadorInfo
    {
        public int id;
        public int x;
        public int y;
    }


    public class Jugador
    {
        public float x, y;
        public float velocidad = 6f;
   
        public int tamano = 16;


        private int direccion; // 0 = arriba, 1 = abajo, 2 = izquierda, 3 = derecha
        private int frame; // Para la animación de caminar
        private bool enMovimiento;
        float xAnterior, yAnterior; // Para el movimiento remoto

        private Image[] caminarArriba;
        private Image[] caminarAbajo;
        private Image[] caminarIzquierda;
        private Image[] caminarDerecha;
        private Image quietoArriba, quietoAbajo, quietoIzquierda, quietoDerecha;

        private Timer timerAnimacion; // Temporizador para la animación
        string directorioBase = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).FullName;

        private float tiempoQuieto = 0f;
        private DateTime ultimaActualizacion = DateTime.Now;



        public Jugador(float x, float y)
        {
            this.x = x;
            this.y = y;

            // Cargar imágenes de caminar
            caminarArriba = new Image[] {
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png")),
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png"))
            };

            caminarAbajo = new Image[] {
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "abajo_caminando.png")),
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "abajo_caminando.png"))
            };

            caminarIzquierda = new Image[] {
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png")),
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png"))
            };

            caminarDerecha = new Image[] {
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png")),
                Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba_caminando.png"))
            };


            // Cargar imágenes de quieto
            quietoArriba = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "arriba.png"));
            quietoAbajo = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "abajo.png"));
            quietoIzquierda = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "izquierda.png"));
            quietoDerecha = Image.FromFile(Path.Combine(directorioBase, "Resources", "images", "mapa", "jugador", "derecha.png"));

            // Inicializar el temporizador de animación
            timerAnimacion = new Timer();
            timerAnimacion.Interval = 150;
            timerAnimacion.Tick += TimerAnimacion_Tick;
        }
                    
        private void TimerAnimacion_Tick(object sender, EventArgs e)
        {
            // Cambiar el frame de la animación
            frame = (frame + 1) % 2; // Alternar entre 0 y 1
        }


        public void Mover(float dx, float dy, Mapa mapa)
        {
            // Calcular la norma del vector de movimiento
            float norma = (float)Math.Sqrt(dx * dx + dy * dy);

            if (norma > 1)
            {
                // Normalizar para que la velocidad sea constante
                dx /= norma;
                dy /= norma;
            }

            float nuevoX = x + dx * velocidad;
            float nuevoY = y + dy * velocidad;

            // Usar la mitad del tamaño del jugador como offset (8 píxeles)
            int offsetCentro = tamano / 2;
            int tileX = (int)((nuevoX + offsetCentro) / 256);
            int tileY = (int)((nuevoY + offsetCentro) / 256);

            if (mapa.EsTransitable(tileX, tileY))
            {
                x = nuevoX;
                y = nuevoY;

                //movimiento animacion
                if (dx != 0 || dy != 0)
                {
                    enMovimiento = true;
                    if (dy < 0) direccion = 0; // Arriba
                    else if (dy > 0) direccion = 1; // Abajo
                    else if (dx < 0) direccion = 2; // Izquierda
                    else if (dx > 0) direccion = 3; // Derecha

                    x += dx;
                    y += dy;

                    timerAnimacion.Start(); // Iniciar animación
                }
                else if (timerAnimacion.Enabled)
                {
                    enMovimiento = false;
                    timerAnimacion.Stop(); // Detener animación
                }
            }
        }

        public void Dibujar(Graphics g, float camX, float camY, int tileSize, Brush color = null)
        {
            if (color == null)
            {
                color = Brushes.Red; // Rojo por defecto (jugador local)  
            }
            float pantallaX = x - camX - (tamano / 2); // Centrar en X  
            float pantallaY = y - camY - (tamano / 2); // Centrar en Y  
            g.FillEllipse(color, pantallaX, pantallaY, tamano, tamano);

            // Dibujar el sprite del jugador
            int spriteSize = tileSize / 2;
            float pantallax = x - camX - spriteSize / 2f;
            float pantallay = y - camY - spriteSize;




            Image imagen;
            // Dependiendo de la dirección y si está en movimiento o quieto, se elige la imagen
            if (enMovimiento)
            {
                switch (direccion)
                {
                    case 0: // Arriba
                        imagen = caminarArriba[frame];
                        break;
                    case 1: // Abajo
                        imagen = caminarAbajo[frame];
                        break;
                    case 2: // Izquierda
                        imagen = caminarIzquierda[frame];
                        break;
                    case 3: // Derecha
                        imagen = caminarDerecha[frame];
                        break;
                    default:
                        imagen = caminarAbajo[frame]; // Default si no hay movimiento
                        break;
                }
            }
            else
            {
                switch (direccion)
                {
                    case 0: // Quieto arriba
                        imagen = quietoArriba;
                        break;
                    case 1: // Quieto abajo
                        imagen = quietoAbajo;
                        break;
                    case 2: // Quieto izquierda
                        imagen = quietoIzquierda;
                        break;
                    case 3: // Quieto derecha
                        imagen = quietoDerecha;
                        break;
                    default:
                        imagen = quietoAbajo; // Default
                        break;
                }
            }
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            

            g.DrawImage(imagen, pantallax, pantallay, spriteSize, spriteSize);

        }

        public void ActualizarMovimientoRemoto(float nuevaX, float nuevaY)
        {
            float dx = nuevaX - x;
            float dy = nuevaY - y;

            DateTime ahora = DateTime.Now;

            if (dx != 0 || dy != 0)
            {
                // Movimiento detectado: reiniciamos tiempo quieto
                tiempoQuieto = 0f;
                ultimaActualizacion = ahora;

                float distancia = (float)Math.Sqrt(dx * dx + dy * dy);
                float vx = dx / distancia;
                float vy = dy / distancia;

                x += vx * velocidad;
                y += vy * velocidad;

                enMovimiento = true;

                if (Math.Abs(dx) > Math.Abs(dy))
                    direccion = (dx < 0) ? 2 : 3; // izquierda o derecha
                else
                    direccion = (dy < 0) ? 0 : 1; // arriba o abajo

                timerAnimacion.Start();
            }
            else
            {
                // Posición igual: acumular tiempo quieto
                float segundosTranscurridos = (float)(ahora - ultimaActualizacion).TotalSeconds;
                tiempoQuieto += segundosTranscurridos;
                ultimaActualizacion = ahora;

                if (tiempoQuieto >= 0.2f)
                {
                    enMovimiento = false;
                    timerAnimacion.Stop();
                }
            }
        }

        public void ComprobarEstadoMovimiento()
        {
            DateTime ahora = DateTime.Now;
            float segundosSinActualizar = (float)(ahora - ultimaActualizacion).TotalSeconds;

            if (segundosSinActualizar >= 0.2f && enMovimiento)
            {
                enMovimiento = false;
                timerAnimacion.Stop();
            }
        }




        public void DibujarEnMinimapa(Graphics g, int x, int y, int size, Brush color = null)
        {
            if (color == null)
            {
                color = Brushes.Red; // Rojo por defecto (jugador local)  
            }
            g.FillEllipse(Brushes.Red, x - size/2, y - size/2, size, size);
        }
    }

    

}

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
        public float velocidad = 4f;
   
        public int tamano = 16; 


        public Jugador(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Mover(float dx, float dy, Mapa mapa)
        {
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

using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System;

public class RombeButton : Button
{
    protected override void OnPaint(PaintEventArgs e)
    {
        // Crear un gráfico con suavizado
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Crear un rectángulo para el botón
        Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

        // Dibujar el fondo del botón
        using (Brush brush = new SolidBrush(this.BackColor))
        {
            g.FillEllipse(brush, rect);
        }

        // Dibujar el borde del botón
        using (Pen pen = new Pen(this.ForeColor, 2))
        {
            g.DrawEllipse(pen, rect);
        }

        // Dibujar el icono en el centro del botón
        if (this.Image != null)
        {
            int iconSize = Math.Min(this.Width, this.Height) / 2;
            int iconX = (this.Width - iconSize) / 2;
            int iconY = (this.Height - iconSize) / 2;
            g.DrawImage(this.Image, iconX, iconY, iconSize, iconSize);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Width = this.Height; // Mantener el botón redondo
        this.Region = new Region(new GraphicsPath(new PointF[] {
            new PointF(0, this.Height / 2),
            new PointF(this.Width / 2, 0),
            new PointF(this.Width, this.Height / 2),
            new PointF(this.Width / 2, this.Height)
        }, new byte[] {
            (byte)PathPointType.Start,
            (byte)PathPointType.Line,
            (byte)PathPointType.Line,
            (byte)PathPointType.Line
        }));
    }
}



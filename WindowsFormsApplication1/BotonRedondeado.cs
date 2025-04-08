using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class BotonRedondeado : Button
{
    public int RadioBorde { get; set; } = 20;
    public Color ColorBorde { get; set; } = Color.Black;
    public Color ColorFondoPersonalizado { get; set; } = Color.LightBlue;
    public Color ColorHover { get; set; } = Color.DeepSkyBlue;

    private bool estaEncima = false;

    public BotonRedondeado()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.Transparent;
        ForeColor = Color.Black;

        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.SupportsTransparentBackColor, true);

        MouseEnter += (s, e) => { estaEncima = true; Invalidate(); };
        MouseLeave += (s, e) => { estaEncima = false; Invalidate(); };
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width, Height);
        GraphicsPath path = CrearRectanguloRedondeado(rect, RadioBorde);

        // Relleno
        Color colorFondo = estaEncima ? ColorHover : ColorFondoPersonalizado;
        using (SolidBrush brush = new SolidBrush(colorFondo))
            e.Graphics.FillPath(brush, path);

        // Borde
        using (Pen pen = new Pen(ColorBorde, 2))
            e.Graphics.DrawPath(pen, path);

        // Texto
        TextRenderer.DrawText(e.Graphics, Text, Font, rect, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private GraphicsPath CrearRectanguloRedondeado(Rectangle rect, int radio)
    {
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
        path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
        path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
        path.CloseFigure();
        return path;
    }
}

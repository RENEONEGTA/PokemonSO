using System;
using System.Drawing;
using System.Windows.Forms;

public class CustomGroupBox : GroupBox
{
    private Color borderColor = Color.Blue; // Color predeterminado del borde

    public Color BorderColor
    {
        get { return borderColor; }
        set { borderColor = value; this.Invalidate(); }
    }

    public CustomGroupBox()
    {
        // Constructor de la clase, equivalente a Sub New() en VB.NET
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
        Rectangle borderRect = e.ClipRectangle;
        borderRect.Y = borderRect.Y + (tSize.Height / 2);
        borderRect.Height = borderRect.Height - (tSize.Height / 2);
        ControlPaint.DrawBorder(e.Graphics, borderRect, borderColor, ButtonBorderStyle.Solid);

        Rectangle textRect = e.ClipRectangle;
        textRect.X = textRect.X + 6;
        textRect.Width = tSize.Width + 2;
        textRect.Height = tSize.Height;
        e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
        e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
    }
}

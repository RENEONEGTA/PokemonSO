using System;
using System.Drawing;
using System.Windows.Forms;

public class ButtonAnimator
{
    public enum AnimationDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public static void AnimateButton(Button button, Point start, Point end, AnimationDirection direction, bool fadeIn)
    {
        int step = 5; // Paso de desplazamiento
        float opacityStep = 0.05f; // Paso de opacidad

        Timer timer = new Timer();
        timer.Interval = 10; // Intervalo de tiempo para la animación

        button.Location = start;
        button.BackColor = Color.FromArgb(fadeIn ? 0 : 255, button.BackColor); // Iniciar con opacidad 0 si es fadeIn

        timer.Tick += (s, args) =>
        {
            bool moved = false;

            // Desplazamiento en X
            if (direction == AnimationDirection.Left || direction == AnimationDirection.Right)
            {
                if (button.Left < end.X)
                {
                    button.Left = Math.Min(button.Left + step, end.X);
                    moved = true;
                }
                else if (button.Left > end.X)
                {
                    button.Left = Math.Max(button.Left - step, end.X);
                    moved = true;
                }
            }

            // Desplazamiento en Y
            if (direction == AnimationDirection.Up || direction == AnimationDirection.Down)
            {
                if (button.Top < end.Y)
                {
                    button.Top = Math.Min(button.Top + step, end.Y);
                    moved = true;
                }
                else if (button.Top > end.Y)
                {
                    button.Top = Math.Max(button.Top - step, end.Y);
                    moved = true;
                }
            }

            // Ajustar opacidad
            if (fadeIn && button.BackColor.A < 255)
            {
                int newAlpha = Math.Min(button.BackColor.A + (int)(opacityStep * 255), 255);
                button.BackColor = Color.FromArgb(newAlpha, button.BackColor.R, button.BackColor.G, button.BackColor.B);
                moved = true;
            }
            else if (!fadeIn && button.BackColor.A > 0)
            {
                int newAlpha = Math.Max(button.BackColor.A - (int)(opacityStep * 255), 0);
                button.BackColor = Color.FromArgb(newAlpha, button.BackColor.R, button.BackColor.G, button.BackColor.B);
                moved = true;
            }

            if (!moved)
            {
                timer.Stop(); // Detener el temporizador cuando se alcanza la posición final y opacidad completa
                if (!fadeIn)
                {
                    button.Visible = false; // Ocultar el botón si es fadeOut
                }
            }
        };

        // Iniciar la animación
        timer.Start();
    }
}

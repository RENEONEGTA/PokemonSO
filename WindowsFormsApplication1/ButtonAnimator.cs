using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication1;

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
        int step = 10; // Paso de desplazamiento
        float opacityStep = 0.05f; // Paso de opacidad

        Timer timer = new Timer();
        timer.Interval = 5; // Intervalo de tiempo para la animación

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


    public static void AnimatePanel(PanelDobleBuffer button, Point start, Point end, AnimationDirection direction, bool fadeIn, Action onAnimationComplete = null)
    {
        int step = 30;
        float opacityStep = 0.05f;

        // Detenemos cualquier animación previa
        if (button.Tag is Timer previousTimer)
        {
            previousTimer.Stop();
            previousTimer.Dispose();
        }

        Timer timer = new Timer { Interval = 15 };
        button.Tag = timer; // Guardamos el timer para futuras animaciones

        button.Location = start;

        // Empezamos con transparencia si es fadeIn
        int alpha = fadeIn ? 150 : 255;
        button.BackColor = Color.FromArgb(alpha, button.BackColor.R, button.BackColor.G, button.BackColor.B);
        button.Visible = true;

        timer.Tick += (s, args) =>
        {
            bool moved = false;
            Point current = button.Location;

            // Movimiento en X
            if (direction == AnimationDirection.Left || direction == AnimationDirection.Right)
            {
                int dx = end.X - current.X;
                if (Math.Abs(dx) > 0)
                {
                    button.Left += Math.Sign(dx) * Math.Min(Math.Abs(dx), step);
                    moved = true;
                }
            }

            // Movimiento en Y
            if (direction == AnimationDirection.Up || direction == AnimationDirection.Down)
            {
                int dy = end.Y - current.Y;
                if (Math.Abs(dy) > 0)
                {
                    button.Top += Math.Sign(dy) * Math.Min(Math.Abs(dy), step);
                    moved = true;
                }
            }

            // Opacidad
            int currentAlpha = button.BackColor.A;
            if (fadeIn && currentAlpha < 255)
            {
                currentAlpha = Math.Min(255, currentAlpha + (int)(opacityStep * 255));
                moved = true;
            }
            else if (!fadeIn && currentAlpha > 150)
            {
                currentAlpha = Math.Max(150, currentAlpha - (int)(opacityStep * 255));
                moved = true;
            }

            button.BackColor = Color.FromArgb(currentAlpha, button.BackColor.R, button.BackColor.G, button.BackColor.B);

            // ¿Terminó la animación?
            if (!moved)
            {
                timer.Stop();
                timer.Dispose();
                button.Tag = null;

                if (!fadeIn)
                    button.Visible = false;

                onAnimationComplete?.Invoke();
            }
        };

        timer.Start();
    }

}

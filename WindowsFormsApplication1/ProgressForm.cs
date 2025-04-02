using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication1
{
    public partial class ProgresForm : Form
    {
        public ProgresForm()
        {
            InitializeComponent();
        }

        // Actualiza el progreso de la barra
        public void UpdateProgress(int percent)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(UpdateProgress), percent);
            }
            else
            {
                progressBar.Value = percent;
            }
        }

        // Configura el estilo de la barra de progreso
        public void SetProgressBarStyle(ProgressBarStyle style)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<ProgressBarStyle>(SetProgressBarStyle), style);
            }
            else
            {
                progressBar.Style = style;
            }
        }

        // Establece el valor máximo de la barra
        public void SetMaxValue(int max)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(SetMaxValue), max);
            }
            else
            {
                progressBar.Maximum = max;
            }
        }
    }

}

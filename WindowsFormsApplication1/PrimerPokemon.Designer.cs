namespace WindowsFormsApplication1
{
    partial class PrimerPokemon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.Charmander = new System.Windows.Forms.Button();
            this.Bulbasaur = new System.Windows.Forms.Button();
            this.Squirtel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(317, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Elige tu primer pokemon";
            // 
            // Charmander
            // 
            this.Charmander.Location = new System.Drawing.Point(88, 297);
            this.Charmander.Name = "Charmander";
            this.Charmander.Size = new System.Drawing.Size(115, 50);
            this.Charmander.TabIndex = 1;
            this.Charmander.Text = "Charmander";
            this.Charmander.UseVisualStyleBackColor = true;
            this.Charmander.Click += new System.EventHandler(this.Charmander_Click);
            // 
            // Bulbasaur
            // 
            this.Bulbasaur.Location = new System.Drawing.Point(333, 297);
            this.Bulbasaur.Name = "Bulbasaur";
            this.Bulbasaur.Size = new System.Drawing.Size(115, 50);
            this.Bulbasaur.TabIndex = 2;
            this.Bulbasaur.Text = "Bulbasaur";
            this.Bulbasaur.UseVisualStyleBackColor = true;
            this.Bulbasaur.Click += new System.EventHandler(this.Bulbasaur_Click);
            // 
            // Squirtel
            // 
            this.Squirtel.Location = new System.Drawing.Point(556, 297);
            this.Squirtel.Name = "Squirtel";
            this.Squirtel.Size = new System.Drawing.Size(115, 50);
            this.Squirtel.TabIndex = 3;
            this.Squirtel.Text = "Squirtel";
            this.Squirtel.UseVisualStyleBackColor = true;
            this.Squirtel.Click += new System.EventHandler(this.Squirtel_Click);
            // 
            // PrimerPokemon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Squirtel);
            this.Controls.Add(this.Bulbasaur);
            this.Controls.Add(this.Charmander);
            this.Controls.Add(this.label1);
            this.Name = "PrimerPokemon";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Charmander;
        private System.Windows.Forms.Button Bulbasaur;
        private System.Windows.Forms.Button Squirtel;
    }
}
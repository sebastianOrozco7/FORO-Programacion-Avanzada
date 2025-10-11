namespace FORO_Programacion_Avanzada
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            txbEdad = new TextBox();
            txbNota1 = new TextBox();
            txbNota2 = new TextBox();
            txbNota3 = new TextBox();
            txbNombre = new TextBox();
            rbMasculino = new RadioButton();
            rbFemenino = new RadioButton();
            label7 = new Label();
            btnRegistrar = new Button();
            dtgvEstudiantes = new DataGridView();
            pbImagen = new PictureBox();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            lbPromedio = new Label();
            lbEstado = new Label();
            menuStrip1 = new MenuStrip();
            cORREOToolStripMenuItem = new ToolStripMenuItem();
            btnLimpiar = new Button();
            chlActividades = new CheckedListBox();
            txbCedula = new TextBox();
            label11 = new Label();
            dtgvEstudianteActividad = new DataGridView();
            btnEditar = new Button();
            btnEliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)dtgvEstudiantes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbImagen).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgvEstudianteActividad).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(37, 35);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 0;
            label1.Text = "DATOS ESTUDIANTES";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 124);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 1;
            label2.Text = "Nombre";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(37, 156);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 2;
            label3.Text = "Edad";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(37, 192);
            label4.Name = "label4";
            label4.Size = new Size(42, 15);
            label4.TabIndex = 3;
            label4.Text = "Nota 1";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(37, 219);
            label5.Name = "label5";
            label5.Size = new Size(42, 15);
            label5.TabIndex = 4;
            label5.Text = "Nota 2";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(37, 249);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 5;
            label6.Text = "Nota 3";
            // 
            // txbEdad
            // 
            txbEdad.Location = new Point(81, 148);
            txbEdad.Name = "txbEdad";
            txbEdad.Size = new Size(100, 23);
            txbEdad.TabIndex = 6;
            // 
            // txbNota1
            // 
            txbNota1.Location = new Point(81, 184);
            txbNota1.Name = "txbNota1";
            txbNota1.Size = new Size(100, 23);
            txbNota1.TabIndex = 7;
            // 
            // txbNota2
            // 
            txbNota2.Location = new Point(81, 213);
            txbNota2.Name = "txbNota2";
            txbNota2.Size = new Size(100, 23);
            txbNota2.TabIndex = 8;
            // 
            // txbNota3
            // 
            txbNota3.Location = new Point(81, 242);
            txbNota3.Name = "txbNota3";
            txbNota3.Size = new Size(100, 23);
            txbNota3.TabIndex = 9;
            // 
            // txbNombre
            // 
            txbNombre.Location = new Point(81, 116);
            txbNombre.Name = "txbNombre";
            txbNombre.Size = new Size(100, 23);
            txbNombre.TabIndex = 10;
            // 
            // rbMasculino
            // 
            rbMasculino.AutoSize = true;
            rbMasculino.Location = new Point(88, 300);
            rbMasculino.Name = "rbMasculino";
            rbMasculino.Size = new Size(80, 19);
            rbMasculino.TabIndex = 11;
            rbMasculino.TabStop = true;
            rbMasculino.Text = "Masculino";
            rbMasculino.UseVisualStyleBackColor = true;
            // 
            // rbFemenino
            // 
            rbFemenino.AutoSize = true;
            rbFemenino.Location = new Point(181, 300);
            rbFemenino.Name = "rbFemenino";
            rbFemenino.Size = new Size(82, 19);
            rbFemenino.TabIndex = 12;
            rbFemenino.TabStop = true;
            rbFemenino.Text = "Femenimo";
            rbFemenino.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(37, 302);
            label7.Name = "label7";
            label7.Size = new Size(45, 15);
            label7.TabIndex = 13;
            label7.Text = "Genero";
            // 
            // btnRegistrar
            // 
            btnRegistrar.Location = new Point(48, 470);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(75, 23);
            btnRegistrar.TabIndex = 17;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // dtgvEstudiantes
            // 
            dtgvEstudiantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvEstudiantes.Location = new Point(204, 58);
            dtgvEstudiantes.Name = "dtgvEstudiantes";
            dtgvEstudiantes.Size = new Size(429, 221);
            dtgvEstudiantes.TabIndex = 20;
            dtgvEstudiantes.SelectionChanged += dtgvEstudiantes_SelectionChanged;
            // 
            // pbImagen
            // 
            pbImagen.Location = new Point(639, 58);
            pbImagen.Name = "pbImagen";
            pbImagen.Size = new Size(88, 90);
            pbImagen.TabIndex = 21;
            pbImagen.TabStop = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(639, 184);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 22;
            label8.Text = "Promedio";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(37, 343);
            label9.Name = "label9";
            label9.Size = new Size(158, 15);
            label9.TabIndex = 23;
            label9.Text = "Actividades Extracurriculares";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(639, 219);
            label10.Name = "label10";
            label10.Size = new Size(42, 15);
            label10.TabIndex = 24;
            label10.Text = "Estado";
            // 
            // lbPromedio
            // 
            lbPromedio.AutoSize = true;
            lbPromedio.Location = new Point(708, 184);
            lbPromedio.Name = "lbPromedio";
            lbPromedio.Size = new Size(19, 15);
            lbPromedio.TabIndex = 25;
            lbPromedio.Text = "....";
            // 
            // lbEstado
            // 
            lbEstado.AutoSize = true;
            lbEstado.Location = new Point(708, 219);
            lbEstado.Name = "lbEstado";
            lbEstado.Size = new Size(19, 15);
            lbEstado.TabIndex = 26;
            lbEstado.Text = "....";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { cORREOToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(860, 24);
            menuStrip1.TabIndex = 28;
            menuStrip1.Text = "menuStrip1";
            // 
            // cORREOToolStripMenuItem
            // 
            cORREOToolStripMenuItem.Name = "cORREOToolStripMenuItem";
            cORREOToolStripMenuItem.Size = new Size(65, 20);
            cORREOToolStripMenuItem.Text = "CORREO";
            cORREOToolStripMenuItem.Click += cORREOToolStripMenuItem_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(129, 470);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 29;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // chlActividades
            // 
            chlActividades.FormattingEnabled = true;
            chlActividades.Location = new Point(48, 370);
            chlActividades.Name = "chlActividades";
            chlActividades.Size = new Size(120, 76);
            chlActividades.TabIndex = 30;
            // 
            // txbCedula
            // 
            txbCedula.Location = new Point(81, 87);
            txbCedula.Name = "txbCedula";
            txbCedula.Size = new Size(100, 23);
            txbCedula.TabIndex = 31;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(31, 90);
            label11.Name = "label11";
            label11.Size = new Size(44, 15);
            label11.TabIndex = 32;
            label11.Text = "Cedula";
            // 
            // dtgvEstudianteActividad
            // 
            dtgvEstudianteActividad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvEstudianteActividad.Location = new Point(381, 279);
            dtgvEstudianteActividad.Name = "dtgvEstudianteActividad";
            dtgvEstudianteActividad.Size = new Size(252, 117);
            dtgvEstudianteActividad.TabIndex = 33;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(210, 470);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(75, 23);
            btnEditar.TabIndex = 34;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(592, 470);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 35;
            btnEliminar.Text = "button1";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 532);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(dtgvEstudianteActividad);
            Controls.Add(label11);
            Controls.Add(txbCedula);
            Controls.Add(chlActividades);
            Controls.Add(btnLimpiar);
            Controls.Add(lbEstado);
            Controls.Add(lbPromedio);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(pbImagen);
            Controls.Add(dtgvEstudiantes);
            Controls.Add(btnRegistrar);
            Controls.Add(label7);
            Controls.Add(rbFemenino);
            Controls.Add(rbMasculino);
            Controls.Add(txbNombre);
            Controls.Add(txbNota3);
            Controls.Add(txbNota2);
            Controls.Add(txbNota1);
            Controls.Add(txbEdad);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dtgvEstudiantes).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbImagen).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dtgvEstudianteActividad).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txbEdad;
        private TextBox txbNota1;
        private TextBox txbNota2;
        private TextBox txbNota3;
        private TextBox txbNombre;
        private RadioButton rbMasculino;
        private RadioButton rbFemenino;
        private Label label7;
        private Button btnRegistrar;
        private DataGridView dtgvEstudiantes;
        private PictureBox pbImagen;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label lbPromedio;
        private Label lbEstado;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem cORREOToolStripMenuItem;
        private Button btnLimpiar;
        private CheckedListBox chlActividades;
        private TextBox txbCedula;
        private Label label11;
        private DataGridView dtgvEstudianteActividad;
        private Button btnEditar;
        private Button btnEliminar;
    }
}

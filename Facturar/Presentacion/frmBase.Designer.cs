namespace Facturar.Presentacion
{
    partial class frmBase
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
            if(disposing && (components != null))
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
            this.panelSuperior = new System.Windows.Forms.Panel();
            this.lbMensaje = new System.Windows.Forms.Label();
            this.panelLateral = new System.Windows.Forms.Panel();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelInferiorGeneralEditar = new System.Windows.Forms.Panel();
            this.panelInferiorGeneral = new System.Windows.Forms.Panel();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnValidar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnLocales = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnEmpresas = new System.Windows.Forms.Button();
            this.btnAbrirPanel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgCerrar = new System.Windows.Forms.PictureBox();
            this.imgMinimizar = new System.Windows.Forms.PictureBox();
            this.btnNnuevo = new System.Windows.Forms.Button();
            this.panelSuperior.SuspendLayout();
            this.panelLateral.SuspendLayout();
            this.panelInferior.SuspendLayout();
            this.panelInferiorGeneralEditar.SuspendLayout();
            this.panelInferiorGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMinimizar)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.LightBlue;
            this.panelSuperior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSuperior.Controls.Add(this.pictureBox1);
            this.panelSuperior.Controls.Add(this.lbMensaje);
            this.panelSuperior.Controls.Add(this.imgCerrar);
            this.panelSuperior.Controls.Add(this.imgMinimizar);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(4, 4);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(942, 40);
            this.panelSuperior.TabIndex = 0;
            // 
            // lbMensaje
            // 
            this.lbMensaje.AutoSize = true;
            this.lbMensaje.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMensaje.Location = new System.Drawing.Point(48, 5);
            this.lbMensaje.Name = "lbMensaje";
            this.lbMensaje.Size = new System.Drawing.Size(227, 25);
            this.lbMensaje.TabIndex = 3;
            this.lbMensaje.Text = "Facturacion de alquieres";
            // 
            // panelLateral
            // 
            this.panelLateral.BackColor = System.Drawing.Color.LightGray;
            this.panelLateral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLateral.Controls.Add(this.btnLocales);
            this.panelLateral.Controls.Add(this.btnClientes);
            this.panelLateral.Controls.Add(this.btnEmpresas);
            this.panelLateral.Controls.Add(this.btnAbrirPanel);
            this.panelLateral.Location = new System.Drawing.Point(4, 44);
            this.panelLateral.Name = "panelLateral";
            this.panelLateral.Size = new System.Drawing.Size(44, 472);
            this.panelLateral.TabIndex = 1;
            // 
            // panelInferior
            // 
            this.panelInferior.BackColor = System.Drawing.Color.LemonChiffon;
            this.panelInferior.Controls.Add(this.panelInferiorGeneralEditar);
            this.panelInferior.Controls.Add(this.panelInferiorGeneral);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(4, 446);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Padding = new System.Windows.Forms.Padding(4);
            this.panelInferior.Size = new System.Drawing.Size(942, 70);
            this.panelInferior.TabIndex = 2;
            // 
            // panelInferiorGeneralEditar
            // 
            this.panelInferiorGeneralEditar.BackColor = System.Drawing.Color.LemonChiffon;
            this.panelInferiorGeneralEditar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInferiorGeneralEditar.Controls.Add(this.btnCancelar);
            this.panelInferiorGeneralEditar.Controls.Add(this.btnValidar);
            this.panelInferiorGeneralEditar.Location = new System.Drawing.Point(40, 0);
            this.panelInferiorGeneralEditar.Name = "panelInferiorGeneralEditar";
            this.panelInferiorGeneralEditar.Size = new System.Drawing.Size(902, 70);
            this.panelInferiorGeneralEditar.TabIndex = 1;
            this.panelInferiorGeneralEditar.Visible = false;
            // 
            // panelInferiorGeneral
            // 
            this.panelInferiorGeneral.BackColor = System.Drawing.Color.LemonChiffon;
            this.panelInferiorGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInferiorGeneral.Controls.Add(this.btnEliminar);
            this.panelInferiorGeneral.Controls.Add(this.btnEditar);
            this.panelInferiorGeneral.Controls.Add(this.btnNuevo);
            this.panelInferiorGeneral.Location = new System.Drawing.Point(40, 0);
            this.panelInferiorGeneral.Name = "panelInferiorGeneral";
            this.panelInferiorGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.panelInferiorGeneral.Size = new System.Drawing.Size(902, 70);
            this.panelInferiorGeneral.TabIndex = 0;
            // 
            // panelCentral
            // 
            this.panelCentral.BackColor = System.Drawing.Color.Tan;
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Location = new System.Drawing.Point(4, 44);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(942, 402);
            this.panelCentral.TabIndex = 3;
            // 
            // btnCancelar
            // 
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::Facturar.Properties.Resources.cancelar;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.Location = new System.Drawing.Point(732, 6);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 60);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.BotonEditar_Click);
            // 
            // btnValidar
            // 
            this.btnValidar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnValidar.FlatAppearance.BorderSize = 0;
            this.btnValidar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnValidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidar.Image = global::Facturar.Properties.Resources.Validar;
            this.btnValidar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnValidar.Location = new System.Drawing.Point(817, 6);
            this.btnValidar.Margin = new System.Windows.Forms.Padding(0);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(80, 60);
            this.btnValidar.TabIndex = 2;
            this.btnValidar.Text = "Validar";
            this.btnValidar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.BotonEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Image = global::Facturar.Properties.Resources.elminar;
            this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEliminar.Location = new System.Drawing.Point(182, 4);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(0);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 60);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.BotonGeneral_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Image = global::Facturar.Properties.Resources.editar;
            this.btnEditar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditar.Location = new System.Drawing.Point(93, 4);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 60);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.BotonGeneral_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevo.Image = global::Facturar.Properties.Resources.añadir;
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNuevo.Location = new System.Drawing.Point(4, 4);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(0);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(80, 60);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Añadir";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.BotonGeneral_Click);
            // 
            // btnLocales
            // 
            this.btnLocales.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLocales.FlatAppearance.BorderSize = 0;
            this.btnLocales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocales.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLocales.Image = global::Facturar.Properties.Resources.Locales;
            this.btnLocales.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLocales.Location = new System.Drawing.Point(2, 230);
            this.btnLocales.Name = "btnLocales";
            this.btnLocales.Size = new System.Drawing.Size(85, 60);
            this.btnLocales.TabIndex = 3;
            this.btnLocales.Text = "Locales";
            this.btnLocales.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLocales.UseVisualStyleBackColor = true;
            this.btnLocales.Visible = false;
            // 
            // btnClientes
            // 
            this.btnClientes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientes.Image = global::Facturar.Properties.Resources.Clientes;
            this.btnClientes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClientes.Location = new System.Drawing.Point(2, 150);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(85, 60);
            this.btnClientes.TabIndex = 2;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Visible = false;
            // 
            // btnEmpresas
            // 
            this.btnEmpresas.FlatAppearance.BorderSize = 0;
            this.btnEmpresas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmpresas.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpresas.Image = global::Facturar.Properties.Resources.Empresas;
            this.btnEmpresas.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEmpresas.Location = new System.Drawing.Point(0, 70);
            this.btnEmpresas.Name = "btnEmpresas";
            this.btnEmpresas.Size = new System.Drawing.Size(85, 60);
            this.btnEmpresas.TabIndex = 1;
            this.btnEmpresas.Text = "Empresas";
            this.btnEmpresas.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEmpresas.UseVisualStyleBackColor = true;
            this.btnEmpresas.Visible = false;
            // 
            // btnAbrirPanel
            // 
            this.btnAbrirPanel.FlatAppearance.BorderSize = 0;
            this.btnAbrirPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirPanel.Image = global::Facturar.Properties.Resources.lista;
            this.btnAbrirPanel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAbrirPanel.Location = new System.Drawing.Point(0, 0);
            this.btnAbrirPanel.Name = "btnAbrirPanel";
            this.btnAbrirPanel.Size = new System.Drawing.Size(40, 40);
            this.btnAbrirPanel.TabIndex = 0;
            this.btnAbrirPanel.UseVisualStyleBackColor = true;
            this.btnAbrirPanel.Click += new System.EventHandler(this.btnAbrirPanel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Facturar.Properties.Resources.Aplicacion;
            this.pictureBox1.Location = new System.Drawing.Point(7, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // imgCerrar
            // 
            this.imgCerrar.Image = global::Facturar.Properties.Resources.cerrar;
            this.imgCerrar.Location = new System.Drawing.Point(905, 5);
            this.imgCerrar.Name = "imgCerrar";
            this.imgCerrar.Size = new System.Drawing.Size(30, 30);
            this.imgCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgCerrar.TabIndex = 2;
            this.imgCerrar.TabStop = false;
            this.imgCerrar.Click += new System.EventHandler(this.imgCerrar_Click);
            // 
            // imgMinimizar
            // 
            this.imgMinimizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgMinimizar.Image = global::Facturar.Properties.Resources.minimizar1;
            this.imgMinimizar.Location = new System.Drawing.Point(859, 5);
            this.imgMinimizar.Name = "imgMinimizar";
            this.imgMinimizar.Size = new System.Drawing.Size(30, 30);
            this.imgMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgMinimizar.TabIndex = 1;
            this.imgMinimizar.TabStop = false;
            this.imgMinimizar.Click += new System.EventHandler(this.imgMinimizar_Click);
            // 
            // btnNnuevo
            // 
            this.btnNnuevo.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnNnuevo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnNnuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNnuevo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNnuevo.Image = global::Facturar.Properties.Resources.añadir;
            this.btnNnuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNnuevo.Location = new System.Drawing.Point(0, 13);
            this.btnNnuevo.Margin = new System.Windows.Forms.Padding(0);
            this.btnNnuevo.Name = "btnNnuevo";
            this.btnNnuevo.Size = new System.Drawing.Size(121, 57);
            this.btnNnuevo.TabIndex = 0;
            this.btnNnuevo.Text = "Nuevo";
            this.btnNnuevo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNnuevo.UseVisualStyleBackColor = true;
            // 
            // frmBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 520);
            this.ControlBox = false;
            this.Controls.Add(this.panelCentral);
            this.Controls.Add(this.panelInferior);
            this.Controls.Add(this.panelLateral);
            this.Controls.Add(this.panelSuperior);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmBase";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBase";
            this.Load += new System.EventHandler(this.frmBase_Load);
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelLateral.ResumeLayout(false);
            this.panelInferior.ResumeLayout(false);
            this.panelInferiorGeneralEditar.ResumeLayout(false);
            this.panelInferiorGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMinimizar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSuperior;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox imgMinimizar;
        private System.Windows.Forms.PictureBox imgCerrar;
        private System.Windows.Forms.Label lbMensaje;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelLateral;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelInferiorGeneral;
        private System.Windows.Forms.Button btnNnuevo;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel panelInferiorGeneralEditar;
        private System.Windows.Forms.Button btnAbrirPanel;
        private System.Windows.Forms.Button btnEmpresas;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnLocales;
    }
}
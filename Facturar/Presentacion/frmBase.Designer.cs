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
            this.panelCentral = new System.Windows.Forms.Panel();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.btnConfigurar = new System.Windows.Forms.Button();
            this.btnContratos = new System.Windows.Forms.Button();
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
            this.panelLateral.BackColor = System.Drawing.Color.Gainsboro;
            this.panelLateral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLateral.Controls.Add(this.btnConfigurar);
            this.panelLateral.Controls.Add(this.btnContratos);
            this.panelLateral.Controls.Add(this.btnLocales);
            this.panelLateral.Controls.Add(this.btnClientes);
            this.panelLateral.Controls.Add(this.btnEmpresas);
            this.panelLateral.Controls.Add(this.btnAbrirPanel);
            this.panelLateral.Location = new System.Drawing.Point(4, 44);
            this.panelLateral.Margin = new System.Windows.Forms.Padding(0);
            this.panelLateral.Name = "panelLateral";
            this.panelLateral.Size = new System.Drawing.Size(45, 472);
            this.panelLateral.TabIndex = 1;
            // 
            // panelCentral
            // 
            this.panelCentral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelCentral.BackColor = System.Drawing.Color.Tan;
            this.panelCentral.Location = new System.Drawing.Point(4, 44);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(942, 472);
            this.panelCentral.TabIndex = 3;
            // 
            // panelInferior
            // 
            this.panelInferior.BackColor = System.Drawing.Color.NavajoWhite;
            this.panelInferior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInferior.Location = new System.Drawing.Point(45, 456);
            this.panelInferior.Margin = new System.Windows.Forms.Padding(0);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(901, 60);
            this.panelInferior.TabIndex = 0;
            // 
            // btnConfigurar
            // 
            this.btnConfigurar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnConfigurar.FlatAppearance.BorderSize = 0;
            this.btnConfigurar.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnConfigurar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnConfigurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigurar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigurar.Image = global::Facturar.Properties.Resources.Utilidades2;
            this.btnConfigurar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfigurar.Location = new System.Drawing.Point(2, 370);
            this.btnConfigurar.Name = "btnConfigurar";
            this.btnConfigurar.Size = new System.Drawing.Size(105, 60);
            this.btnConfigurar.TabIndex = 5;
            this.btnConfigurar.Text = "Configuracion";
            this.btnConfigurar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConfigurar.UseVisualStyleBackColor = true;
            this.btnConfigurar.Visible = false;
            this.btnConfigurar.Click += new System.EventHandler(this.btnConfigurar_Click);
            // 
            // btnContratos
            // 
            this.btnContratos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContratos.FlatAppearance.BorderSize = 0;
            this.btnContratos.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnContratos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnContratos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContratos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContratos.Image = global::Facturar.Properties.Resources.Contratos2_black;
            this.btnContratos.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnContratos.Location = new System.Drawing.Point(2, 245);
            this.btnContratos.Name = "btnContratos";
            this.btnContratos.Size = new System.Drawing.Size(105, 60);
            this.btnContratos.TabIndex = 4;
            this.btnContratos.Text = "Contratos";
            this.btnContratos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContratos.UseVisualStyleBackColor = true;
            this.btnContratos.Visible = false;
            this.btnContratos.Click += new System.EventHandler(this.btnContratos_Click);
            // 
            // btnLocales
            // 
            this.btnLocales.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLocales.FlatAppearance.BorderSize = 0;
            this.btnLocales.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnLocales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnLocales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocales.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLocales.Image = global::Facturar.Properties.Resources.Locales_black;
            this.btnLocales.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLocales.Location = new System.Drawing.Point(2, 180);
            this.btnLocales.Name = "btnLocales";
            this.btnLocales.Size = new System.Drawing.Size(105, 60);
            this.btnLocales.TabIndex = 3;
            this.btnLocales.Text = "Locales";
            this.btnLocales.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLocales.UseVisualStyleBackColor = true;
            this.btnLocales.Visible = false;
            this.btnLocales.Click += new System.EventHandler(this.btnLocales_Click);
            // 
            // btnClientes
            // 
            this.btnClientes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnClientes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientes.Image = global::Facturar.Properties.Resources.Clientes_black;
            this.btnClientes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClientes.Location = new System.Drawing.Point(2, 115);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(105, 60);
            this.btnClientes.TabIndex = 2;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Visible = false;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // btnEmpresas
            // 
            this.btnEmpresas.BackColor = System.Drawing.Color.Transparent;
            this.btnEmpresas.FlatAppearance.BorderSize = 0;
            this.btnEmpresas.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnEmpresas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnEmpresas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmpresas.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpresas.Image = global::Facturar.Properties.Resources.Empresa_black;
            this.btnEmpresas.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEmpresas.Location = new System.Drawing.Point(0, 50);
            this.btnEmpresas.Margin = new System.Windows.Forms.Padding(0);
            this.btnEmpresas.Name = "btnEmpresas";
            this.btnEmpresas.Size = new System.Drawing.Size(105, 60);
            this.btnEmpresas.TabIndex = 1;
            this.btnEmpresas.Text = "Empresas";
            this.btnEmpresas.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEmpresas.UseVisualStyleBackColor = false;
            this.btnEmpresas.Visible = false;
            this.btnEmpresas.Click += new System.EventHandler(this.btnEmpresas_Click);
            // 
            // btnAbrirPanel
            // 
            this.btnAbrirPanel.FlatAppearance.BorderSize = 0;
            this.btnAbrirPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirPanel.Image = global::Facturar.Properties.Resources.Menu_black;
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
            this.btnNnuevo.Image = global::Facturar.Properties.Resources.Añadir_black;
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
            this.Controls.Add(this.panelInferior);
            this.Controls.Add(this.panelCentral);
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
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.Button btnNnuevo;
        private System.Windows.Forms.Button btnAbrirPanel;
        private System.Windows.Forms.Button btnEmpresas;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnLocales;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Button btnContratos;
        private System.Windows.Forms.Button btnConfigurar;
    }
}
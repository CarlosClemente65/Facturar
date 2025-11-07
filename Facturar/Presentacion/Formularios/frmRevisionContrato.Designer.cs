namespace Facturar.Presentacion.Formularios
{
    partial class frmRevisionContrato
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelRevisionContrato_general = new Facturar.Presentacion.Paneles.PanelInferior_general();
            this.panelRevisionContrato_Edicion = new Facturar.Presentacion.Paneles.PanelInferior_Edicion();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.btnVolver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFechaRevision = new System.Windows.Forms.TextBox();
            this.txtPrecioAnterior = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRevision = new System.Windows.Forms.TextBox();
            this.txtPrecioRevisado = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.dgvRevisiones = new System.Windows.Forms.DataGridView();
            this.panelInferior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevisiones)).BeginInit();
            this.SuspendLayout();
            // 
            // panelRevisionContrato_general
            // 
            this.panelRevisionContrato_general.BackColor = System.Drawing.Color.Transparent;
            this.panelRevisionContrato_general.EstadoVisible = false;
            this.panelRevisionContrato_general.Location = new System.Drawing.Point(200, 2);
            this.panelRevisionContrato_general.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panelRevisionContrato_general.Name = "panelRevisionContrato_general";
            this.panelRevisionContrato_general.Size = new System.Drawing.Size(390, 60);
            this.panelRevisionContrato_general.TabIndex = 2;
            // 
            // panelRevisionContrato_Edicion
            // 
            this.panelRevisionContrato_Edicion.Location = new System.Drawing.Point(0, 2);
            this.panelRevisionContrato_Edicion.Margin = new System.Windows.Forms.Padding(0);
            this.panelRevisionContrato_Edicion.Name = "panelRevisionContrato_Edicion";
            this.panelRevisionContrato_Edicion.Size = new System.Drawing.Size(200, 60);
            this.panelRevisionContrato_Edicion.TabIndex = 1;
            this.panelRevisionContrato_Edicion.Visible = false;
            // 
            // panelInferior
            // 
            this.panelInferior.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInferior.BackColor = System.Drawing.Color.OldLace;
            this.panelInferior.Controls.Add(this.btnVolver);
            this.panelInferior.Controls.Add(this.panelRevisionContrato_general);
            this.panelInferior.Controls.Add(this.panelRevisionContrato_Edicion);
            this.panelInferior.Location = new System.Drawing.Point(0, 300);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(683, 60);
            this.panelInferior.TabIndex = 6;
            // 
            // btnVolver
            // 
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(120)))), ((int)(((byte)(60)))));
            this.btnVolver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(180)))), ((int)(((byte)(150)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Image = global::Facturar.Properties.Resources.Vovler;
            this.btnVolver.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVolver.Location = new System.Drawing.Point(590, 2);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(90, 56);
            this.btnVolver.TabIndex = 5;
            this.btnVolver.Text = "Volver";
            this.btnVolver.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVolver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnContratos_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 34);
            this.label1.TabIndex = 8;
            this.label1.Text = "Fecha revision";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFechaRevision
            // 
            this.txtFechaRevision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFechaRevision.Enabled = false;
            this.txtFechaRevision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaRevision.Location = new System.Drawing.Point(12, 251);
            this.txtFechaRevision.Name = "txtFechaRevision";
            this.txtFechaRevision.Size = new System.Drawing.Size(90, 22);
            this.txtFechaRevision.TabIndex = 9;
            this.txtFechaRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFechaRevision.Enter += new System.EventHandler(this.txtFechaRevision_Enter);
            this.txtFechaRevision.Leave += new System.EventHandler(this.txtFechaRevision_Leave);
            // 
            // txtPrecioAnterior
            // 
            this.txtPrecioAnterior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrecioAnterior.Enabled = false;
            this.txtPrecioAnterior.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecioAnterior.Location = new System.Drawing.Point(108, 251);
            this.txtPrecioAnterior.Name = "txtPrecioAnterior";
            this.txtPrecioAnterior.Size = new System.Drawing.Size(100, 22);
            this.txtPrecioAnterior.TabIndex = 75;
            this.txtPrecioAnterior.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPrecioAnterior.Enter += new System.EventHandler(this.txtPrecioAnterior_Enter);
            this.txtPrecioAnterior.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImporte_KeyPress);
            this.txtPrecioAnterior.Leave += new System.EventHandler(this.txtImporte_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(108, 214);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 33);
            this.label9.TabIndex = 76;
            this.label9.Text = "Precio \r\nanterior";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(214, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 33);
            this.label2.TabIndex = 77;
            this.label2.Text = "% \r\nrevision";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRevision
            // 
            this.txtRevision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRevision.Enabled = false;
            this.txtRevision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRevision.Location = new System.Drawing.Point(214, 251);
            this.txtRevision.Name = "txtRevision";
            this.txtRevision.Size = new System.Drawing.Size(80, 22);
            this.txtRevision.TabIndex = 78;
            this.txtRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRevision.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImporte_KeyPress);
            this.txtRevision.Leave += new System.EventHandler(this.txtRevision_Leave);
            // 
            // txtPrecioRevisado
            // 
            this.txtPrecioRevisado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrecioRevisado.Enabled = false;
            this.txtPrecioRevisado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecioRevisado.Location = new System.Drawing.Point(300, 251);
            this.txtPrecioRevisado.Name = "txtPrecioRevisado";
            this.txtPrecioRevisado.Size = new System.Drawing.Size(100, 22);
            this.txtPrecioRevisado.TabIndex = 79;
            this.txtPrecioRevisado.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPrecioRevisado.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImporte_KeyPress);
            this.txtPrecioRevisado.Leave += new System.EventHandler(this.txtImporte_Leave);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(300, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 33);
            this.label3.TabIndex = 80;
            this.label3.Text = "Precio \r\nrevisado";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(406, 233);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 14);
            this.label8.TabIndex = 82;
            this.label8.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservaciones.Enabled = false;
            this.txtObservaciones.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservaciones.Location = new System.Drawing.Point(406, 251);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(274, 22);
            this.txtObservaciones.TabIndex = 81;
            this.txtObservaciones.Leave += new System.EventHandler(this.txtObservaciones_Leave);
            // 
            // dgvRevisiones
            // 
            this.dgvRevisiones.AllowUserToAddRows = false;
            this.dgvRevisiones.AllowUserToDeleteRows = false;
            this.dgvRevisiones.AllowUserToOrderColumns = true;
            this.dgvRevisiones.AllowUserToResizeRows = false;
            this.dgvRevisiones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRevisiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvRevisiones.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRevisiones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRevisiones.ColumnHeadersHeight = 30;
            this.dgvRevisiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRevisiones.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRevisiones.EnableHeadersVisualStyles = false;
            this.dgvRevisiones.Location = new System.Drawing.Point(12, 13);
            this.dgvRevisiones.Margin = new System.Windows.Forms.Padding(0);
            this.dgvRevisiones.MultiSelect = false;
            this.dgvRevisiones.Name = "dgvRevisiones";
            this.dgvRevisiones.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevisiones.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRevisiones.RowHeadersVisible = false;
            this.dgvRevisiones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRevisiones.ShowEditingIcon = false;
            this.dgvRevisiones.ShowRowErrors = false;
            this.dgvRevisiones.Size = new System.Drawing.Size(660, 198);
            this.dgvRevisiones.TabIndex = 83;
            this.dgvRevisiones.SelectionChanged += new System.EventHandler(this.dgvRevisiones_SelectionChanged);
            // 
            // frmRevisionContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.ControlBox = false;
            this.Controls.Add(this.dgvRevisiones);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.txtPrecioRevisado);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRevision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPrecioAnterior);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtFechaRevision);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelInferior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 400);
            this.Name = "frmRevisionContrato";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Revisiones Contrato";
            this.Load += new System.EventHandler(this.frmRevisionContrato_Load);
            this.panelInferior.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevisiones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Paneles.PanelInferior_Edicion panelRevisionContrato_Edicion;
        private Paneles.PanelInferior_general panelRevisionContrato_general;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtFechaRevision;
        private System.Windows.Forms.TextBox txtPrecioAnterior;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRevision;
        private System.Windows.Forms.TextBox txtPrecioRevisado;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtObservaciones;
        protected System.Windows.Forms.DataGridView dgvRevisiones;
    }
}
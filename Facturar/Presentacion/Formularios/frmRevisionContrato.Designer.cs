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
            this.panelRevisionContrato_general = new Facturar.Presentacion.Paneles.PanelInferior_general();
            this.panelRevisionContrato_Edicion = new Facturar.Presentacion.Paneles.PanelInferior_Edicion();
            this.SuspendLayout();
            // 
            // panelRevisionContrato_general
            // 
            this.panelRevisionContrato_general.BackColor = System.Drawing.Color.Transparent;
            this.panelRevisionContrato_general.EstadoVisible = false;
            this.panelRevisionContrato_general.Location = new System.Drawing.Point(217, 289);
            this.panelRevisionContrato_general.Margin = new System.Windows.Forms.Padding(0);
            this.panelRevisionContrato_general.Name = "panelRevisionContrato_general";
            this.panelRevisionContrato_general.Size = new System.Drawing.Size(455, 60);
            this.panelRevisionContrato_general.TabIndex = 2;
            // 
            // panelRevisionContrato_Edicion
            // 
            this.panelRevisionContrato_Edicion.Location = new System.Drawing.Point(13, 288);
            this.panelRevisionContrato_Edicion.Name = "panelRevisionContrato_Edicion";
            this.panelRevisionContrato_Edicion.Size = new System.Drawing.Size(150, 60);
            this.panelRevisionContrato_Edicion.TabIndex = 1;
            this.panelRevisionContrato_Edicion.Visible = false;
            // 
            // frmRevisionContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.panelRevisionContrato_general);
            this.Controls.Add(this.panelRevisionContrato_Edicion);
            this.Name = "frmRevisionContrato";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRevisionContrato";
            this.Load += new System.EventHandler(this.frmRevisionContrato_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Paneles.PanelInferior_Edicion panelRevisionContrato_Edicion;
        private Paneles.PanelInferior_general panelRevisionContrato_general;
    }
}
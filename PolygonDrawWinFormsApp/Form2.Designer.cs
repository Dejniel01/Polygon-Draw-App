
namespace PolygonDrawWinFormsApp
{
    partial class Form2
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
            this.Label = new System.Windows.Forms.Label();
            this.LengthSelector = new System.Windows.Forms.NumericUpDown();
            this.OKButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LengthSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Location = new System.Drawing.Point(12, 9);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(94, 20);
            this.Label.TabIndex = 0;
            this.Label.Text = "Insert length:";
            // 
            // LengthSelector
            // 
            this.LengthSelector.DecimalPlaces = 2;
            this.LengthSelector.Location = new System.Drawing.Point(12, 32);
            this.LengthSelector.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.LengthSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.LengthSelector.Name = "LengthSelector";
            this.LengthSelector.Size = new System.Drawing.Size(208, 27);
            this.LengthSelector.TabIndex = 1;
            this.LengthSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LengthSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(69, 87);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(94, 29);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 128);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.LengthSelector);
            this.Controls.Add(this.Label);
            this.MaximumSize = new System.Drawing.Size(250, 175);
            this.MinimumSize = new System.Drawing.Size(250, 175);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.LengthSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.NumericUpDown LengthSelector;
        private System.Windows.Forms.Button OKButton;
    }
}

namespace PolygonDrawWinFormsApp
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.BresenhamCheckBox = new System.Windows.Forms.CheckBox();
            this.ModesGroupBox = new System.Windows.Forms.GroupBox();
            this.PerpendicularButton = new System.Windows.Forms.RadioButton();
            this.LengthButton = new System.Windows.Forms.RadioButton();
            this.NormalButton = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.ModesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.Canvas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BresenhamCheckBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ModesGroupBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1182, 753);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Canvas
            // 
            this.Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Canvas.Location = new System.Drawing.Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.tableLayoutPanel1.SetRowSpan(this.Canvas, 2);
            this.Canvas.Size = new System.Drawing.Size(1026, 747);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // BresenhamCheckBox
            // 
            this.BresenhamCheckBox.AutoSize = true;
            this.BresenhamCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.BresenhamCheckBox.Location = new System.Drawing.Point(1035, 3);
            this.BresenhamCheckBox.Name = "BresenhamCheckBox";
            this.BresenhamCheckBox.Size = new System.Drawing.Size(144, 24);
            this.BresenhamCheckBox.TabIndex = 1;
            this.BresenhamCheckBox.Text = "use Bresenham?";
            this.BresenhamCheckBox.UseVisualStyleBackColor = true;
            this.BresenhamCheckBox.CheckedChanged += new System.EventHandler(this.BresenhamCheckBox_CheckedChanged);
            // 
            // ModesGroupBox
            // 
            this.ModesGroupBox.Controls.Add(this.PerpendicularButton);
            this.ModesGroupBox.Controls.Add(this.LengthButton);
            this.ModesGroupBox.Controls.Add(this.NormalButton);
            this.ModesGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModesGroupBox.Location = new System.Drawing.Point(1035, 53);
            this.ModesGroupBox.Name = "ModesGroupBox";
            this.ModesGroupBox.Size = new System.Drawing.Size(144, 350);
            this.ModesGroupBox.TabIndex = 2;
            this.ModesGroupBox.TabStop = false;
            this.ModesGroupBox.Text = "App Modes";
            // 
            // PerpendicularButton
            // 
            this.PerpendicularButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.PerpendicularButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.PerpendicularButton.Location = new System.Drawing.Point(3, 223);
            this.PerpendicularButton.Name = "PerpendicularButton";
            this.PerpendicularButton.Size = new System.Drawing.Size(138, 100);
            this.PerpendicularButton.TabIndex = 2;
            this.PerpendicularButton.TabStop = true;
            this.PerpendicularButton.Text = "Perpendicular constraints";
            this.PerpendicularButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PerpendicularButton.UseVisualStyleBackColor = true;
            this.PerpendicularButton.CheckedChanged += new System.EventHandler(this.Button_CheckedChanged);
            // 
            // LengthButton
            // 
            this.LengthButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.LengthButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.LengthButton.Location = new System.Drawing.Point(3, 123);
            this.LengthButton.Name = "LengthButton";
            this.LengthButton.Size = new System.Drawing.Size(138, 100);
            this.LengthButton.TabIndex = 1;
            this.LengthButton.Tag = "";
            this.LengthButton.Text = "Length constraints";
            this.LengthButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LengthButton.UseVisualStyleBackColor = true;
            this.LengthButton.CheckedChanged += new System.EventHandler(this.Button_CheckedChanged);
            // 
            // NormalButton
            // 
            this.NormalButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.NormalButton.Checked = true;
            this.NormalButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.NormalButton.Location = new System.Drawing.Point(3, 23);
            this.NormalButton.Name = "NormalButton";
            this.NormalButton.Size = new System.Drawing.Size(138, 100);
            this.NormalButton.TabIndex = 0;
            this.NormalButton.TabStop = true;
            this.NormalButton.Tag = "";
            this.NormalButton.Text = "Normal";
            this.NormalButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NormalButton.UseVisualStyleBackColor = true;
            this.NormalButton.CheckedChanged += new System.EventHandler(this.Button_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 753);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ModesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.CheckBox BresenhamCheckBox;
        private System.Windows.Forms.GroupBox ModesGroupBox;
        private System.Windows.Forms.RadioButton LengthButton;
        private System.Windows.Forms.RadioButton NormalButton;
        private System.Windows.Forms.RadioButton PerpendicularButton;
    }
}


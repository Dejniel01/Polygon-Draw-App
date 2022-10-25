using PolygonApp.PolygonModel.Helpers;
using PolygonApp.PolygonModel.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PolygonDrawWinFormsApp
{
    public partial class Form2 : Form
    {
        private readonly Form1 mainForm = null;
        private readonly Edge edge = null;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Form1 callingForm, Edge edge)
        {
            mainForm = callingForm;
            this.edge = edge;
            InitializeComponent();
            LengthSelector.Value = (decimal)Utils.Distance(edge.Source, edge.Destination);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            mainForm.AddLengthConstraint(edge, (float)LengthSelector.Value);
            Close();
        }
    }
}

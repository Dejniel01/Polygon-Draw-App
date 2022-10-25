using PolygonApp.PolygonModel.Structures;
using PolygonApp.PolygonModel.Helpers;
using PolygonApp.PolygonPresentationUnit;
using PolygonApp.PolygonProcessingUnit;
using PolygonApp.PolygonProcessingUnit.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonDrawWinFormsApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Width of canvas
        /// </summary>
        private const int WIDTH = 1200;
        /// <summary>
        /// Height of canvas
        /// </summary>
        private const int HEIGHT = 800;

        private readonly PolygonProcU procU = null;
        private readonly PolygonPresU presU = null;

        public Form1()
        {
            InitializeComponent();

            procU = new PolygonProcU();
            presU = new PolygonPresU(Canvas, WIDTH, HEIGHT, procU, DefaultFont);

            CreateSamplePolygonsWithConstraints();
            presU.RedrawCanvas();
        }

        /// <summary>
        /// Creates sample scene with 2 polygons
        /// </summary>
        private void CreateSamplePolygonsWithConstraints()
        {
            var poly1 = new Polygon()
            {
                Points = new List<Vertex>()
                {
                    new Vertex(146, 307),
                    new Vertex(127, 233),
                    new Vertex(277, 100),
                    new Vertex(372, 130),
                    new Vertex(460, 250),
                    new Vertex(447, 326),
                    new Vertex(326, 463),
                    new Vertex(129, 437)
                },
                IsClosed = true
            };


            var poly2 = new Polygon()
            {
                Points = new List<Vertex>()
                {
                    new Vertex(701, 141),
                    new Vertex(868, 159),
                    new Vertex(850, 330),
                    new Vertex(705, 468),
                    new Vertex(555, 336),
                    new Vertex(502, 143)
                },
                IsClosed = true
            };

            poly1.UpdateBounds();
            poly2.UpdateBounds();

            presU.Polygons.Add(poly1);
            presU.Polygons.Add(poly2);

            foreach (var point in poly1.Points)
                presU.vertexToPolygonDictionary.Add(point, poly1);
            foreach (var point in poly2.Points)
                presU.vertexToPolygonDictionary.Add(point, poly2);

            procU.AddLengthConstraint(new Edge(poly1.Points[1], poly1.Points[2]), 200);
            procU.AddLengthConstraint(new Edge(poly1.Points[2], poly1.Points[3]), 100);
            procU.AddLengthConstraint(new Edge(poly1.Points[3], poly1.Points[4]), 150);

            procU.AddLengthConstraint(new Edge(poly2.Points[2], poly2.Points[3]), 200);
            //procU.AddLengthConstraint(new Edge(poly2.Points[4], poly2.Points[5]), 200);

            procU.AddPerpendicularConstraint(new Edge(poly1.Points[6], poly1.Points[7]));
            procU.AddPerpendicularConstraint(new Edge(poly1.Points[7], poly1.Points[0]));

            procU.AddPerpendicularConstraint(new Edge(poly2.Points[0], poly2.Points[1]));
            procU.AddPerpendicularConstraint(new Edge(poly2.Points[1], poly2.Points[2]));

            procU.AddPerpendicularConstraint(new Edge(poly1.Points[5], poly1.Points[6]));
            procU.AddPerpendicularConstraint(new Edge(poly2.Points[3], poly2.Points[4]));

            procU.AddPerpendicularConstraint(new Edge(poly2.Points[0], poly2.Points[1]));
            procU.AddPerpendicularConstraint(new Edge(poly2.Points[5], poly2.Points[0]));

            presU.MoveVertex(poly2.Points[5], poly2.Points[5].X, poly2.Points[5].Y);
        }

        #region Mouse events

        /// <summary>
        /// Event for clicking on canvas
        /// </summary>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (NormalButton.Checked)
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        presU.AddOrInitMovingPolygon(e.X, e.Y);
                        break;
                    case MouseButtons.Right:
                        presU.RemoveVertex(e.X, e.Y);
                        break;
                    case MouseButtons.Middle:
                        presU.InsertVertexInMiddle(e.X, e.Y);
                        break;
                    default:
                        return;
                }
            else if (LengthButton.Checked)
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        LeftButtonDownLength(sender, e);
                        break;
                    case MouseButtons.Right:
                        RightButtonDownLength(sender, e);
                        break;
                    default:
                        return;
                }
            else if (PerpendicularButton.Checked)
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        LeftButtonDownPerp(sender, e);
                        break;
                    case MouseButtons.Right:
                        RightButtonDownPerp(sender, e);
                        break;
                    default:
                        return;
                }

            presU.RedrawCanvas();
        }

        /// <summary>
        /// Event for moving mouse on canvas
        /// </summary>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            presU.MovePolygon(e.X, e.Y);
        }

        /// <summary>
        /// Event for releasing mouse button on canvas
        /// </summary>
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            presU.StopMovingPolygon();
        }

        #endregion

        #region Perpendicularity constraint

        /// <summary>
        /// Adding perpendicularity constraints
        /// </summary>
        private void LeftButtonDownPerp(object sender, MouseEventArgs e)
        {
            var (_, edge, _) = Utils.GetTargetObjects(presU.Polygons, e.X, e.Y);
            if (!(edge is null))
            {
                var cons = procU.AddPerpendicularConstraint(edge);
                if (!(cons is null))
                {
                    var ret = presU.MoveVertex(edge.Source, edge.Source.X, edge.Source.Y);
                    if (!ret)
                        procU.RemoveConstraint(cons);
                }
            }
            else
                presU.Reset();
        }

        /// <summary>
        /// Deleting perpendicularity constraints
        /// </summary>
        private void RightButtonDownPerp(object sender, MouseEventArgs e)
        {
            (_, var edge, _) = Utils.GetTargetObjects(presU.Polygons, e.X, e.Y);

            if (edge is null)
            {
                presU.Reset();
                return;
            }

            procU.RemovePerpendicularConstraint(edge);
        }

        #endregion

        #region Length constraints

        /// <summary>
        /// Adding length constraints
        /// </summary>
        private void LeftButtonDownLength(object sender, MouseEventArgs e)
        {
            (_, var edge, _) = Utils.GetTargetObjects(presU.Polygons, e.X, e.Y);

            if (edge is null) return;

            Form childForm = new Form2(this, edge);
            childForm.Location = new Point(Location.X + Width / 2 - childForm.Width / 2,
                                           Location.Y + Height / 2 - childForm.Height / 2);
            childForm.ShowDialog();
            childForm.Dispose();
        }

        /// <summary>
        /// Deleting length constraints
        /// </summary>
        private void RightButtonDownLength(object sender, MouseEventArgs e)
        {
            (_, var edge, _) = Utils.GetTargetObjects(presU.Polygons, e.X, e.Y);

            if (edge is null)
            {
                presU.Reset();
                return;
            }

            procU.RemoveLengthConstraint(edge);
        }

        public void AddLengthConstraint(Edge e, float length)
        {
            var cons = procU.AddLengthConstraint(e, length);

            if (!(cons is null))
            {
                bool ret = presU.MoveVertex(e.Source, e.Source.X, e.Source.Y);
                if (!ret)
                {
                    ret = presU.MoveVertex(e.Destination, e.Destination.X, e.Destination.Y);
                    if (!ret)
                        procU.RemoveConstraint(cons);
                }
            }
        }

        #endregion

        #region Menu buttons events

        /// <summary>
        /// Update usage of Bresenham's algorthm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BresenhamCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            presU.UseBresenham = BresenhamCheckBox.Checked;
        }

        #endregion

        private void Button_CheckedChanged(object sender, EventArgs e)
        {
            presU.Reset();
        }
    }
}

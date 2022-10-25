using PolygonApp.PolygonModel.Helpers;
using PolygonApp.PolygonModel.Structures;
using PolygonApp.PolygonProcessingUnit;
using PolygonApp.PolygonProcessingUnit.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp.PolygonPresentationUnit
{
    public class PolygonPresU
    {
        private readonly PictureBox canvas = null;
        private readonly Bitmap drawArea = null;
        private readonly PolygonProcU procU = null;
        private readonly Font font = null;

        private Polygon newPolygon = null;
        private Vertex firstPoint = null;
        private Vertex movingPoint = null;
        private Polygon movingPolygon = null;
        private (float X, float Y) prevMousePos = (0, 0);
        private Edge movingEdge = null;
        private Vertex newPoint = null;

        public readonly Dictionary<Vertex, Polygon> vertexToPolygonDictionary = new Dictionary<Vertex, Polygon>();
        private readonly Dictionary<(Vertex V1, Vertex V2), HashSet<IConstraint>> constraintsToDraw = new Dictionary<(Vertex V1, Vertex V2), HashSet<IConstraint>>();

        public List<Polygon> Polygons { get; private set; } = new List<Polygon>();

        public PolygonPresU(PictureBox canvas, int width, int height, PolygonProcU procU, Font font)
        {
            this.canvas = canvas;
            this.canvas.Image = drawArea = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }

            this.procU = procU;
            this.font = font;
        }

        /// <summary>
        /// Property that switches mode of drawing edges - use Bresenham's algorithm or <see cref="Graphics.DrawLine"/>
        /// </summary>
        public bool UseBresenham
        {
            get => _useBresenham;
            set
            {
                _useBresenham = value;
                RedrawCanvas();
            }
        }
        private bool _useBresenham;


        /// <summary>
        /// Clears canvas and redraws it
        /// </summary>
        public void RedrawCanvas()
        {
            InvalidateConstraintsToDraw();
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);

                foreach (var polygon in Polygons)
                    polygon.Draw(drawArea, UseBresenham);

                foreach (var edge in constraintsToDraw)
                {
                    string s = string.Join(';', edge.Value);
                    var size = g.MeasureString(s, font);
                    g.DrawString(s, font, Brushes.Blue, (edge.Key.V1.X + edge.Key.V2.X - size.Width) / 2, (edge.Key.V1.Y + edge.Key.V2.Y - size.Height) / 2);
                }

                var (e, color) = procU.GetSelectedEdge();
                if (!(e is null))
                    g.DrawLine(new Pen(color, 2), e.Source.X, e.Source.Y, e.Destination.X, e.Destination.Y);
            }
            canvas.Refresh();
        }

        /// <summary>
        /// Resets state of unit
        /// </summary>
        public void Reset()
        {
            if (!(newPolygon is null))
                Polygons.Remove(newPolygon);
            newPolygon = null;
            firstPoint = null;
            movingPoint = null;
            movingPolygon = null;
            movingEdge = null;
            newPoint = null;
            procU.Reset();
            RedrawCanvas();
        }

        #region Basic user operations

        /// <summary>
        /// Initializes moving of vertex/edge/polygon or initializes creation of new polygon
        /// </summary>
        /// <param name="x">X coordinate on canvas</param>
        /// <param name="y">Y coordinate on canvas</param>
        public void AddOrInitMovingPolygon(float x, float y)
        {
            prevMousePos = (x, y);

            if (newPolygon is null)
            {
                (movingPolygon, movingEdge, movingPoint) = Utils.GetTargetObjects(Polygons, x, y);
                if (movingPolygon is null)
                {
                    firstPoint = new Vertex(x, y);
                    newPoint = new Vertex(x, y);
                    newPolygon = new Polygon();
                    newPolygon.Points.Add(firstPoint);
                    newPolygon.Points.Add(newPoint);
                    Polygons.Add(newPolygon);
                }
            }
            else if (firstPoint.Contains(x, y))
            {
                if (newPolygon.Points.Count >= 3)
                {
                    newPolygon.IsClosed = true;
                    newPolygon.UpdateBounds();
                    newPolygon.Points.RemoveAt(newPolygon.Points.Count - 1);
                    foreach (var point in newPolygon.Points)
                        vertexToPolygonDictionary.Add(point, newPolygon);
                }
                else
                    Polygons.RemoveAt(Polygons.Count - 1);
                newPolygon = null;
                firstPoint = null;
                newPoint = null;
            }
            else
            {
                newPoint = new Vertex(x, y);
                newPolygon.Points.Add(newPoint);
            }
        }

        /// <summary>
        /// Creates new vertex in the middle of an edge in point specified
        /// </summary>
        public void InsertVertexInMiddle(float x, float y)
        {
            Edge splitEdge = null;
            Polygon splitPolygon = null;

            (splitPolygon, splitEdge, _) = Utils.GetTargetObjects(Polygons, x, y);

            if (!(splitEdge is null))
            {
                procU.RemoveAllConstraints(splitEdge);
                var beginning = splitPolygon.Points.TakeWhile(point => point != splitEdge.Destination).ToList();
                beginning.Add(new Vertex((splitEdge.Source.X + splitEdge.Destination.X) / 2, (splitEdge.Source.Y + splitEdge.Destination.Y) / 2));
                splitPolygon.Points = beginning.Concat(splitPolygon.Points.Skip(beginning.Count - 1).TakeWhile(o => true)).ToList();
            }
        }

        /// <summary>
        /// Deletes vertex in point specified
        /// </summary>
        /// <param name="x">X coordinate on canvas</param>
        /// <param name="y">Y coordinate on canvas</param>
        public void RemoveVertex(float x, float y)
        {
            if (newPoint is null)
            {
                (Polygon poly, _, Vertex point) = Utils.GetTargetObjects(Polygons, x, y);
                if (!(point is null))
                {
                    var neighs = poly.GetNeighbors(point);
                    foreach (var neigh in neighs)
                        procU.RemoveAllConstraints(new Edge(point, neigh));

                    poly.Points.Remove(point);
                    vertexToPolygonDictionary.Remove(point);
                    if (poly.Points.Count < 3)
                    {
                        foreach (var pp in poly.Points)
                            vertexToPolygonDictionary.Remove(pp);
                        Polygons.Remove(poly);
                    }
                }
            }
        }

        /// <summary>
        /// Moves polygon 
        /// </summary>
        /// <param name="x">X coordinate on canvas</param>
        /// <param name="y">Y coordinate on canvas</param>
        public void MovePolygon(float x, float y)
        {
            if (prevMousePos.X == x && prevMousePos.Y == y) return;

            var deltaX = x - prevMousePos.X;
            var deltaY = y - prevMousePos.Y;

            if (!(newPoint is null))
            {
                newPoint.X = x;
                newPoint.Y = y;
            }
            else if (!(movingEdge is null))
            {
                MoveVertex(movingEdge.Source, movingEdge.Source.X + deltaX, movingEdge.Source.Y + deltaY, new List<Vertex>() { movingEdge.Destination });
                MoveVertex(movingEdge.Destination, movingEdge.Destination.X + deltaX, movingEdge.Destination.Y + deltaY);
            }
            else if (!(movingPoint is null))
            {
                MoveVertex(movingPoint, movingPoint.X + deltaX, movingPoint.Y + deltaY);
            }
            else if (!(movingPolygon is null))
            {
                MovePolygon(movingPolygon, deltaX, deltaY);
            }

            prevMousePos.X = x;
            prevMousePos.Y = y;
            RedrawCanvas();
            movingPolygon?.UpdateBounds();
        }

        /// <summary>
        /// Stops moving
        /// </summary>
        public void StopMovingPolygon()
        {
            movingPoint = null;
            movingEdge = null;
            movingPolygon = null;
            RedrawCanvas();
        }

        #endregion

        #region Moving elements

        /// <summary>
        /// Moves vertex to point specified with keeping constraints
        /// </summary>
        /// <param name="v">Vertex to be moved</param>
        /// <param name="x">X coordinate on canvas</param>
        /// <param name="y">Y coordinate on canvas</param>
        /// <param name="caller">Optional, previously moved vertex in direction of which constraint wont be enfoced</param>
        /// <param name="counter">Iteration counter</param>
        /// <returns></returns>
        public bool MoveVertex(Vertex v, float x, float y, List<Vertex> callers = null, int counter = 0)
        {
            if (!(v.NewX is null && v.NewY is null))
            {
                float newX = v.NewX ?? v.X;
                float newY = v.NewY ?? v.Y;
                if (Utils.Distance(x, y, newX, newY) > 1e-2)
                {
                    return false;
                }
            }

            v.NewX = x;
            v.NewY = y;

            if (counter > 1000) return false; // ensures that Stack Overflow won't happen

            foreach (var (nextV, cals, (newX, newY)) in procU.CalculateConstraintPositions(v, callers))
                if (!float.IsNaN(newX) && !float.IsNaN(newX))
                {
                    foreach(var cal in cals)
                    {
                        cal.NewX ??= cal.X;
                        cal.NewY ??= cal.Y;
                    }

                    bool ret = MoveVertex(nextV, newX, newY, cals, counter + 1);


                    if (!ret)
                    {
                        if (counter == 0)
                        {
                            MovePolygon(vertexToPolygonDictionary[v], x - v.X, y - v.Y);
                            ClearNewPositions();
                        }
                        return false;
                    }
                }

            if (counter == 0)
                SetNewPositions();
            return true;
        }

        /// <summary>
        /// Move polygon
        /// </summary>
        /// <param name="polygon">Target polygon</param>
        /// <param name="deltax">Change in X coordinate</param>
        /// <param name="deltay">Change in Y coordinate</param>
        private void MovePolygon(Polygon polygon, float deltax, float deltay)
        {
            foreach (var point in polygon.Points)
            {
                point.X += deltax;
                point.Y += deltay;
            }
        }

        /// <summary>
        /// Clea
        /// </summary>
        private void ClearNewPositions()
        {
            foreach (var poly in Polygons)
                foreach (var point in poly.Points)
                    point.NewX = point.NewY = null;
        }

        private void SetNewPositions()
        {
            foreach (var poly in Polygons)
                foreach (var point in poly.Points)
                {
                    float newPos;
                    if ((newPos = point.NewX.GetValueOrDefault()) != default) point.X = newPos;
                    if ((newPos = point.NewY.GetValueOrDefault()) != default) point.Y = newPos;
                    point.NewX = point.NewY = null;
                }
            RedrawCanvas();
        }

        #endregion

        private void InvalidateConstraintsToDraw()
        {
            constraintsToDraw.Clear();
            foreach (var (v1, v2, constraint) in procU.GetConstraintsToDraw())
                if (constraintsToDraw.TryGetValue((v1, v2), out var set))
                    set.Add(constraint);
                else
                    constraintsToDraw.Add((v1, v2), new HashSet<IConstraint>() { constraint });
        }
    }
}

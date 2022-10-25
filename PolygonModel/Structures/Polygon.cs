using PolygonApp.PolygonModel.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PolygonApp.PolygonModel.Structures
{
    public class Polygon : IDrawingElement
    {
        private static int _id_s = 0;
        private int _id;

        public Polygon()
        {
            _id = ++_id_s;
        }

        public List<Vertex> Points { get; set; } = new List<Vertex>();
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MinX { get; set; }
        public float MinY { get; set; }
        public bool IsClosed { get; set; }

        public void Draw(Bitmap drawArea, bool useBresenham)
        {
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                for (int i = 0; i < Points.Count - 1; i++)
                    if (!(Points[i].BezierVertex is null || Points[i + 1].BezierVertex is null))
                    {
                        using (Pen dashedPen = new Pen(Color.Gray))
                        {
                            dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            g.DrawLine(dashedPen, Points[i].X, Points[i].Y, Points[i].BezierVertex.X, Points[i].BezierVertex.Y);
                            g.DrawLine(dashedPen, Points[i].BezierVertex.X, Points[i].BezierVertex.Y, Points[i + 1].BezierVertex.X, Points[i + 1].BezierVertex.Y);
                            g.DrawLine(dashedPen, Points[i + 1].BezierVertex.X, Points[i + 1].BezierVertex.Y, Points[i + 1].X, Points[i + 1].Y);
                        }
                        Utils.Bezier(Points[i], Points[i].BezierVertex, Points[i + 1].BezierVertex, Points[i+1], drawArea);
                        //g.DrawBezier(Pens.Black, Points[i].X, Points[i].Y, Points[i].BezierVertex.X, Points[i].BezierVertex.Y, Points[i + 1].BezierVertex.X, Points[i + 1].BezierVertex.Y, Points[i + 1].X, Points[i + 1].Y);
                    }
                    else
                    {
                        if (useBresenham)
                            Utils.Bresenham(Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y, drawArea);
                        else
                            g.DrawLine(Pens.Black, Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y);
                    }
                if (IsClosed)
                    if (!(Points[^1].BezierVertex is null || Points[0].BezierVertex is null))
                    {
                        using (Pen dashedPen = new Pen(Color.Gray))
                        {
                            dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            g.DrawLine(dashedPen, Points[^1].X, Points[^1].Y, Points[^1].BezierVertex.X, Points[^1].BezierVertex.Y);
                            g.DrawLine(dashedPen, Points[^1].BezierVertex.X, Points[^1].BezierVertex.Y, Points[0].BezierVertex.X, Points[0].BezierVertex.Y);
                            g.DrawLine(dashedPen, Points[0].BezierVertex.X, Points[0].BezierVertex.Y, Points[0].X, Points[0].Y);
                        }

                        Utils.Bezier(Points[^1], Points[^1].BezierVertex, Points[0].BezierVertex, Points[0], drawArea);
                        //g.DrawBezier(Pens.Black, Points[^1].X, Points[^1].Y, Points[^1].BezierVertex.X, Points[^1].BezierVertex.Y, Points[0].BezierVertex.X, Points[0].BezierVertex.Y, Points[0].X, Points[0].Y);
                    }
                    else
                    {
                        if (useBresenham)
                            Utils.Bresenham(Points[^1].X, Points[^1].Y, Points[0].X, Points[0].Y, drawArea);
                        else
                            g.DrawLine(Pens.Black, Points[^1].X, Points[^1].Y, Points[0].X, Points[0].Y);
                    }
            }

            foreach (var point in Points)
                point.Draw(drawArea);
        }
        public (bool Check, IDrawingElement obj) Contains(float x, float y)
        {
            return (MinX <= x && MaxX >= x && MinY <= y && MaxY >= y, this);
        }

        public void UpdateBounds()
        {
            MaxX = MaxY = float.MinValue;
            MinX = MinY = float.MaxValue;

            foreach (var point in Points)
            {
                if (MaxX < point.X)
                    MaxX = point.X;
                if (MaxY < point.Y)
                    MaxY = point.Y;
                if (MinX > point.X)
                    MinX = point.X;
                if (MinY > point.Y)
                    MinY = point.Y;
            }
        }
        public override string ToString()
        {
            return $"Polygon no {_id}";
        }

        public Vertex[] GetNeighbors(Vertex v)
        {
            var idx = Points.IndexOf(v);
            if (idx == 0) return new Vertex[] { Points[^1], Points[1] };
            if (idx == Points.Count - 1) return new Vertex[] { Points[idx - 1], Points[0] };
            return new Vertex[] { Points[idx - 1], Points[idx + 1] };
        }
    }
}

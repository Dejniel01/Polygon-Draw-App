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
                    if (useBresenham)
                        Utils.Bresenham(Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y, drawArea);
                    else
                      g.DrawLine(Pens.Black, Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y);
                if (IsClosed)
                    if (useBresenham)
                        Utils.Bresenham(Points[^1].X, Points[^1].Y, Points[0].X, Points[0].Y, drawArea);
                    else
                        g.DrawLine(Pens.Black, Points[^1].X, Points[^1].Y, Points[0].X, Points[0].Y);
            }

            foreach (var point in Points)
                point.Draw(drawArea);
        }
        public bool Contains(float x, float y) => MinX <= x && MaxX >= x && MinY <= y && MaxY >= y; 
        public void UpdateBounds()
        {
            MaxX = MaxY = float.MinValue;
            MinX = MinY = float.MaxValue;

            foreach(var point in Points)
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

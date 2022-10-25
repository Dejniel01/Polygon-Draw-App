using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PolygonApp.PolygonModel.Structures
{
    public class Vertex : IDrawingElement
    {

        private static int _id_s = 0;
        private int _id;

        public const float R = 5.0F;

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
            _id = ++_id_s;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float? NewX { get; set; }
        public float? NewY { get; set; }

        public void Draw(Bitmap drawArea, bool useBresenham = false)
        {
            using Graphics g = Graphics.FromImage(drawArea);
            g.FillEllipse(Brushes.Black, X - R, Y - R, 2*R, 2*R);
            //g.DrawString(_id.ToString(), SystemFonts.DefaultFont, Brushes.Black, X - R, Y - R);
        }

        public bool Contains(float x, float y) => (x - X) * (x - X) + (y - Y) * (y - Y) <= R * R;

        public override string ToString()
        {
            return $"Vertex no {_id} X = {X}; Y = {Y}";
        }
    }
}

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

        private float _x;
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                if (!(BezierVertex is null))
                    BezierVertex.X += value - _x;
                _x = value;
            }
        }
        private float _y;

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (!(BezierVertex is null))
                    BezierVertex.Y += value - _y;
                _y = value;
            }
        }

        private float? _newX;
        public float? NewX
        {
            get => _newX;
            set
            {
                if (value is null && !(BezierVertex is null))
                    BezierVertex.NewX = null;
                _newX = value;
            }
        }

        private float? _newY;
        public float? NewY
        {
            get => _newY;
            set
            {
                if (value is null && !(BezierVertex is null))
                    BezierVertex.NewY = null;
                _newY = value;
            }
        }

        public Vertex BezierVertex { get; set; } = null;

        public void Draw(Bitmap drawArea, bool useBresenham = false)
        {
            using Graphics g = Graphics.FromImage(drawArea);
            g.FillEllipse(Brushes.Black, X - R, Y - R, 2 * R, 2 * R);

            BezierVertex?.Draw(drawArea);
            //g.DrawString(_id.ToString(), SystemFonts.DefaultFont, Brushes.Black, X - R, Y - R);
        }

        public (bool Check, IDrawingElement obj) Contains(float x, float y)
        {
            if ((x - X) * (x - X) + (y - Y) * (y - Y) <= R * R)
                return (true, this);
            if (!(BezierVertex is null))
                return BezierVertex.Contains(x, y);
            return (false, this);
        }

        public override string ToString()
        {
            return $"Vertex no {_id} X = {X}; Y = {Y}";
        }
    }
}

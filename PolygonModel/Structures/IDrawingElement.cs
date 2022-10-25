using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PolygonApp.PolygonModel.Structures
{
    public interface IDrawingElement
    {
        public void Draw(Bitmap drawArea, bool useBresenham);
        public (bool Check, IDrawingElement obj) Contains(float x, float y);
    }
}

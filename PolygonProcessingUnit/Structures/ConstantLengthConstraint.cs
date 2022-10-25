using PolygonApp.PolygonModel.Helpers;
using PolygonApp.PolygonModel.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonApp.PolygonProcessingUnit.Structures
{
    public class ConstantLengthConstraint : IConstraint
    {
        public ConstantLengthConstraint(Vertex v1, Vertex v2, float length)
        {
            Length = length;
            Vertices = new List<Vertex>() { v1, v2 };
        }

        public float Length { get; set; }
        public List<Vertex> Vertices { get; private set; }
        public bool IsMarked { get; set; }

        public bool ContainsEdge(Edge e)
        {
            return Vertices.Contains(e.Source) && Vertices.Contains(e.Destination);
        }

        public override string ToString()
        {
            return $"{'\u21D4'}";
        }

        public (Vertex V, List<Vertex> Callers, (float X, float Y) Position) CalculateNewPosition(Vertex movedVertex)
        {
            var nextV = Vertices[0] == movedVertex ? Vertices[1] : Vertices[0];
            return (nextV, new List<Vertex>() { movedVertex }, CalculateNewCoordinatesForConstantLength(nextV, movedVertex, Length));
        }

        private (float X, float Y) CalculateNewCoordinatesForConstantLength(float x1, float y1, float x2, float y2, float d)
        {
            if (Math.Abs(Utils.Distance(x1, y1, x2, y2) - d) <= 1e-2)
                return (float.NaN, float.NaN);
            float A = y2 - y1;
            float B = x2 - x1;
            float inv = 1 / (float)Math.Sqrt(A * A + B * B);

            float newx1;
            float newx2;
            float newy1;
            float newy2;

            if (B != 0)
            {
                newx1 = x2 - B * d * inv;
                newx2 = x2 + B * d * inv;

                newy1 = y2 - A * d * inv;
                newy2 = y2 + A * d * inv;
            }
            else
            {
                newx1 = newx2 = x2;

                newy1 = y2 - d;
                newy2 = y2 + d;
            }

            if (Utils.Distance(newx1, newy1, x1, y1) < Utils.Distance(newx2, newy2, x1, y1))
                return (newx1, newy1);
            else
                return (newx2, newy2);
        }

        private (float X, float Y) CalculateNewCoordinatesForConstantLength(Vertex V1, Vertex V2, float d)
            => CalculateNewCoordinatesForConstantLength(V1.NewX ?? V1.X, V1.NewY ?? V1.Y, V2.NewX ?? V2.X, V2.NewY ?? V2.Y, d);

        public IEnumerable<(Vertex V1, Vertex V2)> GetDrawingData()
        {
            yield return (Vertices[0], Vertices[1]);
        }
    }
}

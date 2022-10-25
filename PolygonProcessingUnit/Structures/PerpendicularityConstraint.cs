using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PolygonApp.PolygonModel.Structures;
using PolygonApp.PolygonModel.Helpers;

namespace PolygonApp.PolygonProcessingUnit.Structures
{
	public class PerpendicularityConstraint : IConstraint
	{
        private static int _s_id = 0;
        private readonly int _id;
		public PerpendicularityConstraint(Edge e1, Edge e2)
		{
            var points = new HashSet<Vertex>(4)
            {
                e1.Source,
                e1.Destination,
                e2.Source,
                e2.Destination
            };
			Vertices = points.ToList();
            this.e1 = e1;
            this.e2 = e2;
            _id = ++_s_id;
		}

		public List<Vertex> Vertices { get; private set; }
        private readonly Edge e1;
        private readonly Edge e2;

        public bool IsMarked { get; set; }

        public bool ContainsEdge(Edge e)
        {
            return e1 == e || e2 == e;
        }

        public (Vertex V, List<Vertex> Callers, (float X, float Y) Position) CalculateNewPosition(Vertex movedVertex)
        {
            if (Vertices.Count == 3 && movedVertex == (e1.Source == e2.Source || e1.Source == e2.Destination ? e1.Source : e1.Destination))
            {
                var otherV = Vertices.Where(v => v != movedVertex);
                var stationary = otherV.First();
                var nextV = otherV.Last();

                return (nextV, new List<Vertex>() { movedVertex }, CalculateNewCoordinatesForPerpendicularity(stationary, movedVertex, movedVertex, nextV));
            }
            else
            {
                Vertex stationary = null;
                if (e1.Source == movedVertex) stationary = e1.Destination;
                else if (e1.Destination == movedVertex) stationary = e1.Source;
                else if (e2.Source == movedVertex) stationary = e2.Destination;
                else if (e2.Destination == movedVertex) stationary = e2.Source;

                var otherV = Vertices.Where(p => p != movedVertex && p != stationary).ToList();

                var callers = new List<Vertex>() { stationary };
                if (otherV.Count == 2)
                    callers.Add(otherV.First());

                return (otherV.Last(), callers, CalculateNewCoordinatesForPerpendicularity(movedVertex, stationary, otherV.Count == 2 ? otherV.First() : stationary, otherV.Last()));
            }
        }

        public override string ToString()
        {
            return $"{'\u27C2'}{_id}";
        }

        private (float X, float Y) CalculateNewCoordinatesForPerpendicularity(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            if (ArePerpendicular(x1, y1, x2, y2, x3, y3, x4, y4))
                return (float.NaN, float.NaN);

			float d = Utils.Distance(x3, y3, x4, y4);
			float A = y2 - y1;
			float B = x2 - x1;
            float inv = 1 / (float)Math.Sqrt(A * A + B * B);

            float newx1;
            float newx2;
            float newy1;
            float newy2;

            if (B != 0)
            {
                newx1 = x3 - A * d * inv;
                newx2 = x3 + A * d * inv;

                newy1 = y3 + B * d * inv;
                newy2 = y3 - B * d * inv;
            }
            else
            {
                newy1 = newy2 = y2;

                newx1 = x2 - d;
                newx2 = x2 + d;
            }

            if (Utils.Distance(newx1, newy1, x4, y4) < Utils.Distance(newx2, newy2, x4, y4))
                return (newx1, newy1);
            else
                return (newx2, newy2);
        }

        private (float X, float Y) CalculateNewCoordinatesForPerpendicularity(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
            => CalculateNewCoordinatesForPerpendicularity(v1.NewX ?? v1.X, v1.NewY ?? v1.Y, v2.NewX ?? v2.X, v2.NewY ?? v2.Y, v3.NewX ?? v3.X, v3.NewY ?? v3.Y, v4.NewX ?? v4.X, v4.NewY ?? v4.Y);

        private bool ArePerpendicular(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            return Math.Abs((x2 - x1) * (x4 - x3) + (y2 - y1) * (y4 - y3)) < 1e-2;
        }

        public IEnumerable<(Vertex V1, Vertex V2)> GetDrawingData()
        {
            foreach (var e in new List<Edge>() { e1, e2 })
                yield return (e.Source, e.Destination);
        }
    }
}

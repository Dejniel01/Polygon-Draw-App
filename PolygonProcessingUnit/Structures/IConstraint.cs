using PolygonApp.PolygonModel.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonApp.PolygonProcessingUnit.Structures
{
    public interface IConstraint
    {
        public List<Vertex> Vertices { get; }

        public bool IsMarked { get; set; }

        public (Vertex V, List<Vertex> Callers, (float X, float Y) Position) CalculateNewPosition(Vertex movedVertex);

        public IEnumerable<(Vertex V1, Vertex V2)> GetDrawingData();

        public bool ContainsEdge(Edge e);
    }
}

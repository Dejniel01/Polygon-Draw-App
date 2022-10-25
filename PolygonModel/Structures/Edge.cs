using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PolygonApp.PolygonModel.Structures
{
    public class Edge
    {
        private static int _id_s = 0;
        private int _id;

        public Edge(Vertex source, Vertex destination)
        {
            Source = source;
            Destination = destination;
            _id = ++_id_s;
        }
        public Vertex Source { get; }
        public Vertex Destination { get; }

        public override string ToString()
        {
            return $"Edge no {_id} between ({Source}) and ({Destination})";
        }

        public static bool operator ==(Edge e1, Edge e2)
        {
            return (e1.Source, e1.Destination) == (e2.Source, e2.Destination)
                || (e1.Source, e1.Destination) == (e2.Destination, e2.Source);
        }

        public static bool operator !=(Edge e1, Edge e2)
        {
            return !(e1 == e2);
        }
    }
}

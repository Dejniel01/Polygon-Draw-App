using PolygonApp.PolygonModel.Helpers;
using PolygonApp.PolygonModel.Structures;
using PolygonApp.PolygonProcessingUnit.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp.PolygonProcessingUnit
{
    public class PolygonProcU
    {
        private readonly Dictionary<Vertex, List<IConstraint>> constraints = new Dictionary<Vertex, List<IConstraint>>();
        private Edge perpEdge = null;
        private Edge removeConstraintsEdge = null;

        public PolygonProcU()
        {

        }

        /// <summary>
        /// Enforces all of the constraints triggered by moving vertex
        /// </summary>
        /// <param name="v">Moved vertex</param>
        /// <param name="caller">Previously moved vertex that won't be moved</param>
        /// <returns>Parameters for next move vertex call</returns>
        public IEnumerable<(Vertex V, List<Vertex> Callers, (float X, float Y) Position)> CalculateConstraintPositions(Vertex v, List<Vertex> callers)
        {
            if (constraints.TryGetValue(v, out var consList))
                foreach (var cons in consList.Where(con => !con.Vertices.IsEquivalent(callers)))
                    yield return cons.CalculateNewPosition(v);
        }

        /// <summary>
        /// Resets state of unit
        /// </summary>
        public void Reset()
        {
            perpEdge = null;
            removeConstraintsEdge = null;
        }

        #region Adding constraints

        /// <summary>
        /// Adds new perpendicular constraint
        /// </summary>
        /// <param name="e">Edge targeted</param>
        /// <returns>Created constraint or null</returns>
        public IConstraint AddPerpendicularConstraint(Edge e)
        {
            if (perpEdge is null)
            {
                perpEdge = e;
                return null;
            }
            else
            {
                if (perpEdge == e)
                    return null;
                return AddPerpendicularConstraintInner(perpEdge, e);
            }
        }

        private IConstraint AddPerpendicularConstraintInner(Edge e1, Edge e2)
        {
            var points = new HashSet<Vertex>(4)
            {
                e1.Source,
                e1.Destination,
                e2.Source,
                e2.Destination
            };
            var constraint = new PerpendicularityConstraint(e1, e2);

            foreach (var p in points)
                AddConstraint(p, constraint);

            this.perpEdge = null;

            return constraint;
        }

        /// <summary>
        /// Adds new length constraint
        /// </summary>
        /// <param name="e">Edge targeted</param>
        /// <param name="length">Length to be set</param>
        /// <returns>Created constraint</returns>
        public IConstraint AddLengthConstraint(Edge e, float length)
        {
            var constraint = new ConstantLengthConstraint(e.Source, e.Destination, length);

            AddConstraint(e.Source, constraint);
            AddConstraint(e.Destination, constraint);

            return constraint;
        }

        private void AddConstraint(Vertex v, IConstraint constraint)
        {
            if (constraints.TryGetValue(v, out var list))
            {
                list.RemoveAll(cons => cons.GetType() == constraint.GetType() && cons.Vertices.All(v => constraint.Vertices.Contains(v)));

                list.Add(constraint);
            }
            else
                constraints.Add(v, new List<IConstraint>() { constraint });
        }

        #endregion

        #region Removing constraints

        /// <summary>
        /// Removes perpendicularity constraint between 2 edges
        /// </summary>
        /// <param name="e">Edge</param>
        public void RemovePerpendicularConstraint(Edge e)
        {
            if (removeConstraintsEdge is null)
                removeConstraintsEdge = e;
            else
            {
                var vertices = new List<Vertex>()
                {
                    e.Source,
                    e.Destination,
                    removeConstraintsEdge.Source,
                    removeConstraintsEdge.Destination
                };
                if (constraints.TryGetValue(e.Source, out var l))
                    l.RemoveAll(cons => cons.Vertices.IsEquivalent(vertices));
                if (constraints.TryGetValue(e.Destination, out l))
                    l.RemoveAll(cons => cons.Vertices.IsEquivalent(vertices));
                if (constraints.TryGetValue(removeConstraintsEdge.Source, out l))
                    l.RemoveAll(cons => cons.Vertices.IsEquivalent(vertices));
                if (constraints.TryGetValue(removeConstraintsEdge.Destination, out l))
                    l.RemoveAll(cons => cons.Vertices.IsEquivalent(vertices));

                Reset();
            }
        }

        /// <summary>
        /// Removes length constraint on edge
        /// </summary>
        /// <param name="e">Edge</param>
        public void RemoveLengthConstraint(Edge e)
        {
            if (constraints.TryGetValue(e.Source, out var l))
                l.RemoveAll(cons => cons.Vertices.Count == 2 && cons.Vertices.Contains(e.Destination));
            if (constraints.TryGetValue(e.Destination, out l))
                l.RemoveAll(cons => cons.Vertices.Count == 2 && cons.Vertices.Contains(e.Source));
        }

        /// <summary>
        /// Removes all constraints on edge
        /// </summary>
        /// <param name="e">Edge</param>
        public void RemoveAllConstraints(Edge e)
        {
            bool marked = false;
            if (constraints.TryGetValue(e.Source, out var l))
            {
                marked = true;
                l.ForEach(cons => cons.IsMarked = cons.ContainsEdge(e));
            }

            if (constraints.TryGetValue(e.Destination, out l))
            {
                marked = true;
                l.ForEach(cons => cons.IsMarked = cons.ContainsEdge(e));
            }

            if (marked)
                foreach (var keyValuePair in constraints)
                    keyValuePair.Value.RemoveAll(cons => cons.IsMarked);
        }

        /// <summary>
        /// Deletes specified constraint
        /// </summary>
        /// <param name="cons"></param>
        public void RemoveConstraint(IConstraint cons)
        {
            foreach (var v in cons.Vertices)
                if(constraints.TryGetValue(v, out var l))
                l.Remove(cons);
        }

        #endregion

        /// <summary>
        /// Gets collections of parameters for placing constraints on canvas
        /// </summary>
        /// <returns>Tuple of 2 vertices and constraint object</returns>
        public IEnumerable<(Vertex V1, Vertex V2, IConstraint Constraint)> GetConstraintsToDraw()
        {
            foreach (var keyValuePair in constraints)
                foreach (var cons in keyValuePair.Value)
                    foreach (var (v1, v2) in cons.GetDrawingData())
                        yield return (v1, v2, cons);
        }

        /// <summary>
        /// Gets edge that should be highlighted
        /// </summary>
        /// <returns>Tuple of edge end color</returns>
        public (Edge e, Color color) GetSelectedEdge()
        {
            if (!(perpEdge is null)) return (perpEdge, Color.Blue);
            if (!(removeConstraintsEdge is null)) return (removeConstraintsEdge, Color.Red);
            return (null, Color.Black);
        }
    }
}

using PolygonApp.PolygonModel.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PolygonApp.PolygonModel.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Increments index modulo n
        /// </summary>
        /// <param name="i">Index</param>
        /// <param name="n">Max value</param>
        public static int Increment(int i, int n) => (i + 1) % n;

        /// <summary>
        /// Distance between 2 points
        /// </summary>
        /// <param name="x1">X coordinate of first point</param>
        /// <param name="y1">Y coordinate of first point</param>
        /// <param name="x2">X coordinate of second point</param>
        /// <param name="y2">Y coordinate of second point</param>
        public static float Distance(float x1, float y1, float x2, float y2)
            => (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

        /// <summary>
        /// Distance between 2 points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        public static float Distance(Vertex p1, Vertex p2)
            => Distance(p1.X, p1.Y, p2.X, p2.Y);

        /// <summary>
        /// Checks if point is in close proximity of line
        /// </summary>
        /// <param name="x0">X coordinate of target point</param>
        /// <param name="y0">Y coordinate of target point</param>
        /// <param name="x1">X coordinate of first point of line</param>
        /// <param name="y1">Y coordinate of first point of line</param>
        /// <param name="x2">X coordinate of second point of line</param>
        /// <param name="y2">Y coordinate of second point of line</param>
        public static bool IsOnLine(float x0, float y0, float x1, float y1, float x2, float y2)
            => Math.Abs(Distance(x0, y0, x1, y1) + Distance(x0, y0, x2, y2) - Distance(x1, y1, x2, y2)) < 0.5F;

        /// <summary>
        /// Checks if point is in close proximity of line
        /// </summary>
        /// <param name="p0">Target point</param>
        /// <param name="p1">First point of line</param>
        /// <param name="p2">Second point of line</param>
        public static bool IsOnLine(Vertex p0, Vertex p1, Vertex p2)
            => IsOnLine(p0.X, p0.Y, p1.X, p1.Y, p2.X, p2.Y);

        public static int Round(float x)
        {
            if (x - (int)x < 0.5)
                return (int)x;
            return (int)x + 1;
        }

        /// <summary>
        /// Draws line using Bresenham's algorithm
        /// (<see cref="https://pl.wikipedia.org/wiki/Algorytm_Bresenhama"/>)
        /// </summary>
        /// <param name="x1f">X coordinate of first point</param>
        /// <param name="y1f">Y coordinate of first point</param>
        /// <param name="x2f">X coordinate of second point</param>
        /// <param name="y2f">Y coordinate of second point</param>
        /// <param name="drawArea">Bitmap</param>
        public static void Bresenham(float x1f, float y1f, float x2f, float y2f, Bitmap drawArea)
        {
            int x1 = Round(x1f);
            int y1 = Round(y1f);
            int x2 = Round(x2f);
            int y2 = Round(y2f);

            // zmienne pomocnicze
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;
            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel
            if (x >= 0 && x < drawArea.Width && y >= 0 && y < drawArea.Height)
                drawArea.SetPixel(x, y, Color.Black);
            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    if (x >= 0 && x < drawArea.Width && y >= 0 && y < drawArea.Height)
                        drawArea.SetPixel(x, y, Color.Black);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    if (x >= 0 && x < drawArea.Width && y >= 0 && y < drawArea.Height)
                        drawArea.SetPixel(x, y, Color.Black);
                }
            }
        }

        public static void Bezier(Vertex v0, Vertex v1, Vertex v2, Vertex v3, Bitmap drawArea)
        {
            float A0x = v0.X;
            float A1x = 3 * (v1.X - v0.X);
            float A2x = 3 * (v2.X - 2 * v1.X + v0.X);
            float A3x = v3.X - 3 * v2.X + 3 * v1.X - v0.X;

            float A0y = v0.Y;
            float A1y = 3 * (v1.Y - v0.Y);
            float A2y = 3 * (v2.Y - 2 * v1.Y + v0.Y);
            float A3y = v3.Y - 3 * v2.Y + 3 * v1.Y - v0.Y;

            for (float t = 0; t <= 1; t += 0.001f)
            {
                int x = Round(((A3x * t + A2x) * t + A1x) * t + A0x);
                int y = Round(((A3y * t + A2y) * t + A1y) * t + A0y);

                if (x >= 0 && x < drawArea.Width && y >= 0 && y < drawArea.Height)
                    drawArea.SetPixel(x, y, Color.Black);
            }
        }

        /// <summary>
        /// Gets objects placed in point specified
        /// </summary>
        /// <param name="polygons">List of polygons</param>
        /// <param name="x">X coordinate of target point</param>
        /// <param name="y">Y coordinate of target point</param>
        /// <returns></returns>
        public static (Polygon Polygon, Edge Edge, Vertex Vertex) GetTargetObjects(List<Polygon> polygons, float x, float y)
        {
            foreach (var polygon in polygons)
            {
                int n = polygon.Points.Count;

                (bool Check, IDrawingElement Obj) ret;

                if ((ret = polygon.Points[0].Contains(x, y)).Check)
                    return (polygon, null, ret.Obj as Vertex);

                for (int i = 0; i < n; i++)
                {
                    if ((ret = polygon.Points[Increment(i, n)].Contains(x, y)).Check)
                        return (polygon, null, ret.Obj as Vertex);
                    if (IsOnLine(x, y, polygon.Points[i].X, polygon.Points[i].Y, polygon.Points[Increment(i, n)].X, polygon.Points[Increment(i, n)].Y))
                        return (polygon, new Edge(polygon.Points[i], polygon.Points[Increment(i, n)]), null);
                }
            }

            return (polygons.Where(poly => poly.Contains(x, y).Check).FirstOrDefault(), null, null);
        }

        /// <summary>
        /// Checks if 2 lists contains the same elements
        /// </summary>
        /// <typeparam name="T">Type of list elements</typeparam>
        /// <param name="list">First list</param>
        /// <param name="other">Second list</param>
        /// <returns></returns>
        public static bool IsEquivalent<T>(this List<T> list, List<T> other)
        {
            if (other == null) return false;

            if (list.Distinct().Count() != other.Distinct().Count()) return false;

            var set = new HashSet<T>(list);
            int prevCount = set.Count;
            foreach (var item in other)
                set.Add(item);
            return prevCount == set.Count;
        }
    }
}

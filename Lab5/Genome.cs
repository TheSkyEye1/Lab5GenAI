using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5
{
    public class Genome
    {
        public List<Point> genes;
        public double fitness = 0;

        public Genome(List<Point> path)
        {
            genes = path;
        }

        public Genome mutate(Random rng)
        {
            List<Point> path = new List<Point>(genes);

            int ind = rng.Next(0, path.Count);
            double x = path[ind].X;
            double y = path[ind].Y;

            x += rng.NextDouble() * 100 - 50;
            y += rng.NextDouble() * 100 - 50;

            path[ind] = new Point(x, y);

            return new Genome(path);

        }

        public void calculateFitness(Point start, Point finish, List<Point> obstacle)
        {
            double distance = 0;
            double point_dist = Point.Subtract(finish, start).Length / genes.Count / 2;
            distance += Point.Subtract(genes[0], start).Length;

            for (int i = 0; i < genes.Count - 1; i++)
            {
                if (obstacleIntersectTest(genes[i], genes[i + 1], obstacle) == true)
                {
                    distance += Point.Subtract(genes[i + 1], genes[i]).Length * 10;
                }
                else
                {
                    distance += Point.Subtract(genes[i + 1], genes[i]).Length;
                }

                if (Point.Subtract(genes[i + 1], genes[i]).Length < point_dist)
                {
                    distance += (point_dist - Point.Subtract(genes[i + 1], genes[i]).Length) * 5;
                }
            }

            distance += Point.Subtract(finish, genes[genes.Count - 1]).Length;

            fitness = distance;
        }

        public bool obstacleIntersectTest(Point a, Point b, List<Point> obstacle)
        {
            bool isIntersect = false;
            for (int i = 0; i < obstacle.Count - 1; i++)
            {
                if (Intersect(a, b, obstacle[i], obstacle[i + 1]))
                {
                    isIntersect = true;
                }
            }

            if (Intersect(a, b, obstacle[0], obstacle[obstacle.Count - 1]))
            {
                isIntersect = true;
            }

            return isIntersect;

        }


        public bool Intersect(Point p1, Point p2, Point q1, Point q2)
        {
            int o1 = Orientation(p1, p2, q1);
            int o2 = Orientation(p1, p2, q2);
            int o3 = Orientation(q1, q2, p1);
            int o4 = Orientation(q1, q2, p2);

            // Общий случай
            if (o1 != o2 && o3 != o4)
                return true;

            // Специальные случаи
            if (o1 == 0 && OnSegment(p1, q1, p2))
                return true;
            if (o2 == 0 && OnSegment(p1, q2, p2))
                return true;
            if (o3 == 0 && OnSegment(q1, p1, q2))
                return true;
            if (o4 == 0 && OnSegment(q1, p2, q2))
                return true;

            return false;
        }
        public int Orientation(Point p, Point q, Point r)
        {
            int val = (int)((q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));
            if (val == 0) return 0; // Колинеарны
            return (val > 0) ? 1 : 2; // 1 - по часовой, 2 - против часовой
        }
        public bool OnSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;
            return false;
        }

    }
}

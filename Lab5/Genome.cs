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
            
            int ind = rng.Next(0,path.Count);
            double x = path[ind].X;
            double y = path[ind].Y;

            x += rng.NextDouble() * 100 - 50;
            y += rng.NextDouble() * 100 - 50;

            path[ind] = new Point(x,y);

            return new Genome(path);

        }

        public void calculateFitness(Point start, Point finish)
        {
            double distance = 0;

            distance += Point.Subtract(genes[0], start).Length;

            for(int i = 0; i<genes.Count-1; i++)
            {
                distance += Point.Subtract(genes[i + 1], genes[i]).Length;
                if(Point.Subtract(genes[i + 1], genes[i]).Length < 20)
                {
                    distance += (20 - Point.Subtract(genes[i + 1], genes[i]).Length) * 5; 
                }
            }

            distance += Point.Subtract(finish, genes[genes.Count - 1]).Length;

            fitness = distance;
        }
    }
}

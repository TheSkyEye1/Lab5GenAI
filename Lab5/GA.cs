using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5
{
    public class GA
    {
        List<Genome> population = new List<Genome>();

        List<Point> obstacle;

        int populationLimit;
        double mutationcChance = 0.7;
        Point start;
        Point finish;
        Random rng = new Random();

        void sortByFitness()
        {
            foreach (Genome g in population)
                if (g.fitness == 0)
                    g.calculateFitness(start, finish, obstacle);

            population.Sort((a, b) => (a.fitness.CompareTo(b.fitness)));

        }

        public double getBestFitness()
        {
            return population[0].fitness;
        }

        public void setPopulation(List<List<Point>> set, Point start, Point finish)
        {
            foreach (List<Point> path in set)
                population.Add(new Genome(path));

            this.start = start;
            this.finish = finish;

            populationLimit = set.Count;

            sortByFitness();
        }

        public List<Genome> parentSElection()
        {
            List<Genome> parents = new List<Genome>();

            for (int i = 0; i < population.Count / 10 + 2; i++)
            {
                int ind = rng.Next(population.Count);

                if (parents.Contains(population[ind]))
                    i--;
                else
                    parents.Add(population[ind]);
            }

            parents.Sort((a, b) => (a.fitness.CompareTo(b.fitness)));

            return parents;
        }

        public void crossover(List<Genome> parents)
        {
            List<Point> points1 = new List<Point>();
            List<Point> points2 = new List<Point>();

            int point = rng.Next(2, parents[0].genes.Count - 2);

            for (int i = 0; i < parents[0].genes.Count; i++)
                if (i < point)
                {
                    points1.Add(parents[0].genes[i]);
                    points2.Add(parents[1].genes[i]);
                }
                else
                {
                    points1.Add(parents[1].genes[i]);
                    points2.Add(parents[0].genes[i]);
                }

            population.Add(new Genome(points1));
            population.Add(new Genome(points2));
        }

        public void nextGeneration()
        {
            int cross = rng.Next(1, population.Count / 2);
            for (int i = 0; i < cross; i++)
                crossover(parentSElection());

            for (int i = 0; i < population.Count; i++)
                if (rng.NextDouble() <= mutationcChance)
                    population.Add(population[i].mutate(rng));

            sortByFitness();

            population.RemoveRange(populationLimit, population.Count - populationLimit);
        }

        public List<List<Point>> getPopulation()
        {
            List<List<Point>> set = new List<List<Point>>();

            foreach (Genome g in population)
            {
                set.Add(g.genes);
            }

            return set;
        }

        public void setObstacles(List<Point> obstacle)
        {
            this.obstacle = obstacle;
        }



    }
}

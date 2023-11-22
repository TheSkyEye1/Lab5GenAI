using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab5
{
    /// <summary>
    /// Логика взаимодейсimalтвия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start = new Point(150, 150);
        Point finish = new Point(550, 150);
        double radius = 30;
        int waypoints = 9;
        int populationSize = 11;

        GA gen = new GA();
        int generation = 0;

        DispatcherTimer timer = new DispatcherTimer();
        Random rng = new Random();
        double optimalLength = 0;
        List<Point> obstacle = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();

            obstacle.Add(new Point(200, 50));
            obstacle.Add(new Point(400, 50));
            obstacle.Add(new Point(400, 250));
            obstacle.Add(new Point(200, 250));

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        public void drawEllipse(Point p, double r)
        {
            Ellipse el = new Ellipse();
            SolidColorBrush cb = new SolidColorBrush();
            cb.Color = Color.FromArgb(255, 255, 255, 0);
            el.Fill = cb;
            el.StrokeThickness = 2;
            el.Stroke = Brushes.Black;
            el.Width = r;
            el.Height = r;
            el.RenderTransform = new TranslateTransform(p.X, p.Y);
            scene.Children.Add(el);
        }

        public void drawLine(Point p1, Point p2, double r, bool shortest)
        {
            Line line = new Line();
            if (shortest)
                line.Stroke = Brushes.Black;
            else
                line.Stroke = Brushes.LightGray;

            line.X1 = p1.X + r / 2;
            line.Y1 = p1.Y + r / 2;
            line.X2 = p2.X + r / 2;
            line.Y2 = p2.Y + r / 2;

            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = 1;
            scene.Children.Add(line);
        }

        public void drawOblstacle()
        {
            for (int i = 0; i < obstacle.Count - 1; i++)
            {
                drawLine(obstacle[i], obstacle[i + 1], radius, true);
            }
            drawLine(obstacle[0], obstacle[obstacle.Count - 1], radius, true);
        }

        public void drawStartNFinish()
        {
            drawEllipse(start, radius);
            drawEllipse(finish, radius);
        }

        public void drawPath(List<Point> path, bool thic)
        {

            drawLine(start, path[0], radius, thic);

            for (int i = 0; i < path.Count - 1; i++)
            {
                drawLine(path[i], path[i + 1], radius, thic);
            }

            drawLine(path[path.Count - 1], finish, radius, thic);
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            generation++;
            LGen.Content = generation.ToString();
            gen.nextGeneration();
            drawSet();
            Llen.Content = (gen.getBestFitness() / optimalLength).ToString();
            if ((gen.getBestFitness() / optimalLength) <= 100.01)
                timer.Stop();
        }

        public void drawSet()
        {
            scene.Children.Clear();

            drawStartNFinish();
            drawOblstacle();

            for (int i = 1; i < gen.getPopulation().Count; i++)
            {
                drawPath(gen.getPopulation()[i], false);
            }
            drawPath(gen.getPopulation()[0], true);
        }


        public List<Point> randomPath(double maxX, double maxY, int len)
        {
            List<Point> path = new List<Point>();
            for (int i = 0; i < len; i++)
                path.Add(new Point(rng.NextDouble() * (maxX - radius) + radius, rng.NextDouble() * (maxY - radius) + radius));
            return path;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            optimalLength = Point.Subtract(finish, start).Length / 100.0;

            List<List<Point>> set = new List<List<Point>>();

            for (int i = 0; i < populationSize; i++)
                set.Add(randomPath(scene.Width - radius, scene.Height - radius, waypoints));

            drawPath(obstacle, true);
            gen.setObstacles(obstacle);
            gen.setPopulation(set, start, finish);




            timer.Start();
        }

        private void StopB_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}

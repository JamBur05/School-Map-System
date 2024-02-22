using SchoolMapSystem.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SchoolMapSystem
{
    public class Category
    {
        public float Percentage { get; set; }
        public string Title { get; set; }
        public Brush ColourBrush { get; set; }
    }
    public partial class frmDataVisualization : Window
    {
        private List<Category> Categories { get; set; }

        public frmDataVisualization()
        {
            InitializeComponent();

            // Define constants
            const float PieWidth = 650;
            const float PieHeight = 650;
            const float Radius = PieWidth / 2;
            const float CenterX = PieWidth / 2;
            const float CenterY = PieHeight / 2;

            SearchCountFinder search = new SearchCountFinder();

            Categories = search.FindSearches();

            // Set canvas size
            mainCanvas.Width = PieWidth;
            mainCanvas.Height = PieHeight;

            // Bind categories to items control
            detailsItemsControl.ItemsSource = Categories;

            // Draw pie chart
            double startAngle = 0;
            foreach (Category category in Categories)
            {
                double angle = category.Percentage / 100.0 * 360.0;
                double endAngle = startAngle + angle;

                double startX = CenterX + Radius * Math.Cos(startAngle * Math.PI / 180);
                double startY = CenterY + Radius * Math.Sin(startAngle * Math.PI / 180);
                double endX = CenterX + Radius * Math.Cos(endAngle * Math.PI / 180);
                double endY = CenterY + Radius * Math.Sin(endAngle * Math.PI / 180);

                bool isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment(
                    new Point(endX, endY),
                    new Size(Radius, Radius),
                    0,
                    isLargeArc,
                    SweepDirection.Clockwise,
                    true);

                var pathFigure = new PathFigure(
                    new Point(CenterX, CenterY),
                    new List<PathSegment>
                    {
            new LineSegment(new Point(startX, startY), false),
            arcSegment,
            new LineSegment(new Point(CenterX, CenterY), false),
                    },
                    true);

                var pathGeometry = new PathGeometry(new List<PathFigure> { pathFigure });
                var path = new Path
                {
                    Fill = category.ColourBrush,
                    Data = pathGeometry,
                };
                mainCanvas.Children.Add(path);

                // Draw outlines
                var outline1 = new Line
                {
                    X1 = CenterX,
                    Y1 = CenterY,
                    X2 = startX,
                    Y2 = startY,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };
                var outline2 = new Line
                {
                    X1 = CenterX,
                    Y1 = CenterY,
                    X2 = endX,
                    Y2 = endY,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };
                mainCanvas.Children.Add(outline1);
                mainCanvas.Children.Add(outline2);

                startAngle = endAngle;
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmMainWindow window = new frmMainWindow("admin");
            window.Show();
            this.Close();
        }
    }
}

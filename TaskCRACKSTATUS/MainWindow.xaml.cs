using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TaskCRACKSTATUS
{
    public partial class MainWindow : Window
    {
        private List<Rectangle> rectangles = new List<Rectangle>();
        private DispatcherTimer timer = new DispatcherTimer();
        private const int N = 5; // Количество циклов до удаления прямоугольника

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация таймера для создания прямоугольников с заданной периодичностью
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Создание нового прямоугольника
            Random random = new Random();
            int width = random.Next(50, 150);
            int height = random.Next(50, 150);
            Color color = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));

            Rectangle rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(color)
            };

            // Установка случайных координат для прямоугольника в пределах холста
            Canvas.SetLeft(rectangle, random.Next((int)(canvas.ActualWidth - width)));
            Canvas.SetTop(rectangle, random.Next((int)(canvas.ActualHeight - height)));

            // Добавление прямоугольника на холст и в список прямоугольников
            canvas.Children.Add(rectangle);
            rectangles.Add(rectangle);

            // Пометка прямоугольников для удаления, если они пересекаются с добавленным
            MarkRectanglesForRemoval(rectangle);

            // Вызов метода для удаления помеченных прямоугольников
            RemoveMarkedRectangles();
        }

        private void MarkRectanglesForRemoval(Rectangle newRectangle)
        {
            Rect newRect = new Rect(Canvas.GetLeft(newRectangle), Canvas.GetTop(newRectangle), newRectangle.Width, newRectangle.Height);

            foreach (Rectangle rectangle in rectangles)
            {
                if (rectangle != newRectangle)
                {
                    Rect rect = new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), rectangle.Width, rectangle.Height);
                    if (rect.IntersectsWith(newRect))
                    {
                        rectangle.Tag = N; // свойство Tag для хранения количества циклов до удаления
                    }
                }
            }
        }

        private void RemoveMarkedRectangles()
        {
            for (int i = rectangles.Count - 1; i >= 0; i--)
            {
                if (rectangles[i].Tag != null)
                {
                    int cyclesLeft = (int)rectangles[i].Tag;
                    if (cyclesLeft == 0)
                    {
                        canvas.Children.Remove(rectangles[i]);
                        rectangles.RemoveAt(i);
                    }
                    else
                    {
                        rectangles[i].Tag = cyclesLeft - 1;
                    }
                }
            }
        }
    }
}
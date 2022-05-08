using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Game : Window
    {
        DispatcherTimer Timer;
        int currentTime;

        Ellipse ellipse;
        double ellipseRadius = 8;
        double ellipseX, ellipseY;
        double ellipseStartSpeed = 3;
        double ellipseSpeedDirectionX, ellipseSpeedDirectionY;

        Rectangle racket;
        double racketWidth = 100;
        double racketPositionX;
        double racketShiftRateX = 50;

        public Game()
        {
            InitializeComponent();
            Restart();

            ellipse = new Ellipse();
            ellipse.Fill = Brushes.Tomato;
            ellipse.Width = 2 * ellipseRadius;
            ellipse.Height = 2 * ellipseRadius;
            ellipse.Margin = new Thickness(ellipseX, ellipseY, 0, 0); 
            ellipse.Stroke = Brushes.Black;
            ellipse.StrokeThickness = 0.5;
            gameField.Children.Add(ellipse);

            racket = new Rectangle();
            racket.Fill = Brushes.Chocolate;
            racket.Width = racketWidth;
            racket.Height = 5;
            racketPositionX = gameField.Width / 2 - racketWidth / 2;
            racket.Margin = new Thickness(racketPositionX, gameField.Height, 0, 0);
            racket.Stroke = Brushes.Black;
            racket.StrokeThickness = 0.5;
            gameField.Children.Add(racket);

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(onTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Start();
        }

        void Restart()
        {
            ellipseX = gameField.Width / 2 - ellipseRadius;
            ellipseY = gameField.Height / 2 - ellipseRadius;

            Random randomValue = new Random();

            double startFlightAngle = randomValue.NextDouble() * Math.PI / 2 + Math.PI / 4;

            ellipseSpeedDirectionX = ellipseStartSpeed * Math.Cos(startFlightAngle);
            ellipseSpeedDirectionY = ellipseStartSpeed * Math.Sin(startFlightAngle);

            racketPositionX = gameField.Width / 2 - racketWidth / 2;

            currentTime = 0;
        }

        void onTick(object sender, EventArgs e)
        {
            currentTime++;

            if ((ellipseX < 0) || (ellipseX > gameField.Width - 2 * ellipseRadius))
            {
                ellipseSpeedDirectionX = -ellipseSpeedDirectionX;
            }

            if ((ellipseY < 0) || (ellipseY > gameField.Height - 2 * ellipseRadius))
            {
                ellipseSpeedDirectionY = -ellipseSpeedDirectionY;
            }

            if (ellipseY > gameField.Height - 2 * ellipseRadius)
            {
                double currentCoordinateX = ellipseX + ellipseRadius;

                if ((currentCoordinateX >= racketPositionX) && (currentCoordinateX <= racketPositionX + racketWidth))
                {
                    ellipseSpeedDirectionX *= 1.1;
                    ellipseSpeedDirectionY *= 1.1;
                }
                else
                {
                    MessageBox.Show($"Nice try!\nYour result is {currentTime/100} seconds.\n\nTry one more time!");
                    Restart();
                    racket.Margin = new Thickness(racketPositionX, gameField.Height, 0, 0);
                }
            }

            ellipseX += ellipseSpeedDirectionX;
            ellipseY += ellipseSpeedDirectionY;

            ellipse.Margin = new Thickness(ellipseX, ellipseY, 0, 0);

            tbTime.Text = (currentTime / 100).ToString();
        }

        private void cmKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                racketPositionX -= racketShiftRateX;
            }

            if (e.Key == Key.Right)
            {
                racketPositionX += racketShiftRateX;
            }

            if (racketPositionX < 0)
            {
                racketPositionX = 0;
            }

            if (racketPositionX > gameField.Width - racketWidth)
            {
                racketPositionX = gameField.Width - racketWidth;
            }

            racket.Margin = new Thickness(racketPositionX, gameField.Height, 0, 0);
        }
    }
}

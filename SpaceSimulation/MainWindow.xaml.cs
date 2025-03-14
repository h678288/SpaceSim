using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic.CompilerServices;
using SpaceSim;

namespace SpaceSimulation
{
    public partial class MainWindow
    {
        private SolarSystem _solarSystem;
        private AnimationController _animationController;
        private bool _isInitialized;
        private double _scale = 0.0000001;
        private double _orbitScale = 0.0000001;
        private const double StandardOrbitScale = 0.0000001;
        private const double StandardScale = 0.0000001;

        public MainWindow()
        {
            InitializeComponent();
            _solarSystem = new SolarSystem();
            _animationController = new AnimationController();

            foreach (var obj in _solarSystem.SpaceObjects)
            {
                if (obj != null)
                {
                    _animationController.DoTick += (_, _) => RedrawCurrentView();
                }
            }

            _isInitialized = true;
            SolarSystemCanvas.SizeChanged += (_, _) => DrawSolarSystem();
            PopulatePlanetSelector();
            _animationController.Start();

            this.MouseRightButtonDown += (_, _) => { PlanetSelector.Text = "All"; };
        }

        private void DrawSolarSystem()
        {
            if (!_isInitialized) return;

            SolarSystemCanvas.Children.Clear();
            foreach (var obj in _solarSystem.SpaceObjects)
            {
                if (obj is Star or Planet)
                {
                    DrawSpaceObject(obj);
                }

                if (obj is AsteroidBelt)
                {
                    DrawAsteroidBelt(obj);
                }
            }
        }

        private void DrawAsteroidBelt(SpaceObject spaceObject)
        {
            double centerX = SolarSystemCanvas.ActualWidth / 2;
            double centerY = SolarSystemCanvas.ActualHeight / 2;

            double orbitSize = (spaceObject.OrbitRadius * _orbitScale) * 2;

            Ellipse orbit = new()
            {
                Width = orbitSize,
                Height = orbitSize,
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = Math.Min(400, orbitSize * _orbitScale * 90000),
                Fill = null
            };
            Canvas.SetLeft(orbit, centerX - orbitSize / 2);
            Canvas.SetTop(orbit, centerY - orbitSize / 2);
            SolarSystemCanvas.Children.Add(orbit);

            if (ShowLabels?.IsChecked == true)
            {
                TextBlock label = new()
                {
                    Text = spaceObject.Name,
                    Foreground = Brushes.White
                };
                Canvas.SetLeft(label, centerX - label.ActualWidth / 2);
                Canvas.SetTop(label, centerY - orbitSize / 2 - 20);
                SolarSystemCanvas.Children.Add(label);
            }
        }

        private void DrawSpaceObject(SpaceObject obj)
        {
            double centerX = SolarSystemCanvas.ActualWidth / 2;
            double centerY = SolarSystemCanvas.ActualHeight / 2;

            double x, y;
            if (obj is Star)
            {
                x = centerX;
                y = centerY;
            }
            else
            {
                var position = obj.CalculatePosition(_animationController.CurrentTime);
                x = centerX + position.X * _orbitScale;
                y = centerY + position.Y * _orbitScale;
            }

            if (ShowOrbits?.IsChecked == true && obj.OrbitRadius > 0)
            {
                double orbitSize = (obj.OrbitRadius * _orbitScale) * 2;
                Ellipse orbit = new()
                {
                    Width = orbitSize,
                    Height = orbitSize,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    Fill = null
                };
                Canvas.SetLeft(orbit, centerX - orbitSize / 2);
                Canvas.SetTop(orbit, centerY - orbitSize / 2);
                SolarSystemCanvas.Children.Add(orbit);
            }

            var objSize = obj is Star ? Math.Max(5, obj.ObjectRadius * _scale * 20) : Math.Max(5, obj.ObjectRadius * _scale);

            Ellipse ellipse = new()
            {
                Width = objSize,
                Height = objSize,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(obj.Color ?? "White"))
            };

            Canvas.SetLeft(ellipse, x - ellipse.Width / 2);
            Canvas.SetTop(ellipse, y - ellipse.Height / 2);
            SolarSystemCanvas.Children.Add(ellipse);

            if (ShowLabels?.IsChecked == true)
            {
                TextBlock label = new()
                {
                    Text = obj.Name,
                    Foreground = Brushes.White
                };
                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, y - ellipse.Height / 2 - 20);
                SolarSystemCanvas.Children.Add(label);
            }
        }

        private void PopulatePlanetSelector()
        {
            PlanetSelector.Items.Add("All");
            foreach (var obj in _solarSystem.SpaceObjects)
            {
                if (obj is Planet)
                {
                    PlanetSelector.Items.Add(obj.Name);
                }
            }
        }

        private void PlanetSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                var planetName = PlanetSelector.SelectedItem.ToString();
                if (planetName == "All")
                {
                    DrawSolarSystem();
                    return;
                }

                var planet = _solarSystem.GetSpaceObjectByName(planetName);
                if (planet != null)
                {
                    DrawPlanetAndMoons(planet);
                }
            }
        }

        private void DrawPlanetAndMoons(SpaceObject planet)
        {
            SolarSystemCanvas.Children.Clear();

            var centerX = SolarSystemCanvas.ActualWidth / 2;
            var centerY = SolarSystemCanvas.ActualHeight / 2;

            const double localScale = 0.0000001;

            var planetSize = Math.Max(30, planet.ObjectRadius * localScale * 4000);
            Ellipse planetEllipse = new()
            {
                Width = planetSize,
                Height = planetSize,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(planet.Color ?? "White"))
            };
            Canvas.SetLeft(planetEllipse, centerX - planetSize / 2);
            Canvas.SetTop(planetEllipse, centerY - planetSize / 2);
            SolarSystemCanvas.Children.Add(planetEllipse);

            if (ShowLabels?.IsChecked == true)
            {
                TextBlock label = new()
                {
                    Text = planet.Name,
                    Foreground = Brushes.White
                };
                Canvas.SetLeft(label, centerX - label.ActualWidth / 2);
                Canvas.SetTop(label, centerY - planetSize / 2 - 20);
                SolarSystemCanvas.Children.Add(label);
            }

            var moons = _solarSystem.GetMoonsOfPlanet(planet.Name);
            double canvasRadius = Math.Min(SolarSystemCanvas.ActualWidth, SolarSystemCanvas.ActualHeight) * 0.4;

            for (int i = 0; i < moons.Count; i++)
            {
                SpaceObject? moon = moons[i];
                if (moon == null) continue;

                double orbitPeriod = moon.OrbitPeriod * 10;
                
                double minDistance = planetSize + 20;
                double maxDistance = canvasRadius - 20;

                double moonDistanceRatio = i / (double)Math.Max(1, moons.Count - 1);
                double moonDistance = minDistance + moonDistanceRatio * (maxDistance - minDistance);

                double direction = orbitPeriod > 0 ? 1 : -1;
                double angle = (2 * Math.PI / Math.Abs(orbitPeriod)) * _animationController.CurrentTime * direction;
                
                double moonX = centerX + moonDistance * Math.Cos(angle);
                double moonY = centerY + moonDistance * Math.Sin(angle);

                if (ShowOrbits.IsChecked == true)
                {
                    double orbitSize = moonDistance * 2;
                    Ellipse orbit = new()
                    {
                        Width = orbitSize,
                        Height = orbitSize,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0.5,
                        Fill = null
                    };
                    Canvas.SetLeft(orbit, centerX - orbitSize / 2);
                    Canvas.SetTop(orbit, centerY - orbitSize / 2);
                    SolarSystemCanvas.Children.Add(orbit);
                }

                var moonSize = Math.Max(10, moon.ObjectRadius * localScale * 2000);
                Ellipse moonEllipse = new()
                {
                    Width = moonSize,
                    Height = moonSize,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(moon.Color ?? "White"))
                };
                Canvas.SetLeft(moonEllipse, moonX - moonSize / 2);
                Canvas.SetTop(moonEllipse, moonY - moonSize / 2);
                SolarSystemCanvas.Children.Add(moonEllipse);

                if (ShowLabels?.IsChecked == true)
                {
                    TextBlock label = new()
                    {
                        Text = moon.Name,
                        Foreground = Brushes.White
                    };
                    Canvas.SetLeft(label, moonX - label.ActualWidth / 2);
                    Canvas.SetTop(label, moonY - moonSize / 2 - 15);
                    SolarSystemCanvas.Children.Add(label);
                }
            }
        }

        private void ShowLabels_Checked(object sender, RoutedEventArgs e)
        {
            RedrawCurrentView();
        }

        private void ShowOrbits_Checked(object sender, RoutedEventArgs e)
        {
            RedrawCurrentView();
        }

        private void ShowLabels_UnChecked(object sender, RoutedEventArgs e)
        {
            RedrawCurrentView();
        }

        private void ShowOrbits_UnChecked(object sender, RoutedEventArgs e)
        {
            RedrawCurrentView();
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _orbitScale = StandardOrbitScale * Math.Pow(1.5, e.NewValue - 1);
            _scale = StandardScale * Math.Pow(1.5, e.NewValue - 1);

            RedrawCurrentView();
        }

        private void RedrawCurrentView()
        {
            if (PlanetSelector.SelectedItem != null)
            {
                string? planetName = PlanetSelector.SelectedItem.ToString();
                if (planetName == "All")
                {
                    DrawSolarSystem();
                    return;
                }

                SpaceObject? planet = _solarSystem.GetSpaceObjectByName(planetName);
                if (planet != null)
                {
                    DrawPlanetAndMoons(planet);
                }
            }
            else
            {
                DrawSolarSystem();
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Reset();
            SpeedSlider.Value = 1;
        }


        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _animationController?.SetSpeed(e.NewValue);
        }
    }

    public class AnimationController
    {
        // Define the event and delegate
        public delegate void TickEventHandler(object sender, TickEventArgs e);

        public event TickEventHandler? DoTick;

        private readonly DispatcherTimer _timer;
        private double _currentTime;
        private double _timeIncrement = 1.0; // Days per tick

        public double CurrentTime => _currentTime;
        public bool IsRunning => _timer.IsEnabled;

        public AnimationController()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16.67); // 20 FPS
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _currentTime += _timeIncrement;
            OnDoTick();
        }

        protected virtual void OnDoTick()
        {
            DoTick?.Invoke(this, new TickEventArgs(_currentTime, _timeIncrement));
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        public void Reset()
        {
            _timer.Stop();
            _currentTime = 0;
            OnDoTick();
        }

        public void SetSpeed(double speed)
        {
            _timeIncrement = speed;
        }
    }

    public class TickEventArgs : EventArgs
    {
        public double CurrentTime { get; }
        public double TimeIncrement { get; }

        public TickEventArgs(double currentTime, double timeIncrement)
        {
            CurrentTime = currentTime;
            TimeIncrement = timeIncrement;
        }
    }
}
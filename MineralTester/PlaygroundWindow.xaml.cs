using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// <summary>
/// Written by Mike Schommer
/// XAML styling by Rick Bowman
/// </summary>
namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for PlaygroundWindow.xaml
    /// </summary>
    public partial class PlaygroundWindow : Window
    {
        User _user;
        UIElement dragObj = null;
        Point offset;
        ImageBrush _brush = new ImageBrush();
        BitmapImage _bitmap = new BitmapImage();
        Ellipse _mineral = new Ellipse();
        Mineral _selectedMineral = new Mineral();
        Ellipse _tester = new Ellipse();
        Tester _selectedTester = new Tester();
        Timer _colorTimer = new Timer();
        Point current;
        bool drawstreak = false;

        /// <summary>
        /// Initial the screen. Get all minerals from database.
        /// </summary>
        /// <param name="currentUser"> The current user who accessed the screen. </param>
        /// 
        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            _user = currentUser;
            List<Mineral> minerals = new List<Mineral>();
            IDatabase database = new Database();
            minerals = database.GetMinerals();
            MineralList.ItemsSource = minerals;
            MineralList.DisplayMemberPath = "Name";
            ShowName.IsEnabled = false;
        }

        /// <summary>
        /// Shows different mineral when different mineral is selected.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void MineralList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Playground.Children.Remove(_mineral);
            ClearLines();
            _selectedMineral = (Mineral)MineralList.SelectedItem;
            if (!(_selectedMineral.Image is null))
            {
                _bitmap = ByteArrayToBitmap(_selectedMineral.Image);
                DisplayMineral(_bitmap, _selectedMineral);
            }

        }

        /// <summary>
        /// Converts ByteArray to Bitmap
        /// </summary>
        /// <param name="imageBytes"> The byte array from mineral.</param>
        /// <returns> A bitmapImage.</returns>
        private BitmapImage ByteArrayToBitmap(byte[] imageBytes)
        {
            using (Stream stream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        /// <summary>
        /// Displays mineral based on window settings.
        /// </summary>
        /// <param name="bitmap"> The image to be used as a backgound.</param>
        /// <param name="displayMineral"> The mineral currently displayed.</param>
        private void DisplayMineral(BitmapImage bitmap, Mineral displayMineral)
        {
            _brush.ImageSource = bitmap;
            if ((bool)HideMineral.IsChecked == true)
            {
                _mineral.Fill = Brushes.Black;
            }
            else
            {
                _mineral.Fill = _brush;
            }
            _mineral.Width = 150;
            _mineral.Height = 150;
            Canvas.SetTop(_mineral, 150);
            Canvas.SetLeft(_mineral, 75);
            _mineral.PreviewMouseDown += Mineral_PreviewMouseDown;
            Playground.Children.Add(_mineral);
        }

        /// <summary>
        /// Turns ellipse into drag object and gets ellipse positioning.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Mineral_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragObj = sender as UIElement;
            offset = e.GetPosition(Playground);
            offset.Y -= Canvas.GetTop(dragObj);
            offset.X -= Canvas.GetLeft(dragObj);
            Playground.CaptureMouse();
        }

        /// <summary>
        /// Handles mouse down logic for drawing a line when doing a streak test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MineralList.SelectedIndex > -1 && drawstreak)
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    current = new Point(e.GetPosition(Playground).X, e.GetPosition(Playground).Y);
            }
        }

        /// <summary>
        /// Handles mouse up logic for drawing a line when doing a streak test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playground_MouseUp(object sender, MouseButtonEventArgs e)
        {
            current = new Point();
            Playground.Children.Remove(_mineral);
            Playground.Children.Add(_mineral);
        }

        /// <summary>
        /// Removes any streak test lines from the canvas
        /// </summary>
        private void ClearLines()
        {
            List<int> children = new List<int>();
            for (int i = 0; i < Playground.Children.Count; i++)
            {
                if (Playground.Children[i] is Line)
                {
                    children.Add(i);
                }
            }
            children.Reverse();
            for (int i = 0; i < children.Count; i++)
            {
                Playground.Children.RemoveAt(children[i]);
            }
        }

        /// <summary>
        /// Handles drawing a line when the user drags the mouse during a streak test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playground_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragObj == null)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed && current.X != 0 && current.Y != 0 && drawstreak)
            {
                Line line = new Line();
                Mineral selectedMineral = (Mineral)MineralList.SelectedItem;
                Color color = (Color)ColorConverter.ConvertFromString(selectedMineral.StreakColor);
                line.Stroke = new SolidColorBrush(color);
                line.StrokeThickness = 3;
                line.X1 = current.X;
                line.Y1 = current.Y;
                line.X2 = e.GetPosition(Playground).X;
                line.Y2 = e.GetPosition(Playground).Y;
                current = e.GetPosition(Playground);
                Playground.Children.Add(line);
            }
        }

        /// <summary>
        /// Moves ellipse to where cursor is. Checks if there is a collision.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragObj == null)
            {
                return;
            }
            var position = e.GetPosition(sender as IInputElement);
            if (position.X < Playground.ActualWidth - 50 && position.Y < Playground.ActualHeight - 50 && position.X > 50 && position.Y > 50)
            {
                Canvas.SetTop(dragObj, position.Y - offset.Y);
                Canvas.SetLeft(dragObj, position.X - offset.X);
                if (CollisonCheck(_mineral, _tester) && _selectedTester != null)
                {
                    if (_selectedTester.TestType == Enums.TestType.Scratch)
                    {
                        ShowScratchResults(_selectedMineral.Hardness < _selectedTester.Hardness);
                    }
                    else if (_selectedTester.TestType == Enums.TestType.Magnestism)
                    {
                        ShowMagnetResult(_selectedMineral.IsMagnetic == _selectedTester.Magnet);
                    }
                    else
                    {
                        ShowAcidResult(_selectedMineral.AcidReaction == _selectedTester.Acid);
                    }

                }
            }
        }

        /// <summary>
        /// Releases mouse capture when user is done dragging mineral.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragObj = null;
            Playground.ReleaseMouseCapture();
        }

        /// <summary>
        /// Shows acid result. 
        /// </summary>
        /// <param name="reacted">Result of test.</param>
        private void ShowAcidResult(bool reacted)
        {
            if (reacted)
            {
                _mineral.Fill = Brushes.Green;
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} reacted with the Hydrochloric acid!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = "The mineral reacted with the Hydrochloric acid!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
            }
            else
            {
                _mineral.Fill = Brushes.Red;
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} did not react with the Hydrochloric acid!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = "The mineral did not react with the Hydrochloric acid!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }

            }
            ResultTimer();
        }

        /// <summary>
        /// Shows magnetism result.
        /// </summary>
        /// <param name="areAttracted">Result of test.</param>
        private void ShowMagnetResult(bool areAttracted)
        {
            if (areAttracted)
            {
                Canvas.SetLeft(_mineral, 500);
                Canvas.SetTop(_mineral, 80);
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} was attracted to the magnet!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = "The mineral was attracted to the magnet!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
            }
            else
            {
                _mineral.Fill = Brushes.Red;
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} was not attracted to the magnet!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = "The mineral was not attracted to the magnet!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
            }
            ResultTimer();
        }

        /// <summary>
        /// Shows scratch result.
        /// </summary>
        /// <param name="scratched">Result of test.</param>
        private void ShowScratchResults(bool scratched)
        {
            if (scratched)
            {
                _mineral.Fill = Brushes.Green;
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} was scratched by a {_selectedTester.Name.Split('(')[0].TrimEnd().ToLower()}!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"The mineral was scratched by a {_selectedTester.Name.Split('(')[0].TrimEnd().ToLower()}!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
            }
            else
            {
                _mineral.Fill = Brushes.Red;
                if ((bool)ShowName.IsChecked || !ShowName.IsEnabled)
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"{_selectedMineral.Name} was not scratched by a {_selectedTester.Name.Split('(')[0].TrimEnd().ToLower()}!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else
                {
                    Result.Content = new TextBlock()
                    {
                        Text = $"The mineral was not scratched by a {_selectedTester.Name.Split('(')[0].TrimEnd().ToLower()}!",
                        TextWrapping = TextWrapping.Wrap
                    };
                }
            }
            ResultTimer();
        }

        /// <summary>
        /// Starts timer to display results of test.
        /// </summary>
        private void ResultTimer()
        {
            _colorTimer.Elapsed += new ElapsedEventHandler(StopTimer);
            _colorTimer.Interval = 2000;
            _colorTimer.Enabled = true;
            _colorTimer.Start();
        }

        /// <summary>
        /// Stops result timer.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void StopTimer(object sender, ElapsedEventArgs e)
        {
            _colorTimer.Stop();
            RevertMineral();
        }

        /// <summary>
        /// Reverts mineral back to its original display.
        /// </summary>
        private void RevertMineral()
        {
            Dispatcher.Invoke(() =>
            {
                if ((bool)HideMineral.IsChecked)
                {
                    _mineral.Fill = Brushes.Black;
                }
                else
                {
                    _brush.ImageSource = _bitmap;
                    _mineral.Fill = _brush;
                }
                Result.Content = "";
            });
        }


        /// <summary>
        /// Calculates position and does a collision check based on the their radii length and distance between them.
        /// </summary>
        /// <param name="mineral">Mineral ellipse.</param>
        /// <param name="tester">Tester ellipse.</param>
        /// <returns></returns>
        private bool CollisonCheck(Ellipse mineral, Ellipse tester)
        {
            var r1 = mineral.ActualWidth / 2; // Find radius of mineral ellipse.
            var x1 = Canvas.GetLeft(mineral) + r1; // Find x-coordinate of edge.
            var y1 = Canvas.GetTop(mineral) + r1; // Find y-coordinate of edge.

            var r2 = tester.ActualWidth / 2; // Find radius of tester ellipse.
            var x2 = Canvas.GetLeft(tester) + r2;
            var y2 = Canvas.GetTop(tester) + r2;

            var d = new Vector(x2 - x1, y2 - y1); // Calculate distance between the two edges.
            return d.Length < r1 + r2; // If the length between their two radii is greater than to the distance between the radii, then they must be touching.
        }

        /// <summary>
        /// Displays tester ellipse with appropriate image as background.
        /// </summary>
        /// <param name="source">The file path name.</param>
        public void DisplayTester(string source)
        {
            _tester.Width = 175;
            _tester.Height = 175;
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/MineralTester.UI;component{source}", UriKind.Absolute));
            _tester.Fill = img;
            Canvas.SetLeft(_tester, 550);
            Canvas.SetTop(_tester, 150);
            Playground.Children.Add(_tester);
        }

        /// <summary>
        /// Removes previous scratch test and replaces with a new one.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void ScratchTesters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drawstreak = false;
            Playground.Children.Remove(_tester);
            _selectedTester = (Tester)ScratchTesters.SelectedItem;
            DisplayTester(_selectedTester.ImgSource);
        }

        /// <summary>
        /// Enables tester list. 
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void ScratchTestButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLines();
            Playground.Background = new SolidColorBrush(Color.FromRgb(112, 128, 144));
            drawstreak = false;
            if (Playground.Children.Contains(_tester))
            {
                Playground.Children.Remove(_tester);
            }
            if (ScratchTesters.Items.Count == 0)
            {
                List<Tester> testers = new List<Tester>();
                FillTesters(ref testers);
                ScratchTesters.IsEnabled = true;
                ScratchTesters.ItemsSource = testers;
                ScratchTesters.DisplayMemberPath = "Name";
            }
            if (ScratchTesters.IsEnabled == false)
            {
                ScratchTesters.IsEnabled = true;
            }
            TestLabel.Content = new TextBlock()
            {
                Text = "Current Test Selction: Hardness",
                TextWrapping = TextWrapping.Wrap
            };
        }

        /// <summary>
        /// Initializes all the tester information.
        /// </summary>
        /// <param name="refList">The mineral list to be filled.</param>
        private void FillTesters(ref List<Tester> refList)
        {
            List<Tester> list = new List<Tester>();
            Tester fingerNail = new Tester("Finger Nail (Hardness: 2.5)", (float)2.5, (Enums.TestType)1, "/images/fingernail.png");
            list.Add(fingerNail);
            Tester copperPenny = new Tester("Copper Penny (Hardness: 3.5)", (float)3.5, (Enums.TestType)1, "/images/penny.png");
            list.Add(copperPenny);
            Tester knife = new Tester("Knife (Hardness: 5.5)", (float)5.5, (Enums.TestType)1, "/images/knife.png");
            list.Add(knife);
            Tester steelNail = new Tester("Steel Nail (Hardness: 6.5)", (float)6.5, (Enums.TestType)1, "/images/nail.png");
            list.Add(steelNail);
            Tester drillBit = new Tester("Masonry Drill Bit (Hardness: 8.5)", (float)8.5, (Enums.TestType)1, "/images/drillbit.png");
            list.Add(drillBit);
            refList = list;
        }

        /// <summary>
        /// Initializes the Magnet Test.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void MagnetismTestButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLines();
            drawstreak = false;
            Playground.Background = new SolidColorBrush(Color.FromRgb(112, 128, 144));
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(_tester);
            Tester magnet = new Tester((Enums.TestType)2, "/images/magnet.png");
            _selectedTester = magnet;
            DisplayTester(magnet.ImgSource);
            TestLabel.Content = new TextBlock()
            {
                Text = "Current Test Selection: Magnet",
                TextWrapping = TextWrapping.Wrap
            };
        }

        private void StreakPlateButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLines();
            Playground.Children.Remove(_tester);
            ScratchTesters.IsEnabled = false;
            drawstreak = true;
            _selectedTester = null;
            Playground.Background = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/MineralTester.UI;component/images/plate.png", UriKind.Absolute)));
            TestLabel.Content = new TextBlock()
            {
                Text = "Current Test Selection: Streak",
                TextWrapping = TextWrapping.Wrap
            };
        }

        /// <summary>
        /// Initializes the Acid Test.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void AcidTestButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLines();
            drawstreak = false;
            Playground.Background = new SolidColorBrush(Color.FromRgb(112, 128, 144));
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(_tester);
            Tester acid = new Tester((Enums.TestType)3, "/images/dropper.png");
            _selectedTester = acid;
            DisplayTester(acid.ImgSource);
            TestLabel.Content = new TextBlock()
            {
                Text = "Current Test Selection: Acid",
                TextWrapping = TextWrapping.Wrap
            };
        }

        /// <summary>
        /// Randomizes mineral selection.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void RandomMineralButton(object sender, RoutedEventArgs e)
        {
            ShowName.IsEnabled = true;
            Random random = new Random();
            int randomIndex = random.Next(MineralList.Items.Count);
            var randomItem = MineralList.Items[randomIndex];
            if ((bool)ShowName.IsChecked)
            {
                MineralList.DisplayMemberPath = "Name";
            }
            else
            {
                MineralList.DisplayMemberPath = "Hidden";
            }
            MineralList.SelectedItem = randomItem;
        }

        /// <summary>
        /// Paints mineral black, "hiding it".
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void HideMineral_Checked(object sender, RoutedEventArgs e)
        {
            _mineral.Fill = Brushes.Black;
        }

        /// <summary>
        /// Shows mineral image, "unhiding it".
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void HideMineral_Unchecked(object sender, RoutedEventArgs e)
        {
            _brush.ImageSource = _bitmap;
            _mineral.Fill = _brush;
        }

        /// <summary>
        /// Sets listbox display to be blank.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void ShowName_Unchecked(object sender, RoutedEventArgs e)
        {
            MineralList.DisplayMemberPath = "Hidden";
        }

        /// <summary>
        /// Set listbox display to mineral name.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void ShowName_Checked(object sender, RoutedEventArgs e)
        {
            MineralList.DisplayMemberPath = "Name";
        }

        /// <summary>
        /// Resets the playground screen.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        /// 
        private void ResetPlaygroundButton(object sender, RoutedEventArgs e)
        {
            Playground.Children.Clear();
            Playground.Children.Add(_mineral);
        }

        /// <summary>
        /// Opens a window showing practice questions.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void DisplayPracticeQuestions(object sender, RoutedEventArgs e)
        {
            PracticeQuestionsWindow practiceQuestionsWindow = new PracticeQuestionsWindow();
            practiceQuestionsWindow.Show();
        }

        /// <summary>
        /// Closes the playground window.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void ExitPlayground(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

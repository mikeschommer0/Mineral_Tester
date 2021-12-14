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

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for PlaygroundWindow.xaml
    /// </summary>
    public partial class PlaygroundWindow : Window
    {
        /////////////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////////////////////////////////////////////////////
        User _user;

        UIElement dragObj = null; // Global varibles used for mineral movement.
        Point offset;

        ImageBrush _brush = new ImageBrush(); // Global varibles for setting image to ellipse.
        BitmapImage _bitmap = new BitmapImage();

        Ellipse _mineral = new Ellipse();
        Mineral _selectedMineral = new Mineral();

        Ellipse _tester = new Ellipse();
        Tester _selectedTester = new Tester();

        Timer _colorTimer = new Timer();

        /// <summary>
        /// Initial the screen. Get all minerals from database.
        /// </summary>
        /// <param name="currentUser">
        /// The current user who accessed the screen.
        /// </param>
        /// 

        /// <summary>
        /// Centers the screen and disables resizing, just prevents UI weirdness when the window size is adjusted.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
        }

        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            _user = currentUser;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            List<Mineral> minerals = new List<Mineral>();
            Database db = new Database();
            minerals = db.GetMinerals(); // Get all minerals from database.

            MineralList.ItemsSource = minerals;
            MineralList.DisplayMemberPath = "Name";
            ShowName.IsEnabled = false; // Initially set this to disabled as names are already shown.
        }

        /////////////////////////////////////////////////////////// MINERALS ///////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Shows different mineral when different mineral is selected.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void MineralList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Playground.Children.Remove(_mineral); // Clear screen of any mineral/test off screen.


            _selectedMineral = (Mineral)MineralList.SelectedItem;

            if (!(_selectedMineral.Image is null)) // If mineral has an image
            {
                _bitmap = ByteArrayToBitmap(_selectedMineral.Image); // Get bitmap from array and save it to global bitmap.
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
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CreateOptions = BitmapCreateOptions.None;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = stream;
                bi.EndInit();
                return bi;
            }
        }

        /// <summary>
        /// Displays mineral based on window settings.
        /// </summary>
        /// <param name="bitmap"> The image to be used as a backgound.</param>
        /// <param name="displayMineral"> The mineral currently displayed.</param>
        private void DisplayMineral(BitmapImage bitmap, Mineral displayMineral)
        {
            _brush.ImageSource = bitmap; // Set image source to global bitmap.
            if ((bool)HideMineral.IsChecked == true)
            {
                _mineral.Fill = Brushes.Black; // If mineral is hidden, fill it with black.
            }
            else
            {
                _mineral.Fill = _brush; // Else, fill it with background image.
            }
            _mineral.Width = 150;
            _mineral.Height = 150;
            Canvas.SetTop(_mineral, 150);
            Canvas.SetLeft(_mineral, 75);
            _mineral.PreviewMouseDown += Mineral_PreviewMouseDown; // Setting ellipse size and starting point. Add mouse down event.


            Playground.Children.Add(_mineral);
        }

        /// <summary>
        /// Turns ellipse into drag object and gets ellipse positioning.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Mineral_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragObj = sender as UIElement; // Makes ellipse into a drag object.
            this.offset = e.GetPosition(this.Playground); // Set offset to where the ellipse is.
            this.offset.Y -= Canvas.GetTop(this.dragObj);
            this.offset.X -= Canvas.GetLeft(this.dragObj);
            this.Playground.CaptureMouse();
        }

        /// <summary>
        /// Moves ellipse to where cursor is. Checks if there is a collision.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragObj == null)
            {
                return;
            }

            var position = e.GetPosition(sender as IInputElement); // Get position of the cursor.

            if (position.X < Playground.ActualWidth - 50 && position.Y < Playground.ActualHeight - 50 && position.X > 50 && position.Y > 50) // Used for setting collison of playground canvas.
            {
                Canvas.SetTop(this.dragObj, position.Y - this.offset.Y);
                Canvas.SetLeft(this.dragObj, position.X - this.offset.X);  // Move ellipse to where cursor is.

                if (CollisonCheck(_mineral, _tester)) // Check if there is a collison.
                {

                    if (_selectedTester.TestType == Enums.TestType.Scratch)
                    {
                        ShowScratchResults(_selectedMineral.Hardness < _selectedTester.Hardness); // Hardness check.

                    }
                    else if (_selectedTester.TestType == Enums.TestType.Magnestism)
                    {
                        ShowMagnetResult(_selectedMineral.IsMagnetic == _selectedTester.Magnet); // Magnet check.

                    }
                    else
                    {
                        ShowAcidResult(_selectedMineral.AcidReaction == _selectedTester.Acid); // Acid check.

                    }

                }
            }
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
            }
            else
            {
                _mineral.Fill = Brushes.Red;
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
            }

            else
            {
                _mineral.Fill = Brushes.Red;
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
            }
            else
            {
                _mineral.Fill = Brushes.Red;
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

            var d = new Vector(x2 - x1, y2 - y1); // Caluculate distance between the two edges.
            return d.Length < r1 + r2; // If the length between their two radii is greater than to the distance between the edges, then they must be touching.
        }

        /// <summary>
        /// Releases mouse capture when user is done dragging mineral.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.dragObj = null;
            this.Playground.ReleaseMouseCapture();
        }

        /////////////////////////////////////////////////////////// TESTERS ///////////////////////////////////////////////////////////////////////////////////////////////////////

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
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(_tester);

            Tester magnet = new Tester((Enums.TestType)2, "/images/magnet.png");
            _selectedTester = magnet;
            DisplayTester(magnet.ImgSource);
        }

        /// <summary>
        /// Initializes the Acid Test.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Contains event data.</param>
        private void AcidTestButton_Click(object sender, RoutedEventArgs e)
        {
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(_tester);

            Tester acid = new Tester((Enums.TestType)3, "/images/dropper.png");
            _selectedTester = acid;
            DisplayTester(acid.ImgSource);
        }

        /////////////////////////////////////////////////////////// OPTIONS ///////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Randomizes mineral selection.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void RandomMineralButton(object sender, RoutedEventArgs e)
        {
            ShowName.IsEnabled = true; // Allow name display to be chosen. 
            Random random = new Random();
            int randomIndex = random.Next(MineralList.Items.Count);
            var randomItem = MineralList.Items[randomIndex];
            if ((bool)ShowName.IsChecked)
            {
                MineralList.DisplayMemberPath = "Name";
            } else
            {
                MineralList.DisplayMemberPath = "Hidden";
            }
            MineralList.SelectedItem = randomItem;
            // I use this if statement because choosing random list item still leaves it highlighted.
            // I made it up to the user if they want to see the names of the random mineral.
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
        }

        /////////////////////////////////////////////////////////// QUESTIONS ///////////////////////////////////////////////////////////////////////////////////////////////////////

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

        //////////////////////////////////////////////////////////// EXIT ///////////////////////////////////////////////////////////////////////////////////////////////////////

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

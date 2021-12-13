using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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

        ImageBrush brush = new ImageBrush(); // Global varibles for setting image to ellipse.
        BitmapImage bitmap = new BitmapImage();

        Ellipse mineral = new Ellipse();
        Mineral selectedMineral = new Mineral();

        Ellipse tester = new Ellipse();
        Tester selectedTester = new Tester();

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
            Playground.Children.Remove(mineral); // Clear screen of any mineral/test off screen.


            selectedMineral = (Mineral)MineralList.SelectedItem;

            if (!(selectedMineral.Image is null)) // If mineral has an image
            {
                bitmap = ByteArrayToBitmap(selectedMineral.Image); // Get bitmap from array and save it to global bitmap.
                DisplayMineral(bitmap, selectedMineral);
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
            brush.ImageSource = bitmap; // Set image source to global bitmap.
            if ((bool)HideMineral.IsChecked == true)
            {
                mineral.Fill = Brushes.Black; // If mineral is hidden, fill it with black.
            }
            else
            {
                mineral.Fill = brush; // Else, fill it with background image.
            }
            mineral.Width = 150;
            mineral.Height = 150;
            Canvas.SetTop(mineral, 150);
            Canvas.SetLeft(mineral, 75);
            mineral.PreviewMouseDown += Mineral_PreviewMouseDown; // Setting ellipse size and starting point. Add mouse down event.


            Playground.Children.Add(mineral);
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
        /// Moves ellipse to where cursor is.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

                if (collisonCheck(mineral, tester))
                {
                    Canvas.SetTop(mineral, 150);
                    Canvas.SetLeft(mineral, 75);
                    if (selectedTester.TestType == Enums.TestType.Scratch)
                    {
                        showScratchResults(selectedMineral.Hardness < selectedTester.Hardness);

                    }
                    else if (selectedTester.TestType == Enums.TestType.Magnestism)
                    {
                        showMagnetResult(selectedMineral.IsMagnetic == selectedTester.Magnet);

                    }
                    else
                    {
                        showAcidResult(selectedMineral.AcidReaction == selectedTester.Acid);

                    }
                    this.dragObj = null;
                    this.Playground.ReleaseMouseCapture();
                }
            }
        }

        private void showAcidResult(bool reacted)
        {
            if (reacted)
            {
                MessageBox.Show($"{selectedMineral.Name} fizzled! A reaction happened.");
            }
            else
            {
                MessageBox.Show($"{selectedMineral.Name} got wet, no reaction happened.");
            }
        }

        private void showMagnetResult(bool areAttracted)
        {
            if (areAttracted)
            {
                MessageBox.Show($"{selectedMineral.Name} was stuck to the magnet!");
            }

            else
            {
                MessageBox.Show($"{selectedMineral.Name} did not stick to the magnet.");
            }
        }

        private void showScratchResults(bool scratched)
        {
            if (scratched)
            {

                MessageBox.Show($"{selectedMineral.Name} was scratched!");
            }
            else
            {
                MessageBox.Show($"Nothing happened. The {selectedTester.Name} left no scratch");
            }

        }

        private bool collisonCheck(Ellipse mineral, Ellipse tester)
        {
            var r1 = mineral.ActualWidth / 2;
            var x1 = Canvas.GetLeft(mineral) + r1;
            var y1 = Canvas.GetTop(mineral) + r1;

            var r2 = tester.ActualWidth / 2;
            var x2 = Canvas.GetLeft(tester) + r2;
            var y2 = Canvas.GetTop(tester) + r2;

            var d = new Vector(x2 - x1, y2 - y1);
            return d.Length <= r1 + r2;
        }

        /// <summary>
        /// Releases mouse capture when user is done dragging mineral.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void Playgroud_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.dragObj = null;
            this.Playground.ReleaseMouseCapture();
        }

        /// <summary>
        /// Resets the playground screen.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        /// 

        /////////////////////////////////////////////////////////// TESTERS ///////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        public void displayTester(string source)
        {
            tester.Width = 175;
            tester.Height = 175;
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/MineralTester.UI;component{source}", UriKind.Absolute));
            tester.Fill = img;
            Canvas.SetLeft(tester, 550);
            Canvas.SetTop(tester, 150);
            Playground.Children.Add(tester);
        }

        private void ScratchTesters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Playground.Children.Remove(tester);
            selectedTester = (Tester)ScratchTesters.SelectedItem;
            displayTester(selectedTester.ImgSource);
        }

        private void ScratchTestButton_Click(object sender, RoutedEventArgs e)
        {
            if(Playground.Children.Contains(tester))
            {
                Playground.Children.Remove(tester);
            }

            if (ScratchTesters.Items.Count == 0)
            {
                List<Tester> testers = new List<Tester>();
                fillTesters(ref testers);
                ScratchTesters.IsEnabled = true;
                ScratchTesters.ItemsSource = testers;
                ScratchTesters.DisplayMemberPath = "Name";
            }

            if(ScratchTesters.IsEnabled == false)
            {
                ScratchTesters.IsEnabled = true;
            }
        }

        private void fillTesters(ref List<Tester> refList)
        {
            List<Tester> list = new List<Tester>();

            Tester fingerNail = new Tester("Finger Nail", (float)2.5, (Enums.TestType)1, "/images/fingernail.png");
            list.Add(fingerNail);

            Tester copperPenny = new Tester("Copper Penny", (float)3.5, (Enums.TestType)1, "/images/penny.png");
            list.Add(copperPenny);

            Tester knife = new Tester("Knife", (float)5.5, (Enums.TestType)1, "/images/knife.png");
            list.Add(knife);

            Tester steelNail = new Tester("Steel Nail", (float)6.5, (Enums.TestType)1, "/images/nail.png");
            list.Add(steelNail);

            Tester drillBit = new Tester("Masonry Drill Bit", (float)8.5, (Enums.TestType)1, "/images/drillbit.png");
            list.Add(drillBit);

            refList = list;
        }

        private void MagnetismTestButton_Click(object sender, RoutedEventArgs e)
        {
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(tester);

            Tester magnet = new Tester((Enums.TestType)2, "/images/magnet.png");
            selectedTester = magnet;
            displayTester(magnet.ImgSource);
        }

        private void AcidTestButton_Click(object sender, RoutedEventArgs e)
        {
            ScratchTesters.IsEnabled = false;
            Playground.Children.Remove(tester);

            Tester acid = new Tester((Enums.TestType)3, "/images/dropper.png");
            selectedTester = acid;
            displayTester(acid.ImgSource);
        }


        private void ResetPlaygroundButton(object sender, RoutedEventArgs e)
        {

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
            MineralList.SelectedItem = randomItem;
            if ((bool)ShowName.IsChecked == false)
            {
                MineralList.DisplayMemberPath = "Mineral";
            }
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
            mineral.Fill = Brushes.Black;
        }

        /// <summary>
        /// Shows mineral image, "unhiding it".
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void HideMineral_Unchecked(object sender, RoutedEventArgs e)
        {
            brush.ImageSource = bitmap;
            mineral.Fill = brush;
        }

        /// <summary>
        /// Sets listbox display to be blank.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void ShowName_Unchecked(object sender, RoutedEventArgs e)
        {
            MineralList.DisplayMemberPath = "Mineral";
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

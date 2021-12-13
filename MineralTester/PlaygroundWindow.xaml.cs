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

        Ellipse mineral = new Ellipse();
        ImageBrush brush = new ImageBrush(); // Global varibles for setting image to ellipse.
        BitmapImage bitmap = new BitmapImage();
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
            Playground.Children.Clear(); // Clear screen of any mineral/test off screen.


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

                if(collisonCheck(mineral, tester))
                {
                    if(selectedMineral.Hardness < selectedTester.Hardness)
                    {
                        Canvas.SetTop(mineral, 150);
                        Canvas.SetLeft(mineral, 75);
                        MessageBox.Show($"{selectedMineral.Name} was scratched!");
                    } else
                    {
                        Canvas.SetTop(mineral, 150);
                        Canvas.SetLeft(mineral, 75);
                        MessageBox.Show($"Nothing happened. The {selectedTester.Name} left no scratch");
                    }
                    this.dragObj = null;
                    this.Playground.ReleaseMouseCapture();
                }
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

        private void ScratchTesters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTester = (Tester)ScratchTesters.SelectedItem;
            tester.Width = 150;
            tester.Height = 150;
            tester.Fill = Brushes.Blue;
            Canvas.SetLeft(tester, 550);
            Canvas.SetTop(tester, 150);

            if (!Playground.Children.Contains(tester))
            {
                Playground.Children.Add(tester);
            }
        }

        private void ScratchTestButton_Click(object sender, RoutedEventArgs e)
        {
            List<Tester> testers = new List<Tester>();
            fillTesters(ref testers);
            ScratchTesters.IsEnabled = true;
            ScratchTesters.ItemsSource = testers;
            ScratchTesters.DisplayMemberPath = "Name";
        }

        private void fillTesters(ref List<Tester> refList)
        {
            List<Tester> list = new List<Tester>();

            Tester fingerNail = new Tester("Finger Nail", (float)2.5);
            list.Add(fingerNail);

            Tester copperPenny = new Tester("Copper Penny", (float)3.5);
            list.Add(copperPenny);

            Tester knife = new Tester("Knife", (float)5.5);
            list.Add(knife);

            Tester steelNail = new Tester("Steel Nail", (float)6.5);
            list.Add(steelNail);

            Tester drillBit = new Tester("Masonry Drill Bit", (float)8.5);
            list.Add(drillBit);

            refList = list;
        }

        private void AcidTestButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MagnetismTestButton_Click(object sender, RoutedEventArgs e)
        {

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

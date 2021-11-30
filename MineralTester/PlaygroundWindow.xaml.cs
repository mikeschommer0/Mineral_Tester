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
        User _user;
        UIElement dragObj = null;
        System.Windows.Point offset;
        private Random _random = new Random();
        Ellipse mineral = new Ellipse();


        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            _user = currentUser;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            List<Mineral> minerals = new List<Mineral>();
            Database db = new Database();
            minerals = db.GetMinerals();

            MineralList.ItemsSource = minerals;
            MineralList.DisplayMemberPath = "Name";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
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


        private void Mineral_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragObj = sender as UIElement;
            this.offset = e.GetPosition(this.Playground);
            this.offset.Y -= Canvas.GetTop(this.dragObj);
            this.offset.X -= Canvas.GetLeft(this.dragObj);
            this.Playground.CaptureMouse();
        }

        private void Playgroud_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.dragObj == null)
            {
                return;
            }

            if (e.GetPosition(sender as IInputElement).X < Playground.ActualWidth - 50 &&
                e.GetPosition(sender as IInputElement).Y < Playground.ActualHeight - 50 &&
                e.GetPosition(sender as IInputElement).X > 50 && e.GetPosition(sender as IInputElement).Y > 50)
            {
                var position = e.GetPosition(sender as IInputElement);
                Canvas.SetTop(this.dragObj, position.Y - this.offset.Y);
                Canvas.SetLeft(this.dragObj, position.X - this.offset.X);
            }
        }

        private void Playgroud_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.dragObj = null;
            this.Playground.ReleaseMouseCapture();
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

        private void MineralList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Playground.Children.Clear();
            HideMineral.IsEnabled = true;

            Mineral selectedMineral = new Mineral();
            selectedMineral = (Mineral)MineralList.SelectedItem;

            //MessageBox.Show(selectedMineral.Image.Length.ToString());
            if (!(selectedMineral.Image is null))
            {
                BitmapImage bitmap = ByteArrayToBitmap(selectedMineral.Image);
                DisplayMineral(bitmap, selectedMineral);
            }

        }

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

        private void DisplayMineral(BitmapImage bitmap, Mineral displayMineral)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = bitmap;
            mineral.Fill = brush;

            mineral.Width = 100;
            mineral.Height = 100;
            Canvas.SetTop(mineral, 200);
            Canvas.SetLeft(mineral, 400);
            mineral.PreviewMouseDown += Mineral_PreviewMouseDown;


            Playground.Children.Add(mineral);
        }

        private void ResetPlaygroundButton(object sender, RoutedEventArgs e)
        {

        }

        private void RandomMineralButton(object sender, RoutedEventArgs e)
        {

            int randomIndex = _random.Next(MineralList.Items.Count);
            var randomItem = MineralList.Items[randomIndex];
            // MessageBox.Show($"Random item at index {randomIndex} is {randomItem}");
        }

        private void HideMineral_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void HideMineral_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}

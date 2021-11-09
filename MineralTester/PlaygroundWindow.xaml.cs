using MineralTester.Classes;
using System.Windows;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for PlaygroundWindow.xaml
    /// </summary>
    public partial class PlaygroundWindow : Window
    {
        User _user;

        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            _user = currentUser;
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

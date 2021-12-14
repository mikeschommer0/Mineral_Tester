using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for PracticeQuestionsWindow.xaml
    /// </summary>
    public partial class PracticeQuestionsWindow : Window
    {
        IDatabase database = new Database();
        List<Question> questions;

        /// <summary>
        /// Default constructor, sets data binding for user interface
        /// </summary>
        public PracticeQuestionsWindow()
        {
            InitializeComponent();
            questions = database.GetQuestions();
            foreach (Question question in questions)
            {
                cboQuestions.Items.Add(question);
            }
            cboQuestions.DisplayMemberPath = "Description";
        }

        /// <summary>
        /// Event raised when a question is selected from the question combo box,
        /// binds the question's answers to the answer combo box.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void cboQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboQuestions.SelectedIndex > -1)
            {
                Question selectedQuestion = (Question)cboQuestions.SelectedItem;
                cboAnswers.ItemsSource = selectedQuestion.Answers;
                cboAnswers.DisplayMemberPath = "Description";
            }
        }

        /// <summary>
        /// Determines if a user's answer to a question is correct or not.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Answer selectedAnswer = (Answer)cboAnswers.SelectedItem;
            if (selectedAnswer.IsCorrect)
            {
                MessageBox.Show("Correct!");
            }
            else
            {
                MessageBox.Show("Incorrect, try again.");
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

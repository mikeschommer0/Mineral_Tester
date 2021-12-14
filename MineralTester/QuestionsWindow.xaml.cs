using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for QuestionsWindow.xaml
    /// </summary>
    public partial class QuestionsWindow : Window
    {
        IDatabase database = new Database();

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionsWindow()
        {
            InitializeComponent();
            DisplayQuestions();
        }

        /// <summary>
        /// Gets a list of questions from the database and displays them in the user interface
        /// </summary>
        private void DisplayQuestions()
        {
            List<Question> questions = database.GetQuestions();
            lvQuestions.ItemsSource = null;
            lvQuestions.ItemsSource = questions;
            lvQuestions.DisplayMemberPath = "Description";
            dgAnswers.ItemsSource = null;
        }

        /// <summary>
        /// Opens a window to create a new question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertQuestionClick(object sender, RoutedEventArgs e)
        {
            MaintainQuestionWindow maintainQuestion = new MaintainQuestionWindow();
            maintainQuestion.ShowDialog();
            DisplayQuestions();
        }

        /// <summary>
        /// Opens a window to update an existing question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateQuestionClick(object sender, RoutedEventArgs e)
        {
            Question questionToUpdate = lvQuestions.SelectedItem as Question;
            if (questionToUpdate == null)
            {
                MessageBox.Show("Select a question to update.");
                return;
            }
            MaintainQuestionWindow maintainQuestion = new MaintainQuestionWindow(questionToUpdate);
            maintainQuestion.ShowDialog();
            DisplayQuestions();
        }

        /// <summary>
        /// Tells the database to delete the selected question and its' associated answers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteQuestionClick(object sender, RoutedEventArgs e)
        {
            Question questionToDelete = lvQuestions.SelectedItem as Question;
            if (questionToDelete == null)
            {
                MessageBox.Show("Select a question to delete.");
                return;
            }
            database.DeleteQuestion(questionToDelete.QuestionID);
            DisplayQuestions();
        }

        /// <summary>
        /// Displays the answers associated with a specific question when it is selected in the question list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvQuestions.SelectedIndex > -1)
            {
                Question selectedQuestion = lvQuestions.SelectedItem as Question;
                dgAnswers.ItemsSource = selectedQuestion.Answers;
            }
        }

        /// <summary>
        /// Formats the answer data grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgAnswers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            if (e.Column.Header.ToString() == "Description")
            {
                e.Column.Header = "Answer Description";
                e.Column.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            }
            if (e.Column.Header.ToString() == "IsCorrect")
            {
                e.Column.Header = "Correct Answer";
                e.Column.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

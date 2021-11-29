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

        public QuestionsWindow()
        {
            InitializeComponent();
            DisplayQuestions();
        }

        private void DisplayQuestions()
        {
            List<Question> questions = database.GetQuestions();
            lvQuestions.ItemsSource = null;
            lvQuestions.ItemsSource = questions;
            lvQuestions.DisplayMemberPath = "Description";
            dgAnswers.ItemsSource = null;
        }

        private void InsertQuestion(object sender, RoutedEventArgs e)
        {
            InsertQuestionWindow insertQuestionWindow = new InsertQuestionWindow();
            insertQuestionWindow.ShowDialog();
            DisplayQuestions();
        }

        private void DeleteQuestion(object sender, RoutedEventArgs e)
        {
            Question questionToDelete = new Question();
            questionToDelete = lvQuestions.SelectedItem as Question;
            if (questionToDelete == null)
            {
                MessageBox.Show("Select a question to delete.");
                return;
            }
            database.DeleteQuestion(questionToDelete.QuestionID);
            DisplayQuestions();
        }

        private void lvQuestions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvQuestions.SelectedIndex > -1)
            {
                Question selectedQuestion = new Question();
                selectedQuestion = lvQuestions.SelectedItem as Question;
                dgAnswers.ItemsSource = selectedQuestion.Answers;
            }
        }
    }
}

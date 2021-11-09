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
        public PracticeQuestionsWindow()
        {
            InitializeComponent();
            questions = database.GetQuestions();
            foreach (Question question in questions)
            {
                question.Answers = database.GetQuestionAnswers(question.QuestionID);
                cboQuestions.Items.Add(question);
            }
            cboQuestions.DisplayMemberPath = "Description";
        }

        private void cboQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboQuestions.SelectedIndex > -1)
            {
                Question selectedQuestion = (Question)cboQuestions.SelectedItem;
                cboAnswers.ItemsSource = selectedQuestion.Answers;
                cboAnswers.DisplayMemberPath = "Description";
            }
        }

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
    }
}

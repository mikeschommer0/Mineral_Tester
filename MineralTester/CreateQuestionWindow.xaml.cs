using MineralTester.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MineralTester.UI
{
    public enum ScreenMode
    {
        Question = 1,
        Answer
    }

    public partial class CreateQuestionWindow : Window
    {
        IDatabase database = new Database();
        List<Question> questions;
        List<Answer> answers;

        public CreateQuestionWindow()
        {
            InitializeComponent();
            DisplayQuestions();
            DisplayAnswers();
        }

        private void RefreshScreen()
        {
            DisplayQuestions();
            DisplayAnswers();
            rdoAnswer1.IsChecked = false;
            rdoAnswer2.IsChecked = false;
            rdoAnswer3.IsChecked = false;
            rdoAnswer4.IsChecked = false;
        }

        private void DisplayQuestions()
        {
            cboQuestions.ItemsSource = null;
            questions = database.GetQuestions();
            cboQuestions.ItemsSource = questions;
            cboQuestions.DisplayMemberPath = "Description";
            cboQuestions.SelectedValuePath = "QuestionID";
        }

        private void DisplayAnswers()
        {
            cboAnswer1.ItemsSource = null;
            cboAnswer2.ItemsSource = null;
            cboAnswer3.ItemsSource = null;
            cboAnswer4.ItemsSource = null;
            answers = database.GetAnswers();
            cboAnswer1.ItemsSource = answers;
            cboAnswer1.DisplayMemberPath = "Description";
            cboAnswer1.SelectedValuePath = "AnswerID";
            cboAnswer2.ItemsSource = answers;
            cboAnswer2.DisplayMemberPath = "Description";
            cboAnswer2.SelectedValuePath = "AnswerID";
            cboAnswer3.ItemsSource = answers;
            cboAnswer3.DisplayMemberPath = "Description";
            cboAnswer3.SelectedValuePath = "AnswerID";
            cboAnswer4.ItemsSource = answers;
            cboAnswer4.DisplayMemberPath = "Description";
            cboAnswer4.SelectedValuePath = "AnswerID";
        }

        private void btnQuestions_Click(object sender, RoutedEventArgs e)
        {
            MaintainQA maintainQuestions = new MaintainQA(Enums.QAEditMode.EditQuestions);
            maintainQuestions.ShowDialog();
            DisplayQuestions();
        }

        private void btnAnswers_Click(object sender, RoutedEventArgs e)
        {
            MaintainQA maintainQuestions = new MaintainQA(Enums.QAEditMode.EditAnswers);
            maintainQuestions.ShowDialog();
            DisplayAnswers();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cboQuestions.SelectedIndex > -1)
            {
                Question question = (Question)cboQuestions.SelectedItem;
                question.Answers = new List<Answer>();
                setAnswers(question);
                setCorrectAnswer(question);
                database.DeleteQuestionAnswers(question.QuestionID);
                var executionResult = database.InsertQuestionAnswers(question);
                if (executionResult.isSuccess)
                {
                    MessageBox.Show("Successfully created practice question.");
                }
                else
                {
                    MessageBox.Show($"Unable to create practice question: {executionResult.exceptionMessage}.");
                }
                RefreshScreen();
            }
        }

        private void setCorrectAnswer(Question question)
        {
            if ((bool)rdoAnswer1.IsChecked)
            {
                question.Answers[0].IsCorrect = true;
            }
            if ((bool)rdoAnswer2.IsChecked)
            {
                question.Answers[1].IsCorrect = true;
            }
            if ((bool)rdoAnswer3.IsChecked)
            {
                question.Answers[2].IsCorrect = true;
            }
            if ((bool)rdoAnswer4.IsChecked)
            {
                question.Answers[3].IsCorrect = true;
            }
        }

        private void setAnswers(Question question)
        {
            if (cboAnswer1.SelectedIndex > -1)
            {
                question.Answers.Add((Answer)cboAnswer1.SelectedItem);
            }
            if (cboAnswer2.SelectedIndex > -1)
            {
                question.Answers.Add((Answer)cboAnswer2.SelectedItem);
            }
            if (cboAnswer3.SelectedIndex > -1)
            {
                question.Answers.Add((Answer)cboAnswer3.SelectedItem);
            }
            if (cboAnswer4.SelectedIndex > -1)
            {
                question.Answers.Add((Answer)cboAnswer4.SelectedItem);
            }
        }
    }
}

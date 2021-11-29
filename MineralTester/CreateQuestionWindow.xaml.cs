using MineralTester.Classes;
using System.Collections.Generic;
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

        /// <summary>
        /// Constructor that sets up the user interface
        /// </summary>
        public CreateQuestionWindow()
        {
            InitializeComponent();
            DisplayQuestions();
            DisplayAnswers();
        }

        /// <summary>
        /// Refreshes the user interface to reflect data changes.
        /// </summary>
        private void RefreshScreen()
        {
            DisplayQuestions();
            DisplayAnswers();
            rdoAnswer1.IsChecked = false;
            rdoAnswer2.IsChecked = false;
            rdoAnswer3.IsChecked = false;
            rdoAnswer4.IsChecked = false;
        }

        /// <summary>
        /// Gets a list of questions from the database and displays them in the question combo box.
        /// </summary>
        private void DisplayQuestions()
        {
            cboQuestions.ItemsSource = null;
            questions = database.GetQuestions();
            cboQuestions.ItemsSource = questions;
            cboQuestions.DisplayMemberPath = "Description";
            cboQuestions.SelectedValuePath = "QuestionID";
        }

        /// <summary>
        /// Gets a list of answers from the database and displays them in the answer combo boxes.
        /// </summary>
        private void DisplayAnswers()
        {
            cboAnswer1.ItemsSource = null;
            cboAnswer2.ItemsSource = null;
            cboAnswer3.ItemsSource = null;
            cboAnswer4.ItemsSource = null;
            //answers = database.GetAnswers();
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

        /// <summary>
        /// Shows the MaintainQA window to edit question data.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnQuestions_Click(object sender, RoutedEventArgs e)
        {
            MaintainQA maintainQuestions = new MaintainQA(Enums.QADataType.Questions);
            maintainQuestions.ShowDialog();
            DisplayQuestions();
        }

        /// <summary>
        /// Shows the MaintainQA window to edit answer data.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnAnswers_Click(object sender, RoutedEventArgs e)
        {
            MaintainQA maintainQuestions = new MaintainQA(Enums.QADataType.Answers);
            maintainQuestions.ShowDialog();
            DisplayAnswers();
        }

        /// <summary>
        /// Inserts a new question and answer set into the database.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cboQuestions.SelectedIndex > -1)
            {
                Question question = (Question)cboQuestions.SelectedItem;
                question.Answers = new List<Answer>();
                setAnswers(question);
                setCorrectAnswer(question);
                //database.DeleteQuestionAnswers(question.QuestionID);
                //var executionResult = database.InsertQuestionAnswers(question);
                //MessageBox.Show(executionResult.message);
                RefreshScreen();
            }
        }

        /// <summary>
        /// Sets which answer is correct for the indicated question.
        /// </summary>
        /// <param name="question"> The question which the answers belong to</param>
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

        /// <summary>
        /// Determines which answers belong to the question.
        /// </summary>
        /// <param name="question"></param>
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

using MineralTester.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MineralTester.UI
{
    public partial class InsertQuestionWindow : Window
    {
        IDatabase database = new Database();

        /// <summary>
        /// Constructor that sets up the user interface
        /// </summary>
        public InsertQuestionWindow()
        {
            InitializeComponent();
            RefreshScreen();
        }

        private void RefreshScreen()
        {
            txtQuestion.Text = "";
            txtAnswer1.Text = "";
            txtAnswer2.Text = "";
            txtAnswer3.Text = "";
            txtAnswer4.Text = "";
            rdoAnswer1.IsChecked = false;
            rdoAnswer2.IsChecked = false;
            rdoAnswer3.IsChecked = false;
            rdoAnswer4.IsChecked = false;
        }

        /// <summary>
        /// Inserts a new question and answer set into the database.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Question questionToInsert = new Question();
            questionToInsert.Answers = new List<Answer>();
            if (!string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                questionToInsert.Description = txtQuestion.Text;
                if (setAnswers(questionToInsert))
                {
                    if (questionToInsert.Answers.Count < 2)
                    {
                        MessageBox.Show("You must enter at least 2 answers");
                    }
                    else if (!questionToInsert.Answers.Any(a => a.IsCorrect == true))
                    {
                        MessageBox.Show("Please provide a correct answer.");
                    }
                    else
                    {
                        database.InsertQuestion(questionToInsert);
                        MessageBox.Show("Successfully created practice question.");
                        RefreshScreen();
                    }
                }
                else
                {
                    MessageBox.Show("Unable to create practice question, answer set contains duplicate values.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a question.");
                txtQuestion.Focus();
            }
        }

        private bool setAnswers(Question question)
        {
            var hashset = new HashSet<string>();
            if (!string.IsNullOrWhiteSpace(txtAnswer1.Text))
            {
                if (!hashset.Add(txtAnswer1.Text.ToLower()))
                {
                    return false;
                }
                else
                {
                    bool isCorrect = (bool)rdoAnswer1.IsChecked;
                    question.Answers.Add(new Answer(txtAnswer1.Text, isCorrect));
                }
            }
            if (!string.IsNullOrWhiteSpace(txtAnswer2.Text))
            {
                if (!hashset.Add(txtAnswer2.Text.ToLower()))
                {
                    return false;
                }
                else
                {
                    bool isCorrect = (bool)rdoAnswer2.IsChecked;
                    question.Answers.Add(new Answer(txtAnswer2.Text, isCorrect));
                }
            }
            if (!string.IsNullOrWhiteSpace(txtAnswer3.Text))
            {
                if (!hashset.Add(txtAnswer3.Text.ToLower()))
                {
                    return false;
                }
                else
                {
                    bool isCorrect = (bool)rdoAnswer3.IsChecked;
                    question.Answers.Add(new Answer(txtAnswer3.Text, isCorrect));
                }
            }
            if (!string.IsNullOrWhiteSpace(txtAnswer4.Text))
            {
                if (!hashset.Add(txtAnswer4.Text.ToLower()))
                {
                    return false;
                }
                else
                {
                    bool isCorrect = (bool)rdoAnswer4.IsChecked;
                    question.Answers.Add(new Answer(txtAnswer4.Text, isCorrect));
                }
            }
            return true;
        }
    }
}

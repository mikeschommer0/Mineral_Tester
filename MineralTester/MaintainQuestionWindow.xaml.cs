using MineralTester.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MineralTester.UI
{
    public partial class MaintainQuestionWindow : Window
    {
        IDatabase database = new Database();
        Question _questionToUpdate = null;

        /// <summary>
        /// Constructor that sets up the user interface
        /// </summary>
        public MaintainQuestionWindow(Question question = null)
        {
            InitializeComponent();
            RefreshScreen();
            if (question != null)
            {
                _questionToUpdate = question;
                PopulateTextBoxes(_questionToUpdate);
            }
        }

        /// <summary>
        /// If an exiting question is being updated, populate the text boxes with associated information
        /// </summary>
        /// <param name="question"></param>
        private void PopulateTextBoxes(Question question)
        {
            txtQuestion.Text = question.Description;
            if (question.Answers.Count > 0)
            {
                txtAnswer1.Text = question.Answers[0].Description;
                if (question.Answers[0].IsCorrect)
                {
                    rdoAnswer1.IsChecked = true;
                }
            }
            if (question.Answers.Count > 1)
            {
                txtAnswer2.Text = question.Answers[1].Description;
                if (question.Answers[1].IsCorrect)
                {
                    rdoAnswer2.IsChecked = true;
                }
            }
            if (question.Answers.Count > 2)
            {
                txtAnswer3.Text = question.Answers[2].Description;
                if (question.Answers[2].IsCorrect)
                {
                    rdoAnswer3.IsChecked = true;
                }
            }
            if (question.Answers.Count > 3)
            {
                txtAnswer4.Text = question.Answers[3].Description;
                if (question.Answers[3].IsCorrect)
                {
                    rdoAnswer4.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Refreshes the user interface
        /// </summary>
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
            if (_questionToUpdate == null)
            {
                InsertNewQuestion();
            }
            else
            {
                UpdateQuestion();
            }
        }

        /// <summary>
        /// Takes user input and updates an existing question and associated answers in the database
        /// </summary>
        private void UpdateQuestion()
        {
            if (!string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                _questionToUpdate.Description = txtQuestion.Text;
                _questionToUpdate.Answers = new List<Answer>();
                if (setAnswers(_questionToUpdate))
                {
                    if (_questionToUpdate.Answers.Count < 2)
                    {
                        MessageBox.Show("You must enter at least 2 answers");
                    }
                    else if (!_questionToUpdate.Answers.Any(a => a.IsCorrect == true))
                    {
                        MessageBox.Show("Please provide a correct answer.");
                    }
                    else
                    {
                        database.UpdateQuestion(_questionToUpdate);
                        MessageBox.Show("Successfully updated practice question.");
                        Close();
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

        /// <summary>
        /// Takes user input and inserts a new question and associated answers into the database
        /// </summary>
        private void InsertNewQuestion()
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

        /// <summary>
        /// Associates a set of answers to a specific question
        /// </summary>
        /// <param name="question"> The question the answer set is associated to </param>
        /// <returns></returns>
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

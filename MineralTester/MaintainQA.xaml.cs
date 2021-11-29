using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for MaintainQA.xaml
    /// </summary>
    public partial class MaintainQA : Window
    {
        IDatabase database = new Database();
        List<Question> _questions;
        List<Answer> _answers;
        Question _question;
        Answer _answer;
        Enums.QADataType _dataType;

        /// <summary>
        /// Constructor for MaintainQA window, sets up user interface based on which type of data is being worked with.
        /// </summary>
        /// <param name="dataType"> Represents which type of data is being worked with.</param>
        public MaintainQA(Enums.QADataType dataType)
        {
            InitializeComponent();
            _dataType = dataType;
            RefreshScreen();
            Title = string.Format($"{_dataType} Editor");
            lblAttributes.Content = string.Format($"{_dataType}:");
        }

        /// <summary>
        /// Refreshes the user interface to reflect data changes
        /// </summary>
        private void RefreshScreen()
        {
            cboAttributes.ItemsSource = null;
            switch (_dataType)
            {
                case Enums.QADataType.Questions:
                    _questions = database.GetQuestions();
                    cboAttributes.ItemsSource = _questions;
                    break;
                case Enums.QADataType.Answers:
                    //_answers = database.GetAnswers();
                    cboAttributes.ItemsSource = _answers;
                    break;
                default:
                    break;
            }
            cboAttributes.DisplayMemberPath = "Description";
            txtAttributes.Text = string.Empty;
        }

        /// <summary>
        /// Tells the database to insert a new question/answer.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            switch (_dataType)
            {
                case Enums.QADataType.Questions:
                    //database.InsertQuestion(txtAttributes.Text);
                    break;
                case Enums.QADataType.Answers:
                    //database.InsertAnswer(txtAttributes.Text);
                    break;
                default:
                    break;
            }
            RefreshScreen();
        }

        /// <summary>
        /// Tells the database to update the selected question/answer.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_dataType)
                {
                    case Enums.QADataType.Questions:
                        database.UpdateQuestion(_question.QuestionID, txtAttributes.Text);
                        break;
                    case Enums.QADataType.Answers:
                        //database.UpdateAnswer(_answer.AnswerID, txtAttributes.Text);
                        break;
                    default:
                        break;
                }
            }
            RefreshScreen();
        }

        /// <summary>
        /// Tells the database to delete the selected question/answer.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_dataType)
                {
                    case Enums.QADataType.Questions:
                        database.DeleteQuestion(_question.QuestionID);
                        break;
                    case Enums.QADataType.Answers:
                        //database.DeleteAnswer(_answer.AnswerID);
                        break;
                    default:
                        break;
                }
            }
            RefreshScreen();
        }

        /// <summary>
        /// Takes a question description and adds it as editable text in the txtAttributes text box.
        /// </summary>
        /// <param name="sender"> Reference to the control/object that raised the event.</param>
        /// <param name="e"> Contains event data.</param>
        private void cboAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_dataType)
                {
                    case Enums.QADataType.Questions:
                        _question = (Question)cboAttributes.SelectedItem;
                        txtAttributes.Text = _question.Description;
                        break;
                    case Enums.QADataType.Answers:
                        _answer = (Answer)cboAttributes.SelectedItem;
                        txtAttributes.Text = _answer.Description;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

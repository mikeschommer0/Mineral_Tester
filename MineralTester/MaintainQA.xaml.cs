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
        Enums.QAEditMode _editMode;

        public MaintainQA(Enums.QAEditMode editMode)
        {
            InitializeComponent();
            _editMode = editMode;
            RefreshScreen();
            string title = _editMode.ToString();
            title.Insert(4, " ");
            Title = title;
            lblAttributes.Content = _editMode.ToString().Substring(4);
        }

        private void RefreshScreen()
        {
            cboAttributes.ItemsSource = null;
            switch (_editMode)
            {
                case Enums.QAEditMode.EditQuestions:
                    _questions = database.GetQuestions();
                    cboAttributes.ItemsSource = _questions;
                    break;
                case Enums.QAEditMode.EditAnswers:
                    _answers = database.GetAnswers();
                    cboAttributes.ItemsSource = _answers;
                    break;
                default:
                    break;
            }
            cboAttributes.DisplayMemberPath = "Description";
            txtAttributes.Text = string.Empty;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            switch (_editMode)
            {
                case Enums.QAEditMode.EditQuestions:
                    database.InsertQuestion(txtAttributes.Text);
                    break;
                case Enums.QAEditMode.EditAnswers:
                    database.InsertAnswer(txtAttributes.Text);
                    break;
                default:
                    break;
            }
            RefreshScreen();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_editMode)
                {
                    case Enums.QAEditMode.EditQuestions:
                        database.UpdateQuestion(_question.QuestionID, txtAttributes.Text);
                        break;
                    case Enums.QAEditMode.EditAnswers:
                        database.UpdateAnswer(_answer.AnswerID, txtAttributes.Text);
                        break;
                    default:
                        break;
                }
            }
            RefreshScreen();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_editMode)
                {
                    case Enums.QAEditMode.EditQuestions:
                        database.DeleteQuestion(_question.QuestionID);
                        break;
                    case Enums.QAEditMode.EditAnswers:
                        database.DeleteAnswer(_answer.AnswerID);
                        break;
                    default:
                        break;
                }
            }
            RefreshScreen();
        }

        private void cboAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                switch (_editMode)
                {
                    case Enums.QAEditMode.EditQuestions:
                        _question = (Question)cboAttributes.SelectedItem;
                        txtAttributes.Text = _question.Description;
                        break;
                    case Enums.QAEditMode.EditAnswers:
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

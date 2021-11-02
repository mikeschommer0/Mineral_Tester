using MineralTester.Classes;
using System;
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
        Database database = new Database();
        List<Question> questions;
        List<Answer> answers;

        public CreateQuestionWindow()
        {
            InitializeComponent();
            DisplayQuestions();
            DisplayAnswers();
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

        }

        private void btnAnswers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cboQuestions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for CreateQuestion.xaml
    /// </summary>
    public partial class CreateQuestion : Window
    {
        public CreateQuestion()
        {
            InitializeComponent();
        }

        private void AddAnotherProblemButton(object sender, RoutedEventArgs e)
        {

        }

        private void ExitQuestonWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

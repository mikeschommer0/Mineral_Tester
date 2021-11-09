using Microsoft.Win32;
using System;
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
using MineralTester.Classes;


namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for AddMineralWindow.xaml
    /// </summary>
    public partial class AddMineralWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();
        public AddMineralWindow()
        {
            InitializeComponent();
        }

        private void AddAnotherMineral(object sender, RoutedEventArgs e)
        {
            List<object> fields = new List<object>(); 
            String name = MineralNameTextBox.Text;
            fields.Add(name);
            float hardness = float.TryParse(MineralHardnessTextBox.Text.Trim(), out hardness) ? hardness : 0; // Try to parse as float, if it fails it will default to zero. Validator will fail any value of 0.
            fields.Add(hardness);  
            List<bool> validFields = bl.ValidateMineralData(fields);

            if (validFields.Contains(false)) // If any invaild fields, show message box for appropriate invalid field.
            { 
                EntryErrors(validFields);
            }
            else
            {
                Close();
            }
        }

        private void EntryErrors(List<bool> validFields)
        {
            if (validFields[0] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidNameLength");
            }
            if (validFields[1] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidHardnessLevel");
            }
            if (MineralImage.Source == null)
            {
                MessageBox.Show("Error while adding Mineral:\nNoPhotoChosen");
            }
        }

        private void ExitMineralWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|(*.png)|*.png"; // Allows user to only choose jpeg or png files.
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                Uri fileUri = new Uri(selectedFileName);
                MineralImage.Source = new BitmapImage(fileUri);
            }
        }
    }
}

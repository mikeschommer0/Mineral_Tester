using Microsoft.Win32;
using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for AddMineralWindow.xaml
    /// </summary>
    public partial class AddMineralWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();

        private string selectedFileName = "";
        private bool magnetic;
        private bool acidReaction;
        public AddMineralWindow()
        {
            InitializeComponent();
        }

        private void AddAnotherMineral(object sender, RoutedEventArgs e)
        {
            List<object> fields = new List<object>();
            String name = MineralNameTextBox.Text;
            fields.Add(name);

            // Try to parse as float, if it fails it will default to zero. Validator will fail any value of 0.
            float hardness = float.TryParse(MineralHardnessTextBox.Text.Trim(), out hardness) ? hardness : 0;
            //possible combine these 2
            fields.Add(hardness);
            List<bool> validFields = bl.ValidateMineralData(fields);

            if (validFields.Contains(false)) // If any invaild fields, show message box for appropriate invalid field.
            {
                MessageBox.Show(EntryErrors(validFields));
            }
            else
            {
                byte[] imgBytes = null;

                // Only read file if exists.
                if (!selectedFileName.Equals(""))
                {
                    FileStream stream = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(stream);
                    imgBytes = br.ReadBytes((int)stream.Length);
                    Mineral toAdd = new Mineral(0, name, hardness, magnetic, acidReaction, imgBytes);
                    bl.AddMineral(toAdd);
                    Close();
                }
                else
                {
                    MessageBox.Show("Please select a photo and try again.");
                }
            }
        }

        private string EntryErrors(List<bool> validFields)
        {
            string errors = "Error(s) while adding Mineral:\n";

            if (validFields[0] == false)
            {
                errors += "\nInvalidNameLength";
            }
            if (validFields[1] == false)
            {
                errors += "\nInvalidHardnessLevel";
            }
            if (MineralImage.Source == null)
            {
                errors += "\nNoPhotoChosen";
            }
            if (validFields[2] == false)
            {
                errors += "\nMineralNameAlreadyExists";
            }

            return errors;
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
                selectedFileName = dlg.FileName;
                Uri fileUri = new Uri(selectedFileName);
                MineralImage.Source = new BitmapImage(fileUri);
            }
        }

        private void ReactsWithAcid(object sender, RoutedEventArgs e)
        {
            this.acidReaction = true;
        }

        private void DoesntReactWithAcid(object sender, RoutedEventArgs e)
        {
            this.acidReaction = false;
        }

        private void IsMagnetic(object sender, RoutedEventArgs e)
        {
            this.magnetic = true;
        }

        private void IsntMagnetic(object sender, RoutedEventArgs e)
        {
            this.magnetic = false;
        }
    }
}

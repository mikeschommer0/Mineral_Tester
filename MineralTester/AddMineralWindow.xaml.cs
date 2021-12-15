using Microsoft.Win32;
using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

/// <summary>
/// Coded by: Quinn Nimmer
/// XAML styling by Rick Bowman
/// </summary>
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

        /// <summary>
        /// Adds a mineral to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAnotherMineral(object sender, RoutedEventArgs e)
        {
            FeedBack.Text = "";
            List<object> fields = new List<object>();
            string name = MineralNameTextBox.Text;
            fields.Add(name);
            float hardness = float.TryParse(MineralHardnessTextBox.Text.Trim(), out hardness) ? hardness : 0;
            fields.Add(hardness);
            List<bool> validFields = bl.ValidateMineralData(fields);
            if (validFields.Contains(false))
            {
                FeedBack.Text = EntryErrors(validFields);
            }
            else
            {
                byte[] imgBytes;
                if (!selectedFileName.Equals(""))
                {
                    FileStream stream = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(stream);
                    imgBytes = br.ReadBytes((int)stream.Length);
                    Mineral toAdd = new Mineral(0, name, hardness, magnetic, acidReaction, imgBytes, cpColor.SelectedColor.ToString());
                    bl.AddMineral(toAdd);
                    Close();
                }
                else
                {
                    FeedBack.Text = "Please select a photo and try again.";
                }
            }
        }

        /// <summary>
        /// Builds a string representation of the errors encountered during input sanitation.
        /// </summary>
        /// <param name="validFields"> Represents a list of valid or invalid inputs. </param>
        /// <returns> A string representation of sanitation errors. </returns>
        private string EntryErrors(List<bool> validFields)
        {
            string errors = "Error(s) while adding Mineral:";

            if (validFields[0] == false)
            {
                errors += "\nInvalid Name Length";
            }
            if (validFields[1] == false)
            {
                errors += "\nInvalid Hardness Level";
            }
            if (MineralImage.Source == null)
            {
                errors += "\nNo Photo Chosen";
            }
            if (validFields[2] == false)
            {
                errors += "\nMineral Name Already Exists";
            }

            return errors;
        }

        /// <summary>
        /// Exits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMineralWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the addition of a mineral image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|(*.png)|*.png";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                selectedFileName = dlg.FileName;
                Uri fileUri = new Uri(selectedFileName);
                MineralImage.Source = new BitmapImage(fileUri);
                AddMineral.IsEnabled = true;
            }
        }

        /// <summary>
        /// Mineral reacts with acid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReactsWithAcid(object sender, RoutedEventArgs e)
        {
            acidReaction = true;
        }

        /// <summary>
        /// Mineral does not react with acid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoesntReactWithAcid(object sender, RoutedEventArgs e)
        {
            acidReaction = false;
        }

        /// <summary>
        /// Mineral is magnetic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsMagnetic(object sender, RoutedEventArgs e)
        {
            magnetic = true;
        }

        /// <summary>
        /// Mineral is not magnetic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsntMagnetic(object sender, RoutedEventArgs e)
        {
            magnetic = false;
        }
    }
}

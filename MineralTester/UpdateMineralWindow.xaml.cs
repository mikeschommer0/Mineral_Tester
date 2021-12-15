using Microsoft.Win32;
using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// Coded by: Quinn Nimmer
/// XAML styling by Rick Bowman
/// </summary>
namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for UpdateMineralWindow.xaml
    /// </summary>
    public partial class UpdateMineralWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();
        private Mineral mineralToModify;
        private string selectedFileName = "";
        private bool magnetic;
        private bool acidReaction;

        public UpdateMineralWindow(Mineral mineralToUpdate = null)
        {
            InitializeComponent();
            if (mineralToUpdate != null)
            {
                mineralToModify = mineralToUpdate;
                PopulateInterface(mineralToModify);
            }
        }

        /// <summary>
        /// Populates the interface with mineral data.
        /// </summary>
        /// <param name="mineral"> The mineral to pull data from. </param>
        private void PopulateInterface(Mineral mineral)
        {
            MineralNameTextBox.Text = mineral.Name;
            MineralHardnessTextBox.Text = mineral.Hardness.ToString();
            AcidReaction.IsChecked = mineral.AcidReaction;
            MagneticReaction.IsChecked = mineral.IsMagnetic;
            MineralImage.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(mineral.Image);
            cpColor.SelectedColor = (Color)ColorConverter.ConvertFromString(mineral.StreakColor);
        }

        /// <summary>
        /// Gets information from the UI to update a mineral's data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateMineral(object sender, RoutedEventArgs e)
        {
            if (mineralToModify != null)
            {
                mineralToModify = bl.GetMineral(mineralToModify.Name);
                mineralToModify.Name = MineralNameTextBox.Text;
                List<object> fields = new List<object>();
                string name = MineralNameTextBox.Text;
                fields.Add(name);
                float hardness = float.TryParse(MineralHardnessTextBox.Text.Trim(), out hardness) ? hardness : 0;
                fields.Add(hardness);
                List<bool> validFields = bl.ValidateMineralData(fields);
                if (validFields[0] == false || validFields[1] == false)
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
                        string streakColor = cpColor.SelectedColor.ToString();
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness, magnetic, acidReaction, imgBytes, streakColor);
                        bl.UpdateMineral(mineralToModify);
                        ExitMineralWindow(sender, e);
                    }
                    else
                    {
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness, magnetic, acidReaction, mineralToModify.Image, cpColor.SelectedColor.ToString());
                        bl.UpdateMineral(mineralToModify);
                        ExitMineralWindow(sender, e);
                    }
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
            string errors = "Error(s) while updating Mineral:";
            if (validFields[0] == false)
            {
                errors += "\nInvalid Name Length";
            }
            if (validFields[1] == false)
            {
                errors += "\nInvalid Hardness Level";
            }
            return errors;
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

        /// <summary>
        /// Exits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMineralWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

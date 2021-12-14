using Microsoft.Win32;
using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for EditOrDeleteMineral.xaml
    /// </summary>
    public partial class EditOrDeleteMineral : Window
    {
        IBusinessLogic bl = new BusinessLogic();

        private Mineral mineralToModify;

        private string selectedFileName = "";
        private bool magnetic;
        private bool acidReaction;
        public EditOrDeleteMineral(Mineral mineralToUpdate = null)
        {
            InitializeComponent();
            if (mineralToUpdate != null)
            {
                mineralToModify = mineralToUpdate;
                PopulateInterface(mineralToModify);
            }
        }

        private void PopulateInterface(Mineral mineral)
        {
            MineralNameTextBox.Text = mineral.Name;
            MineralHardnessTextBox.Text = mineral.Hardness.ToString();
            AcidReaction.IsChecked = mineral.AcidReaction;
            MagneticReaction.IsChecked = mineral.IsMagnetic;
            MineralImage.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(mineral.Image);
        }

        private void UpdateMineral(object sender, RoutedEventArgs e)
        {
            FeedBackBox.Text = "";
            if (mineralToModify != null)
            {
                mineralToModify = bl.GetMineral(mineralToModify.Name);
                mineralToModify.Name = MineralNameTextBox.Text;

                List<object> fields = new List<object>();

                String name = MineralNameTextBox.Text;
                fields.Add(name);

                // Try to parse as float, if it fails it will default to zero. Validator will fail any value of 0.
                float hardness = float.TryParse(MineralHardnessTextBox.Text.Trim(), out hardness) ? hardness : 0;
                fields.Add(hardness);

                List<bool> validFields = bl.ValidateMineralData(fields);

                if (validFields[0] == false || validFields[1] == false)
                {
                    FeedBackBox.Text = EntryErrors(validFields);
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
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness, magnetic, acidReaction, imgBytes);

                        bl.UpdateMineral(mineralToModify);

                        ExitMineralWindow(sender, e);
                    }

                    // Should use the same img.
                    else
                    {
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness,
                            magnetic, acidReaction, mineralToModify.Image);

                        bl.UpdateMineral(mineralToModify);

                        ExitMineralWindow(sender, e);
                    }
                }
            }
        }

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

        private void ExitMineralWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

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
using System.IO;

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
        public EditOrDeleteMineral()
        {
            InitializeComponent();
            List<Mineral> minerals = bl.GetMinerals();

            MineralList.ItemsSource = minerals;
            MineralList.DisplayMemberPath = "Name";
        }

        private void UpdateMineral(object sender, RoutedEventArgs e)
        {
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
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness, magnetic, acidReaction, imgBytes);

                        bl.UpdateMineral(mineralToModify);

                        MessageBox.Show("MINERAL MODIFIED: \n" + mineralToModify.Name + ".");
                        ExitMineralWindow(sender, e);
                    }
                    // Should use the same img.
                    else
                    {
                        mineralToModify = new Mineral(mineralToModify.ID, name, hardness,
                            magnetic, acidReaction, mineralToModify.Image);

                        bl.UpdateMineral(mineralToModify);

                        MessageBox.Show("MINERAL MODIFIED: \n" + mineralToModify.Name + "\n(Image was not updated).");
                        ExitMineralWindow(sender, e);
                    }
                }
            }
            else
            {
                MessageBox.Show("MODIFICATION FAILED: A Mineral must be select to modify.");
            }
        }

        private string EntryErrors(List<bool> validFields)
        {
            string errors = "Error(s) while updating Mineral:\n";

            if (validFields[0] == false)
            {
                errors += "\nInvalidNameLength";
            }
            if (validFields[1] == false)
            {
                errors += "\nInvalidHardnessLevel";
            }

            return errors;
        }

        private void DeleteMineral (object sender, RoutedEventArgs e)
        {
            
            if (mineralToModify != null)
            {
                mineralToModify = bl.GetMineral(mineralToModify.Name); 

                MessageBox.Show("MINERAL DELETED: \n" + mineralToModify.Name);
                bl.DeleteMineral(mineralToModify);
                ExitMineralWindow(sender, e);
            }
            else
            {
                MessageBox.Show("A Mineral must be select to delete.");
            }
        }

        private void MineralList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Mineral selectedMineral = new Mineral();
            selectedMineral = (Mineral)MineralList.SelectedItem;

            mineralToModify = selectedMineral;


            // Update values in window and tracker vars
            MineralNameTextBox.Text = selectedMineral.Name;
            MineralHardnessTextBox.Text = "" + selectedMineral.Hardness;
            
            this.acidReaction = selectedMineral.AcidReaction;
            AcidReaction.IsChecked = selectedMineral.AcidReaction;
            
            this.magnetic = selectedMineral.IsMagnetic;
            MagneticReaction.IsChecked = selectedMineral.IsMagnetic;

            // Render img stored.
            MineralImage.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(selectedMineral.Image);
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

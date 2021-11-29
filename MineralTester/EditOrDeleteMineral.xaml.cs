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
            List<Mineral> minerals = new List<Mineral>();
            Database db = new Database();
            minerals = db.GetMinerals();

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
                    EntryErrors(validFields);
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
                        MessageBox.Show("MINERAL MODIFIED: " + mineralToModify.Name);
                    }
                    else
                    {
                        MessageBox.Show("A valid photo must be provided to update.");
                    }
                }
            }
            else
            {
                MessageBox.Show("A Mineral must be select to update.");
            }

            ExitMineralWindow(sender, e);
        }

        private void EntryErrors(List<bool> validFields)
        {
            if (validFields[0] == false)
            {
                MessageBox.Show("Error while updating Mineral:\nInvalidNameLength");
            }
            if (validFields[1] == false)
            {
                MessageBox.Show("Error while updating Mineral:\nInvalidHardnessLevel");
            }
            if (MineralImage.Source == null)
            {
                MessageBox.Show("Error while updating Mineral:\nNoPhotoChosen");
            }
        }

        private void DeleteMineral (object sender, RoutedEventArgs e)
        {
            
            if (mineralToModify != null)
            {
                mineralToModify = bl.GetMineral(mineralToModify.Name); 

                MessageBox.Show("MINERAL DELETED: " + mineralToModify.Name);
                bl.DeleteMineral(mineralToModify);
            }
            else
            {
                MessageBox.Show("A Mineral must be select to delete.");
            }

            ExitMineralWindow(sender, e);
        }

        private void MineralList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Mineral selectedMineral = new Mineral();
            selectedMineral = (Mineral)MineralList.SelectedItem;

            mineralToModify = selectedMineral;

            MineralNameTextBox.Text = selectedMineral.Name;
            MineralHardnessTextBox.Text = "" + selectedMineral.Hardness;

            if (selectedMineral.AcidReaction)
            {
                this.acidReaction = true;
                AcidReaction.IsChecked = true;
            }
            else
            {
                this.acidReaction = false;
                AcidReaction.IsChecked = false;
            }
            if (selectedMineral.IsMagnetic)
            {
                this.magnetic = true;
                MagneticReaction.IsChecked = true;
            }
            else
            {
                this.magnetic = false;
                MagneticReaction.IsChecked = false;
            }
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

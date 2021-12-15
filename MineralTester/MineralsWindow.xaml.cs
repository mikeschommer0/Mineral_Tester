using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Written by Rick Bowman
/// XAML styling by Rick Bowman
/// </summary>
namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for MineralsWindow.xaml
    /// </summary>
    public partial class MineralsWindow : Window
    {
        IDatabase database = new Database();
        IBusinessLogic businessLogic = new BusinessLogic();

        public MineralsWindow()
        {
            InitializeComponent();
            DisplayMinerals();
        }

        private void DisplayMinerals()
        {
            List<Mineral> minerals = businessLogic.GetMinerals();
            dgMinerals.ItemsSource = null;
            dgMinerals.ItemsSource = minerals;
        }

        /// <summary>
        /// Handles a user selecting to add a new mineral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMineralClick(object sender, RoutedEventArgs e)
        {
            AddMineralWindow addMineral = new AddMineralWindow();
            addMineral.ShowDialog();
            DisplayMinerals();
        }

        /// <summary>
        /// Handles a user selecting to update a mineral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateMineralClick(object sender, RoutedEventArgs e)
        {
            Mineral mineralToUpdate = dgMinerals.SelectedItem as Mineral;
            if (mineralToUpdate == null)
            {
                MessageBox.Show("Select a mineral to update.");
                return;
            }
            mineralToUpdate = businessLogic.GetMineral(mineralToUpdate.Name);
            UpdateMineralWindow editOrDeleteMineral = new UpdateMineralWindow(mineralToUpdate);
            editOrDeleteMineral.ShowDialog();
            DisplayMinerals();
        }

        /// <summary>
        /// Handles a user selecting to delete a mineral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMineralClick(object sender, RoutedEventArgs e)
        {
            Mineral mineralToDelete = dgMinerals.SelectedItem as Mineral;
            if (mineralToDelete == null)
            {
                MessageBox.Show("Select a mineral to delete.");
                return;
            }
            mineralToDelete = businessLogic.GetMineral(mineralToDelete.Name);
            businessLogic.DeleteMineral(mineralToDelete);
            DisplayMinerals();
        }

        /// <summary>
        /// Formats data grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgMinerals_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID" || e.Column.Header.ToString() == "Image")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "IsMagnetic")
            {
                e.Column.Header = "Magnetic";
            }
            if (e.Column.Header.ToString() == "AcidReaction")
            {
                e.Column.Header = "Reacts to acid";
            }
            if (e.Column.Header.ToString() == "Hidden")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "StreakColor")
            {
                e.Column.Header = "Streak Color";
            }
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        /// <summary>
        /// Exits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

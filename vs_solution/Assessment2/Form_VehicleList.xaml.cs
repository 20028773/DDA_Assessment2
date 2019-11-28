using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR FORM_VEHICLELIST.XAML
    /// </summary>
    public partial class Form_VehicleList
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Form_VehicleList()
        {
            InitializeComponent();

            UpdateList();
        }
        /// <summary>
        /// UPDATE THE LIST VIEW ACCORDING TO THE TXT FILTER
        /// </summary>
        public void UpdateList()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                lvVehicleList.ItemsSource = Vehicle.vehicleList;
            }
            else
            {
                lvVehicleList.ItemsSource = Vehicle.vehicleList.Where(x => x.Manufacturer.ToUpper().Contains(txtFilter.Text.ToUpper())
                                                                        || x.Model.ToUpper().Contains(txtFilter.Text.ToUpper())
                                                                        || x.Registration.ToUpper().Contains(txtFilter.Text.ToUpper()));
            }

            //lvVehicleList.ItemsSource = Sql.selectTable("SELECT * FROM VEHICLE");
            lvVehicleList.Items.Refresh();
        }
        /// <summary>
        /// ON BTN DELETE CLICK DELETE THE VEHICLE FROM THE LIST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will erase the vehicle from the database, do you still want to continue ?", "Vehicle", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (lvVehicleList.SelectedItem != null)
                {
                    Vehicle.DeleteVehicle((Vehicle)lvVehicleList.SelectedItem);
                    UpdateList();
                }
            }
        }
        /// <summary>
        /// ON BTN EDIT CLICK OPEN THE VEHICLE FORM TO UPDATE ITS INFORMATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvVehicleList.SelectedItem != null)
            {
                Form_Vehicle form_Vehicle = new Form_Vehicle((Vehicle)lvVehicleList.SelectedItem);
                form_Vehicle.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// ON BTN ADD CLICK OPEN A FORM TO ADD A NEW VEHICLE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddVehicle_Click(object sender, RoutedEventArgs e)
        {
            Form_Vehicle form_Vehicle = new Form_Vehicle();
            form_Vehicle.ShowDialog();
            UpdateList();
        }
        /// <summary>
        /// ON THE LIST VIEW DOUBLE CLICK OPEN THE VEHICLE INFORMATIOM FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvVehicleList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvVehicleList.SelectedItem != null)
            {
                Form_VehicleInformation form_VehicleInformation = new Form_VehicleInformation((Vehicle)lvVehicleList.SelectedItem);
                form_VehicleInformation.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// ON BTN SERVICE CLICK CREATE A SERVICE FOR THE VEHICLE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Have you serviced the vehicle ?", "Vehicle", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Button button = sender as Button;
                Vehicle vehicleItem = button.DataContext as Vehicle;
                Service.recordService(vehicleItem.Id);
                UpdateList();
            }
        }
        /// <summary>
        /// ON BTN FUEL PURCHASE CLICK OPEN THE FUEL FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPurchaseFuel_Click(object sender, RoutedEventArgs e)
        {
            if (lvVehicleList.SelectedItem != null)
            {
                Form_FuelPurchase form_FuelPurchase = new Form_FuelPurchase((Vehicle)lvVehicleList.SelectedItem);
                form_FuelPurchase.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// ON BTN HISTORY CLICK OPEN THE HISTORY FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (lvVehicleList.SelectedItem != null)
            {
                Form_VehicleHistory form_VehicleHistory = new Form_VehicleHistory((Vehicle)lvVehicleList.SelectedItem);
                form_VehicleHistory.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// ON THE TXT FILTER CHANGE UPDATE THE LISTVIEW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }
    }
}

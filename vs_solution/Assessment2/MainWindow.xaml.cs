using System.Windows;
using System.Windows.Controls;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR MAINWINDOW.XAML
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UpdateList();
        }
        /// <summary>
        /// UPDATE THE LIST VIEW ACCORDING TO THE FILTER
        /// </summary>
        public void UpdateList()
        {
            lvRentVehicleList.ItemsSource = Rental.GetAvailableVehicles(txtFilter.Text);
            lvRentVehicleList.Items.Refresh();
        }
        /// <summary>
        /// ON BTN VEHICLE CLICK OPEN THE VEHICLE LIST FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiVehicles_Click(object sender, RoutedEventArgs e)
        {
            Form_VehicleList form_VehicleList = new Form_VehicleList();
            form_VehicleList.ShowDialog();
            UpdateList();
        }
        /// <summary>
        /// ON THE LIST VIEW DOUBLE CLICK OPEN THE FORM TO CREATE A NEW RENTAL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvRentVehicleList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvRentVehicleList.SelectedItem != null)
            {
                Vehicle v = (Vehicle)lvRentVehicleList.SelectedItem;
                Form_Rental form_Rental = new Form_Rental(v.Id, v.vehicleDescription, v.Odometer);
                form_Rental.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// ON BTN RENT CLICK OPEN THE RENTAL LIST FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiRents_Click(object sender, RoutedEventArgs e)
        {
            Form_RentalList form_RentalList = new Form_RentalList();
            form_RentalList.ShowDialog();
            UpdateList();
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
        /// <summary>
        /// ON THE BTN CANCEL CLICK CLOSE THE FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

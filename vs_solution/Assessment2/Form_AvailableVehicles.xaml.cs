using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assessment2
{
    /// <summary>
    /// Interaction logic for Form_AvailableVehicles.xaml
    /// </summary>
    public partial class Form_AvailableVehicles 
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Form_AvailableVehicles()
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
        /// LIST VIEW DOUBLE CLICK EVENT - OPEN THE RENTAL PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvRentVehicleList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

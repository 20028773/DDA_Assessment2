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
        public Form_AvailableVehicles()
        {
            InitializeComponent();
            UpdateList();
        }

        public void UpdateList()
        {
            lvRentVehicleList.ItemsSource = Rental.GetAvailableVehicles(txtFilter.Text);
            lvRentVehicleList.Items.Refresh();
        }

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

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }
    }
}

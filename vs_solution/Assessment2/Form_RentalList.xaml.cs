using System.Windows;
using System.Windows.Input;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR FORM_RENTALLIST.XAML
    /// </summary>
    public partial class Form_RentalList : Window
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Form_RentalList()
        {
            InitializeComponent();
            UpdateList();
        }
        /// <summary>
        /// ON THE LIST VIEW DOUBLE CLICK OPEN THE FORM TO FINALIZE THE SELECTED RENTAL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvRentalList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvRentalList.SelectedItem != null)
            {
                Form_Rental form_Rental = new Form_Rental((Rental)lvRentalList.SelectedItem);
                form_Rental.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// UPDATE THE LIST VIEW ACCORDING TO THE FILTER
        /// </summary>
        public void UpdateList()
        {
            lvRentalList.ItemsSource = Rental.getRentalList(txtFilter.Text, cbFinalized.IsChecked.Value);
            lvRentalList.Items.Refresh();
        }
        /// <summary>
        /// ON THE TXT FILTER CHANGE UPDATE THE LISTVIEW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateList();
        }
        /// <summary>
        /// ON THE CHECK BOX FINALIZED CHECK EVENT UPDATE THE LISTVIEW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbFinalized_Checked(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
        /// <summary>
        /// ON THE CHECK BOX FINALIZED UNCHECK EVENT UPDATE THE LISTVIEW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbFinalized_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
    }
}

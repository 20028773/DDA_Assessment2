using System.Windows;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR FORM_FUELPURCHASE.XAML
    /// </summary>
    public partial class Form_FuelPurchase : Window
    {
        Vehicle vehicle = new Vehicle();
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="v"></param>
        public Form_FuelPurchase(Vehicle v)
        {
            InitializeComponent();

            txtVehicle.Text = v.vehicleDescription;
            txtOdometer.Text = v.OdometerReading.ToString();
            vehicle = v;
        }
        /// <summary>
        /// ON THE BTN SAVE CLICK CREATES A NEW FUEL PURCHASE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string sMessage = FuelPurchase.AddPurchaseFuel(vehicle, double.Parse(txtOdometer.Text), double.Parse(txtQuantity.Text), double.Parse(txtPrice.Text));

            if (string.IsNullOrEmpty(sMessage))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(sMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// ON THE BTN CANCEL CLICK CLOSE THE FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

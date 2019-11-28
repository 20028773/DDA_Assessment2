using System;
using System.Windows;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR FORM_RENTAL.XAML
    /// </summary>
    public partial class Form_Rental 
    {
        private ulong vehicleId;
        private ulong rentalId;
        private decimal dEndOdometer;
        private DateTime? dtEndDate = null;
        /// <summary>
        /// INITLIALIZE THE FORM COMPONENTS AND ADD THE RENT TYPE TO THE COMBO BOX
        /// </summary>
        public void initialize()
        {
            InitializeComponent();
            cbRentType.Items.Add(Rental.type.Day);
            cbRentType.Items.Add(Rental.type.KM);

            this.Title = "New Rental";
        }
       
        /// <summary>
        /// CONSTRUCTOR - CALLED TO FINALIZE AN ONGOING RENTAL
        /// </summary>
        /// <param name="rental"></param>
        public Form_Rental(Rental rental)
        {
            initialize();
            this.Title = "Finalize Rental";

            rentalId = rental.Id;
            txtCustomer.Text = rental.customerName;
            txtVehicle.Text = rental.vehicleDescription;
            txtStartOdometer.Text = rental.startOdometer.ToString();
            dpStartDate.Text = rental.startDate.ToString();
            dpEndDate.Text = rental.endDate.ToString();
            txtNotes.Text = rental.notes;
            cbRentType.Text = rental.rentType.ToString();

            btnSave.Content = "Finalize Booking";
            btnSave.Width = 120;
            btnSave.Margin = new Thickness(50, 0, 0, 0);

            dpStartDate.IsEnabled = false;
            txtEndOdometer.IsEnabled = true;
            dpEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// CONSTRUCTOR - CALLED TO CREATE A NEW RENTAL
        /// </summary>
        public Form_Rental(ulong vId, string vDescription, decimal odometer)
        {
            initialize();

            vehicleId = vId;
            txtVehicle.Text = vDescription;
            txtStartOdometer.Text = odometer.ToString();

            dpStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// VALIDATE THE FORM FIELDS AND RETURN A ERROR MESSAGE IF NEEDED
        /// </summary>
        /// <returns></returns>
        public string validate()
        {
            if (string.IsNullOrEmpty(txtCustomer.Text))
            {
                return "Customer Required";
            }

            if (cbRentType.SelectedItem == null)
            {
                return "Rent Type Required";
            }

            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                DateTime startDate = DateTime.Parse(dpStartDate.Text);
                dtEndDate = DateTime.Parse(dpEndDate.Text);

                if (startDate > dtEndDate)
                {
                    return "Start Date can't be after End Date";
                }
            }

            if (txtEndOdometer.IsEnabled)
            {
                try
                {
                    dEndOdometer = decimal.Parse(txtEndOdometer.Text.Replace("_", ""));
                }
                catch
                {
                    return "Invalid End Odometer";
                }
            }

            return "";
        }
        /// <summary>
        /// ON THE BTN SAVE CLICK EITHER CREATES A NEW RENTAL OR FINALIZE THE RENTAL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string sMessage = validate();

            if (!string.IsNullOrEmpty(sMessage))
            {
                MessageBox.Show(sMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Rental.type rentaltype = cbRentType.SelectedItem.ToString() == Rental.type.Day.ToString() ? Rental.type.Day : Rental.type.KM;

            if (((System.Windows.Controls.ContentControl)sender).Content.ToString() == "Save")
            {
                Rental.AddRental(vehicleId,
                                    txtCustomer.Text,
                                    rentaltype,
                                    decimal.Parse(txtStartOdometer.Text),
                                    DateTime.Parse(dpStartDate.Text),
                                    dtEndDate,
                                    txtNotes.Text);
            }
            else
            {
                sMessage = Rental.FinalizeRental(rentalId, dEndOdometer, dtEndDate.Value, txtNotes.Text);
            }

            if (!string.IsNullOrEmpty(sMessage))
            {
                MessageBox.Show(sMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Close();
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

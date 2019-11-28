using System.Windows;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR VEHICLE.XAML
    /// </summary>
    public partial class Form_Vehicle 
    {
        private ulong id = 0;
        private string sManufacturer;
        private string sModel;
        private int nYear;
        private string sRegistration;
        private decimal dOdometer;
        private decimal dTank;
        /// <summary>
        /// CONSTRUCTOR - CALLED TO CREATE A NEW VEHICLE
        /// </summary>
        public Form_Vehicle()
        {
            InitializeComponent();

            txtManufacturer.Focus();
            this.Title = "Add Vehicle";
        }
        /// <summary>
        /// CONSTRUCTOR - CALLED TO UPDATE THE VEHICLE INFORMATION
        /// </summary>
        /// <param name="updateVehicle"></param>
        public Form_Vehicle(Vehicle updateVehicle)
        {
            InitializeComponent();

            this.Title = "Edit Vehicle";

            txtManufacturer.Focus();

            id = updateVehicle.Id;

            txtManufacturer.Text = updateVehicle.Manufacturer;
            txtModel.Text = updateVehicle.Model;
            txtYear.Text = updateVehicle.Year.ToString();
            txtRegistration.Text = updateVehicle.Registration;
            txtOdometer.Text = updateVehicle.Odometer.ToString();
            txtTank.Text = updateVehicle.Tank.ToString();
        }
        /// <summary>
        /// VALIDATE THE FORM FIELDS AND RETURN A ERROR MESSAGE IF NEEDED
        /// </summary>
        /// <returns></returns>
        public string validate()
        {
            sManufacturer = txtManufacturer.Text.Replace("_", "");

            if (string.IsNullOrEmpty(sManufacturer))
            {
                return "Manufacturer Required";
            }

            sModel = txtModel.Text.Replace("_", "");

            if (string.IsNullOrEmpty(sModel))
            {
                return "Model Required";
            }

            try
            {
                nYear = int.Parse(txtYear.Text.Replace("_", ""));

                if (nYear < 1900 || nYear > 2030)
                {
                    return "Invalid Year";
                }
            }
            catch
            {
                return "Invalid Year";
            }

            sRegistration = txtRegistration.Text.Replace("_", "");

            if (string.IsNullOrEmpty(sRegistration))
            {
                return "Registration Required";
            }

            try
            {
                dOdometer = decimal.Parse(txtOdometer.Text.Replace("_", ""));

                if (dOdometer < 0)
                {
                    return "Invalid Odometer";
                }
            }
            catch
            {
                return "Invalid Odometer";
            }

            try
            {
                dTank = decimal.Parse(txtTank.Text.Replace("_", ""));

                if (dTank < 0)
                {
                    return "Invalid Tank Capacity";
                }
            }
            catch
            {
                return "Invalid Tank Capacity";
            }

            return "";
        }
        /// <summary>
        /// ON THE BTN SAVE CLICK EITHER CREATES A NEW VEHICLE OR UPDATE THE VEHICLE INFORMATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string sMessage = validate();

            if (string.IsNullOrEmpty(sMessage))
            {
                if (id != 0)
                {
                    Vehicle.EditVehicle(id, sManufacturer, sModel, nYear, sRegistration, dOdometer, dTank);
                }
                else
                {
                    Vehicle.AddVehicle(sManufacturer, sModel, nYear, sRegistration, dOdometer, dTank);
                    //if( !Vehicle.AddVehicle(sManufacturer, sModel, nYear, sRegistration, dOdometer, dTank))
                    //{
                    //    return;
                    //}
                }

                MessageBox.Show("Vehicle " + (id != 0 ? "Edited" : "Created") + " Successfully!", "Vehicle", MessageBoxButton.OK, MessageBoxImage.Information);

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

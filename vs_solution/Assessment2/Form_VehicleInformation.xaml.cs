using System.Windows;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR FORM_VEHICLEINFORMATION.XAML
    /// </summary>
    public partial class Form_VehicleInformation : Window
    {
        /// <summary>
        /// CONSTRUCTOR - CALL THE VEHICLE PRINT DETAIL METHOD
        /// </summary>
        /// <param name="v"></param>
        public Form_VehicleInformation(Vehicle v)
        {
            InitializeComponent();

            lblInformation.Content = Vehicle.printDetails(v);
        }
    }
}

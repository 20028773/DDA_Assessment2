using System.Diagnostics;
using System.Windows;

namespace Assessment2
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About 
    {
        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

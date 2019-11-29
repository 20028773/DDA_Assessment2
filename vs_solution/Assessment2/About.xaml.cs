using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Assessment2
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public About()
        {
            InitializeComponent();

            Uri img = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\profile_picture.png", UriKind.RelativeOrAbsolute);
            imgAuthor.Source = new BitmapImage(img);
        }
        /// <summary>
        /// METHOD TO OPEN THE LINK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

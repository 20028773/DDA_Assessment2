using System;
using System.Windows;
using System.Windows.Controls;

namespace Assessment2
{
    /// <summary>
    /// Interaction logic for InformationCard.xaml
    /// </summary>
    public partial class InformationCard : UserControl
    {
        public InformationCard()
        {
            InitializeComponent();
            CardGrid.DataContext = this;
        }

        #region BackgroundColour DP

        public string BackgroundColour
        {
            set { SetValue(ColourBG, value); }
        }

        public static readonly DependencyProperty ColourBG =
            DependencyProperty.Register(
                "BackgroundColour",
                typeof(string),
                typeof(InformationCard),
                new PropertyMetadata(""));

        #endregion

        #region BorderColour DP

        public string BorderColour
        {
            set { SetValue(ColourB, value); }
        }

        public static readonly DependencyProperty ColourB =
            DependencyProperty.Register(
                "BorderColour",
                typeof(string),
                typeof(InformationCard),
                new PropertyMetadata(""));

        #endregion

        #region BorderWidth DP

        public string BorderWidth
        {
            set { SetValue(WidthB, value); }
        }

        public static readonly DependencyProperty WidthB =
            DependencyProperty.Register(
                "BorderWidth",
                typeof(string),
                typeof(InformationCard),
                new PropertyMetadata(""));

        #endregion

        #region Title DP

        public string Title
        {
            get { return (String)GetValue(titleDP); }
            set { SetValue(titleDP, value); }
        }

        public static readonly DependencyProperty titleDP =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(InformationCard),
                new PropertyMetadata(""));

        #endregion

        #region lblValue DP

        public string ValueProperty
        {
            set { SetValue(valueDP, value); }
        }

        public static readonly DependencyProperty valueDP =
            DependencyProperty.Register(
                "ValueProperty",
                typeof(string),
                typeof(InformationCard),
                new PropertyMetadata(""));

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Movements
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNewMovement : Page
    {
        public AddNewMovement()
        {
            this.InitializeComponent();
        }
        //private bool isFirstHalfSelected = true;

        //private void CustomButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (isFirstHalfSelected)
        //    {
        //        // Schimbați culoarea primei secțiuni la apăsare
        //        firstHalf.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
        //        secondHalf.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
        //    }
        //    else
        //    {
        //        // Schimbați culoarea celei de-a doua secțiuni la apăsare
        //        firstHalf.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
        //        secondHalf.Background = new SolidColorBrush(Windows.UI.Colors.Green);
        //    }

        //    // Inversați starea
        //    isFirstHalfSelected = !isFirstHalfSelected;
        //}
    }
}

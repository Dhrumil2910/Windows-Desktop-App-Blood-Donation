using App15.Helpers;
using App15.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App15.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddConatct : Page
    {
        public AddConatct()
        {
            this.InitializeComponent();
        }

        private async void AddContact_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
            if (NametxtBx.Text != "" & PhonetxtBx.Text != "")
            {
                Db_Helper.Insert(new Contacts(NametxtBx.Text, PhonetxtBx.Text));
                Frame.Navigate(typeof(ReadContactList));//after add contact redirect to contact listbox page 
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Please fill two fields");//Text should not be empty 
                await messageDialog.ShowAsync();
            }
        }

    }
}

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
using App15.Helpers;
using App15.Models;
using Windows.UI.Popups;
using Bing.Maps.Search;
using Bing.Maps;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App15.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignIn : Page
    {
        public SignIn()
        {
            this.InitializeComponent();
        }

        private string lat;
        private string lon;

        //private async void AddUser_Click(object sender, RoutedEventArgs e)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
        //    if (NametxtBx.Text != "" & PhonetxtBx.Text != "")
        //    {
        //        Db_Helper.Insert(new Contacts(NametxtBx.Text, PhonetxtBx.Text));
        //        Frame.Navigate(typeof(ReadContactList));//after add contact redirect to contact listbox page 
        //    }
        //    else
        //    {
        //        MessageDialog messageDialog = new MessageDialog("Please fill all the fields");//Text should not be empty 
        //        await messageDialog.ShowAsync();
        //    }
        //}



        public void addNewUser(object sender, RoutedEventArgs e)
        {
            int validate = 0;
            Regex email = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

            if (!Regex.IsMatch(rEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                rMessage.Text = "Invalid Email";
                validate = 1;
            }
            if (rPassword1.Password.Length <= 6 & validate != 1)
            {
                rMessage.Text = "Password Length less than 6 characters";
                validate = 1;
            }
            if (rPassword1.Password != rPassword2.Password & validate != 1)
            {
                rMessage.Text = "Passwords do not match";
                validate = 1;
            }

            if (validate == 0)
            {
                string rLat = lat, rLon = lon;

                AccountsDatabaseHelperClass Db_Helper = new AccountsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
                if (rName.Text != "" & rBlood.SelectedItem.ToString() != "" & rEmail.Text != "" & rPassword1.Password != "" & rContact.Text != "" & rContact.Text != "" & rLocation.Text != "")
                {
                    if (rPassword1.Password != rPassword2.Password)
                    {
                        rMessage.Text = "Passwords Do Not Match!";
                        return;
                    }
                    Accounts newUser = new Accounts(rName.Text, rContact.Text, rBlood.SelectedItem.ToString(), rEmail.Text, rPassword1.Password, rLat, rLon, rLocation.Text);
                    Db_Helper.Insert(newUser);
                    Frame.Navigate(typeof(MainMap), newUser);//after add contact redirect to contact listbox page 
                    //header.Text = rName.Text + rBlood.SelectedItem.ToString() + rEmail.Text + rContact.Text + rPassword1.Password + rLocation.Text;

                    rMessage.Text = "Yaay! did not crash";
                }
                else
                {
                    rMessage.Text = "Some Fields are Empty!";
                }

            }

        }



        private void logInUser(object sender, RoutedEventArgs e)
        {
            try
            {
                Accounts loggedUser = new Accounts();
                //ObservableCollection<Accounts> DB_UserList = new ObservableCollection<Accounts>();
                AccountsDatabaseHelperClass Db_Helper = new AccountsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
                if (lEmail.Text != "" & lPassword.Password != "")
                {
                    loggedUser = Db_Helper.LoginUser(lEmail.Text, lPassword.Password);
                    rMessage.Text = "Welcome" + loggedUser.Name;
                    Frame.Navigate(typeof(MainMap), loggedUser);//after add contact redirect to contact listbox page 
                }
                else
                {
                    rMessage.Text = "Some Fields are Empty!";
                }
            }
            catch (Exception ex)
            {
                rMessage.Text = "User Does Not Exist!";
                //Debug.WriteLine(ex.ToString());
            }
        }

        private void locatePosition(object sender, RoutedEventArgs e)
        {
            if (rLocation.Text != null)
            {
                performSearch(rLocation.Text);
            }


        }
        public async void performSearch(string address)
        {
            try
            {
                // Set the address string to geocode
                Bing.Maps.Search.GeocodeRequestOptions requestOptions = new Bing.Maps.Search.GeocodeRequestOptions(address);
                // Make the geocode request 
                Bing.Maps.Search.SearchManager searchManager = myMap.SearchManager;
                Bing.Maps.Search.LocationDataResponse response = await searchManager.GeocodeAsync(requestOptions);
                myMap.SetView(response.LocationData[0].Location, 10.0f);
                int i = 1;

                foreach (GeocodeLocation l in response.LocationData)
                {
                    //Get the location of each result
                    Bing.Maps.Location location = l.Location;


                    //await dialog.ShowAsync();
                    Pushpin pin = new Pushpin()
                    {
                        Tag = "X",
                        Text = l.Location.Latitude.ToString()
                    };

                    lat = l.Location.Latitude.ToString();
                    lon = l.Location.Longitude.ToString();
                    i++;
                    MapLayer.SetPosition(pin, location);

                    myMap.Children.Add(pin);
                }
            }

            catch (Exception e)
            {
                rMessage.Text = "Couldnot find location.. Please refine your location string";
            }

        }




    }
}

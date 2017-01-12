using Bing.Maps;
using Bing.Maps.Search;
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
using App15.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App15.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditUser2 : Page
    {

        Accounts loggedUser = new Accounts();

        public EditUser2()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loggedUser = e.Parameter as Accounts;
            fillTextBoxes();
        }

        private void fillTextBoxes()
        {
            uName.Text = loggedUser.Name;
            uBlood.SelectedItem = loggedUser.BloodGroup;
            uEmail.Text = loggedUser.Email;
            uContact.Text = loggedUser.ContactNumber;
            uLocation.Text = loggedUser.Address;
        }

        private void SearchPlace(object sender, RoutedEventArgs e)
        {
            if (uLocation.Text != null)
            {
                geoLocation(uLocation.Text);
            }
        }

        //Update User
        private void updateUser(object sender, RoutedEventArgs e)
        {
            AccountsDatabaseHelperClass Db_Helper = new AccountsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
            try
            {
                if (uName.Text != "" & uBlood.SelectedItem.ToString() != "" & uEmail.Text != "" & uContact.Text != "" & uLocation.Text != "")
                {
                    loggedUser.Name = uName.Text;
                    loggedUser.BloodGroup = uBlood.SelectedItem.ToString();
                    loggedUser.Email = uEmail.Text;
                    loggedUser.ContactNumber = uContact.Text;
                    loggedUser.Address = uLocation.Text;

                    Db_Helper.UpdateAccount(loggedUser);
                    Frame.Navigate(typeof(MainMap), loggedUser);//after add contact redirect to contact listbox page 
                    //header.Text = rName.Text + rBlood.SelectedItem.ToString() + rEmail.Text + rContact.Text + rPassword1.Password + rLocation.Text;
                }
                else
                {
                    errorBox.Text = "Some Fields are Empty!";
                }
            }
            catch (Exception)
            {
                errorBox.Text = "Could Not Update. Something Went Wrong!";
            }
        }

        //Delete Current User
        private void deleteUser(object sender, RoutedEventArgs e)
        {
            AccountsDatabaseHelperClass Db_Helper = new AccountsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
            try
            {
                Db_Helper.DeleteAccount(loggedUser.Id);
                loggedUser = null;
                Frame.Navigate(typeof(SignIn));
            }
            catch (Exception)
            {
                errorBox.Text = "Could Not Delete. Something Went Wrong!";
            }
        }

        public async void geoLocation(string address)
        {
            try
            {
                // Set the address string to geocode
                Bing.Maps.Search.GeocodeRequestOptions requestOptions = new Bing.Maps.Search.GeocodeRequestOptions(address);

                // Make the geocode request 
                Bing.Maps.Search.SearchManager searchManager = myMap2.SearchManager;
                Bing.Maps.Search.LocationDataResponse response = await searchManager.GeocodeAsync(requestOptions);
                int i = 1;
                foreach (GeocodeLocation l in response.LocationData)
                {
                    //Get the location of each result
                    Bing.Maps.Location location = l.Location;

                    //await dialog.ShowAsync();
                    Pushpin pin = new Pushpin()
                    {
                        Tag = l.Name,
                        Text = l.Location.Latitude.ToString()
                    };

                    i++;
                    MapLayer.SetPosition(pin, location);

                    myMap2.Children.Add(pin);
                    loggedUser.Latitude = l.Location.Latitude.ToString();
                    loggedUser.Longitude = l.Location.Longitude.ToString();
                }
                myMap2.SetView(response.LocationData[0].Bounds);
            }
            catch (Exception e)
            {
                errorBox.Text = "Error : " + e.ToString();
            }
        }

        private void gotoMapPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainMap), loggedUser);//after add contact redirect to contact listbox page 
        }

    }
}

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
using Bing.Maps;
using Windows.UI.Popups;
using Windows.UI;
using Bing.Maps.Search;
using Windows.Devices.Geolocation;
using App15.Models;
using App15.Helpers;
using App15.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App15
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMap : Page
    {
        Double Lat;
        Double Lon;
        Accounts loggedUser = new Accounts();
        List<Pushpin> PushpinList = new List<Pushpin>();

        public MainMap()
        {

            this.InitializeComponent();
            //addUserPushpin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loggedUser = e.Parameter as Accounts;
            lUserName.Text = "Hi, " + loggedUser.Name;
            xlUserName.Text = "Hi, " + loggedUser.Name;
            Lat = Double.Parse(loggedUser.Latitude);
            Lon = Double.Parse(loggedUser.Longitude);
            addUserPushpin();
            createRequestView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private ObservableCollection<Accounts> getUsers()
        {
            ObservableCollection<Accounts> DB_UserList = new ObservableCollection<Accounts>();
            try
            {
                AccountsDatabaseHelperClass Db_Helper = new AccountsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
                DB_UserList = Db_Helper.ReadAccounts();
                return DB_UserList;

            }
            catch (Exception ex)
            {
                Error.Text = "Error : " + ex.ToString();
                
            }
            return DB_UserList;
        }

        private ObservableCollection<Requests> getRequests()
        {
            ObservableCollection<Requests> DB_RequestList = new ObservableCollection<Requests>();
            try
            {
                RequestsDatabaseHelperClass Db_Helper = new RequestsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
                DB_RequestList = Db_Helper.ReadRequests();
                return DB_RequestList;

            }
            catch (Exception ex)
            {
                Error.Text = "Error : " + ex.ToString();

            }
            return DB_RequestList;
        }

        private void createRequestView()
        {
            int i=0;
            int j=0;
            ObservableCollection<Requests> DB_RequestList = getRequests();
            ObservableCollection<Accounts> DB_UserList = getUsers();
            for (i = 0; i < DB_RequestList.Count; i++)
            {
                string id = DB_RequestList[i].IdDonor.ToString();
                for (j = 0; j < DB_UserList.Count; j++)
                {
                    if (id == DB_UserList[j].Id.ToString() & id != loggedUser.Id.ToString())
                    {
                        Accounts user = DB_UserList[j];
                        // Create a StackPanel and Add children
                        StackPanel myStackPanel = new StackPanel();
                        Border myBorder1 = new Border();
                        myBorder1.BorderThickness = new Thickness(1);
                        Border myBorder2 = new Border();
                        myBorder2.BorderThickness = new Thickness(1);
                        Border myBorder3 = new Border();
                        myBorder3.BorderThickness = new Thickness(1);
                        Border myBorder4 = new Border();
                        myBorder4.BorderThickness = new Thickness(1);
                        //Blood Group
                        TextBlock bloodGrp = new TextBlock();
                        bloodGrp.FontSize = 30;
                        bloodGrp.Text = user.BloodGroup;
                        myBorder1.Child = bloodGrp;

                        //Name
                        TextBlock name = new TextBlock();
                        name.FontSize = 20;
                        name.Text = user.Name;
                        myBorder2.Child = name;

                        //Address
                        TextBlock address = new TextBlock();
                        address.FontSize = 20;
                        address.Text = user.Address;
                        myBorder3.Child = address;

                        //Contact
                        TextBlock contact = new TextBlock();
                        contact.FontSize = 30;
                        contact.Text = user.ContactNumber;
                        myBorder4.Child = contact;

                        // Add the Borders to the StackPanel Children Collection
                        myStackPanel.Children.Add(myBorder1);
                        myStackPanel.Children.Add(myBorder2);
                        myStackPanel.Children.Add(myBorder3);
                        myStackPanel.Children.Add(myBorder4);

                        RequestsContainer.Children.Add(myStackPanel);
                    }
                }
            }
        }
        //private void placePushpins()
        //{
        //    ObservableCollection<Accounts> DB_UserList = getUsers();

        //    for (int i=0; i<DB_UserList.Count; i++)
        //    {
        //        Debug.WriteLine(DB_UserList[i].Latitude);
        //    }
        //}

        private void Search(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CityName.Text != null)
                {
                    searchCity(CityName.Text);
                }
                if (Radius.SelectedItem.ToString() != null)
                {
                    setZoomLevel(Radius.SelectedItem.ToString());
                }
                FilterPushpins();
            }
            catch (Exception exSearch)
            {
                Error.Text = "Error : " + e.ToString();
            }

        }
        private void addUserPushpin()
        {
            ObservableCollection<Accounts> DB_UserList = getUsers();
            int i = 0;
            try
            {

                for (i = 0; i < DB_UserList.Count; i++)
                {
                    Pushpin pushpin = new Pushpin()
                    {
                        Tag = new Metadata()
                        {
                            DonarName = DB_UserList[i].Name,
                            PhoneNo = DB_UserList[i].ContactNumber,
                            BloodGroup = DB_UserList[i].BloodGroup,
                            Address = DB_UserList[i].Address
                        },
                        Text = DB_UserList[i].BloodGroup
                    };
                    //pushpin.Template = (ControlTemplate)(Application.Current.Resources["PushPinTemplate"]) ;
                    pushpin.Background = new SolidColorBrush(Colors.Red);
                    pushpin.BorderBrush = new SolidColorBrush(Colors.Black);

                    Double lat = Convert.ToDouble(DB_UserList[i].Latitude);
                    Double lon = Convert.ToDouble(DB_UserList[i].Longitude);
                    MapLayer.SetPosition(pushpin, new Location(lat, lon));
                    myMap.Children.Add(pushpin);
                    PushpinList.Add(pushpin);
                    pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                }
                Location location = new Location(Lat, Lon);
                myMap.Center = new Location(Lat, Lon);
                myMap.ZoomLevel = 10;

            }
            catch (Exception e)
            {
                Error.Text = i.ToString() + e;
                Debug.WriteLine(e);
            }
        }

        private async void pushpinTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Pushpin p = sender as Pushpin;
            Metadata m = (Metadata)p.Tag;
            if (!String.IsNullOrEmpty(m.DonarName) || !String.IsNullOrEmpty(m.BloodGroup) || !String.IsNullOrEmpty(m.PhoneNo) || !String.IsNullOrEmpty(m.Address))
            {
                Infobox.DataContext = m;

                Infobox.Visibility = Visibility.Visible;

                MapLayer.SetPosition(Infobox, MapLayer.GetPosition(p));
            }
            else
            {
                Infobox.Visibility = Visibility.Collapsed;
            }

        }

        private void CloseInfobox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Infobox.Visibility = Visibility.Collapsed;
        }

        private void AllVisible()
        {
            int i = 0;
            for (i = 0; i < PushpinList.Count; i++)
            {
                PushpinList[i].Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void FilterPushpins()
        {
            try { 
                string selected = FilterBlood.SelectedItem.ToString();
                if (selected == "All")
                {
                    AllVisible();
                    return;
                }
                    
                //Pushpin p = sender as Pushpin;
                //Metadata m = (Metadata)p.Tag;
                //if (!String.IsNullOrEmpty(m.DonarName)

                int i = 0;
                for (i = 0; i < PushpinList.Count; i++)
                {
                    Metadata m = (Metadata)PushpinList[i].Tag;
                    if (m.BloodGroup != selected)
                    {
                        PushpinList[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async void searchCity(string address)
        {
            try
            {
                // Set the address string to geocode
                Bing.Maps.Search.GeocodeRequestOptions requestOptions = new Bing.Maps.Search.GeocodeRequestOptions(address);
                // Make the geocode request 
                Bing.Maps.Search.SearchManager searchManager = myMap.SearchManager;
                Bing.Maps.Search.LocationDataResponse response = await searchManager.GeocodeAsync(requestOptions);
                myMap.SetView(response.LocationData[0].Bounds);
            }
            catch (Exception e)
            {
                Error.Text = "Error : " + e.ToString();
            }
        }
        private void setZoomLevel(string radius)
        {
            try
            {
                if (radius == "5")
                {
                    myMap.ZoomLevel = 12;
                }
                else if (radius == "10")
                {
                    myMap.ZoomLevel = 11;
                }
                else if (radius == "15")
                {
                    myMap.ZoomLevel = 10;
                }
            }
            catch (Exception e)
            {
                Error.Text = "Error: " + e.ToString();
            }
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            loggedUser = null;
            Frame.Navigate(typeof(SignIn));//after add contact redirect to contact listbox page 
        }

        private void gotoEditProfile(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditUser2), loggedUser);//after add contact redirect to contact listbox page 
        }

        private void showBloodRequests(object sender, RoutedEventArgs e)
        {
            AllVisible();
            requestsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            filterGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            requestIcon.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            filterIcon.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void showFilterDisplay(object sender, RoutedEventArgs e)
        {
            FilterBlood.SelectedItem = "All";
            requestsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            filterGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            requestIcon.Visibility = Windows.UI.Xaml.Visibility.Visible;
            filterIcon.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AddBloodRequest(object sender, RoutedEventArgs e)
        {

            RequestsDatabaseHelperClass Db_Helper = new RequestsDatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs 
            if (RequestBlood.SelectedItem.ToString() != "")
            {

                Db_Helper.Insert(new Requests(loggedUser.Id, RequestBlood.SelectedItem.ToString()));
                //Frame.Navigate(typeof(ReadContactList));//after add contact redirect to contact listbox page 
                //header.Text = rName.Text + rBlood.SelectedItem.ToString() + rEmail.Text + rContact.Text + rPassword1.Password + rLocation.Text;
                Error.Text = "Record Successfully Added";
            }
            else
            {
                
            }
        }
    }
    public class Metadata
    {
        public string DonarName { get; set; }
        public string PhoneNo { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }

    }
}

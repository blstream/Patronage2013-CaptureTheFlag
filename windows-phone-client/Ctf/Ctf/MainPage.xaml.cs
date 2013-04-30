using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ctf.Resources;
using System.Diagnostics;


namespace Ctf
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

          


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            ApplicationSettings.Instance.UserChanged += UserHasChanged;
        }

        public void UserHasChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("AppSett EVENT!. User has changed.");
        }

        //private async void LoginTest(object sender, RoutedEventArgs e)
        //{
        //    Login Logger = new Login();
        //    Debug.WriteLine(Logger.LoggedAs());

        //    //await Logger.logInAs(new UserCredentials("piotrekm44@o2.pl", "weakPassword"), "secret");
        //    await Logger.LogInAs(new UserCredentials("abcdef", "abcdef"), "secret");
        //    //TODO: NullPointerException
        //    //if(Logger.loggedAs().username != null)
        //    //    Debug.WriteLine(Logger.loggedAs().username);
        //}

        //private async void RegisterTest(object sender, RoutedEventArgs e)
        //{
        //    Registration Register = new Registration();
        //    await Register.Register(new UserCredentials("abcdef", "abcdef"), "abcdef");
        //}

        public void SomeMessage(object sender, EventArgs e)
        {
            Debug.WriteLine("MESSAGE.");
        }

        private async void LogIn(object sender, RoutedEventArgs e)
        {
            Login Logger = new Login();
            // Debug.WriteLine(Logger.LoggedAs());
            if (UserCredentials.IsUsernameProperLength(usernameBox.Text) && UserCredentials.IsPasswordProperLength(passwordBox.Password))
            {
                await Logger.LogInAs(new UserCredentials(usernameBox.Text, passwordBox.Password), "secret");
                NavigationService.Navigate(new Uri("/LoggedIn.xaml?text=" + usernameBox.Text, UriKind.Relative));
                

            }

            else
            {
                MessageBox.Show("Nie wprowadzono nazwy lub hasła", "Brak danych", MessageBoxButton.OK);
            }
        }


        private void LogoutTest(object sender, RoutedEventArgs e)
        {
            Login Logger = new Login();
            if (Logger.LogOut())
                Debug.WriteLine("Logout SUCCESSFUL");
            else
                Debug.WriteLine("Logout FAILED");
        }

       

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void MakeTestUserCredentials()
        {
            UserCredentials user = null;
            string username = "aaaaaaaa";
            string password = "bbbbb";
            try
            {
                user = new UserCredentials(username, password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
            if (user != null)
            {
                Debug.WriteLine("username: " + user.GetUsername());
                Debug.WriteLine("password: " + user.GetPassword());
            }
            if (user.HasMatchingPassword(password))
                Debug.WriteLine("match: " + user.GetPassword() + " == " + password);
            if (user.HasMatchingPassword(username))
                Debug.WriteLine("match: " + user.GetPassword() + " == " + username);
        }
    }
}
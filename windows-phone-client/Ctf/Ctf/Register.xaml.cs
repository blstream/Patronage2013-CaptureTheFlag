using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Ctf
{
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Registration Register = new Registration();
            await Register.Register(new UserCredentials(userNameRegister.Text, passwordRegister1.Text), passwordRegister2.Text);
            // await Register.Register(new UserCredentials("qazwsx", "qazwsx"), "qazwsx");
            NavigationService.GoBack();
            
        }

    }
}
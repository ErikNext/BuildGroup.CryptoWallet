using System;
using System.Windows;
using System.Windows.Navigation;
using BuildGroup.CryptoWallet.Frontend.Wpf.Api;
using BuildGroup.CryptoWallet.Frontend.Wpf.Pages;

namespace BuildGroup.CryptoWallet.Frontend.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
    }
}

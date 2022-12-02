using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuildGroup.CryptoWallet.Frontend.Wpf.Api;

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Pages
{
    public partial class ClientsPage : Page
    {
        private readonly UserProcessor _userProcessor = new();
        private readonly TransactionProcessor _transactionProcessor = new();

        public ClientsPage()
        {
            InitializeComponent();
        }

        private async void ButtonCreate_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxAddUsername.Text == String.Empty 
                || textBoxAddBalance.Text == String.Empty 
                || comboBoxCurrency.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);

            }

            else if (Decimal.TryParse(textBoxAddBalance.Text, out var balance))
            {
                var result = await _userProcessor.Create(new(textBoxAddUsername.Text, balance, comboBoxCurrency.SelectedValue.ToString()));

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    textboxAddId.Text = result.Value.Id;
                    stackPanelCreatedUserId.Visibility = Visibility.Visible;
                }

                else 
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
                MessageBox.Show("Некорректный баланс", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void ButtonFind_OnClick(object sender, RoutedEventArgs e)
        {
            if (textboxFindId.Text == String.Empty)
            {
                MessageBox.Show("Поле обязательно к заполнению!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);

            }
            else
            {
                var result = await _userProcessor.Get(textboxFindId.Text);
                if (result.Success)
                {
                    var userModel = result.Value;

                    textboxGetUsername.Text = userModel.Username;
                    textboxGetBalance.Text = userModel.Balance.ToString();
                    textboxGetCurrency.Text = userModel.CurrencyType;
                }
                else
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxDeleteId.Text == String.Empty)
            {
                MessageBox.Show("Поле обязательно к заполнению!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);

            }
            else
            {
                var result = await _userProcessor.Delete(textBoxDeleteId.Text);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCurrencyTypes();
        }

        private async Task LoadCurrencyTypes()
        {
            var result = await _transactionProcessor.GetCurrencyTypes();

            if (result.Success)
            {
                foreach (var type in result.Value)
                {
                    comboBoxCurrency.Items.Add(type);
                    comboBoxUpdateCurrency.Items.Add(type);
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ButtonLoadAllClients_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await _userProcessor.Search();

            if (result.Success)
            {
                dataGridUsers.ItemsSource = result.Value;
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ButtonUpdateFind_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxUpdateId.Text == String.Empty)
            {
                MessageBox.Show("Поле обязательно к заполнению!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else
            {
                var result = await _userProcessor.Get(textBoxUpdateId.Text);
                if (result.Success)
                {
                    var userModel = result.Value;

                    textboxUpdateUsername.Text = userModel.Username;
                    textboxUpdateBalance.Text = userModel.Balance.ToString();

                    foreach (var item in comboBoxUpdateCurrency.Items)
                    {
                        if (item.ToString() == userModel.CurrencyType)
                            comboBoxUpdateCurrency.SelectedItem = item;
                    }
                }
                else
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxUpdateId.Text == String.Empty
                || textboxUpdateBalance.Text == String.Empty
                || textboxUpdateUsername.Text == String.Empty
                || comboBoxUpdateCurrency.SelectionBoxItem == null)
            {
                MessageBox.Show("Все поля обязательны к заполнению!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else if (Decimal.TryParse(textboxUpdateBalance.Text, out var balance))
            {
                var result = await _userProcessor.Update(
                    textBoxUpdateId.Text,
                    new(
                        textboxUpdateUsername.Text, 
                        balance,
                        comboBoxUpdateCurrency.SelectedValue.ToString()));

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
                MessageBox.Show("Некорректный баланс", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

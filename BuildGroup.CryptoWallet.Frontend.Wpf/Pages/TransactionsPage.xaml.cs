using BuildGroup.CryptoWallet.Frontend.Wpf.Api;
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

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для TransactionsPage.xaml
    /// </summary>
    public partial class TransactionsPage : Page
    {
        private readonly UserProcessor _userProcessor = new();
        private readonly TransactionProcessor _transactionProcessor = new();

        public TransactionsPage()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async Task LoadTransactionTypes()
        {
            var result = await _transactionProcessor.GetTransactionTypes();

            if (result.Success)
            {
                foreach (var type in result.Value)
                {
                    comboBoxType.Items.Add(type);
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ButtonCreate_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxCreateAmount.Text == String.Empty 
                || textBoxCreateFromIdUsername.Text == String.Empty 
                || textBoxCreateToIdUsername.Text == String.Empty
                || comboBoxType.SelectedItem == null)
            {
                MessageBox.Show($"Заполните все поля!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var resultUserFrom = await _userProcessor.Get(textBoxCreateFromIdUsername.Text);
            var resultUserTo = await _userProcessor.Get(textBoxCreateToIdUsername.Text);

            if (!Decimal.TryParse(textBoxCreateAmount.Text, out var amount))
            {
                MessageBox.Show($"Введена некорректная сумма!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!resultUserFrom.Success || !resultUserTo.Success)
            {
                MessageBox.Show($"Не удалось найти клиентов", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var fromUserBalance = resultUserFrom.Value.Balance - amount;

            if (fromUserBalance < 0)
            {
                MessageBox.Show($"Недостаточно средств на счете!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (resultUserFrom.Value.CurrencyType != resultUserTo.Value.CurrencyType)
            {
                MessageBox.Show($"Ошибка! Разные валюты!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }


            var resultCreateTransaction = await _transactionProcessor.Create(new(
                resultUserFrom.Value.Id,
                resultUserTo.Value.Id, 
                amount, 
                comboBoxType.SelectedValue.ToString()));

            if (resultCreateTransaction.Success)
            {
                MessageBox.Show($"{resultCreateTransaction.Message}", "Successful", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"{resultCreateTransaction.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTransactionTypes();
        }

        private async void ButtonFind_OnClick(object sender, RoutedEventArgs e)
        {
            if (textBoxFindId.Text == String.Empty)
            {
                MessageBox.Show("Поле обязательно к заполнению!", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);

            }
            else
            {
                var result = await _transactionProcessor.Get(textBoxFindId.Text);
                if (result.Success)
                {
                    var transactionModel = result.Value;

                    textboxFindToUserId.Text = transactionModel.ToUserId;
                    textboxFindFromUserId.Text = transactionModel.FromUserId;
                    textboxFindDate.Text = transactionModel.Date.ToString("G");
                    textboxFindType.Text = transactionModel.TransactionType;
                    textboxFindAmount.Text = transactionModel.Amount.ToString();
                    textboxFindCurrency.Text = transactionModel.CurrencyType;
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
                var result = await _transactionProcessor.Delete(textBoxDeleteId.Text);
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

        private async void ButtonLoadAllTransactions_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await _transactionProcessor.Search();

            if (result.Success)
            {
                dataGridTransactions.ItemsSource = result.Value;
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

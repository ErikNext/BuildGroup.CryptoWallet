using BuildGroup.CryptoWallet.Frontend.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuildGroup.CryptoWallet.Frontend.Wpf.Api.Requests;

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api;

class TransactionProcessor
{
    public async Task<ApiResult<List<string>>> GetCurrencyTypes()
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions/CurrencyTypes";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<string>>();

                    return ApiResult<List<string>>.OK(result);
                }

                return ApiResult<List<string>>.Failed("Ошибка получения типов!");
            }
        }
        catch
        {
            return ApiResult<List<string>>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<List<string>>> GetTransactionTypes()
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions/TransactionTypes";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<string>>();

                    return ApiResult<List<string>>.OK(result);
                }

                return ApiResult<List<string>>.Failed("Ошибка получения типов!");
            }
        }
        catch
        {
            return ApiResult<List<string>>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<TransactionModel>> Create(CreateTransactionRequest request)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var transaction = await response.Content.ReadAsAsync<TransactionModel>();

                    return ApiResult<TransactionModel>.OK(transaction, "Транзакция была успешно создана!");
                }

                return ApiResult<TransactionModel>.Failed("Не удалось создать транзакцию!");
            }
        }
        catch
        {
            return ApiResult<TransactionModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<TransactionModel>> Get(string id)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions/{id}";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    TransactionModel user = await response.Content.ReadAsAsync<TransactionModel>();

                    return ApiResult<TransactionModel>.OK(user);
                }

                return ApiResult<TransactionModel>.Failed("Не удалось найти транзакцию!");
            }
        }
        catch
        {
            return ApiResult<TransactionModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<List<TransactionModel>>> Search()
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions/Search";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, null))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionModel> transactions = await response.Content.ReadAsAsync<List<TransactionModel>>();

                    return ApiResult<List<TransactionModel>>.OK(transactions);
                }

                return ApiResult<List<TransactionModel>>.Failed("Ошибка получения данных!");
            }
        }
        catch
        {
            return ApiResult<List<TransactionModel>>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult> Delete(string id)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Transactions/{id}";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.DeleteAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return ApiResult.Ok("Транзакция успешно удалена!");
                }

                return ApiResult<UserModel>.Failed("Не удалось удалить транзакцию!");
            }
        }
        catch
        {
            return ApiResult<UserModel>.Error("Не удалось подключиться к серверу!");
        }
    }
}
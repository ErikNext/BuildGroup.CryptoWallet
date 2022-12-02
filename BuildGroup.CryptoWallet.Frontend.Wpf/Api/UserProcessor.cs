using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BuildGroup.CryptoWallet.Frontend.Wpf.Api.Requests;
using BuildGroup.CryptoWallet.Frontend.Wpf.Models;

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api;

public class UserProcessor
{
    public async Task<ApiResult<UserModel>> Get(string id)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Users/{id}";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    UserModel user = await response.Content.ReadAsAsync<UserModel>();

                    return ApiResult<UserModel>.OK(user);
                }

                return ApiResult<UserModel>.Failed("Не удалось найти клиента!");
            }
        }
        catch
        {
            return ApiResult<UserModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<UserModel>> Create(CreateUserRequest request)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Users";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    UserModel user = await response.Content.ReadAsAsync<UserModel>();

                    return ApiResult<UserModel>.OK(user, "Пользователь был успешно создан!");
                }

                return ApiResult<UserModel>.Failed("Не удалось создать клиента!");
            }
        }
        catch
        {
            return ApiResult<UserModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult> Delete(string id)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Users/{id}";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.DeleteAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return ApiResult.Ok("Клиент успешно удален!");
                }

                return ApiResult<UserModel>.Failed("Не удалось удалить клиента!");
            }
        }
        catch
        {
            return ApiResult<UserModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<UserModel>> Update(string id, UpdateUserRequest request)
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Users/{id}";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(url, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    UserModel user = await response.Content.ReadAsAsync<UserModel>();

                    return ApiResult<UserModel>.OK(user, "Пользователь был отредактирован!");
                }

                return ApiResult<UserModel>.Failed("Не удалось найти клиента!");
            }
        }
        catch
        {
            return ApiResult<UserModel>.Error("Не удалось подключиться к серверу!");
        }
    }

    public async Task<ApiResult<List<UserModel>>> Search()
    {
        string url = $"{ApiHelper.ApiClient.BaseAddress}Users/Search";

        try
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, null))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<UserModel> users = await response.Content.ReadAsAsync<List<UserModel>>();

                    return ApiResult<List<UserModel>>.OK(users);
                }

                return ApiResult<List<UserModel>>.Failed("Ошибка получения данных!");
            }
        }
        catch
        {
            return ApiResult<List<UserModel>>.Error("Не удалось подключиться к серверу!");
        }
    }
}
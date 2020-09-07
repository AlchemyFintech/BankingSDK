using BankingSDK.Base.Ibanity.Contexts;
using BankingSDK.Base.Ibanity.Extensions;
using BankingSDK.Base.Ibanity.Models;
using BankingSDK.Base.Ibanity.Models.Requests;
using BankingSDK.Common;
using BankingSDK.Common.Enums;
using BankingSDK.Common.Exceptions;
using BankingSDK.Common.Interfaces;
using BankingSDK.Common.Interfaces.Contexts;
using BankingSDK.Common.Models;
using BankingSDK.Common.Models.Data;
using BankingSDK.Common.Models.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BankingSDK.Base.Ibanity
{

    public class IbanityConnector : SdkBaseConnector, IBankingConnector
    {
        private IbanityUserContext _userContextLocal => (IbanityUserContext)_userContext;

        private readonly string _bankId;
        private readonly Uri _sandboxUrl = new Uri("https://api.ibanity.com/");
        private readonly Uri _productionUrl = new Uri("https://api.ibanity.com/");

        private Uri apiUrl => SdkApiSettings.IsSandbox ? _sandboxUrl : _productionUrl;

        public string UserContext
        {
            get => JsonConvert.SerializeObject(_userContext);
            set
            {
                _userContext = JsonConvert.DeserializeObject<IbanityUserContext>(value);
                UserContextChanged = false;
            }
        }

        public IbanityConnector(BankSettings settings, int connectorId, string bankId, string keyId) : base(settings, connectorId)
        {
            _bankId = bankId;
        }

        #region User
        public async Task<BankingResult<IUserContext>> RegisterUserAsync(string userId)
        {
            //_token = await GetToken(userId);
            _userContext = new IbanityUserContext
            {
                UserId = userId
            };

            UserContextChanged = false;
            return new BankingResult<IUserContext>(ResultStatus.DONE, null, _userContext, JsonConvert.SerializeObject(_userContext));
        }
        #endregion

        #region Financial Institutions
        //public async Task<BankingResult<List<FinancialInstitution>>> GetFinancialInstitutions(IPagerContext context = null)
        //{
        //    var requestedAt = DateTime.UtcNow;
        //    var watch = Stopwatch.StartNew();
        //    IbanityPagerContext pagerContext = (context as IbanityPagerContext) ?? new IbanityPagerContext();
        //    var client = GetClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string url = $"/xs2a/financial-institutions{pagerContext.GetRequestParams()}";
        //    client.SignRequest(HttpMethod.Get, url, _certificatePath, _certificatePassword, _keyId);
        //    var result = await client.GetAsync(url);

        //    if (!result.IsSuccessStatusCode)
        //    {
        //        watch.Stop();
        //        sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
        //        throw new ApiCallException(await result.Content.ReadAsStringAsync());
        //    }

        //    string rawData = await result.Content.ReadAsStringAsync();

        //    var model = JsonConvert.DeserializeObject<FinancialInstitutionsModel>(rawData);
        //    pagerContext.SetBefore(model.Meta.Paging.Before);
        //    switch (pagerContext.GetDirection())
        //    {
        //        case PageDirection.FIRST:
        //        case PageDirection.NEXT:
        //            if (model.Data.Count() > pagerContext.GetLimit())
        //            {
        //                model.Data.RemoveAt(model.Data.Count() - 1);
        //                pagerContext.SetAfter(model.Data.Last().Id);
        //            }
        //            else
        //            {
        //                pagerContext.SetAfter(null);
        //            }
        //            break;
        //        case PageDirection.PREVIOUS:
        //            pagerContext.SetAfter(model.Meta.Paging.After);
        //            break;
        //    }

        //    var data = model.Data.Select(x => new FinancialInstitution
        //    {
        //        Id = x.Id,
        //        Name = x.Attributes.Name
        //    }).ToList();

        //    return new BankingResult<List<FinancialInstitution>>(ResultStatus.DONE, url, data, rawData, pagerContext);
        //}

        //public async Task<BankingResult<FinancialInstitution>> GetFinancialInstitution(string institutionId)
        //{
        //    var requestedAt = DateTime.UtcNow;
        //    var watch = Stopwatch.StartNew();
        //    var client = GetClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string url = $"/xs2a/financial-institutions/{institutionId}";
        //    client.SignRequest(HttpMethod.Get, url, _certificatePath, _certificatePassword, _keyId);
        //    var result = await client.GetAsync(url);

        //    if (!result.IsSuccessStatusCode)
        //    {
        //        watch.Stop();
        //        sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
        //        throw new ApiCallException(await result.Content.ReadAsStringAsync());
        //    }

        //    string rawData = await result.Content.ReadAsStringAsync();
        //    var model = JsonConvert.DeserializeObject<FinancialInstitutionModel>(rawData);

        //    var data = new FinancialInstitution
        //    {
        //        Id = model.Data.Id,
        //        Name = model.Data.Attributes.Name
        //    };

        //    watch.Stop();
        //    sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
        //    return new BankingResult<FinancialInstitution>(ResultStatus.DONE, url, data, rawData);
        //}
        #endregion

        #region Accounts
        public RequestAccountsAccessOption GetRequestAccountsAccessOption()
        {
            return RequestAccountsAccessOption.NotCustomizable;
        }

        public async Task<BankingResult<string>> RequestAccountsAccessAsync(AccountsAccessRequest request)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new IbanityAccountsAccessRequest
                {
                    Data = new Models.Requests.IbanityAccountInformationAccessData
                    {
                        Attributes = new Models.Requests.IbanityAccountInformationAccessAttributes
                        {
                            //ConsentReference = tppConsent,
                            RedirectUri = $"{request.RedirectUrl}?flowId={request.FlowId}"
                        }
                    }
                }), Encoding.UTF8, "application/json");
                var client = GetClient(await content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Add("Authorization", _token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"/xs2a/customer/financial-institutions/{_bankId}/account-information-access-requests";
                client.SignRequest(HttpMethod.Post, url, _settings.SigningCertificate, _settings.AppClientId);
                var result = await client.PostAsync(url, content);

                string rawData = await result.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IbanityAccountInformationAccessModel>(rawData);
                return new BankingResult<string>(ResultStatus.REDIRECT, url, model.Data.Links.Redirect, rawData);
            }
            catch (ApiCallException e) { throw e; }
            catch (SdkUnauthorizedException e) { throw e; }
            catch (Exception e)
            {
                await LogAsync(apiUrl, 500, Http.Post, e.ToString());
                throw e;
            }
        }

        public async Task<BankingResult<List<Account>>> GetAccountsAsync()
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _token);
            string url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts?limit=100";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AccountsModel>(rawData);
            

            var data = model.Data.Select(x => new Account
            {
                Id = x.Id,
                AvailableBalance = x.Attributes.AvailableBalance,
                Currency = x.Attributes.Currency,
                Reference = x.Attributes.Reference,
                ReferenceType = x.Attributes.ReferenceType,
                FinantialInstitutionId = x.Relationships.FinancialInstitution.Data.Id,
                Description = x.Attributes.Description
            }).ToList();

            return new BankingResult<List<Account>>(ResultStatus.DONE, url, data, rawData);
        }

        public async Task<BankingResult<Account>> GetAccount(string institutionId, string accountId)
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _token);
            string url = $"/xs2a/customer/financial-institutions/{institutionId}/accounts/{accountId}";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AccountModel>(rawData);
            var data = new Account
            {
                Id = model.Data.Id,
                AvailableBalance = model.Data.Attributes.AvailableBalance,
                Currency = model.Data.Attributes.Currency,
                Reference = model.Data.Attributes.Reference,
                ReferenceType = model.Data.Attributes.ReferenceType,
                FinantialInstitutionId = model.Data.Relationships.FinancialInstitution.Data.Id,
                Description = model.Data.Attributes.Description
            };
            watch.Stop();
            sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));

            return new BankingResult<Account>(ResultStatus.DONE, url, data, rawData);
        }
        #endregion

        #region Transactions
        public async Task<BankingResult<List<Transaction>>> GetTransactions(string institutionId, string accountId, IPagerContext context = null)
        {
            var requestedAt = DateTime.UtcNow;
            var watch = Stopwatch.StartNew();
            IbanityPagerContext pagerContext = (context as IbanityPagerContext) ?? new IbanityPagerContext();
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _token);
            string url = $"/xs2a/customer/financial-institutions/{institutionId}/accounts/{accountId}/transactions{pagerContext.GetRequestParams()}";
            client.SignRequest(HttpMethod.Get, url, _certificatePath, _certificatePassword, _keyId);
            var result = await client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                watch.Stop();
                sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
                throw new ApiCallException(await result.Content.ReadAsStringAsync());
            }

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TransactionsModel>(rawData);
            pagerContext.SetBefore(model.Meta.Paging.Before);
            switch (pagerContext.GetDirection())
            {
                case PageDirection.FIRST:
                case PageDirection.NEXT:
                    if (model.Data.Count() > pagerContext.GetLimit())
                    {
                        model.Data.RemoveAt(model.Data.Count() - 1);
                        pagerContext.SetAfter(model.Data.Last().id);
                    }
                    else
                    {
                        pagerContext.SetAfter(null);
                    }
                    break;
                case PageDirection.PREVIOUS:
                    pagerContext.SetAfter(model.Meta.Paging.After);
                    break;
            }

            var data = model.Data.Select(x => new Transaction
            {
                Id = x.id,
                Amount = x.attributes.amount,
                CounterpartReference = x.attributes.counterpartReference,
                Currency = x.attributes.currency,
                Description = x.attributes.description,
                ExecutionDate = x.attributes.executionDate,
                ValueDate = x.attributes.valueDate
            }).ToList();
            watch.Stop();
            sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));

            return new BankingResult<List<Transaction>>(ResultStatus.DONE, url, data, rawData, pagerContext);
        }

        public async Task<BankingResult<Transaction>> GetTransaction(string institutionId, string accountId, string transactionId)
        {
            var requestedAt = DateTime.UtcNow;
            var watch = Stopwatch.StartNew();
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _token);
            string url = $"/xs2a/customer/financial-institutions/{institutionId}/accounts/{accountId}/transactions/{transactionId}";
            client.SignRequest(HttpMethod.Get, url, _certificatePath, _certificatePassword, _keyId);
            var result = await client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                watch.Stop();
                sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
                throw new ApiCallException(await result.Content.ReadAsStringAsync());
            }

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TransactionModel>(rawData);

            var data = new Transaction
            {
                Id = model.Data.id,
                Amount = model.Data.attributes.amount,
                CounterpartReference = model.Data.attributes.counterpartReference,
                Currency = model.Data.attributes.currency,
                Description = model.Data.attributes.description,
                ExecutionDate = model.Data.attributes.executionDate,
                ValueDate = model.Data.attributes.valueDate
            };
            watch.Stop();
            sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));

            return new BankingResult<Transaction>(ResultStatus.DONE, url, data, rawData);
        }
        #endregion

        #region Payment

        public async Task<BankingResult<string>> CreatePaymentInitiationRequest(PaymentInitiationRequest model)
        {
            var paymentRequest = new IbanityPaymentInitiationRequest
            {
                Data = new IbanityPaymentInitiationData
                {
                    Type = "paymentInitiationRequest",
                    Attributes = new IbanityPaymentInitiationAttributes
                    {
                        RedirectUri = $"{model.RedirectUrl}?flowId={model.FlowId}",
                        ConsentReference = model.ConsentReference,
                        ProductType = "sepa-credit-transfer",
                        //RemittanceInformation="payment",
                        RemittanceInformationType = "unstructured",
                        RequestedExecutionDate = DateTime.UtcNow,
                        Currency = model.Currency,
                        Amount = model.Amount,
                        //DebtorName = "Delmer Hermann",
                        DebtorAccountReference = model.Debtor.Iban,
                        DebtorAccountReferenceType = "IBAN",
                        CreditorName = model.Recipient.Name,
                        CreditorAccountReference = model.Recipient.Iban,
                        CreditorAccountReferenceType = "IBAN",
                        //CreditorAgent="NBBEBEBB203",
                        //CreditorAgentType="BIC",
                        EndToEndId = Guid.NewGuid().ToString().Replace("-", ""),
                        //Locale = "en",
                        //CustomerIpAddress = "1.1.1.1"
                    }
                }
            };

            var result = await CreatePaymentInitiationRequest(model.FinancialInstitutionId, paymentRequest, watch, requestedAt);

            return new BankingResult<string>(ResultStatus.REDIRECT, string.Join("\n", accountResult.GetURL(), result.GetURL()), result.GetData().Data.Links.Redirect, result.GetRawResponse());
        }

        private async Task<BankingResult<PaymentInitiationModel>> CreatePaymentInitiationRequest(string institutionId, PaymentInitiation model, Stopwatch watch, DateTime requestedAt)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var client = GetClient(await content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _token);
            var url = $"/customer/financial-institutions/{institutionId}/payment-initiation-requests";
            client.SignRequest(HttpMethod.Post, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.PostAsync(url, content);

            if (!result.IsSuccessStatusCode)
            {
                watch.Stop();
                sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt));
                throw new ApiCallException(await result.Content.ReadAsStringAsync());
            }

            var rawData = await result.Content.ReadAsStringAsync();
            watch.Stop();
            sdkApiConnector.Log(await GetSdkToken(), PrepareLog((int)watch.ElapsedMilliseconds, url, (int)result.StatusCode, Http.Get, requestedAt, model.Data.Attributes.Amount));

            return new BankingResult<PaymentInitiationModel>(ResultStatus.DONE, url, JsonConvert.DeserializeObject<PaymentInitiationModel>(rawData), rawData);
        }
        #endregion

        #region Pager Context
        public IPagerContext RestorePagerContext(string json)
        {
            return JsonConvert.DeserializeObject<IbanityPagerContext>(json);
        }

        public IPagerContext CreatePageContext(byte limit)
        {
            return new IbanityPagerContext(limit);
        }
        #endregion

        private async Task<string> GetToken(string customerId)
        {
            var content = new StringContent(
                $"{{\"data\":{{\"type\":\"customerAccessToken\",\"attributes\":{{\"applicationCustomerReference\":\"{customerId}\"}}}}}}",
                Encoding.UTF8, "application/json");
            var client = GetClient(await content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.PostAsync("/xs2a/customer-access-tokens", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new ApiCallException(await result.Content.ReadAsStringAsync());
            }
            var accessInfo = JsonConvert.DeserializeObject<IbanityAccessInfo>(await result.Content.ReadAsStringAsync());

            return $"Bearer {accessInfo.Data.Attributes.AccessToken}";
        }

        private SdkHttpClient GetClient(string payload = "")
        {
            SdkHttpClient client = GetSdkClient(apiUrl);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"));
            using (SHA512 sha512Hash = SHA512.Create())
            {
                client.DefaultRequestHeaders.Add("Digest", "SHA-512=" + Convert.ToBase64String(sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(payload))));
            }

            return client;
        }
    }
}

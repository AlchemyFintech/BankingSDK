using BankingSDK.Base.Ibanity.Contexts;
using BankingSDK.Base.Ibanity.Enums;
using BankingSDK.Base.Ibanity.Extensions;
using BankingSDK.Base.Ibanity.Models;
using BankingSDK.Base.Ibanity.Models.Requests;
using BankingSDK.Common;
using BankingSDK.Common.Contexts;
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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

        public IbanityConnector(BankSettings settings, int connectorId, string bankId) : base(settings, connectorId)
        {
            _bankId = bankId;
        }

        #region User
        public async Task<BankingResult<IUserContext>> RegisterUserAsync(string userId)
        {
            var contextId = Guid.NewGuid();
            _userContext = new IbanityUserContext
            {
                UserId = userId,
                ContextId = contextId
            };
            _userContextLocal.Token = await GetToken($"{userId}_{contextId}");
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
                            ConsentReference = request.FlowId,
                            RedirectUri = $"{request.RedirectUrl}?flowId={request.FlowId}"
                        }
                    }
                }), Encoding.UTF8, "application/json");
                var client = GetClient(await content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"/xs2a/customer/financial-institutions/{_bankId}/account-information-access-requests";
                client.SignRequest(HttpMethod.Post, url, _settings.SigningCertificate, _settings.AppClientId);
                var result = await client.PostAsync(url, content);

                string rawData = await result.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IbanityAccountInformationAccessModel>(rawData);
                var flowContext = new FlowContext
                {
                    Id = request.FlowId,
                    ConnectorType = ConnectorType,
                    FlowType = FlowType.AccountsAccess
                };
                return new BankingResult<string>(ResultStatus.REDIRECT, url, model.Data.Links.Redirect, rawData, flowContext: flowContext);
            }
            catch (ApiCallException e) { throw e; }
            catch (SdkUnauthorizedException e) { throw e; }
            catch (Exception e)
            {
                await LogAsync(apiUrl, 500, Http.Post, e.ToString());
                throw e;
            }
        }

        public async Task<BankingResult<IUserContext>> RequestAccountsAccessFinalizeAsync(FlowContext flowContext, string queryString)
        {
            try
            {
                //do we need more stuff here?

                return new BankingResult<IUserContext>(ResultStatus.DONE, null, _userContext, JsonConvert.SerializeObject(_userContext));
            }
            catch (ApiCallException e) { throw e; }
            catch (ApiUnauthorizedException e) { throw e; }
            catch (PagerException e) { throw e; }
            catch (SdkUnauthorizedException e) { throw e; }
            catch (Exception e)
            {
                await LogAsync(apiUrl, 500, Http.Get, e.ToString());
                throw e;
            }
        }

        public async Task<BankingResult<IUserContext>> RequestAccountsAccessFinalizeAsync(string flowContextJson, string queryString)
        {
            return await RequestAccountsAccessFinalizeAsync(JsonConvert.DeserializeObject<FlowContext>(flowContextJson), queryString);
        }

        public async Task<BankingResult<List<Account>>> GetAccountsAsync()
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
            string url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts?limit=100";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IbanityAccountsModel>(rawData);


            var data = model.Data.Select(x => new Account
            {
                Id = x.Id,
                Currency = x.Attributes.Currency,
                BalancesConsent = new ConsentInfo { ConsentId = x.Id, ValidUntil = x.Attributes.AuthorizationExpirationExpectedAt },
                TransactionsConsent = new ConsentInfo { ConsentId = x.Id, ValidUntil = x.Attributes.AuthorizationExpirationExpectedAt },
                Iban = x.Attributes.Reference,
                Description = x.Attributes.Description
            }).ToList();

            return new BankingResult<List<Account>>(ResultStatus.DONE, url, data, rawData);
        }

        public new async Task<BankingResult<BankingAccount>> DeleteAccountAsync(string accountId)
        {
            var result = await DeleteConsentAsync(accountId);
            var data = result.GetData().FirstOrDefault();
            return new BankingResult<BankingAccount>(ResultStatus.DONE, result.GetURL(), data, JsonConvert.SerializeObject(data));
        }

        public async Task<BankingResult<List<BankingAccount>>> DeleteConsentAsync(string consentId)
        {
            try
            {
                var data = new List<BankingAccount> { await GetAccount(consentId) };
                var client = GetClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
                var url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts/{consentId}";
                client.SignRequest(HttpMethod.Delete, url, _settings.SigningCertificate, _settings.AppClientId);
                var result = await client.DeleteAsync(url);

                return new BankingResult<List<BankingAccount>>(ResultStatus.DONE, url, data, JsonConvert.SerializeObject(data));
            }
            catch (ApiCallException e) { throw e; }
            catch (ApiUnauthorizedException e) { throw e; }
            catch (PagerException e) { throw e; }
            catch (SdkUnauthorizedException e) { throw e; }
            catch (Exception e)
            {
                await LogAsync(apiUrl, 500, "DELETE", e.ToString());
                throw e;
            }
        }
        private async Task<BankingAccount> GetAccount(string accountId)
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
            string url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts/{accountId}";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AccountModel>(rawData);
            return new BankingAccount
            {
                Currency = model.Data.Attributes.Currency,
                Iban = model.Data.Attributes.Reference,
            };
        }
        #endregion

        #region Balances

        public async Task<BankingResult<List<Balance>>> GetBalancesAsync(string accountId)
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
            string url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts/{accountId}";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AccountModel>(rawData);
            var currentBalance = new Balance
            {
                BalanceType = "current",
                BalanceAmount = new BalanceAmount
                {
                    Amount = model.Data.Attributes.CurrentBalance,
                    Currency = model.Data.Attributes.Currency
                },
                LastChangeDateTime = model.Data.Attributes.CurrentBalanceChangedAt,
                ReferenceDate = model.Data.Attributes.CurrentBalanceReferenceDate
            };
            var availableBalance = new Balance
            {
                BalanceType = "available",
                BalanceAmount = new BalanceAmount
                {
                    Amount = model.Data.Attributes.AvailableBalance,
                    Currency = model.Data.Attributes.Currency
                },
                LastChangeDateTime = model.Data.Attributes.AvailableBalanceChangedAt,
                ReferenceDate = model.Data.Attributes.AvailableBalanceReferenceDate
            };

            return new BankingResult<List<Balance>>(ResultStatus.DONE, url, new List<Balance> { currentBalance, availableBalance }, rawData);
        }

        #endregion

        #region Transactions
        public async Task<BankingResult<List<Transaction>>> GetTransactionsAsync(string accountId, IPagerContext context = null)
        {
            IbanityPagerContext pagerContext = (context as IbanityPagerContext) ?? new IbanityPagerContext();
            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
            string url = $"/xs2a/customer/financial-institutions/{_bankId}/accounts/{accountId}/transactions{pagerContext.GetRequestParams()}";
            client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.GetAsync(url);

            string rawData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IbanityTransactionsModel>(rawData);
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

            return new BankingResult<List<Transaction>>(ResultStatus.DONE, url, data, rawData, pagerContext);
        }

        #endregion

        #region Payment

        public async Task<BankingResult<string>> CreatePaymentInitiationRequestAsync(PaymentInitiationRequest model)
        {
            var paymentRequest = new IbanityPaymentInitiationRequest
            {
                Data = new IbanityPaymentInitiationData
                {
                    Type = "paymentInitiationRequest",
                    Attributes = new IbanityPaymentInitiationAttributes
                    {
                        RedirectUri = $"{model.RedirectUrl}?flowId={model.FlowId}",
                        ConsentReference = model.FlowId,
                        ProductType = "sepa-credit-transfer",
                        //RemittanceInformation="payment",
                        RemittanceInformationType = "unstructured",
                        RequestedExecutionDate = DateTime.UtcNow,
                        Currency = model.Currency,
                        Amount = model.Amount,
                        DebtorName = model.Debtor.Name,
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


            var content = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");
            var client = GetClient(await content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", _userContextLocal.Token);
            var url = $"/customer/financial-institutions/{_bankId}/payment-initiation-requests";
            client.SignRequest(HttpMethod.Post, url, _settings.SigningCertificate, _settings.AppClientId);
            var result = await client.PostAsync(url, content);

            var rawData = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IbanityPaymentInitiationModel>(rawData);

            var flowContext = new FlowContext
            {
                Id = model.FlowId,
                ConnectorType = ConnectorType,
                FlowType = FlowType.Payment,
                RedirectUrl = model.RedirectUrl,
                PaymentProperties = new PaymentProperties
                {
                    PaymentId = data.Data.Id
                }
            };

            return new BankingResult<string>(ResultStatus.REDIRECT, url, data.Data.Links.Redirect, data.Data.Links.Redirect, flowContext: flowContext);
        }

        public async Task<BankingResult<PaymentStatus>> CreatePaymentInitiationRequestFinalizeAsync(FlowContext flowContext, string queryString)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(queryString);
                var error = query.Get("error");
                if (error != null)
                {
                    throw new ApiCallException(query.Get("error_description"));
                }

                var client = GetClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"/xs2a/customer/financial-institutions/{_bankId}/payment-initiation-requests/{flowContext.PaymentProperties.PaymentId}";
                client.SignRequest(HttpMethod.Get, url, _settings.SigningCertificate, _settings.AppClientId);
                var result = await client.GetAsync(url);

                var rawData = await result.Content.ReadAsStringAsync();
                var paymentResult = JsonConvert.DeserializeObject<IbanityPaymentInitiationModel>(rawData);

                var data = new PaymentStatus
                {
                    Amount = new BankingAccountInstructedAmount
                    {
                        Amount = paymentResult.Data.Attributes.Amount,
                        Currency = paymentResult.Data.Attributes.Currency
                    },
                    Creditor = new BankingAccount
                    {
                        Iban = paymentResult.Data.Attributes.CreditorAccountReference,
                        Currency = paymentResult.Data.Attributes.Currency
                    },
                    CreditorName = paymentResult.Data.Attributes.CreditorName,
                    Debtor = new BankingAccount
                    {
                        Iban = paymentResult.Data.Attributes.DebtorAccountReference,
                        Currency = paymentResult.Data.Attributes.Currency
                    },
                    EndToEndIdentification = paymentResult.Data.Attributes.EndToEndId,
                    //Status = paymentResult.Data.Attributes.Status
                };

                return new BankingResult<PaymentStatus>(ResultStatus.DONE, url, data, rawData);
            }
            catch (ApiCallException e) { throw e; }
            catch (ApiUnauthorizedException e) { throw e; }
            catch (PagerException e) { throw e; }
            catch (SdkUnauthorizedException e) { throw e; }
            catch (Exception e)
            {
                await LogAsync(apiUrl, 500, Http.Get, e.ToString());
                throw e;
            }
        }

        public async Task<BankingResult<PaymentStatus>> CreatePaymentInitiationRequestFinalizeAsync(string flowContextJson, string queryString)
        {
            return await CreatePaymentInitiationRequestFinalizeAsync(JsonConvert.DeserializeObject<FlowContext>(flowContextJson), queryString);
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

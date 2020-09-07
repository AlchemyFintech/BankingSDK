using BankingSDK.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BankingSDK.Base.Ibanity.Extensions
{
    internal static class HttpClientExtensions
    {
        internal static void SignRequest(this SdkHttpClient client, HttpMethod method, string path, X509Certificate2 cert, string keyId)
        {
            var auth = string.IsNullOrEmpty(client.DefaultRequestHeaders.Authorization?.Parameter) ? string.Empty : $"\nauthorization: {client.DefaultRequestHeaders.Authorization.Scheme} {client.DefaultRequestHeaders.Authorization.Parameter}";
            var signingString = $"(request-target): {method.Method.ToLower()} {path}\nhost: {client.BaseAddress.Host}\ndigest: {client.DefaultRequestHeaders.GetValues("Digest").First()}\ndate: {client.DefaultRequestHeaders.GetValues("Date").First()}{auth}";
            var headerList = $"(request-target) host digest date" + (string.IsNullOrEmpty(auth) ? string.Empty : " authorization");
            var signature = Convert.ToBase64String(cert.GetRSAPrivateKey().SignData(Encoding.UTF8.GetBytes(signingString), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            var signatureHeader = $"keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"{headerList}\",signature=\"{signature}\"";
            client.DefaultRequestHeaders.Add("Signature", signatureHeader);
        }
    }
}

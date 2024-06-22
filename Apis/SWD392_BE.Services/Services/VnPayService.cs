using Microsoft.Extensions.Configuration;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.PaymentModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class VnPayService :IVnPayService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public VnPayService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public string CreatePaymentUrl(VnPayPaymentRequest model, string ipAddress)
        {
            Random random = new Random();
            string randomTxnRef = random.Next(100000, 999999).ToString();
            var requestData = new SortedList<string, string>(new VnPayCompare())
    {
        { "vnp_Version", "2.1.0" },
        { "vnp_Command", "pay" },
        { "vnp_TmnCode", _configuration["VNPAY:TmnCode"] },
        { "vnp_Amount", ((int)(model.Amount * 100)).ToString() },
        { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
        { "vnp_CurrCode", "VND" },
        { "vnp_IpAddr", ipAddress },
        { "vnp_Locale", "vn" },
        { "vnp_OrderInfo", Uri.EscapeDataString($"Deposit {model.Amount} into wallet with transaction id: {model.TxnRef}") },
        { "vnp_OrderType", "other" },
        { "vnp_ReturnUrl", Uri.EscapeDataString(_configuration["VNPAY:ReturnUrl"]) },
        { "vnp_TxnRef", randomTxnRef },
        { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") },
        { "vnp_BankCode", "NCB" }
    };

            string signData = string.Join("&", requestData.Select(kv => $"{kv.Key}={kv.Value}"));
            string vnpSecureHash = HmacSHA512(_configuration["VNPAY:HashSecret"], signData);
            return $"{_configuration["VNPAY:Url"]}?{signData}&vnp_SecureHash={vnpSecureHash}";
        }
        public bool ValidateSignature(string inputHash, SortedList<string, string> responseData)
        {
            string signData = string.Join("&", responseData
                .Where(kv => kv.Key != "vnp_SecureHash")
                .OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

            string computedHash = HmacSHA512(_configuration["VNPAY:HashSecret"], signData);
            return inputHash == computedHash;
        }

        public async Task<ResultModel> ProcessPaymentResponse(SortedList<string, string> responseData)
        {
            string vnp_ResponseCode = responseData["vnp_ResponseCode"];
            if (vnp_ResponseCode == "00")
            {
                string userId = responseData["vnp_OrderInfo"];
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    var result = await _userService.UpdateUserBalance(userId, int.Parse(responseData["vnp_Amount"]) / 100);
                    return result;
                }
            }

            return new ResultModel { IsSuccess = false, Code = 400, Message = "Payment failed" };
        }

        private string HmacSHA512(string key, string data, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Key and data cannot be null or empty.");
            }

            encoding = encoding ?? Encoding.UTF8;

            using (var hmac = new HMACSHA512(encoding.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(encoding.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
        private class VnPayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.CompareOrdinal(x, y);
            }
        }
    }
}
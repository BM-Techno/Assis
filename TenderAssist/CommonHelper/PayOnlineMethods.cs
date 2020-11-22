using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace TenderAssist.CommonHelper
{
    public class PayOnlineMethods
    {
        public static string TransferFund(string MerchantLogin, string MerchantPass, string MerchantDiscretionaryData, string ProductID,
            string ClientCode, string CustomerAccountNo, string TransactionType, string TransactionAmount,
            string TransactionCurrency, string TransactionServiceCharge, string TransactionID, string TransactionDateTime, string BankID,
            string successPage)
        {

            string strURL, strClientCode, strClientCodeEncoded;
            byte[] b;
            string strResponse = "";

            // MerchantLogin = "197";
            // MerchantPass = "Test@123";
            // TransactionType = "NBFundtransfer";
            // ProductID = "NSE";
            // TransactionID = "123";
            // TransactionAmount = "100";
            // TransactionCurrency = "INR";
            // BankID = "2001";
            // //string ru = localhost:35652/Pages/FundTransferSuccess.aspx";
            //string ru = "http://localhost:258252/Pages/FundTransferFailed.aspx";

            try
            {
                var PaymentUrl = ConfigurationManager.AppSettings["PaymentUrl"].ToString();
                b = Encoding.UTF8.GetBytes(ClientCode);
                strClientCode = Convert.ToBase64String(b);
                strClientCodeEncoded = HttpUtility.UrlEncode(strClientCode);

                strURL = "" + ConfigurationManager.AppSettings["TransferURL"].ToString();///
                strURL = strURL.Replace("[PaymentURL]", PaymentUrl);
                strURL = strURL.Replace("[MerchantLogin]", MerchantLogin + "&");
                strURL = strURL.Replace("[MerchantPass]", MerchantPass + "&");
                strURL = strURL.Replace("[TransactionType]", TransactionType + "&");
                strURL = strURL.Replace("[ProductID]", ProductID + "&");
                strURL = strURL.Replace("[TransactionAmount]", TransactionAmount + "&");
                strURL = strURL.Replace("[TransactionCurrency]", TransactionCurrency + "&");
                strURL = strURL.Replace("[TransactionServiceCharge]", TransactionServiceCharge + "&");
                strURL = strURL.Replace("[ClientCode]", strClientCodeEncoded + "&");
                strURL = strURL.Replace("[TransactionID]", TransactionID + "&");
                strURL = strURL.Replace("[TransactionDateTime]", TransactionDateTime + "&");
                strURL = strURL.Replace("[CustomerAccountNo]", CustomerAccountNo + "&");
                strURL = strURL.Replace("[MerchantDiscretionaryData]", MerchantDiscretionaryData + "&");
                strURL = strURL.Replace("[BankID]", BankID + "&");
                strURL = strURL.Replace("[ru]", successPage + "&");// Remove on Production

                //  string reqHashKey = requestkey;
                string reqHashKey = ConfigurationManager.AppSettings["ReqHashKey"];

                string signature = "";
                string strsignature = MerchantLogin + MerchantPass + TransactionType + ProductID + TransactionID + TransactionAmount + TransactionCurrency;
                byte[] bytes = Encoding.UTF8.GetBytes(reqHashKey);
                byte[] bt = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(strsignature));
                // byte[] b = new HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(prodid));
                signature = byteToHexString(bt).ToLower();
                strURL = strURL.Replace("[signature]", signature);

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; // comparable to modern browsers

                //HttpContext.Current.Response.Redirect(strURL, false);
                strResponse = strURL;

                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string byteToHexString(byte[] byData)
        {
            StringBuilder sb = new StringBuilder((byData.Length * 2));
            for (int i = 0; (i < byData.Length); i++)
            {
                int v = (byData[i] & 255);
                if ((v < 16))
                {
                    sb.Append('0');
                }

                sb.Append(v.ToString("X"));

            }
            return sb.ToString();
        }
    }
}
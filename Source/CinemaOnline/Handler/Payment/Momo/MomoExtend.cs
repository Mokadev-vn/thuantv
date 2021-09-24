using System;
using Newtonsoft.Json.Linq;

namespace CinemaOnline.Handler.Payment.Momo
{
    public class MomoExtend
    {
        public static string GenUrlPay(string orderInfo, string returnUrl, string notifyUrl, string amount, string orderId)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";

            string ipnUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
            string requestType = "captureWallet";

            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            const string partnerCode = "MOMO5RGX20191128";

            const string accessKey = "M8brj9K6E22vXoDB";
            const string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + returnUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.SignSha256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", returnUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }
            };

            string responseFromMomo = PaymentRequest.SendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return jmessage.GetValue("payUrl").ToString();

            //var crypto = new MoMoSecurity();
            ////sign signature SHA256
            //var signature = crypto.SignSha256(rawHash, serectKey);

            ////build body json request
            //var message = new JObject
            //{
            //    { "partnerCode", partnerCode },
            //    { "accessKey", accessKey },
            //    { "requestId", requestId },
            //    { "amount", amount },
            //    { "orderId", orderId },
            //    { "orderInfo", orderInfo },
            //    { "returnUrl", returnUrl },
            //    { "notifyUrl", notifyUrl },
            //    { "extraData", extraData },
            //    { "requestType", "captureMoMoWallet" },
            //    { "signature", signature }
            //};
            //var responseFromMomo = PaymentRequest.SendPaymentRequest(endPoint, message.ToString());

            //var jMessage = JObject.Parse(responseFromMomo);
            //return jMessage.GetValue("payUrl").ToString();
        }
    }
}
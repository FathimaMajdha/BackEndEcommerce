
using Razorpay.Api.Errors;
using System.Security.Cryptography;
using System.Text;



namespace BackendProject

{
    public static class RazorpayHelper
    {
        public static void VerifyPaymentSignature(Dictionary<string, string> attrs, string secret)
        {
            string payload = attrs["razorpay_order_id"] + "|" + attrs["razorpay_payment_id"];
            string expectedSig = GenerateSignature(payload, secret);
            if (expectedSig != attrs["razorpay_signature"])
                throw new SignatureVerificationError("Invalid signature");
        }


        private static string GenerateSignature(string payload, string secret)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(payload);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToHexString(hashmessage).ToLower(); 
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OverFy
{
    public class Keys
    {
        public static Keys LoadKeys()
        {
            var _auth = new Keys();

            using (var f = new FileStream("./tchubarubas.json", FileMode.Open))
            {
                var content = new StreamReader(f);
                var result = JsonConvert.DeserializeObject<Keys>(content.ReadToEnd());

                if (!String.IsNullOrEmpty(result.EncodedUser))
                {
                    result.u = Base64Decode(result.EncodedUser);
                }
                else
                {
                    result.u = null;
                }

                return result;
            }
        }

        public static void SaveKeys(Keys k)
        {
            using (var f = new StreamWriter("./tchubarubas.json", false))
            {
                k.EncodedUser = Base64Encode(k.u);

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(f, k);
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText + "|11/03 Robbie <3");
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes).Split('|')[0];
        }

        private string _client_id;

        public string ClientId
        {
            get { return _client_id; }
            set { _client_id = value; }
        }

        private string _secret_id;

        public string SecretId
        {
            get { return _secret_id; }
            set { _secret_id = value; }
        }

        private string user;

        public string EncodedUser
        {
            get { return user; }
            set { user = value; }
        }

        [JsonIgnore]
        private string _u;

        [JsonIgnore]
        public string u
        {
            get { return _u; }
            set { _u = value; }
        }

    }
}

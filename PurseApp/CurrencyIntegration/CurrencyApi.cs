using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PurseApp.CurrencyIntegration
{
    public class CurrencyApi : ICurrencyApi
    {
        private readonly HttpClient HttpClient;

        public CurrencyApi(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<ValCurs> GetApi()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var currencyResponseXml = await Get($"/scripts/XML_daily.asp");

            return currencyResponseXml;
        }

        private static ValCurs Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof(ValCurs)
                , new XmlRootAttribute("ValCurs")
            );
            using (var xmlStringReader = new StringReader(xml))
            {
                return (ValCurs)serializer.Deserialize(xmlStringReader);
            }
        }

        private async Task<ValCurs> Get(string path)
        {
            var response = await SendAsync(path, HttpMethod.Get);
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception("Failed");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = Deserialize(responseContent);
            return result;
        }

        private async Task<HttpResponseMessage> SendAsync(string path, HttpMethod method, HttpContent content =
            null)
        {
            var request = new HttpRequestMessage(method, path)
            {
                Content = content,
            };
            return await HttpClient.SendAsync(request);
        }
    }
}
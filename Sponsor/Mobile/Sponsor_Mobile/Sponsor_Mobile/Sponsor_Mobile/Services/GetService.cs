using Newtonsoft.Json;
using Sponsor_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sponsor_Mobile.Services
{
    public class GetService
    {
        private string url = "http://10.105.13.82:45455/api/get/";
        public HttpClient client { get; set; }
        public GetService()
        {
            client = new HttpClient();
        }

        public async Task<List<Competitor>> GetCompetitors(long? cid = null, long? sid = null)
        {
            try
            {
                var url = this.url + $"GetCompetitors?cid={cid}&sid={sid}";
                var res = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Competitor>>(res);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<Captcha> GetCaptcha()
        {
            try
            {
                var url = this.url + $"GetCaptcha";
                var res = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Captcha>(res);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<List<Currency>> GetCurrencies()
        {
            try
            {
                var url = this.url + $"GetCurrencies";
                var res = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Currency>>(res);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<HttpStatusCode> storeSponsor(Sponsorship sponsor)
        {
            try
            {
                var url = this.url + $"storeSponsor";

                var json = JsonConvert.SerializeObject(sponsor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(url, content);
                return res.StatusCode;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<string> ValidateCard(Card card)
        {
            try
            {
                var url = this.url + $"ValidateCard";

                var json = JsonConvert.SerializeObject(card);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<string>(result);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return string.Empty;
            }
        }
    }
}

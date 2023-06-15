using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RegisterMobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RegisterMobile.Services
{
    public class GetService
    {
        private string url = "http://10.105.13.82:45455/api/get/";
        public HttpClient client { get; set; }
        public GetService()
        {
            client = new HttpClient();
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                string url = this.url + $"login?username={username}&password={password}";

                var res = await client.GetAsync(url);

                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var com = JsonConvert.DeserializeObject<Competitor>(result);
                    App.Competitor = com;

                    return true;
                }

                return false;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }

        public async Task<List<Competition>> GetCompetitions(long comId)
        {
            try
            {
                string url = this.url + $"GetCompetitions?comId={comId}";

                var res = await client.GetAsync(url);

                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var coms = JsonConvert.DeserializeObject<List<Competition>>(result);

                    return coms;
                }

                return null;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<bool> ChangeStatus(long Id)
        {
            try
            {
                string url = this.url + $"ChangeStatus?Id={Id}";

                var res = await client.PostAsync(url, null);

                if (res.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }
    }
}

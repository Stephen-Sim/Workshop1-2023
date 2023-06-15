using Newtonsoft.Json;
using Session5_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Session5_Mobile.Services
{
    public class Session5Service
    {
        private string url = "http://10.105.13.82:45455/api/session5/";
        private HttpClient client;
        public Session5Service()
        {
            client = new HttpClient();
        }

        public async Task<bool> login(string username, string password)
        {
            string url = this.url + $"login?username={username}&password={password}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);

                App.User = user;

                return true;
            }

            return false;
        }

        public async Task<List<ServiceType>> getServiceTypes()
        {
            string url = this.url + $"getServiceTypes";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<ServiceType>>(res);

            return result;
        }

        public async Task<List<Service>> getServices(long id)
        {
            string url = this.url + $"getServices?id={id}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<Service>>(res);

            return result;
        }
        public async Task<int> getPostByDate(long serviceId, DateTime date)
        {
            string url = this.url + $"getPostByDate?serviceId={serviceId}&date={date}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<int>(res);

            return result;
        }

        public async Task<bool> storeAddonService(AddonServiceDetail addonServiceDetail)
        {
            string url = this.url + $"storeAddonService";

            var json = JsonConvert.SerializeObject(addonServiceDetail);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, data);
            
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var count = JsonConvert.DeserializeObject<int>(result);

                App.User.AddOnServiceCount = count;

                return true;
            }

            return false;
        }

        public async Task<List<AddonServiceDetail>> getAddonServiceDetails(long id)
        {
            string url = this.url + $"getAddonServiceDetails?id={id}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<AddonServiceDetail>>(res);

            return result;
        }

        public async Task<bool> delAddonServiceDetail(long id)
        {
            string url = this.url + $"delAddonServiceDetail?id={id}&addonServiceId={App.User.AddOnServiceId}";

            var res = await client.GetAsync(url);
            
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var count = JsonConvert.DeserializeObject<int>(result);

                App.User.AddOnServiceCount = count;

                return true;
            }

            return false;
        }

        public async Task<Coupon> checkCoupon(string code)
        {
            string url = this.url + $"checkCoupon?code={code}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var coupon = JsonConvert.DeserializeObject<Coupon>(result);
                return coupon;
            }

            return null;
        }

        public async Task<bool> proceedPayment(long addonSeriveId, long couponId)
        {
            string url = this.url + $"proceedPayment?addonSeriveId={addonSeriveId}&couponId={couponId}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);

                App.User = user;

                return true;
            }

            return false;
        }
    }
}

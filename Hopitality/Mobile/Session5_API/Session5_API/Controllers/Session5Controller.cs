using Session5_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Session5_API.Controllers
{
    public class Session5Controller : ApiController
    {
        public WSC2022SE_Session5Entities ent { get; set; }

        public Session5Controller()
        {
            ent = new WSC2022SE_Session5Entities();
        }

        [HttpGet]
        public object login(string username, string password)
        {
            var user = ent.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.AddonServices.Any(x => x.CouponID == null))
            {
                var addons = new AddonService
                {
                    GUID = Guid.NewGuid(),
                    UserID = user.ID,
                    CouponID = null
                };

                ent.AddonServices.Add(addons);
                ent.SaveChanges();
            }

            return Ok(new {
                user.ID,
                user.FullName,
                user.FamilyCount,
                AddOnServiceCount = user.AddonServices.FirstOrDefault(x => x.CouponID == null).AddonServiceDetails.Count(),
                AddOnServiceId = user.AddonServices.FirstOrDefault(x => x.CouponID == null).ID
            });
        }

        [HttpGet]
        public object getServiceTypes()
        {
            var serviceTypes = ent.ServiceTypes.ToList().Select(x => new
            {
                x.ID,
                x.Name,
                IconName = x.IconName.Substring(4).Replace('-', '_'),
                x.Description
            });

            return Ok(serviceTypes);
        }

        [HttpGet]
        public object getServices(long id)
        {
            var services = ent.Services.ToList().Where(x => x.ServiceTypeID == id).Select(x => new {
                x.ID,
                x.Name,
                x.Price,
                x.Duration,
                x.Description,
                x.DayOfWeek,
                x.DayOfMonth,
                x.DailyCap,
                x.BookingCap
            });

            return Ok(services);
        }

        [HttpGet]
        public object getPostByDate(long serviceId, DateTime date)
        {
            var service = ent.Services.FirstOrDefault(x => x.ID == serviceId);

            int count = (int)service.DailyCap - (service.AddonServiceDetails.Any(x => x.FromDate == date) ? service.AddonServiceDetails.Count(x => x.FromDate == date) : 0);

            return Ok(count);
        }

        [HttpPost]
        public object storeAddonService(AddonServiceDetail addonServiceDetail)
        {
            ent.AddonServiceDetails.Add(addonServiceDetail);
            ent.SaveChanges();

            int count = ent.AddonServiceDetails.Count(x => x.AddonServiceID == addonServiceDetail.AddonServiceID);

            return Ok(count);
        }

        [HttpGet]
        public object getAddonServiceDetails(long id)
        {
            var addonservicedetail = ent.AddonServiceDetails.Where(x => x.AddonServiceID == id).ToList().Select(x => new
            {
                x.ID,
                x.GUID,
                x.AddonServiceID,
                x.ServiceID,
                x.Price,
                x.FromDate,
                x.Notes,
                x.NumberOfPeople,
                x.isRefund,
                FromDateString = x.FromDate.ToString("dd/MM/yyyy"),
                ServiceName = x.Service.Name,
                IconName = x.Service.ServiceType.IconName.Substring(4).Replace('-', '_')
            });

            return Ok(addonservicedetail);
        }

        [HttpGet]
        public object delAddonServiceDetail(long id, long addonServiceId)
        {
            ent.AddonServiceDetails.Remove(ent.AddonServiceDetails.FirstOrDefault(x => x.ID == id));
            ent.SaveChanges();

            int count = ent.AddonServiceDetails.Count(x => x.AddonServiceID == addonServiceId);

            return Ok(count);
        }

        [HttpGet]
        public object checkCoupon(string code)
        {
            var coupon = ent.Coupons.FirstOrDefault(x => x.CouponCode == code);

            if (coupon == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                coupon.ID,
                coupon.DiscountPercent,
                coupon.MaximimDiscountAmount
            });
        }

        [HttpGet]
        public object proceedPayment(long addonSeriveId, long couponId)
        {
            var addonServices = ent.AddonServices.FirstOrDefault(x => x.ID == addonSeriveId);

            addonServices.CouponID = couponId;
            ent.SaveChanges();

            var addons = new AddonService
            {
                GUID = Guid.NewGuid(),
                UserID = addonServices.User.ID,
                CouponID = null
            };

            ent.AddonServices.Add(addons);
            ent.SaveChanges();

            var user = ent.Users.FirstOrDefault(x => x.ID == addons.UserID);

            return Ok((new
            {
                user.ID,
                user.FullName,
                user.FamilyCount,
                AddOnServiceCount = user.AddonServices.FirstOrDefault(x => x.CouponID == null).AddonServiceDetails.Count(),
                AddOnServiceId = user.AddonServices.FirstOrDefault(x => x.CouponID == null).ID
            }));
        }
    }
}

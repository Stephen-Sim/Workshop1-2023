﻿using Sponsor_API.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sponsor_API.Controllers
{
    public class GetController : ApiController
    {
        private Models.ASC2023_SponsorEntities1 ent;
        public GetController()
        {
            this.ent = new ASC2023_SponsorEntities1();
        }

        [HttpGet]
        public object GetCompetitors(long? cid = null, long? sid = null)
        {
            var com = ent.Competitors.ToList().Select(x => new
            {
                x.Id,
                x.SkillId,
                x.CountryID,
                x.Name,
                SkillName = x.Skill.Name,
                CountryName = x.Country.Name,
                RequiredAmount = (x.RequiredAmount - (ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Any() ? ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Sum(y => y.Amount * y.Currency.Rate) : 0.00m)).ToString("0.00"),
                Color = new Func<string>(() =>
                {
                    var percentage = (ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Any() ? ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Sum(y => y.Amount * y.Currency.Rate) : 0.00m) / x.RequiredAmount * 100;

                    if (percentage < 25)
                    {
                        return "Red";
                    }
                    else if (percentage < 50 && percentage < 75)
                    {
                        return "Orange";
                    }
                    else if (percentage >= 75 && percentage < 100)
                    {
                        return "Yellow";
                    }
                    else
                    {
                        return "Green";
                    }
                })()
            });

            if (cid != null)
            {
                com = com.Where(x => x.CountryID == cid).ToList();
            }

            if (sid != null)
            {
                com = com.Where(x => x.SkillId == sid).ToList();
            }

            return Ok(com);
        }

        [HttpGet]
        public object GetCaptcha()
        {
            Random r1 = new Random();
            int number = r1.Next(1000, 10000);
            var image = new Bitmap(300, 100);
            var font = new System.Drawing.Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            graphics.DrawString(number.ToString(), font, Brushes.Black, new Point(50, 50));

            // Add some noise to the captcha
            for (int i = 0; i < 30; i++)
            {
                // calculate line start and end point here using the Random class:
                int x0 = r1.Next(0, image.Width);
                int y0 = r1.Next(0, image.Height);
                int x1 = r1.Next(0, image.Width);
                int y1 = r1.Next(0, image.Height);
                graphics.DrawLine(new Func<Pen>(() =>
                {
                    if (i % 3 == 2)
                    {
                        return Pens.Red;
                    }
                    else if (i % 3 == 1)
                    {
                        return Pens.Blue;
                    }
                    else
                    {
                        return Pens.Green;
                    }
                })(), x0, y0, x1, x1);
            }

            byte[] arr;

            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);

                arr = memoryStream.ToArray();
            }

            return Ok(new
            {
                CaptchaByteArr = arr,
                CaptcapText = number.ToString()
            });
        }

        [HttpGet]
        public object GetCurrencies()
        {
            var cur = ent.Currencies.ToList().Select(x => new
            {
                x.Id,
                x.Name,
                x.Rate
            });

            return Ok(cur);
        }

        [HttpPost]
        public object storeSponsor(Sponsorship sponsorship)
        {
            try
            {
                ent.Sponsorships.Add(sponsorship);
                ent.SaveChangesAsync();

                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }

        }
    }
}

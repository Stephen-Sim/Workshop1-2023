using Registration_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Registration_API.Controllers
{
    public class GetController : ApiController
    {
        public ASC2023_RegistrationEntities ent { get; set; }

        public GetController()
        {
            ent = new ASC2023_RegistrationEntities();
        }

        [HttpGet]
        public object Login(string username, string password)
        {
            var com = ent.Competitors.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (com == null)
            {
                return BadRequest();
            }

            return new
            {
                com.Id,
                com.Name,
                Country = com.Country.Name,
                Skill = com.Skill.Name,
            };
        }

        [HttpGet]
        public object GetCompetitions(long comId)
        {
            var com = ent.Competitors.FirstOrDefault(x => x.Id == comId);

            var competitions = ent.Competitons.Where(x => x.SkillId == com.SkillId).ToList();

            foreach (var competition in competitions)
            {
                var tempAtt = ent.Attendances.FirstOrDefault(x => x.CompetitonId == competition.Id && x.CompetitorId == com.Id);

                if (tempAtt == null)
                {
                    var att = new Attendance
                    {
                        CompetitonId = competition.Id,
                        CompetitorId = com.Id,
                        Status = false
                    };

                    ent.Attendances.Add(att);
                    ent.SaveChanges();
                }
            }

            var atts = ent.Attendances.Where(x => x.CompetitorId == com.Id).OrderBy(x => x.Competiton.DateTime).ToList().Select(x => new
            {
                x.Id,
                Name = x.Competiton.Name,
                DateTime = x.Competiton.DateTime.ToString("dd/MM/yyyy HH:mm"),
                Location = x.Competiton.Location.Length > 35 ? x.Competiton.Location.Substring(0, 30) + "..." : x.Competiton.Location,
                Color = x.Status == true ? "Green" : "Red",
                x.Status
            }).ToList();

            return atts;
        }

        [HttpPost]
        public object ChangeStatus(long Id)
        {
            var att = ent.Attendances.FirstOrDefault(x => x.Id == Id);

            att.Status = !att.Status;
            ent.SaveChanges();

            return Ok();
        }
    }
}

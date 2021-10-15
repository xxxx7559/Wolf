using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using SignalRSample.DB;
using SignalRSample.Models;

namespace SignalRSample.Hubs
{
    public class SampleHub : Hub
    {
        private readonly IDbServices _dbServices;

        public SampleHub(IDbServices dbServices)
        {
            _dbServices = dbServices;
        }

        public async Task SendUserName(string user)
        {
            List<Employees> json = _dbServices.GetEmployees();
            try
            {
                var player = json.Where(e => e.Name != "Admin").ToList();
                if (player.Count >= 13)
                    throw new System.Exception("人數已達12人");
                json.Add(new Employees
                {
                    uuid = Context.ConnectionId,
                    Name = user
                });

                _dbServices.SetEmployees(json);

                await Clients.Caller.SendAsync("ReceiveMessage", user);
                var admin = json.FirstOrDefault(e => e.Name.ToUpper() == "ADMIN");
                if (admin != null)
                {
                    await Clients.Client(admin.uuid).SendAsync("SetPlayerName", user);
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
        }

        public Task SendAdmin(string user)
        {
            try
            {
                var json = _dbServices.CreateEmployees(new Employees
                {
                    uuid = Context.ConnectionId,
                    Name = user,
                });
                var employees = json.Where(e => e.Name.ToUpper() != "ADMIN").ToList();
                return Clients.Caller.SendAsync("SetAllPlayerName",
                JsonConvert.SerializeObject(employees));
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
        }

        public async Task GetEmpIdentity(string wolfTypeId)
        {
            var w_json = _dbServices.GetWolfTypesById(wolfTypeId);
            try
            {
                if (w_json == null) throw new System.Exception("牌庫異常");
                var wolfs = w_json.Value.OrderBy(e => Guid.NewGuid()).OrderBy(e => Guid.NewGuid()).ToList();

                List<Employees> e_json = _dbServices.GetEmployees();

                var employees = e_json.Where(e => e.Name.ToUpper() != "ADMIN").ToList();
                if (employees.Count == 0) throw new System.Exception("人數為0");
                var empCount = employees.Count - 1;
                foreach (var x in e_json)
                {
                    if (x.Name.ToUpper() == "ADMIN") continue;
                    x.Identity = wolfs[empCount];
                    empCount--;
                }
                _dbServices.SetEmployees(e_json);
                employees = e_json.Where(e => e.Name.ToUpper() != "ADMIN").ToList();
                await Clients.Caller.SendAsync("SetAllPlayerName", JsonConvert.SerializeObject(employees));
                for (int i = 0; i < employees.Count; i++)
                {
                    await Clients.Client(employees[i].uuid).SendAsync("SetEmpIdentity", employees[i].Identity);
                }
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            List<Employees> json = _dbServices.GetEmployees().Where(e => e.uuid != Context.ConnectionId).ToList();
            _dbServices.SetEmployees(json);

            var employees = json.Where(e => e.Name.ToUpper() != "ADMIN").ToList();
            var admin = json.Where(e => e.Name.ToUpper() == "ADMIN").FirstOrDefault();
            if (admin != null)
            {
                await Clients.Client(admin.uuid).SendAsync("SetAllPlayerName", JsonConvert.SerializeObject(employees));
            }
        }
    }
}
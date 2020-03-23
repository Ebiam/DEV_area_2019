using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Area_api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Area_api.Services
{


    public class WorkerHostedService : BackgroundService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly ApplicationDbContext _context;
        //static HttpClient client = new HttpClient();
        private Task doWorkTask;
        private bool completed = true;
        //HttpClient client = new HttpClient();
        //private readonly IHttpClientFactory _clientFactory;
        private static readonly HttpClient client = new HttpClient();
        private static readonly HttpClient client2 = new HttpClient();



        public WorkerHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory/*, IHttpClientFactory clientFactory/*, ApplicationDbContext context*/)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            //_clientFactory = clientFactory;
            //_context = context;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("http://localhost:64195/");

            client2.DefaultRequestHeaders.Accept.Clear();
            client2.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client2.BaseAddress = new Uri("http://localhost:64195/");
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            //Do your preparation (e.g. Start code) here
            if (completed == true)
            {
                _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero,
                    TimeSpan.FromSeconds(5));
            }
            /*while (!stopToken.IsCancellationRequested)
            {
                await DoWorkAsync();
            }*/
            //Do your cleanup (e.g. Stop code) here
            //return 
        }

        private /*async /*Task*/ void DoWorkAsync(object state)
        {
            //completed = false;
            //var count = Interlocked.Increment(ref executionCount);
            /*static*/
            
            _logger.LogInformation(
                "Timed Hosted Service is working"/*. Count: {Count}"/*, count*/);
            doWorkTask = DoWork();
            //completed = true;
        }
        private async Task DoWork()
        {
            completed = false;
            _logger.LogInformation("Do work");
            using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();     
            
                    var actions = _context.Actions;
                        var reactions = _context.Reactions;
                        var areas = _context.Areas;
                    var triggers = _context.Triggers;
                    var users = _context.Users;

                    foreach (var area in areas)
                    {
                        _logger.LogInformation("Trying area with id {nb}", area.id);
                        var user = users.Where(u => u.id == area.userId);
                        var _action = actions.Where(a => a.id == area.actionId);
                        var _reaction = reactions.Where(a => a.id == area.reactionId);
                        var _trigger = triggers.Where(t => t.areaId == area.id);


                    if (_trigger.Count() == 0)
                    {
                        _logger.LogInformation("NO TRIGGER");
                        continue;
                    }    
                        if (user != null && user.Count() > 0
                            && _action != null && _action.Count() > 0
                            && _reaction != null && _reaction.Count() > 0)
                        {
                            _logger.LogInformation("Area {nb} is valid", area.id);

                            var usr = user.First();
                            var isaction = false;
                            var action = _action.First();
                            var reaction = _reaction.First();
                            var trigger = (_trigger.Count() > 0) ? _trigger.First() : null;

                            if (trigger == null)
                                _logger.LogInformation("NO TRIGGER !");


                            _logger.LogInformation("action n {num} s {s}  reaction n {nn} s {ss}", action.id, action.serviceId, reaction.id, reaction.serviceId);
                            _logger.LogInformation("Checking for tokens");
                            var _usrToken_a = _context.Apis.Where(t => t.userId == usr.id && t.serviceId == action.serviceId);
                            if (_usrToken_a == null || _usrToken_a.Count() == 0)
                                continue;
                            _logger.LogInformation("tokens action ok");
                            var _usrToken_r = _context.Apis.Where(t => t.userId == usr.id && t.serviceId == reaction.serviceId);
                            if (_usrToken_r == null || _usrToken_r.Count() == 0)
                                continue;

                            _logger.LogInformation("tokens reaction ok");
                            //var client = _clientFactory.CreateClient();
                            

                            //HttpRequestMessage request = null;
                            //_logger.LogInformation("tokens ok");

                            //HttpResponseMessage response = null;
                            

                            switch (action.serviceId) // Verif action happened
                            {
                            case 1://Yammer
                                {
                                    switch (action.id)
                                    {
                                        case 6: // new notification
                                            {
                                                _logger.LogInformation("Trying request User Post new notif yams {d}", _usrToken_a.First().accessToken);

                                                /*var request = new HttpRequestMessage(HttpMethod.Get,
                                                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");*/

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://www.yammer.com/api/v1/streams/notifications");
                                                request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);


                                                _logger.LogInformation("Tr");
                                                var response = await client.SendAsync(request);
                                                _logger.LogInformation("Trdddd");
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    _logger.LogInformation("Status code ok");
                                                    var jsonstr = await response.Content.ReadAsStringAsync()/*.ToString()*/;
                                                    _logger.LogInformation("content {c}", jsonstr);
                                                    _logger.LogInformation(jsonstr);
                                                    //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int size = 0;
                                                    //_logger.LogInformation((string)a.name);
                                                    foreach (var a in jObj.items)
                                                    {
                                                        size++;
                                                        //_logger.LogInformation((string)a.link);
                                                    }
                                                    _logger.LogInformation("Action value was {v}", trigger.Ivalue);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = size;
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue < size)
                                                        {
                                                            _logger.LogInformation("ACTION HAPPEND");
                                                            isaction = true;
                                                            trigger.Ivalue = size;
                                                        }
                                                        else
                                                        {
                                                            trigger.Ivalue = size;
                                                            isaction = false;
                                                        }
                                                    }
                                                    trigger.init = false;
                                                    _context.Triggers.Update(trigger);
                                                    _context.SaveChanges();
                                                    _logger.LogInformation("Action is {s} and value is {v}", isaction, size);
                                                    break;
                                                }
                                                else
                                                    _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    break;
                                }
                            case 2: //Spotify
                                {
                                    switch (action.id)
                                    {
                                        case 7: // new playlist
                                            {
                                                _logger.LogInformation("Trying request User Post new playlist {d}", _usrToken_a.First().accessToken);

                                                /*var request = new HttpRequestMessage(HttpMethod.Get,
                                                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");*/

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/playlists");
                                                request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);


                                                _logger.LogInformation("Tr");
                                                var response = await client.SendAsync(request);
                                                _logger.LogInformation("Trdddd");
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    _logger.LogInformation("Status code ok");
                                                    var jsonstr = await response.Content.ReadAsStringAsync()/*.ToString()*/;
                                                    _logger.LogInformation("content {c}", jsonstr);
                                                    _logger.LogInformation(jsonstr);
                                                    //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int size = jObj.total;
                                                    //_logger.LogInformation((string)a.name);
                                                    
                                                    _logger.LogInformation("Action value was {v}", trigger.Ivalue);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = size;
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue < size)
                                                        {
                                                            _logger.LogInformation("ACTION HAPPEND");
                                                            isaction = true;
                                                            trigger.Ivalue = size;
                                                        }
                                                        else
                                                        {
                                                            trigger.Ivalue = size;
                                                            isaction = false;
                                                        }
                                                    }
                                                    trigger.init = false;
                                                    _context.Triggers.Update(trigger);
                                                    _context.SaveChanges();
                                                    _logger.LogInformation("Action is {s} and value is {v}", isaction, size);
                                                    break;
                                                }
                                                else
                                                    _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                                break;
                                            }
                                        case 10:
                                            {
                                                _logger.LogInformation("Trying request User Post new playlist {d}", _usrToken_a.First().accessToken);

                                                /*var request = new HttpRequestMessage(HttpMethod.Get,
                                                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");*/

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/playlists");
                                                request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);


                                                _logger.LogInformation("Tr");
                                                var response = await client.SendAsync(request);
                                                _logger.LogInformation("Trdddd");
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    _logger.LogInformation("Status code ok");
                                                    var jsonstr = await response.Content.ReadAsStringAsync()/*.ToString()*/;
                                                    _logger.LogInformation("content {c}", jsonstr);
                                                    _logger.LogInformation(jsonstr);
                                                    //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int size = jObj.total;
                                                    //_logger.LogInformation((string)a.name);

                                                    _logger.LogInformation("Action value was {v}", trigger.Ivalue);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = size;
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue > size)
                                                        {
                                                            _logger.LogInformation("ACTION HAPPEND");
                                                            isaction = true;
                                                            trigger.Ivalue = size;
                                                        }
                                                        else
                                                        {
                                                            trigger.Ivalue = size;
                                                            isaction = false;
                                                        }
                                                    }
                                                    trigger.init = false;
                                                    _context.Triggers.Update(trigger);
                                                    _context.SaveChanges();
                                                    _logger.LogInformation("Action is {s} and value is {v}", isaction, size);
                                                    break;
                                                }
                                                else
                                                    _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    break;
                                }
                                case (5): //Imgur
                                    {
                                        _logger.LogInformation("action service id 1");
                                        switch (action.id)
                                        {
                                            case 1: // User Post new image
                                                {
                                                    _logger.LogInformation("Trying request User Post new image");

                                                    /*var request = new HttpRequestMessage(HttpMethod.Get,
                                                    "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                                    request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                                    request.Headers.Add("User-Agent", "HttpClientFactory-Sample");*/

                                                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.imgur.com/3/account/me/images");
                                                    request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);



                                                    var response = await client.SendAsync(request);
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        _logger.LogInformation("Status code ok");
                                                        var jsonstr = await response.Content.ReadAsStringAsync()/*.ToString()*/;
                                                        _logger.LogInformation("content {c}", jsonstr);
                                                        _logger.LogInformation(jsonstr);
                                                        //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                        dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                        int size = 0;
                                                        //_logger.LogInformation((string)a.name);
                                                        foreach (var a in jObj.data)
                                                        {
                                                            size++;
                                                            //_logger.LogInformation((string)a.link);
                                                        }
                                                    _logger.LogInformation("Action value was {v}", trigger.Ivalue);
                                                    if (trigger.init == true)
                                                        {
                                                            trigger.Ivalue = size;
                                                        } else {
                                                        if (trigger.Ivalue < size)
                                                        {
                                                            isaction = true;
                                                            trigger.Ivalue = size;
                                                        }
                                                        else
                                                        {
                                                            trigger.Ivalue = size;
                                                            isaction = false;
                                                        }
                                                        }
                                                        trigger.init = false;
                                                        _context.Triggers.Update(trigger);
                                                        _context.SaveChanges();
                                                    _logger.LogInformation("Action is {s} and value is {v}", isaction, size);
                                                        break;
                                                    }
                                                    else
                                                        _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                                    //isaction = true;
                                                    break;
                                                }
                                            case 2: // User reputation
                                            {
                                                _logger.LogInformation("Trying request User reputation");

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.imgur.com/3/account/enzobiams");
                                                var clientid = _context.Services.Where(s => s.id == 5).First().clientId;
                                                request.Headers.Add("Authorization", "Client-ID " + /*_usrToken_a.First().accountId*/clientid);

                                                var response = await client.SendAsync(request);

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var jsonstr = await response.Content.ReadAsStringAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int rep = jObj.data.reputation;
                                                    _logger.LogInformation("content {c}", rep);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = rep;
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue < rep)
                                                        {
                                                            isaction = true;
                                                            trigger.Ivalue = rep;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (rep < trigger.Ivalue)
                                                        {
                                                            trigger.Ivalue = rep;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        //isaction = false;
                                                    }

                                                }
                                                else
                                                {
                                                    _logger.LogInformation("failed {d} {e}", response.StatusCode, response.Content.ToString());
                                                }

                                                break;
                                            }
                                            case 3: // Reply notification
                                            {
                                                _logger.LogInformation("Trying request Reply notification from {f}");

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.imgur.com/3/account/" + _usrToken_a.First().username + "/notifications/replies?new=true");
                                                request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);

                                                var response = await client.SendAsync(request);

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var jsonstr = await response.Content.ReadAsStringAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int not = 0;
                                                    foreach (var da in jObj.data)
                                                    {
                                                        not++;
                                                    }
                                                    _logger.LogInformation("there is {cx} notifs", not);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = not;
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue < not)
                                                        {
                                                            isaction = true;
                                                            trigger.Ivalue = not;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (not < trigger.Ivalue)
                                                        {
                                                            trigger.Ivalue = not;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        //isaction = false;
                                                    }
                                                }
                                                else
                                                {
                                                }
                                                break;
                                            }
                                        case 8:
                                            {
                                                _logger.LogInformation("Trying request User Post new image");

                                                /*var request = new HttpRequestMessage(HttpMethod.Get,
                                                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");*/

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.imgur.com/3/account/me/images");
                                                request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);



                                                var response = await client.SendAsync(request);
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    _logger.LogInformation("Status code ok");
                                                    var jsonstr = await response.Content.ReadAsStringAsync()/*.ToString()*/;
                                                    //_logger.LogInformation("content {c}", jsonstr);
                                                    //_logger.LogInformation(jsonstr);
                                                    //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int size = 0;
                                                    //_logger.LogInformation((string)a.name);
                                                    foreach (var a in jObj.data)
                                                    {
                                                        size++;
                                                        //_logger.LogInformation((string)a.link);
                                                    }
                                                    _logger.LogInformation("Action value was {v}", trigger.Ivalue);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = size;
                                                        trigger.init = false;
                                                        _context.Triggers.Update(trigger);
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        if (trigger.Ivalue > size)
                                                        {
                                                            _logger.LogInformation("ACTION HAPPPEND MAN");
                                                            isaction = true;
                                                            trigger.Ivalue = size;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (trigger.Ivalue != size)
                                                        {
                                                            trigger.Ivalue = size;
                                                            isaction = false;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                    }
                                                    
                                                    _logger.LogInformation("Action OW is {s} and value is now {v}", isaction, size);
                                                    break;
                                                }
                                                else
                                                    _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                                //isaction = true;
                                                break;
                                            }
                                            default:
                                                isaction = false;
                                                break;
                                        }

                                        break;
                                    }
                                case 3: // Trello
                                    {
                                    switch (action.id)
                                    {
                                        case 4: // New Board
                                            {
                                                _logger.LogInformation("Trying request New Board");

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.trello.com/1/members/me/?key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=" + _usrToken_a.First().accessToken);// 759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65");
                                                //request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);

                                                var response = await client.SendAsync(request);

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var jsonstr = await response.Content.ReadAsStringAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int boards = 0;
                                                    foreach (var b in jObj.idBoards)
                                                        boards++;
                                                    _logger.LogInformation("There is {n} boards", boards);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = boards;
                                                        trigger.init = false;
                                                        _context.Triggers.Update(trigger);
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        if (boards > trigger.Ivalue)
                                                        {
                                                            _logger.LogInformation("A new BOARD !!!!!");
                                                            trigger.Ivalue = boards;
                                                            isaction = true;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (boards < trigger.Ivalue)
                                                        {
                                                            trigger.Ivalue = boards;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }

                                                    }
                                                    
                                                    //jObj.idBoards
                                                }
                                                else
                                                {
                                                }
                                                //isaction = false;
                                                break;
                                            }
                                        case 9:
                                            {
                                                _logger.LogInformation("Trying request deleted Board");

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.trello.com/1/members/me/?key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=" + _usrToken_a.First().accessToken);// 759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65");
                                                //request.Headers.Add("Authorization", "Bearer " + _usrToken_a.First().accessToken);

                                                var response = await client.SendAsync(request);

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var jsonstr = await response.Content.ReadAsStringAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int boards = 0;
                                                    foreach (var b in jObj.idBoards)
                                                        boards++;
                                                    _logger.LogInformation("There is {n} boards", boards);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = boards;
                                                        trigger.init = false;
                                                        _context.Triggers.Update(trigger);
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        if (boards < trigger.Ivalue)
                                                        {
                                                            _logger.LogInformation("A deleted BOARD !!!!!");
                                                            trigger.Ivalue = boards;
                                                            isaction = true;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (boards > trigger.Ivalue)
                                                        {
                                                            trigger.Ivalue = boards;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }

                                                    }

                                                    //jObj.idBoards
                                                }
                                                else
                                                {
                                                }
                                                //isaction = false;
                                                break;
                                            }
                                        default:
                                            isaction = false;
                                            break;
                                    }
                                    break;
                                    }
                            case 4:
                                {
                                    switch (action.id)
                                    {
                                        case 5: // New repo
                                            {
                                                //"https://api.github.com/user/repos";
                                                _logger.LogInformation("Trying request New repo");

                                                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/repos");// 759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65");
                                                //client.DefaultRequestHeaders.Accept.Clear();
                                                request.Headers.Add("Authorization", "token " + _usrToken_a.First().accessToken);
                                                request.Headers.Add("Accept", "*/*");
                                                request.Headers.Add("User-Agent", "request");
                                                var response = await client.SendAsync(request);

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var jsonstr = await response.Content.ReadAsStringAsync();
                                                    dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                    int repos = 0;
                                                    foreach (var b in jObj)
                                                        repos++;
                                                    _logger.LogInformation("There is {n} repos", repos);
                                                    if (trigger.init == true)
                                                    {
                                                        trigger.Ivalue = repos;
                                                        trigger.init = false;
                                                        _context.Triggers.Update(trigger);
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        if (repos > trigger.Ivalue)
                                                        {
                                                            _logger.LogInformation("A new REPO !!!!!");
                                                            trigger.Ivalue = repos;
                                                            isaction = true;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }
                                                        else if (repos < trigger.Ivalue)
                                                        {
                                                            trigger.Ivalue = repos;
                                                            _context.Triggers.Update(trigger);
                                                            _context.SaveChanges();
                                                        }

                                                    }

                                                    //jObj.idBoards
                                                }
                                                else
                                                {
                                                    _logger.LogInformation("new repo failed st code {n}: {dsdd} {dsd} {ds}!!!!!", response.StatusCode, response.Content.ToString(), response.ReasonPhrase, response.RequestMessage);
                                                }
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }

                            if (isaction == true)
                            {
                                _logger.LogInformation("ACTION HAPPENED");
                                switch (reaction.serviceId)
                                {
                                    case (3): //Trello
                                        {
                                            switch (reaction.id)
                                            {
                                                case 1: // create board
                                                {
                                                    Random rnd = new Random();
                                                    int num = rnd.Next(1, 1113);

                                                    _logger.LogInformation("Trying request New Board {s} {dd}", area.reactionParam.Split(';')[0] + num.ToString(), _usrToken_r.First().accessToken);
                                                    string urll = "https://api.trello.com/1/boards/?name=" + area.reactionParam.Split(';')[0] + "&defaultLabels=true&defaultLists=false&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=" + _usrToken_r.First().accessToken;
                                                    //var request2 = new HttpRequestMessage(HttpMethod.Post, urll);
                                                    _logger.LogInformation("url {u}", urll);


                                                     client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                                                     client2.DefaultRequestHeaders.Add("Accept", "*/*");

                                                    var Parameters = new List<KeyValuePair<string, string>>
                                                    {
                                                        new KeyValuePair<string, string>("key", "7b20ee1cf6225cb2dab5bcd4025c7d83"),
                                                        new KeyValuePair<string, string>("token", _usrToken_r.First().accessToken),
                                                    };

                                                    var request2 = new HttpRequestMessage(HttpMethod.Post, urll)
                                                    {
                                                        Content = new FormUrlEncodedContent(Parameters)
                                                        
                                                    };



                                                    //request2.Headers.Add("Content-Type", "application/json");
                                                    //request2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                                                    //var postParams = new Dictionary<string, string> { { "token", _usrToken_r.First().accessToken }, { "key", "7b20ee1cf6225cb2dab5bcd4025c7d83" } };

                                                    //request2.Content = new StringContent("{\"token\":\"" + _usrToken_r.First().accessToken + "\",\"key\":\"" + "7b20ee1cf6225cb2dab5bcd4025c7d83" + "\"}", Encoding.UTF8, "application/json");
                                                    //_logger.LogInformation("url {u}", urll);
                                                    //var bdy = "{\"token\":\"" + _usrToken_r.First().accessToken + "\",\"key\":\"" + "7b20ee1cf6225cb2dab5bcd4025c7d83" + "\"}";
                                                    //dynamic jObj = JsonConvert.SerializeObject(bdy);
                                                    var response2 = await client2.SendAsync(request2);

                                                    if (response2.IsSuccessStatusCode)
                                                    {
                                                        _logger.LogInformation(" OK New Board");
                                                    }
                                                    else {
                                                        
                                                        _logger.LogInformation("KO New Board {d} {sd}", response2.StatusCode, response2.Content);
                                                    }
                                                    break;
                                                }
                                                default:
                                                    
                                                    break;
                                            }

                                            break;
                                        }
                                case 4: // Github
                                    {
                                        switch (reaction.id)
                                        {
                                            case 2: // create repo
                                                {
                                                    Random rnd = new Random();
                                                    int num = rnd.Next(1, 1113);
                                                    _logger.LogInformation("Trying request New Repo {s} {dd}", area.reactionParam.Split(';')[0] + num.ToString(), _usrToken_r.First().accessToken);
                                                    string urll = "https://api.github.com/user/repos"; // + _usrToken_r.First().accessToken;
                                                    //var request2 = new HttpRequestMessage(HttpMethod.Post, urll);
                                                    _logger.LogInformation("url {u}", urll);
                                                    
                                                    client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                    client2.DefaultRequestHeaders.Add("Accept", "*/*");

                                                    _logger.LogInformation("salut");
                                                    var Parameters = new List<KeyValuePair<string, string>>
                                                    {
                                                        new KeyValuePair<string, string>("name", area.reactionParam.Split(';')[0]),
                                                    };

                                                    var request2 = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/user/repos");/*new HttpRequestMessage(HttpMethod.Post, urll)
                                                    {
                                                        Content = new FormUrlEncodedContent(Parameters)

                                                        
                                                    };*/
                                                    string myJson = "{\"name\": \"" + area.reactionParam.Split(';')[0] + num.ToString() + "\"}";

                                                    //new FormUrlEncodedContent(Parameters);
                                                    request2.Headers.Add("Authorization", "token " + _usrToken_r.First().accessToken);
                                                    //request2.Headers.Add("Authorization", "token " + _usrToken_a.First().accessToken);
                                                    request2.Headers.Add("Accept", "*/*");
                                                    request2.Headers.Add("User-Agent", "PostmanRuntime/7.22.0");
                                                    request2.Content = new StringContent(myJson, Encoding.UTF8, "application/json");

                                                    _logger.LogInformation("mdr ca passe");
                                                    //var response2 = await client2.PostAsync(request2);
                                                    var response2 = await client2.SendAsync(request2);
                                                    _logger.LogInformation("OUIIIIIIIIIII");
                                                    if (response2.IsSuccessStatusCode)
                                                    {
                                                        _logger.LogInformation(" OK New Repo");
                                                    }
                                                    else
                                                    {
                                                        _logger.LogInformation("KO New Repo {d} {sd}", response2.StatusCode, response2.Content.ToString());
                                                        var jsonstr = await response2.Content.ReadAsStringAsync();
                                                        dynamic jObj = JsonConvert.DeserializeObject(jsonstr);
                                                        _logger.LogInformation("KO New Repo {d} {ds}", (string)jObj.message , response2.Content.ToString());
                                                    }
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                        break;
                                    }
                                    default:
                                        break;
                                }
                            }
                            isaction = false;
                        }
                    }
                }
            
            completed = true;
            _logger.LogInformation("Do work done");
            //return (response);
        }
    }









    #region snippet1
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly ApplicationDbContext _context;
        //static HttpClient client = new HttpClient();

        public TimedHostedService(ILogger<TimedHostedService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private /*async Task*/ void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
            /*var actions = _context.Actions;
            var reactions = _context.Reactions;
            var areas = _context.Areas;

            foreach (var area in areas)
            {
                _logger.LogInformation("Trying area with id {nb}", area.id);
                var user = _context.Users.Where(u => u.id == area.userId);
                var _action = _context.Actions.Where(a => a.id == area.actionId);
                var _reaction = _context.Reactions.Where(a => a.id == area.reactionId);

                if (user != null && user.Count() > 0
                    && _action != null && _action.Count() > 0
                    && _reaction != null && _reaction.Count() > 0)
                {
                    _logger.LogInformation("Area {nb} is valid", area.id);
                    
                    var usr = user.First();
                    var isaction = false;
                    var action = _action.First();
                    var reaction = _reaction.First();

                    _logger.LogInformation("action n {num} s {s}  reaction n {nn} s {ss}", action.id, action.serviceId, reaction.id, reaction.serviceId);
                    _logger.LogInformation("Checking for tokens");
                    var _usrToken_a = _context.Apis.Where(t => t.userId == usr.id && t.serviceId == action.serviceId);
                    if (_usrToken_a == null || _usrToken_a.Count() == 0)
                        break;
                    var _usrToken_r = _context.Apis.Where(t => t.userId == usr.id && t.serviceId == reaction.serviceId);
                    if (_usrToken_r == null || _usrToken_r.Count() == 0)
                        break;

                    _logger.LogInformation("tokens ok");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri("http://localhost:64195/");

                    HttpRequestMessage request = null;
                    _logger.LogInformation("tokens ok");
                    switch (action.serviceId) // Verif action happened
                    {
                        case (1): //Imgur
                            {
                                switch (action.id)
                                {
                                    case 1:
                                        {
                                            _logger.LogInformation("Trying request");
                                            request = new HttpRequestMessage(HttpMethod.Get,
                                            "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                                            request.Headers.Add("Accept", "application/vnd.github.v3+json");
                                            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                                            var response = await client.SendAsync(request);
                                            if (response.IsSuccessStatusCode)
                                            {
                                                _logger.LogInformation("Status code ok");
                                                var jsonstr = response.Content.ReadAsStringAsync().ToString();
                                                _logger.LogInformation("content {c}", jsonstr);
                                                //using var responseStream = await response.Content.ReadAsStreamAsync();
                                                dynamic jObj = JsonConvert.DeserializeObject(jsonstr);

                                                /*var Branches = await JsonSerializer.DeserializeAsync
                                                    <IEnumerable<GitHubBranch>>(responseStream);

                                            } else
                                                _logger.LogInformation("Wrong status code {nb}", response.StatusCode.ToString());

                                            isaction = true;
                                            break;
                                        }
                                    default:
                                        isaction = false;
                                        break;
                                }
                                
                                break;
                            }
                        case 2:
                            {
                                switch (action.id)
                                {
                                    case 1:
                                        isaction = true;
                                        break;
                                    default:
                                        isaction = false;
                                        break;
                                }

                                break;
                            }
                        default:
                            break;
                    }

                    if (isaction == true)
                    {
                        switch (reaction.serviceId)
                        {
                            case (1): //Imgur
                                {
                                    switch (reaction.id)
                                    {
                                        case 1:
                                            isaction = true;
                                            break;
                                        default:
                                            isaction = false;
                                            break;
                                    }

                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }*/
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    #endregion
}
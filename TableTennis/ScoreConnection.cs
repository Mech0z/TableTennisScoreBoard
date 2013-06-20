using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;

namespace TableTennis
{
    public class ScoreConnection : PersistentConnection
    {
        

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return base.OnConnected(request, connectionId);
        }
    }

    public class BroadCastMessage
    {
        public string MatchType { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
    }
}
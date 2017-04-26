using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Configuration;

namespace SignalRCharts
{
    [HubName("charthub")]
    public class ChartsHub:Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        [HubMethodName("sendlinechart")]
        public static void SendLineChart()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChartsHub>();
            context.Clients.All.updateLineChart();
        }

        [HubMethodName("sendariachart")]
        public static void SendAreaChart()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChartsHub>();
            context.Clients.All.updateAreaChart();
        }

        [HubMethodName("sendbarchart")]
        public static void SendBarChart()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChartsHub>();
            context.Clients.All.updateBarChart();
        }

        [HubMethodName("senddountchart")]
        public static void SendDountChart()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChartsHub>();
            context.Clients.All.updatedountChart();
        }
    }
}
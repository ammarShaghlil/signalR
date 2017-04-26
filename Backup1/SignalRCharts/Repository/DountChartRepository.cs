using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SignalRCharts.App_Code;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace SignalRCharts.Repository
{
    public class DountChartRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string GetDountChart()
        {
            var chartval = new List<Sumary>();
            string result;
            Random random = new Random();
            using (var connection = new SqlConnection(_connString))
            {
                SqlCommand CommandUsers;
                connection.Open();
                //var queries = new[] { @"SELECT [Year] ,[CompanyName] ,[Sells] FROM [ChartsDB].[dbo].[LineChart]" };
                //var q = string.Join("; ", queries);
                using (var CommandMessage = new SqlCommand(@"SELECT [CompanyName] ,[Sells] FROM [dbo].[DountChart]", connection))
                {
                    CommandMessage.Notification = null;

                    var MessageDependency = new SqlDependency(CommandMessage);
                    SqlDependency.Start(_connString);
                    MessageDependency.OnChange += new OnChangeEventHandler(dependencyMessages_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var readerMessage = CommandMessage.ExecuteReader();


                    while (readerMessage.Read())
                    {
                        var Generatedcolor = String.Format("#{0:X6}", random.Next(0x1000000));
                        chartval.Add(item: new Sumary { label = (string)readerMessage["CompanyName"], value = (double)readerMessage["Sells"], color = Generatedcolor, highlight = Generatedcolor });
                    }
                    result = new JavaScriptSerializer().Serialize(chartval);
                }

            }
            return result;
        }

        private void dependencyMessages_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;
            if (dependency != null) dependency.OnChange -= dependencyMessages_OnChange;

            if (e.Type == SqlNotificationType.Change)
            {
                ChartsHub.SendDountChart();
            }
        }
    }
}
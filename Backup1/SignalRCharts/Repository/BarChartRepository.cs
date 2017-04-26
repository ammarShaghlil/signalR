using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRCharts.App_Code;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;

namespace SignalRCharts.Repository
{
    public class BarChartRepository
    {

        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string GetBarChart()
        {
            var chartval = new List<datasets>();
            string result;
            Random random = new Random();
            using (var connection = new SqlConnection(_connString))
            {
                SqlCommand CommandUsers;
                connection.Open();
                using (var CommandMessage = new SqlCommand(@"SELECT [Year] ,[CompanyName] ,[Sells] FROM [dbo].[BarChart]", connection))
                {
                    CommandMessage.Notification = null;

                    var MessageDependency = new SqlDependency(CommandMessage);
                    SqlDependency.Start(_connString);
                    MessageDependency.OnChange += new OnChangeEventHandler(dependencyMessages_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var readerMessage = CommandMessage.ExecuteReader();
                    
                    List<string[]> years = new List<string[]>();
                    string[] a;

                    while (readerMessage.Read())
                    {
                        a = new string[3] { (readerMessage["Year"]).ToString(), (readerMessage["CompanyName"]).ToString(), (readerMessage["Sells"]).ToString() };
                        years.Add(a);
                    }

                    List<string> y = new List<string>();
                    List<string> coms = new List<string>();
                    for (int i = 0; i < years.Count; i++)
                    {
                        y.Add((years[i])[0]);
                        coms.Add((years[i])[1]);
                    }
                    y = y.Distinct().ToList();
                    coms = coms.Distinct().ToList();
                    List<string[]> salList = new List<string[]>();
                    string[] salarr;

                    for (int i = 0; i < coms.Count; i++)
                    {
                        float res = years.Count / coms.Count;
                        int rounded = (int)Math.Ceiling(res);
                        salarr = new string[rounded];
                        string cc = coms[i];
                        int count = 0;
                        for (int j = 0; j < years.Count; j++)
                        {
                            if (cc == (years[j])[1])
                            {
                                salarr[count] = ((years[j])[2]);
                                count++;
                            }
                        }
                        salList.Add(salarr);
                    }

                    Charts chart = new Charts();
                    chart.labels = y.ToArray();
                    datasets data;
                    for (int i = 0; i < coms.Count; i++)
                    {
                        var Generatedcolor = String.Format("#{0:X6}", random.Next(0x1000000));
                        data = new datasets();
                        data.label = coms[i];
                        data.fillColor = Generatedcolor;
                        data.strokColor = Generatedcolor;
                        data.pointColor = Generatedcolor;
                        data.pointHighlightFill = Generatedcolor;
                        data.pointHighlightStroke = Generatedcolor;
                        data.data = salList[i];

                        chartval.Add(data);
                    }
                    chart.datasets = chartval;

                    string ch = new JavaScriptSerializer().Serialize(chart);

                    result = ch;
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
                ChartsHub.SendBarChart();
            }
        }

    }
}
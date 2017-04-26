using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using SignalRCharts.App_Code;
using System.Web.Script.Serialization;

namespace SignalRCharts.Repository
{
    public class SumaryRepository
    {

        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string GetSumary()
        {
            var chartval = new List<datasets>();
            string result;
            Random random = new Random();
            using (var connection = new SqlConnection(_connString))
            {
                SqlCommand CommandUsers;
                connection.Open();
                //var queries = new[] { @"SELECT [Year] ,[CompanyName] ,[Sells] FROM [ChartsDB].[dbo].[LineChart]" };
                //var q = string.Join("; ", queries);
                using (var CommandMessage = new SqlCommand(@"SELECT [Year] ,[CompanyName] ,[Selles] FROM [dbo].[AreaChart]", connection))
                {
                    //CommandUsers = new SqlCommand(@"SELECT [UserID] ,[UserName] ,[UserFullName] ,[UserPhone] ,[RegisterDate] ,[SectionID] ,[Salary] FROM [BlogDemos].[dbo].[User]" , connection);
                    CommandMessage.Notification = null;
                    //CommandUsers.Notification = null;

                    var MessageDependency = new SqlDependency(CommandMessage);
                    //var UserDependency = new SqlDependency(CommandUsers);
                    SqlDependency.Start(_connString);
                    MessageDependency.OnChange += new OnChangeEventHandler(dependencyMessages_OnChange);
                    //UserDependency.OnChange += new OnChangeEventHandler(dependencyUsers_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var readerMessage = CommandMessage.ExecuteReader();
                    //while (readerMessage.Read())
                    //{
                    //    messages.Add(item: new Messages { MessageID = (int)readerMessage["MessageID"], Message = (string)readerMessage["Message"], EmptyMessage = readerMessage["EmptyMessage"] != DBNull.Value ? (string)readerMessage["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(readerMessage["Date"]) });
                    //}

                    //readerMessage.NextResult();


                    //while (readerMessage.Read())
                    //{
                    //    users.Add(item: new Users { UserID = (int)readerMessage["UserID"], UserName = (string)readerMessage["UserName"], UserFullName = (string)readerMessage["UserFullName"], UserPhone = (string)readerMessage["UserPhone"], RegisterDate = Convert.ToDateTime(readerMessage["RegisterDate"]), SectionID = (int)readerMessage["SectionID"], Salary = (string)readerMessage["Salary"] });
                    //}

                    //readerMessage.NextResult();
                    //var random = new Random();
                    //while (readerMessage.Read())
                    //{
                    //    var Generatedcolor = String.Format("#{0:X6}", random.Next(0x1000000));
                    //    salary.Add(item: new Salary { label = (string)readerMessage["CompanyName"], value = (double)readerMessage["CompanySalary"], color = Generatedcolor, highlight = Generatedcolor });
                    //}
                    //tring jsonSalary = new JavaScriptSerializer().Serialize(salary);

                    //readerMessage.NextResult();

                    List<string[]> years = new List<string[]>();
                    string[] a;

                    while (readerMessage.Read())
                    {
                        a = new string[3] { (readerMessage["Year"]).ToString(), (readerMessage["CompanyName"]).ToString(), (readerMessage["Selles"]).ToString() };
                        //var Generatedcolor = String.Format("#{0:X6}", random.Next(0x1000000));
                        years.Add(a);
                    }
                    //List<string> colors = new List<string>();

                    //readerMessage.NextResult();
                    //while (readerMessage.Read())
                    //{
                    //    colors.Add((readerMessage["ColorHex"]).ToString());
                    //}


                    //string jsonYearSalary = new JavaScriptSerializer().Serialize(salary);
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

                    /*
                     *  labels: ["2015","2014","2013"],
                            datasets: [
                            {
                                label: "Digital Goods",
                                fillColor: "#ffffff",
                                strokeColor: "rgba(60,141,188,0.8)",
                                pointColor: "#3b8bba",
                                pointStrokeColor: "rgba(60,141,188,1)",
                                pointHighlightFill: "#fff",
                                pointHighlightStroke: "rgba(60,141,188,1)",
                                data: [90,0,50]
                            }
                     */

                    //connection.Close();
                    //connection.Open();
                    //var readerUser = CommandUsers.ExecuteReader();
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
                ChartsHub.SendAreaChart();
            }
        }

    }
}
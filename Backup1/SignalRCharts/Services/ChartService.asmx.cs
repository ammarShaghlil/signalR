using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using SignalRCharts.Repository;

namespace SignalRCharts.Services
{
    /// <summary>
    /// Summary description for ChartService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ChartService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public string GetAreaChart()
        {

            try
            {
                SumaryRepository _messageRepository = new SumaryRepository();
                return _messageRepository.GetSumary();
            }
            catch (Exception ex)
            {
                
                return "Faild";
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetBarChart()
        {

            try
            {
                BarChartRepository _barchartrepository = new BarChartRepository();
                return _barchartrepository.GetBarChart();
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetLineChart()
        {

            try
            {
                LineChartRepository _linechartrepository = new LineChartRepository();
                return _linechartrepository.GetSumary();
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetDonutChart()
        {

            try
            {
                DountChartRepository _messageRepository = new DountChartRepository();
                return _messageRepository.GetDountChart();
            }
            catch (Exception ex)
            {

                return "Faild";
            }
        }
    }
}

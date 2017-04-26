using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRCharts.App_Code
{
    public class datasets
    {
        public string label { get; set; }

        public string fillColor { get; set; }

        public string strokColor { get; set; }

        public string pointColor { get; set; }

        public string pointStrokeColor { get; set; }

        public string pointHighlightFill { get; set; }

        public string pointHighlightStroke { get; set; }

        public string[] data { get; set; }
    }
}
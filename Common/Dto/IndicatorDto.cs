using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsDto
{
    public class IndicatorDto
    {
        public string id { get; set; }
        public string title { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int value { get; set; }
        public string dateValue { get; set; }

    }
}

namespace Common.Models
{
    using System;
    public class Measure : IMeasure
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

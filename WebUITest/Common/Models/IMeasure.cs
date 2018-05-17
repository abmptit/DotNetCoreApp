namespace Common.Models
{
    using System;
    public interface IMeasure
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}

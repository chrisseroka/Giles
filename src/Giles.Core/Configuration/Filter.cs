using System;
using System.Collections.Generic;
using System.Linq;

namespace Giles.Core.Configuration
{
    [Serializable]
    public class Filter
    {
        public Filter() { }

        public Filter(string convertToFilter)
        {
            foreach (var entry in FilterLookUp.Where(entry => convertToFilter.Contains(entry.Key)))
            {
                Type = entry.Value;
                Name = convertToFilter.Replace(entry.Key, string.Empty).Trim();

                break;
            }

            if (!string.IsNullOrWhiteSpace(Name)) return;

            Name = convertToFilter.Trim();
            Type = FilterType.Inclusive;
        }

        public string Name { get; set; }
        public FilterType Type { get; set; }

        public static readonly IDictionary<string, FilterType> FilterLookUp = new Dictionary<string, FilterType>
                                                                           {
                                                                               {"-i", FilterType.Inclusive},
                                                                               {"-e", FilterType.Exclusive}
                                                                           };
    }

    public enum FilterType
    {
        Inclusive,
        Exclusive
    }
}
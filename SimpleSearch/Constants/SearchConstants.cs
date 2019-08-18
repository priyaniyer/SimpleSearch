using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSearch.Constants
{
    public class SearchConstants
    {
        public const int NumberSearchResults = 100;
        public const string URLRegex = "(<a href=\"/)(\\w+[a-zA-Z0-9.-?=/]*)";
    }
}

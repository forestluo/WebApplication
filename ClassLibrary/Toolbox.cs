using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLDBLibrary
{
    public static class Toolbox
    {
        public static object[] DictionaryToObjectArrary(Dictionary<string, object> dict)
        {
            object[] arr = new object[dict.Keys.Count];
            int i = 0;
            foreach (string key in dict.Keys)
            {
                arr[i] = dict[key];
                i++;
            }
            return arr;
        }
    }
}

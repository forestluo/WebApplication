using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLDBLibrary
{
    public static class DataInterfaceCache
    {
        public static string CurrentDataInterfaceName = null;

        public static DataInterface CurrentDataInterface { get { return DataInterfaceCache.GetDataInterface(CurrentDataInterfaceName); } }

        private static Dictionary<string, DataInterface> dataInterfaces = new Dictionary<string,DataInterface>();

        public static void AddDataInterface(string name, string connectionString)
        {
            DataInterface di = new DataInterface(connectionString);
            DataInterfaceCache.dataInterfaces.Add(name, di);
        }

        public static DataInterface GetDataInterface(string name)
        {
            return DataInterfaceCache.dataInterfaces[name];
        }
    }
}

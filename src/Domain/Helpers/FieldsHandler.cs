using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Reflection;

namespace Domain.Helpers
{
    public class FieldsHandler
    {
        public static List<Tuple<string, string>> GetSettedProperties(Fields field)
        {
            Type type = typeof(Fields);
            List<Tuple<string, string>> props = new List<Tuple<string, string>>();

            foreach (PropertyInfo pi in type.GetProperties())
            {
                object value = pi.GetValue(field, null);
                if (value != null && value.ToString() != "0")
                {
                    props.Add(new Tuple<string, string>(pi.Name, value.ToString()));
                }
            }
            return props;
        }
    }
}

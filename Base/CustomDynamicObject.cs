using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Base
{
    public class CustomDynamicObject : DynamicObject
    {
        // The inner dictionary to store field names and values.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        // Get the property value.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        // Set the property value.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;
            return true;
        }

        // Set the property value by index.
        public override bool TrySetIndex(
            SetIndexBinder binder, object[] indexes, object value)
        {
            string index = (string)indexes[0];

            // If a corresponding property already exists, set the value.
            if (dictionary.ContainsKey(index))
                dictionary[index] = value;
            else
                // If a corresponding property does not exist, create it.
                dictionary.Add(index, value);
            return true;
        }

        // Get the property value by index.
        public override bool TryGetIndex(
            GetIndexBinder binder, object[] indexes, out object result)
        {

            string index = (string)indexes[0];
            return dictionary.TryGetValue(index, out result);
        }
    }
}

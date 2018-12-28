using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Base
{
    public static class Utilities
    {
        public static string ClassToSelectedFields<T>(){
            var Properties = typeof(T).GetProperties();
            int PropertyLength = Properties.Length;
            string Fields = "";
            for (int i = 0; i < PropertyLength; i++)
            {
                Fields += Properties[i].Name;
                if (i < PropertyLength - 1)
                {
                    Fields += ",";
                }
            }
            return Fields;
        }

        public static bool CheckExceptPaths(List<string> ExceptPaths, string Path)
        {
            int ExceptPathLength = ExceptPaths.Count;
            bool status = false;
            for(int i = 0; i < ExceptPathLength; i++)
            {
                status = CheckExceptPath(ExceptPaths[i], Path);
                if(status == true)
                {
                    break;
                }
            }
            return status;
        }

        private static bool CheckExceptPath(string ExceptPath, string Path)
        {
            string TempPath = Path.ToLower();
            if(TempPath.IndexOf("/") == 0)
            {
                TempPath = TempPath.Substring(1);
            }
            string[] TempPathSegments = TempPath.Split("/");
            int TempPathSegmentLength = TempPathSegments.Length;

            string TempExceptPath = ExceptPath.ToLower();
            if (TempExceptPath.IndexOf("/") == 0)
            {
                TempExceptPath = TempExceptPath.Substring(1);
            }
            string[] TempExceptPaths = TempExceptPath.Split("/");
            int TempExceptPathLength = TempExceptPaths.Length;
            List<int> status = new List<int>();
            for (int i = 0; i < TempPathSegmentLength; i++)
            {
                status.Add(0);
            }
            if(TempExceptPathLength == TempPathSegmentLength)
            {
                for (int i = 0; i < TempPathSegmentLength; i++)
                {
                    if(TempExceptPaths[i].IndexOf("{") == 0 && TempExceptPaths[i].IndexOf("}") == TempExceptPaths[i].Length - 1)
                    {
                        status[i] = 1;
                    }
                    else if(TempPathSegments[i] == TempExceptPaths[i])
                    {
                        status[i] = 1;
                    }
                }
            }

            bool realStatus = false;
            int lastStatus = 1;
            status.ForEach(s =>
            {
                lastStatus *= s;
            });

            if(lastStatus == 1)
            {
                realStatus = true;
            }

            return realStatus;
        }

        public static void Initiate<T>(DbSet<T> DbSetInstance, List<JObject> Objects) where T : class
        {
            var Properties = typeof(T).GetProperties();
            int PropertyLength = Properties.Length;
            Objects.ForEach(O =>
            {
                JObject Temp = new JObject();
                for (int i = 0; i < PropertyLength; i++)
                {
                    if (Properties[i].Name != "Id")
                    {
                        if (Properties[i].Name == "CreatedDate")
                        {
                            Temp.Add(Properties[i].Name, JToken.FromObject(DateTime.Now));
                        }
                        else if (Properties[i].Name == "UpdatedDate")
                        {
                            Temp.Add(Properties[i].Name, JToken.FromObject(DateTime.Now));
                        }
                        else
                        {
                            var Value = O[Properties[i].Name];
                            if (Value == null)
                            {
                                Temp.Add(Properties[i].Name, JValue.CreateNull());
                            }
                            else
                            {
                                Temp.Add(Properties[i].Name, JToken.FromObject(Value));
                            }
                        }
                    }
                }
                string JsonString = JsonConvert.SerializeObject(Temp);
                T RealTemp = JsonConvert.DeserializeObject<T>(JsonString);
                DbSetInstance.Add(RealTemp);
            });
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}

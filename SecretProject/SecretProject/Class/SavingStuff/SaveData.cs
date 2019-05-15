using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.ItemStuff.Items;

namespace SecretProject.Class.SavingStuff
{
    [Serializable]
    [XmlRoot("SaveData")]
    public static class SaveData
    {
        #region player


        #endregion

        //Generic Loader
        public static void LoadItems<T1, T2>(this Dictionary<T1, T2> dic, Stream stream)
        {
            var reader = new XmlSerializer(typeof(List<Entry<T1, T2>>));
            var list = (List<Entry<T1, T2>>)reader.Deserialize(stream);

            foreach(var item in list)
            {
                //if(!list.Contains(item))
               // {
                    dic.Add(item.Key, item.Value);
               // }
                
            }
        }


        public static void Load<T1, T2>(this Dictionary<T1, T2> dic, string path)
        {
            using (var f = File.OpenRead(path))
            {
                LoadItems(dic, f);
            }
        }

        public static void Save<T1, T2>(this Dictionary<T1, T2> dic, Stream stream)
        {
            var list = new List<Entry<T1, T2>>();
            foreach(var pair in dic)
            {
                list.Add(new Entry<T1, T2> { Key = pair.Key, Value = pair.Value });
            }

            var writer = new XmlSerializer(typeof(List<Entry<T1, T2>>));
            writer.Serialize(stream, list);
        }

        public static void Save<T1, T2>(this Dictionary<T1, T2> dic, string path)
        {
            using (var f = File.OpenWrite(path))
            {
                Save(dic, f);

            }
        }



                //used so we can get around serializing dictionary items...
        public class Entry<T1, T2>
        {
            [XmlAttribute]
            public T1 Key { get; set; }
            public T2 Value { get; set; }


        }


    }
}


using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Storage;
using UnityEngine;

namespace Serialization
{
    public static class Serializator
    {
        private static readonly string filePath = Path.Combine(Application.persistentDataPath, "Data");

        public static void Serialization(IEnumerable<IBaseHologramObject> objects) 
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    var fileName = Path.Combine(filePath, $"{obj.HologramData.Id}.bin");
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    using var saveFs = File.Create(fileName);
                    var memo = new Memo(obj);
                    var serializer = new BinaryFormatter();
                    serializer.Serialize(saveFs, memo);
                }
            }
        }

        public static void Serialization(IBaseHologramObject obj)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (obj != null)
            {
                var fileName = Path.Combine(filePath, $"{obj.HologramData.Id}.bin");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using var saveFs = File.Create(fileName);
                var memo = new Memo(obj);
                var serializer = new BinaryFormatter();
                serializer.Serialize(saveFs, memo);
            }
        }

        public static void Serialization<T>(string name, T value)
        {
            var fp = Path.Combine(filePath, "SingleData");

            if (!Directory.Exists(fp))
            {
                Directory.CreateDirectory(fp);
            }

            var fileName = Path.Combine(fp, $"{name}.bin");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using var saveFs = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(saveFs, value);
        }

        public static T Deserialization<T>(string name)
        {
            var fp = Path.Combine(filePath, "SingleData");

            if (!Directory.Exists(fp))
            {
                Directory.CreateDirectory(fp);
                return default;
            }

            var fileName = Path.Combine(fp, $"{name}.bin");

            if (!File.Exists(fileName))
            {
                return default;
            }

            using var openFs = File.OpenRead(fileName);
            var deserializer = new XmlSerializer(typeof(T));
            return (T) deserializer.Deserialize(openFs);

        }

        public static T[] Deserialization<T>()
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                return null;
            }

            var files = Directory.GetFiles(filePath);
            if (files.Length == 0) return null;

            var memos = new T[files.Length];
            for (var m = 0; m < memos.Length; m++)
            {
                var fileName = files[m];
                if (File.Exists(fileName))
                {
                    using var openFs = File.OpenRead(fileName);
                    var deserializer = new BinaryFormatter();
                    memos[m] = (T) deserializer.Deserialize(openFs);
                }
            }

            return memos;
        }

        public static void Delete(string name)
        {
            var fileName = Path.Combine(filePath, $"{name}.bin");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}

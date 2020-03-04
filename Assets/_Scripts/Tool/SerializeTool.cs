/*文件名：SerializeTool.cs
 * 作者：YZY 
 * 说明：序列化工具，将数据序列化为XML或json；将XML或json反序列化为数据
 * 上次修改时间：2019/12/16 15：31：50 *
 * */
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;//链接：https://pan.baidu.com/s/1Yxbesci1zRpTdZV1VJSEDQ 提取码：yoyw
using System.Collections.Generic;
using System;

public class SerializeTool
{
    public static SerializeType serializeType = SerializeType.Json;
    public static void SerializeToFile<T>(T _data, string _path)
    {
        switch (serializeType)
        {
            case SerializeType.Json:
                SerializeToJson(_data, _path);
                break;
            case SerializeType.XML:
                SerializeToXML(_data, _path);
                break;
            case SerializeType.Binary:
                SerializeToBinary(_data, _path);
                break;
        }
    }
    public static T DeSerializeFromFile<T>(string _path)
    {
        switch (serializeType)
        {
            case SerializeType.Json:
                return DeSerializeFromJson<T>(_path);
            case SerializeType.XML:
                return DeSerializeFromXML<T>(_path);
            case SerializeType.Binary:
                return DeSerializeFromBinary<T>(_path);
        }
        return default;
    }


    #region XML
    /// <summary>
    /// 将数据序列化为XML：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">xml保存路径</param>
    static void SerializeToXML<T>(T _data, string _path)
    {
        FileStream fs = new FileStream(_path + ".xml", FileMode.OpenOrCreate);
        XmlSerializer xml = new XmlSerializer(typeof(T));
        xml.Serialize(fs, _data);
        fs.Close();
    }
    /// <summary>
    /// 将XML反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    static T DeSerializeFromXML<T>(string _path)
    {
        FileStream fs = new FileStream(_path + ".xml", FileMode.Open);
        XmlSerializer bf = new XmlSerializer(typeof(T));
        T _data = (T)bf.Deserialize(fs);
        fs.Close();
        return _data;
    }
    #endregion

    #region 二进制
    /// <summary>
    /// 将数据序列化为二进制：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">二进制文件保存路径</param>
    static void SerializeToBinary<T>(T _data, string _path)
    {
        FileStream fs = new FileStream(_path + ".data", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, _data);
        fs.Close();
    }
    /// <summary>
    /// 将二进制文件反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    static T DeSerializeFromBinary<T>(string _path)
    {
        FileStream fs = new FileStream(_path + ".data", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        T _data = (T)bf.Deserialize(fs);
        fs.Close();
        return _data;
    }
    #endregion

    #region Json
    /// <summary>
    /// 将数据序列化为Json文件：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">Json文件保存路径</param>
    static void SerializeToJson<T>(T _data, string _path)
    {
        var _text = JsonConvert.SerializeObject(_data);
        File.WriteAllText(_path + ".json", _text);
    }
    /// <summary>
    /// 将Json文件反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    static T DeSerializeFromJson<T>(string _path)
    {
        var _fullPath = _path + ".json";
        if (File.Exists(_fullPath))
        {
            var _text = File.ReadAllText(_path + ".json");
            T _data = JsonConvert.DeserializeObject<T>(_text);
            return _data;
        }
        else
        {
          
            var file = File.Create(_fullPath);
            file.Close();
            //var fs=new FileStream(_fullPath, FileMode.Create);
            //fs.Close();
            SerializeToJson<T>(default, _path);
            return default;
        }
    }
    public static string SerializeToJsonString<T>(T _data)
    {
        return JsonConvert.SerializeObject(_data);
    }
    public static T DeSerializeFromJsonString<T>(string _text)
    {
        return JsonConvert.DeserializeObject<T>(_text);
    }
    #endregion
    public static void InitFolder()
    {
        var _enum = Enum.GetValues(typeof(StaticData.FilePathType));
        foreach (StaticData.FilePathType n in _enum)
        {
            var _path = StaticData.GetDataFloderPath(n);
            DirectoryInfo i = new DirectoryInfo(_path);
            if (!i.Exists)
            {
                Directory.CreateDirectory(_path);
            }
        }
    }
    public static void DeleteFile(string _path)
    {
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }
}
public enum SerializeType
{
    Json,
    Binary,
    XML,
}

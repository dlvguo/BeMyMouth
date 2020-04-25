using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class FileUtil
{
    public enum FileType
    {
        Txt,
        Json
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="content">文件内容</param>
    /// <param name="fileType">文件类型</param>
    public static void SaveFile(string path, string filename, string content, FileType fileType)
    {
        string filePath = string.Format("{0}{1}.{2}", path, filename, fileType.ToString());
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine(content);
            }
        }
    }

    /// <summary>
    /// 读取文件内容
    /// </summary>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public static string ReadFile(string path, string filename, FileType fileType)
    {
        string filePath = string.Format("{0}{1}.{2}", path, filename, fileType.ToString());
        return ReadFile(filePath);
    }

    public static string ReadFile(string filePath)
    {
        string content = string.Empty;

        if (File.Exists(filePath))
        {
            content = File.ReadAllText(filePath);
        }
        return content;
    }

    /// <summary>
    /// 获取文件名字
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public static List<string> GetDirectoryFilesNames(string directoryPath, FileType fileType)
    {
        List<string> names = new List<string>();
        var filenames = Directory.GetFiles(directoryPath, "*." + fileType.ToString());
        foreach (var filename in filenames)
        {
            var str = Regex.Replace(filename, @".+[/\\]", "");
            str = Regex.Replace(str, @"\..*", "");
            names.Add(str);
        }
        return names;
    }


    //获取文件绝对路径名称
    public static List<string> GetDirectoryFilesNamesAbsolutePath(string directoryPath, FileType fileType)
    {
        List<string> paths = new List<string>();
        var files = Directory.GetFiles(directoryPath, "*." + fileType.ToString());
        paths.AddRange(files);
        return paths;
    }

    //获取某文件夹下所有某后缀文件内容
    public static List<string> ReadFilesContent(string directoryPath, FileType fileType)
    {
        List<string> contents = new List<string>();
        var filesnames = GetDirectoryFilesNamesAbsolutePath(directoryPath, fileType);
        foreach (var filename in filesnames)
        {
            var content = ReadFile(filename);
            contents.Add(content);
        }
        return contents;
    }

}

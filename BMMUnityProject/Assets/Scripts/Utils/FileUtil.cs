using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// 读取文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public static string ReadFile(string path, string filename, FileType fileType)
    {
        string filePath = string.Format("{0}{1}.{2}", path, filename, fileType.ToString());
        string content = string.Empty;
        if (File.Exists(filePath))
        {
            content = File.ReadAllText(filePath);
        }
        return content;
    }
}

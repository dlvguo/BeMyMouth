using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtil : MonoBehaviour
{

    //读取文本
    public static TextAsset ReadText(string path)
    {
        TextAsset txt = Resources.Load(path) as TextAsset;
        return txt;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//删除贴图
public class DestroyTexture : MonoBehaviour
{
    private bool IsNull = true;

    void Update()
    {
        if (GameObject.Find("LeapController(Clone)"))
            IsNull = false;
        else
            IsNull = true;
    }

    public void OnSendClick()
    {
        if (IsNull)
            return;
        if (CreateText.instance.texts == null)
            return;
        TextCrete.instance.Width = 0;
        TextCrete.instance.Height = 0;
        try
        {
            foreach (var item in TextCrete.instance.texts)
            {
                // TextCrete.instance.texts.Remove(item);
                Destroy(item.gameObject);
            }
        }
        catch
        {

        }
        TextCrete.instance.texts.Clear();
    }
}

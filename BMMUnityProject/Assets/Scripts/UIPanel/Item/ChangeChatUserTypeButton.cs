using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UserType
{
    Normal,
    Deaf
}

//更改聊天窗口
public class ChangeChatUserTypeButton : BaseItem
{
    public Text buttonText;
    public ChatPanel cPanel;

    public Image icon;
    public Sprite hand;
    public Sprite voice;

    public Button voiceButoon;
    public Button handButoon;

    private void Start()
    {
        buttonText.text = cPanel.GetUserType().ToString();
        isDestructible = false;
    }


    public void ChangeType()
    {
        cPanel.ChangeChatUserType();
        buttonText.text = cPanel.GetUserType().ToString();
    }

    //改变类型事件
    public void OnUserTypeChange(UserType ut)
    {
        if (ut == UserType.Deaf)
        {
            voiceButoon.gameObject.SetActive(false);
            handButoon.gameObject.SetActive(true);
        }
        else
        {
            handButoon.gameObject.SetActive(false);
            voiceButoon.gameObject.SetActive(true);
        }
    }
}


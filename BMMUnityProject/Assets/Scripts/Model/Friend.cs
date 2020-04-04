using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend
{
    public int Id { get; set; }
    public string Nickname { get; set; }

    /// <summary>
    /// 待定
    /// </summary>
    public bool Login { get; set; } = false;
    public Friend(int id, string nickname)
    {
        Id = id;
        Nickname = nickname;
    }
}
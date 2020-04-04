using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户管理
/// </summary>
public class UserManager : BaseManager
{
    //包含角色信息
    private User user;
    //朋友信息
    private List<Friend> friends = new List<Friend>();

    /// <summary>
    /// 添加朋友
    /// </summary>
    /// <param name="fr"></param>
    public void AddFriendInformation(Friend fr)
    {
        friends.Add(fr);
    }

    public void ClearFriendInformation()
    {
        friends.Clear();
    }

    public List<Friend> GetFriends()
    {
        return friends;
    }

    public UserManager(Facade facade) : base(facade)
    {
    }

    /// <summary>
    /// 设置用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="nickname"></param>
    /// <param name="ifl"></param>
    public void SetUserInformation(int id, string username, string nickname, bool ifl)
    {
        user = new User(id, username, null, nickname, ifl);
    }

    /// <summary>
    /// 返回用户ID
    /// </summary>
    /// <returns></returns>
    public int GetUserID()
    {
        return user.Id;
    }

    /// <summary>
    /// 获取用户昵称
    /// </summary>
    /// <returns></returns>
    public string GetNickname()
    {
        return user.Nickname;
    }

    /// <summary>
    /// 设置用户昵称
    /// </summary>
    /// <param name="nickname"></param>
    public void SetNickName(string nickname)
    {
        user.IsLoginFirst = false;
        user.Nickname = nickname;
    }

    /// <summary>
    /// 返回用户是否第一次登录
    /// </summary>
    public bool IsFirstTimeLogin()
    {
        return user.IsLoginFirst;
    }
}
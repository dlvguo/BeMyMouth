namespace Common
{
    //请求代码
    public enum RequestCode
    {
        None,
        Login,
        Register,
        VerifyRepeat,
        SearchUser,
        GetFriendList,
        SetFirstLoginInformation,
        SendAndSaveChatMessage,
        SetReaded,
        SearchFriend,
        ApplyForAddFriend,
        AddFriend,
        //response

        ReciveChatMessage,
        GetNotification,

        SendMessage,
        ShowNotification,

        SendApplyNotice,
        SetInfo

    }
}
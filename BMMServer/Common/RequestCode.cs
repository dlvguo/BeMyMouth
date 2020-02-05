namespace Common
{
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


    }
}
using BMMServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMMServer.DBS
{
    public class BeMyMouthDBHelper
    {
        #region Chat部分
        /// <summary>
        /// 保存消息
        /// </summary>
        public static void SavaMessage(string message, int sendId, int reciveId, DateTime dateTime)
        {
            try
            {
                Messages messages = new Messages()
                {
                    Message = message,
                    SendId = sendId,
                    ReciveId = reciveId,
                };
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    db.Messages.Add(messages);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 获取消息提示
        /// </summary>
        /// <param name="reciveId"></param>
        /// <returns></returns>
        public static string GetNotificationId(int reciveId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    var messages = db.Messages.Where((m) => reciveId == m.ReciveId && m.HasRead == false);
                    foreach (var mes in db.Messages)
                    {
                        if (mes.ReciveId == reciveId && mes.HasRead == false)
                        {
                            sb.Append(mes.SendId + ',');
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("当GetNotification时出现异常：" + e);
                return null;
            }
        }
        /// <summary>
        /// 获取聊天消息
        /// </summary>
        /// <param name="sendId"></param>
        /// <returns></returns>
        public static string GetReciveMessage(int sendId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    foreach (var mes in db.Messages)
                    {
                        if (mes.SendId == sendId && mes.HasRead == false)
                        {
                            sb.Append(mes.SendId + ',');
                        }
                    }
                }
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("当GetReciveMessage时出现异常：" + e);
                return null;
            }
        }

        #endregion

        #region User部分
        /// <summary>
        /// 根据用户名获取UserModel
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User GetUserInfoByUserName(string username)
        {
            using (BeMyMouthDB db = new BeMyMouthDB())
            {
                foreach (var user in db.Users)
                {
                    if (user.UserName.Equals(username))
                        return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据用户ID获取昵称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetNickNameById(int id)
        {
            using (BeMyMouthDB db = new BeMyMouthDB())
            {
                foreach (var user in db.Users)
                {
                    if (user.Id == id)
                        return user.NickName;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过昵称获取用户数据
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public static string GetUsersByNickname(string nickname)
        {
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    foreach (var user in db.Users)
                    {
                        if (user.NickName.Equals(nickname))
                            return user.UserName;
                    }
                }
                return string.Empty;
            }
            catch (Exception e)
            {


                return string.Empty;
            }
        }

        /// <summary>
        ///获取好友列表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetFriendsIdById(int id)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    User user = db.Users.Where((u) => u.Id == id).First();
                    foreach (var friend in db.Friends)
                    {
                        if (friend.LeftId == id)
                        {
                            sb.Append(friend.RightId + ',');

                        }
                    }
                    foreach (var friend in db.Friends)
                    {
                        if (friend.RightId == id)
                        {
                            sb.Append(friend.LeftId + ',');

                        }
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("当GetFriendsIdById时出现异常：" + e);
                return string.Empty;
            }
        }

        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerifyUserInfo(string username, string password)
        {
            using (BeMyMouthDB db = new BeMyMouthDB())
            {
                foreach (var user in db.Users)
                {
                    if (user.UserName.Equals(username) && user.Password.Equals(password))
                        return true;
                }
            }
            return false;
        }

        public static bool VerifyExist(string username)
        {
            using (BeMyMouthDB db = new BeMyMouthDB())
            {
                foreach (var user in db.Users)
                {
                    if (user.UserName.Equals(username))
                        return true;
                }
            }
            return false;

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool AddUser(string username, string password)
        {
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    User user = new User()
                    {
                        UserName = username,
                        Password = password
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 通过ID添加好友
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="selfId"></param>
        /// <returns></returns>
        public static bool AddFriendById(int friendId, int selfId)
        {

            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    Friend friend = new Friend()
                    {
                        LeftId = friendId,
                        RightId = selfId
                    };
                    db.Friends.Add(friend);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置首次登录信息
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public static bool SetFirstLoginInformationById(int id, string nickName)
        {
            try
            {
                using (BeMyMouthDB db = new BeMyMouthDB())
                {
                    foreach (var user in db.Users)
                    {
                        if (user.Id == id)
                        {
                            user.NickName = nickName;
                            user.IsFirstLogin = false;
                            break;
                        }
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("当SetFirstLoginInformationById时出现异常：" + e);
                return false;
            }
        }
        #endregion
    }
}

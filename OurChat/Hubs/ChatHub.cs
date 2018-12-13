using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using OurChat.Entities;
using OurChat.Models;
using OurChat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        static List<ChatUser> ConnectedUsers = new List<ChatUser>();
        static List<ChatMessage> CurrentMessage = new List<ChatMessage>();

        private readonly IMemberService _memberService;
        private readonly IConfiguration _configuration;

        public ChatHub(IMemberService memberService,
            IConfiguration configuration)
        {
            _memberService = memberService;
            _configuration = configuration;
        }

        public void Connect(string memberUniqueID)
        {
            var id = Context.ConnectionId;

            Member member = _memberService.FindMemberByUniqueID(memberUniqueID);
            string fullname = string.Empty;
            string mID = string.Empty;
            if (member != null)
            {
                fullname = string.Concat(member.Title, " ", member.Fname, " ", member.Lname);
                mID = member.UniqueID;
            }
            if (ConnectedUsers.Count(x => x.UniqueID == id) == 0)
            {
                string UserImg = "dummy-user.png"; //GetUserImage(userName);
                string logintime = DateTime.Now.ToString();

                ConnectedUsers.Add(new ChatUser { UniqueID = id, UserName = fullname, UserImage = UserImg, LoginTime = logintime, ID = mID });
                // send to caller
                Clients.Caller.SendAsync("onConnected", id, fullname, ConnectedUsers, CurrentMessage, mID);

                // send to all except caller client
                Clients.AllExcept(id).SendAsync("onNewUserConnected", id, fullname, UserImg, logintime, mID);
            }
        }

        //public void SendMessageToAll(string userName, string message, string time)
        //{
        //    string UserImg = GetUserImage(userName);
        //    // store last 100 messages in cache
        //    AddMessageinCache(userName, message, time, UserImg);

        //    // Broad cast message
        //    Clients.All.SendAsync("messageReceived", userName, message, time, UserImg);

        //}

        //private void AddMessageinCache(string userName, string message, string time, string UserImg)
        //{
        //    CurrentMessage.Add(new ChatMessage { UserName = userName, Message = message, Time = time, UserImage = UserImg });

        //    if (CurrentMessage.Count > 100)
        //        CurrentMessage.RemoveAt(0);

        //}

        //// Clear Chat History
        //public void clearTimeout()
        //{
        //    CurrentMessage.Clear();
        //}

        //public string GetUserImage(string username)
        //{
        //    string RetimgName = "dummy-user.png";

        //    return RetimgName;
        //}

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           // await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            var item = ConnectedUsers.FirstOrDefault(x => x.UniqueID == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                await Clients.All.SendAsync("onUserDisconnected", id, item.UserName);

            }
            await base.OnDisconnectedAsync(exception);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnConnectedAsync();
        //}


        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.UniqueID == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.UniqueID == fromUserId);

            if (toUser != null && fromUser != null)
            {
                string CurrentDateTime = DateTime.Now.ToString();
                // string UserImg = GetUserImage(fromUser.UserName);
                string UserImg = string.Empty;
                // send to 
                Clients.Client(toUserId).SendAsync("sendPrivateMessage", fromUser.ID, fromUser.UserName, message, UserImg, CurrentDateTime);

                // send to caller user
                Clients.Caller.SendAsync("sendPrivateMessage", toUser.ID, fromUser.UserName, message, UserImg, CurrentDateTime);
            }

        }

    }
}

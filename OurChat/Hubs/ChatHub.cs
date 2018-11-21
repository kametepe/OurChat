using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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
        //static List<ChatUser> ConnectedUsers = new List<ChatUser>();
        //static List<ChatMessage> CurrentMessage = new List<ChatMessage>();

        //private readonly IMemberService _memberService;
        //private readonly IConfiguration _configuration;

        //public ChatHub (IMemberService memberService,
        //    IConfiguration configuration)
        //{
        //    _memberService = memberService;
        //    _configuration = configuration;
        //}

        //public void Connect(string userName)
        //{
        //    var id = Context.ConnectionId;


        //    if (ConnectedUsers.Count(x => x.UniqueID == id) == 0)
        //    {
        //        string UserImg = "dummy-user.png"; //GetUserImage(userName);
        //        string logintime = DateTime.Now.ToString();

        //        ConnectedUsers.Add(new ChatUser { UniqueID = id, UserName = userName, UserImage = UserImg, LoginTime = logintime });
        //        // send to caller
        //        Clients.Caller.SendAsync("onConnected", id, userName, ConnectedUsers, CurrentMessage);

        //        // send to all except caller client
        //        Clients.AllExcept(id).SendAsync("onNewUserConnected", id, userName, UserImg, logintime);
        //    }
        //}

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

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
        //    var item = ConnectedUsers.FirstOrDefault(x => x.UniqueID == Context.ConnectionId);
        //    if (item != null)
        //    {
        //        ConnectedUsers.Remove(item);

        //        var id = Context.ConnectionId;
        //        await Clients.All.SendAsync("onUserDisconnected", id, item.UserName);

        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}

        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnConnectedAsync();
        //}


        //public void SendPrivateMessage(string toUserId, string message)
        //{

        //    string fromUserId = Context.ConnectionId;

        //    var toUser = ConnectedUsers.FirstOrDefault(x => x.UniqueID == toUserId);
        //    var fromUser = ConnectedUsers.FirstOrDefault(x => x.UniqueID == fromUserId);

        //    if (toUser != null && fromUser != null)
        //    {
        //        string CurrentDateTime = DateTime.Now.ToString();
        //        string UserImg = GetUserImage(fromUser.UserName);
        //        // send to 
        //        Clients.Client(toUserId).SendAsync("sendPrivateMessage", fromUserId, fromUser.UserName, message, UserImg, CurrentDateTime);

        //        // send to caller user
        //        Clients.Caller.SendAsync("sendPrivateMessage", toUserId, fromUser.UserName, message, UserImg, CurrentDateTime);
        //    }

        //}

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace ChiatoBot
{
    class Requests
    {
        public ulong mID;
        public Discord.WebSocket.SocketUser uID;
        public Discord.WebSocket.SocketRole reqRole;
    }

    class GamblingRequests
    {
        public ulong mID;
        public Discord.WebSocket.SocketUser uID;
        public Discord.WebSocket.SocketUser vsID;
        public int coinsToGamble;
        public int gamePlayed;
        public int rpsUser;
        public int rpsVser;
        public Discord.WebSocket.ISocketMessageChannel msgChannel;
    }

    class TimedEventsLimit
    {
        public System.DateTime StopTime;
        public Discord.WebSocket.SocketUser uID;
        //public string commandUsed;
        public int stage;
    }

    class Giveaways
    {
        public Discord.WebSocket.SocketUser enteredUsers;
    }

    class ListOfGiveawaysClass
    {
        public int giveawayAmount;
        public ulong mID;
    }
}

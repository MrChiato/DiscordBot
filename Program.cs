using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ChiatoBot
{
    class Program
    {

        private Discord.WebSocket.DiscordSocketClient _client;
        private CommandService _commands;

        public static List<Requests> reactionMsgs = new List<Requests>();
        public static List<GamblingRequests> gamblingMsgs = new List<GamblingRequests>();
        public static List<TimedEventsLimit> TimedEventsList = new List<TimedEventsLimit>();
        public static List<Giveaways> GiveawayList = new List<Giveaways>();
        public static List<ListOfGiveawaysClass> ListOfGiveaways = new List<ListOfGiveawaysClass>();

        public static int Language = 0; //Bot language, 0 = English, 1 = Danish
        public static string versionNumber = "0.0.1.2";
        public static string coinsName = "Coins";



        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            _client = new Discord.WebSocket.DiscordSocketClient();
            _client.Log += Log;
            _commands = new CommandService();
            _client.ReactionAdded += ReactionEvent;
            _client.UserJoined += NewUserInfo;
            _client.MessageReceived += MessageReceived;
            _client.ReactionRemoved += ReactionRemoved;
            

            CommandHandler ch = new CommandHandler(_client, _commands);
            var token = "" //Discord bot token;

            var config = new DiscordConfig
            {
                LogLevel = LogSeverity.Debug,
            };

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await ch.InstallCommandsAsync();

            //Api calls
            ApiCalls.InitializeClient();




            // Block this task until the program is closed.
            await Task.Delay(-1);
        }


        public class CommandHandler
        {
            private readonly DiscordSocketClient _client;
            private readonly CommandService _commands;

            // Retrieve client and CommandService instance via ctor
            public CommandHandler(DiscordSocketClient client, CommandService commands)
            {
                _commands = commands;
                _client = client;
            }

            public async Task InstallCommandsAsync()
            {
                _client.MessageReceived += HandleCommandAsync;
                await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                                services: null);
            }

            private async Task HandleCommandAsync(SocketMessage messageParam)
            {
                // Don't process the command if it was a system message
                var message = messageParam as SocketUserMessage;
                if (message == null) return;



                // Create a number to track where the prefix ends and the command begins
                int argPos = 0;

                // Determine if the message is a command based on the prefix and make sure no bots trigger commands
                if (!(message.HasCharPrefix('!', ref argPos) ||
                    message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                    message.Author.IsBot)
                    return;

                // Create a WebSocket-based command context based on the message
                var context = new SocketCommandContext(_client, message);

                // Execute the command with the command context we just
                // created, along with the service provider for precondition checks.
                await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null);

            }

        }

        public async Task ReactionEvent(Cacheable<IUserMessage, UInt64> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reactionMsgs.Exists(x => x.mID == message.Id))
            {
                var allowedRoles = new List<ulong>();
                allowedRoles.Add(553614396669296640);
                allowedRoles.Add(553614468316397589);


                var msgInList = reactionMsgs.Find(x => x.mID == message.Id);
                var user = msgInList.uID;
                var msgId = msgInList.mID;
                var reqRole = msgInList.reqRole;

                if (reqRole.Id == 758140198428606464)
                {
                    allowedRoles.Add(758140198428606464);
                }
                else if (reqRole.Id == 553617713851793408)
                {
                    allowedRoles.Add(553617713851793408);
                }
                if (reaction.Emote.Name == "✅")
                {
                    SocketGuildUser reactUser = (SocketGuildUser)reaction.User;
                    if (reactUser.Roles.Any(x => allowedRoles.Any(y => y == x.Id)))
                    {

                        //Add the role
                        await (user as IGuildUser).AddRoleAsync(reqRole);

                        //Confirmation msg
                        switch (Language)
                        {
                            //English
                            case 0:
                                await channel.SendMessageAsync($"{user.Mention} has been given the role '{reqRole}' by {reactUser.Mention}");
                                break;
                            //Danish
                            case 1:
                                await channel.SendMessageAsync($"{user.Mention} har fået tildelt rollen '{reqRole}' af {reactUser.Mention}");
                                break;
                            default:
                                await channel.SendMessageAsync("No language selected, please select a language: '!languages' for options.");
                                break;
                        }

                        reactionMsgs.Remove(reactionMsgs.Find(x => x.mID == message.Id));
                    }

                    if (!reactUser.Roles.Any(x => allowedRoles.Any(y => y == x.Id)) && !reactUser.Roles.Any(b => b.Id == 574353825113309215))
                    {
                        await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                    }

                }
                else
                {
                    await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                }
            }

            if (gamblingMsgs.Exists(x => x.mID == message.Id))
            {
                SocketGuildUser reactUser = (SocketGuildUser)reaction.User;
                var msgInList = gamblingMsgs.Find(x => x.mID == message.Id);
                Random rnd = new Random();
                if (reaction.Emote.Name == "👍")
                {

                    if (reactUser == msgInList.vsID)
                    {
                        if (msgInList.gamePlayed == 1)
                        {
                            await channel.SendMessageAsync($"Roll between {msgInList.uID.Mention} and {msgInList.vsID.Mention} accepted!");

                            int userRoll, vsRoll;
                            userRoll = rnd.Next(1, msgInList.coinsToGamble + 1);
                            vsRoll = rnd.Next(1, msgInList.coinsToGamble + 1);
                            int pot = 0;
                            if (userRoll > vsRoll)
                            {
                                pot = userRoll - vsRoll;
                                await channel.SendMessageAsync($"{msgInList.uID.Username} rolled {userRoll}, {msgInList.vsID.Username} rolled {vsRoll}, {msgInList.uID.Username} won {pot} {coinsName}!");
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, pot, true); //Give user the pot
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, pot, false); //Take the pot from vs

                            }
                            else if (userRoll < vsRoll)
                            {
                                pot = vsRoll - userRoll;
                                await channel.SendMessageAsync($"{msgInList.uID.Username} rolled {userRoll}, {msgInList.vsID.Username} rolled {vsRoll}, {msgInList.vsID.Username} won {pot} {coinsName}!");
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, pot, true); //Give vs the pot
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, pot, false); //Take the pot from user
                            }
                            else
                                await channel.SendMessageAsync($"You both rolled the same number.");
                        }
                        else if (msgInList.gamePlayed == 2)
                        {
                            int userRoll, vsRoll;
                            userRoll = rnd.Next(1, 101);
                            vsRoll = rnd.Next(1, 101);
                            int pot = msgInList.coinsToGamble;
                            if (userRoll > vsRoll)
                            {
                                await channel.SendMessageAsync($"{msgInList.uID.Username} rolled {userRoll}, {msgInList.vsID.Username} rolled {vsRoll}, {msgInList.uID.Username} won {pot} {coinsName}!");
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, pot, true); //Give user the pot
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, pot, false); //Take the pot from vs

                            }
                            else if (userRoll < vsRoll)
                            {
                                await channel.SendMessageAsync($"{msgInList.uID.Username} rolled {userRoll}, {msgInList.vsID.Username} rolled {vsRoll}, {msgInList.vsID.Username} won {pot} {coinsName}!");
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, pot, true); //Give vs the pot
                                await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, pot, false); //Take the pot from user
                            }
                            else
                                await channel.SendMessageAsync($"The coin landed on the side");
                        }

                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.mID == message.Id));
                    }
                    else if (reactUser.Id != 762683246941044747)
                        await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                }
                else if (reaction.Emote.Name == "👎")
                {
                    if (reactUser == msgInList.vsID || reactUser == msgInList.uID)
                    {
                        await channel.SendMessageAsync($"Game between {msgInList.vsID.Username} and {msgInList.uID.Username} canceled by {reactUser.Username}!");
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.mID == message.Id));
                    }
                    else if (reactUser.Id != 762683246941044747)
                        await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                }

                else
                    await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
            }

            if (ListOfGiveaways.Exists(x => x.mID == message.Id))
            {
                SocketGuildUser reactUser = (SocketGuildUser)reaction.User;
                var giveAwayInList = new Giveaways();

                if (GiveawayList.Exists(x => x.enteredUsers == reactUser))
                {
                    await (await message.DownloadAsync()).RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                    return;
                }
                else if (reactUser.Id != 762683246941044747)
                {
                    giveAwayInList.enteredUsers = reactUser;
                    GiveawayList.Add(giveAwayInList);
                }
                    
            }
        }

        public async Task ReactionRemoved(Cacheable<IUserMessage, UInt64> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (ListOfGiveaways.Exists(x => x.mID == message.Id))
            {
                SocketGuildUser reactUser = (SocketGuildUser)reaction.User;
                //var giveAwayInList = new Giveaways();

                if (reaction.Emote.Name == "💸")
                {
                    if (reactUser.Id != 762683246941044747)
                    {
                        var thisUser = GiveawayList.Find(x => x.enteredUsers == reactUser);
                        GiveawayList.Remove(thisUser);
                    }
                }
            }
        }

        private async Task MessageReceived(SocketMessage msg)
        {


            if (gamblingMsgs.Exists(x => x.uID.Id == msg.Author.Id))
            {
                var msgInList = gamblingMsgs.Find(x => x.uID.Id == msg.Author.Id);
                if (msg.Content == "rock" || msg.Content == "Rock")
                {
                    msgInList.rpsUser = 1;
                    if (msgInList.rpsVser != 0)
                    {
                        if (msgInList.rpsVser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked rock. It's a draw.");
                        }
                        else if (msgInList.rpsVser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked paper, {msgInList.uID.Username} picked rock. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs
                        }
                        else if (msgInList.rpsVser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked scissors, {msgInList.uID.Username} picked rock. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.uID.Id == msg.Author.Id));
                    }
                }
                else if (msg.Content == "paper" || msg.Content == "Paper")
                {
                    msgInList.rpsUser = 2;
                    if (msgInList.rpsVser != 0)
                    {
                        if (msgInList.rpsVser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked rock, {msgInList.uID.Username} picked paper. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs

                        }
                        else if (msgInList.rpsVser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked paper. It's a draw.");
                        }
                        else if (msgInList.rpsVser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked scissors, {msgInList.uID.Username} picked paper. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.uID.Id == msg.Author.Id));
                    }
                }
                else if (msg.Content == "scissor" || msg.Content == "scissors" || msg.Content == "Scissors" || msg.Content == "Scissor")
                {
                    msgInList.rpsUser = 3;
                    if (msgInList.rpsVser != 0)
                    {
                        if (msgInList.rpsVser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked rock, {msgInList.uID.Username} picked scissors. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs

                        }
                        else if (msgInList.rpsVser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked paper, {msgInList.uID.Username} picked scissors. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs
                        }
                        else if (msgInList.rpsVser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked scissors. It's a draw.");
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.uID.Id == msg.Author.Id));
                    }
                }
            }
            else if (gamblingMsgs.Exists(x => x.vsID.Id == msg.Author.Id))
            {
                var msgInList = gamblingMsgs.Find(x => x.vsID.Id == msg.Author.Id);
                if (msg.Content == "rock" || msg.Content == "Rock")
                {
                    msgInList.rpsVser = 1;
                    if (msgInList.rpsUser != 0)
                    {
                        if (msgInList.rpsUser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked rock. It's a draw.");
                        }
                        else if (msgInList.rpsUser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked rock, {msgInList.uID.Username} picked paper. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs
                        }
                        else if (msgInList.rpsUser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked rock, {msgInList.uID.Username} picked scissors. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.vsID.Id == msg.Author.Id));
                    }
                }
                else if (msg.Content == "paper" || msg.Content == "Paper")
                {
                    msgInList.rpsVser = 2;
                    if (msgInList.rpsUser != 0)
                    {
                        if (msgInList.rpsUser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked paper, {msgInList.uID.Username} picked rock. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs

                        }
                        else if (msgInList.rpsUser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked paper. It's a draw.");
                        }
                        else if (msgInList.rpsUser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked paper, {msgInList.uID.Username} picked scissors. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.vsID.Id == msg.Author.Id));
                    }
                }
                else if (msg.Content == "scissor" || msg.Content == "scissors" || msg.Content == "Scissors" || msg.Content == "Scissor")
                {
                    msgInList.rpsVser = 3;
                    if (msgInList.rpsUser != 0)
                    {
                        if (msgInList.rpsUser == 1)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked scissors, {msgInList.uID.Username} picked rock. {msgInList.uID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, true); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, false); //Take the pot from vs

                        }
                        else if (msgInList.rpsUser == 2)
                        {
                            await msgInList.msgChannel.SendMessageAsync($"{msgInList.vsID.Username} picked scissors, {msgInList.uID.Username} picked paper. {msgInList.vsID.Mention} wins {msgInList.coinsToGamble} {coinsName}!");
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.uID.Id, msgInList.coinsToGamble, false); //Give user the pot
                            await ChiatoBotDatabase.DBUpdateCoins(msgInList.vsID.Id, msgInList.coinsToGamble, true); //Take the pot from vs
                        }
                        else if (msgInList.rpsUser == 3)
                        {
                            await msgInList.msgChannel.SendMessageAsync("You bothed picked scissors. It's a draw.");
                        }
                        gamblingMsgs.Remove(gamblingMsgs.Find(x => x.vsID.Id == msg.Author.Id));
                    }
                }
            }
        }

        public async Task NewUserInfo(SocketGuildUser user)
        {
            var channel = _client.GetChannel(553617308816506883) as SocketTextChannel;

            switch (Language)
            {
                //English
                case 0:
                    await channel.SendMessageAsync($"Hello, {user.Mention} welcome to SERVER Type '!vener' to request friends role or '!amongus' to request the Among us role for access to among us channels, another member with the role will have to accept the request");
                    break;
                //Danish
                case 1:
                    await channel.SendMessageAsync($"Hejsa, {user.Mention} velkommen til SERVER Skriv '!vener' for at anmode om 'venner' rollen, eller '!amongus' for at anmode om 'amongus' rollen, der giver adgang til Among Us kanalern. Et andet medlem der allerede har rollen, skal godkende din anmodning.");
                    break;
                default:
                    await channel.SendMessageAsync("No language selected, please select a language: '!languages' for options.");
                    break;
            }

        }


        private Task Log(Discord.LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}


    }
}

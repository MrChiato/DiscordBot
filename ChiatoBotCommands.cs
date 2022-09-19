using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;
using System.Timers;

namespace ChiatoBot
{

    public class InfoModule : ModuleBase<SocketCommandContext>
    {

        public int Language = Program.Language;
        public string coinsName = Program.coinsName;
        public ulong coinAdmins = ; //disc Id, eventually rewrite this to a list or DB call and make the admin check if the user of the command exists.

        [Command("cbhelp")]
        [Alias("cbinfo", "cbcommands")]
        public async Task CBHelpAsync()
        {
            string commandlist = "\n '!cbhelp' - prints the newest version of this command list." +
                "\n '!roleinfo' - prints info about commands you can use to request discord roles." +
                "\n '!rank \"league username\"' - prints solo queue rank for the specified username." +
                "\n '!languages' - prints the different languages the bot can speak." +
                "\n '!colors' - prints a list of colors you can choose for your discord name." +
                "\n '!coincommands' - prints a list for all coin and gambling commands." +
                "\n '!roll #number' - rolls a random number from 1 to #.";

            switch (Language)
            {
                //English
                case 0:
                    await ReplyAsync($"{Context.Message.Author.Mention} here is a list of all current commands {commandlist}.");
                    break;
                //Danish
                case 1:
                    await ReplyAsync($"{Context.Message.Author.Mention} her er en liste over alle eksisterende kommandoer {commandlist}.");
                    break;
                default:
                    await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                    break;
            }
        }
        [Command("coincommands")]
        public async Task CoinHelpAsync()
        {
            string coinCommandList = "\n '!coincommands' - prints the newest version of this command list." +
                "\n '!coins' - shows how many coins you have" +
                "\n '!balance \"@user\"' - shows how many coins the tagged user has" +
                "\n '!leaderboard #number' - shows the top # coin holders" +
                "\n '!overunder #amount over/under/on' - guess if the bot will roll over, under or on 7, while rolling between 1-14. If you guess correctly you will double the # you bet, if you guess 'on' you will win x13 the amount." +
                "\n '!rps \"@user\" #amount' - request rock/paper/scissors game vs another user, whisper the bot rock/paper/scissors and it will determine who won." +
                "\n '!rollbattle #amount' - rolls vs the bot, you will both roll between 1 and # amount of coins. Whoever rolls the highest wins the difference between the 2 rolls." +
                "\n '!rollvs \"@user\" #amount' - same as rollbattle but vs another user." +
                "\n '!coinflip \"@user\" #amount' - rolls 1-100 vs another user, the highest roll gets the full pot." +
                "\n '!give \"@user\" #amount' - gives another user #amount of coins" +
                "\n '!guess #number' - every 10 minutes you can guess a number the bot will roll, if the bot rolls what you guess you will be able to guess again. The first guess will be between 1-2, type !guess \"1/2\", to start." +
                " if you guess correctly you will get 32 coins and be able to guess again, this time between 1 and 3, everytime you guess correctly you can guess again, the roll will be between 1 more number each time, and the payout coins will double." +
                " when you fail a guess you will have to wait 10 minutes to try again, and start over from 1-2.";

            await ReplyAsync($"{Context.Message.Author.Mention} here is a list of all current coin commands {coinCommandList}.");
        }

        [Command("test")]
        public async Task TestAsync()
        {
            await ReplyAsync("Test succesful");
        }

        [Command("amongus")]
        public async Task AmongUsRoleRequestAsync()
        {
            ulong roleIDNumber = 758140198428606464;
            var requestedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == roleIDNumber);
            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (!sUser.Roles.Any(x => x.Id == roleIDNumber))
            {
                var rctmsg = Program.reactionMsgs;

                if (!rctmsg.Exists(x => x.uID == Context.User))
                {
                    var upvoteEmoji = new Emoji("✅");

                    string fillerMsg = "";

                    switch (Language)
                    {
                        //English
                        case 0:
                            fillerMsg = "has requested the role";
                            break;
                        //Danish
                        case 1:
                            fillerMsg = "har anmodet om rollen";
                            break;
                        default:
                            await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                            break;
                    }
                    var sendmsg = await ReplyAsync($"{Context.Message.Author.Mention} {fillerMsg} '{requestedRole}'");

                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(upvoteEmoji);

                    var usermsg = new Requests();
                    usermsg.mID = msgID;
                    usermsg.uID = Context.User;
                    usermsg.reqRole = requestedRole;
                    rctmsg.Add(usermsg);
                }
                else
                {
                    switch (Language)
                    {
                        //English
                        case 0:
                            await ReplyAsync($"{Context.Message.Author.Mention} You already have an open request");
                            break;
                        //Danish
                        case 1:
                            await ReplyAsync($"{Context.Message.Author.Mention} Du har allerede en ubesvaret anmodning");
                            break;
                        default:
                            await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                            break;
                    }

                }
            }
            else
            {
                switch (Language)
                {
                    //English
                    case 0:
                        await ReplyAsync($"{Context.Message.Author.Mention} You already have the '{requestedRole}' role.");
                        break;
                    //Danish
                    case 1:
                        await ReplyAsync($"{Context.Message.Author.Mention} Du har allerede rollen '{requestedRole}'.");
                        break;
                    default:
                        await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                        break;
                }

            }

        }

        [Command("vener")]
        public async Task VenerRoleRequestAsync()
        {
            ulong roleIDNumber = 553617713851793408;
            var requestedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == roleIDNumber);
            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (!sUser.Roles.Any(x => x.Id == roleIDNumber))
            {
                var rctmsg = Program.reactionMsgs;

                if (!rctmsg.Exists(x => x.uID == Context.User))
                {
                    var upvoteEmoji = new Emoji("✅");
                    string fillerMsg = "";

                    switch (Language)
                    {
                        //English
                        case 0:
                            fillerMsg = "has requested the role";
                            break;
                        //Danish
                        case 1:
                            fillerMsg = "har anmodet om rollen";
                            break;
                        default:
                            await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                            break;
                    }
                    var sendmsg = await ReplyAsync($"{Context.Message.Author.Mention} {fillerMsg} '{requestedRole}'");
                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(upvoteEmoji);

                    var usermsg = new Requests();
                    usermsg.mID = msgID;
                    usermsg.uID = Context.User;
                    usermsg.reqRole = requestedRole;
                    rctmsg.Add(usermsg);
                }
                else
                {
                    switch (Language)
                    {
                        //English
                        case 0:
                            await ReplyAsync($"{Context.Message.Author.Mention} You already have an open request");
                            break;
                        //Danish
                        case 1:
                            await ReplyAsync($"{Context.Message.Author.Mention} Du har allerede en ubesvaret anmodning");
                            break;
                        default:
                            await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                            break;
                    }
                }
            }
            else
            {
                switch (Language)
                {
                    //English
                    case 0:
                        await ReplyAsync($"{Context.Message.Author.Mention} You already have the '{requestedRole}' role.");
                        break;
                    //Danish
                    case 1:
                        await ReplyAsync($"{Context.Message.Author.Mention} Du har allerede rollen '{requestedRole}'.");
                        break;
                    default:
                        await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                        break;
                }
            }

        }

        [Command("printlist")]
        public async Task PrintListAsync()
        {
            var rctmsg = Program.reactionMsgs;

            foreach (Requests x in rctmsg)
            {
                var sendmsg = await ReplyAsync($"Message ID : {x.mID}, User ID : {x.uID}, Requested Role : {x.reqRole}");
            }
        }

        [Command("version")]
        public async Task PatchVersionAsync()
        {
            await ReplyAsync($"Version : {Program.versionNumber}");
        }

        [Command("leagueaccountid")]
        public async Task RiotApiAsync(string userArgs)
        {
            var summonerData = await RiotApiProcessor.LoadSummonerData(userArgs);

            await ReplyAsync($"{summonerData.id}");
        }

        [Command("rank")]
        [Alias("ranked")]
        public async Task LeagueRankedStatsAsync(string userArgs)
        {

            if (userArgs == null)
            {
                await ReplyAsync("Wrong input, please use !rank \"name\".");
                return;
            }


            var summonerId = await RiotApiProcessor.LoadSummonerData(userArgs);
            string[] dataSplitted = await RiotApiProcessor.LoadRankedData(summonerId.id);


            int fixedStringRow = 0;
            string summonerName = "";
            string tier = "";
            string rank = "";
            int leaguePoints = 0;
            int wins = 0;
            int losses = 0;

            for (var k = 0; k < dataSplitted.Length; k++)
            {
                if (dataSplitted[k] != "RANKED_SOLO_5x5")
                {
                    continue;
                }
                else if (dataSplitted[k] == "RANKED_SOLO_5x5")
                {
                    fixedStringRow = k;
                }
            }
            summonerName = dataSplitted[fixedStringRow + 24];
            tier = dataSplitted[fixedStringRow + 6];
            rank = dataSplitted[fixedStringRow + 12];
            leaguePoints = Int32.Parse(dataSplitted[fixedStringRow + 29]);
            wins = Int32.Parse(dataSplitted[fixedStringRow + 33]);
            losses = Int32.Parse(dataSplitted[fixedStringRow + 37]);

            await ReplyAsync($"{Context.Message.Author.Mention} here are the stats for '{summonerName}'" +
                $"\n Rank: {tier} {rank} " +
                $"\n LP: {leaguePoints}" +
                $"\n Wins: {wins} Losses: {losses}");
        }

        [Command("matchhistory")]
        public async Task MatchHistoryAsync(string userArgs)
        {
            var accountId = await RiotApiProcessor.LoadSummonerData(userArgs);
            await RiotApiProcessor.LoadMatchHistoryData(accountId.accountId);
        }

        [Command("roleinfo")]
        public async Task PrintInfoAsync()
        {

            switch (Language)
            {
                //English
                case 0:
                    await ReplyAsync($"Type '!vener' to request friends role or '!amongus' to request the Among us role for access to among us channels, another member with the role will have to accept the request");
                    break;
                //Danish
                case 1:
                    await ReplyAsync($"Skriv '!vener' for at anmode om 'venner' rollen, eller '!amongus' for at anmode om 'amongus' rollen, der giver adgang til Among Us kanalern. Et andet medlem der allerede har rollen, skal godkende din anmodning.");
                    break;
                default:
                    await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                    break;
            }
        }
        }

        [Command("english")]
        [Alias("danish", "languages", "language")]
        public async Task SelectLanguageAsync()
        {
            if (Context.Message.ToString() == "!english")
            {
                Program.Language = 0;
                await ReplyAsync("I will now speak English, '!languages' for other options.");
            }
            else if (Context.Message.ToString() == "!danish")
            {
                Program.Language = 1;
                await ReplyAsync("Jeg vil nu skrive på dansk, skriv '!languages' for andre valgmuligheder.");
            }
            else
                await ReplyAsync("'!english', '!danish'");

        }

        [Command("colors")]
        [Alias("purple", "blue", "green", "white", "black", "pink", "yellow", "cyan", "violet", "orange", "red", "brown")]
        public async Task ColorRoleAsync()
        {
            SocketGuildUser sUser = (SocketGuildUser)Context.User;

            var colorList = new List<ulong>();
            ulong purple = (763067075064234014); //Purple
            ulong blue = (763068123993473074); //Blue
            ulong green = (763068193551941652); //Green
            ulong white = (763068371318603836); //White
            ulong black = (763068269761789952); //Black
            ulong pink = (763092728782520330); //Pink
            ulong yellow = (763093103392849990); //Yellow
            ulong cyan = (763093363435110471); //Cyan
            ulong violet = (763094107055849512); //Violet
            ulong orange = (763094582090137641); //Orange
            ulong red = (763149547693146152); //Red
            ulong brown = (763735368368521256); //Brown

            colorList.Add(purple);  //Purple
            colorList.Add(blue);    //Blue
            colorList.Add(green);   //Green
            colorList.Add(white);   //White
            colorList.Add(black);   //Black
            colorList.Add(pink);    //Pink
            colorList.Add(yellow);  //Yellow
            colorList.Add(cyan);    //Cyan
            colorList.Add(violet);  //Violet
            colorList.Add(orange);  //Orange
            colorList.Add(red);     //Red
            colorList.Add(brown);   //Brown

            string colorListTxt = "'!purple', '!blue', '!green', '!white', '!black', '!pink', '!yellow', '!cyan', '!violet', '!orange', '!red', '!brown'";

            if (Context.Message.ToString() == "!colors")
            {
                switch (Language)
                {
                    //English
                    case 0:
                        await ReplyAsync($"Here is a list of all available colors: {colorListTxt}.");
                        break;
                    //Danish
                    case 1:
                        await ReplyAsync($"Her er en liste over alle tilgængelige farver: {colorListTxt}.");
                        break;
                    default:
                        await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                        break;
                }

            }
            else if (sUser.Roles.Any(x => colorList.Any(y => y == x.Id)))
            {
                foreach (var i in colorList)
                {

                    var checkRole = Context.Guild.Roles.FirstOrDefault(h => h.Id == i);
                    if (sUser.Roles.Any(k => k == checkRole))
                    {
                        await sUser.RemoveRoleAsync(checkRole);

                    }
                }
            }
            ulong chosenColorId = 0;
            if (Context.Message.ToString() == "!purple")
            {
                chosenColorId = purple;
            }
            if (Context.Message.ToString() == "!blue")
            {
                chosenColorId = blue;
            }
            if (Context.Message.ToString() == "!green")
            {
                chosenColorId = green;
            }
            if (Context.Message.ToString() == "!white")
            {
                chosenColorId = white;
            }
            if (Context.Message.ToString() == "!black")
            {
                chosenColorId = black;
            }
            if (Context.Message.ToString() == "!pink")
            {
                chosenColorId = pink;
            }
            if (Context.Message.ToString() == "!yellow")
            {
                chosenColorId = yellow;
            }
            if (Context.Message.ToString() == "!cyan")
            {
                chosenColorId = cyan;
            }
            if (Context.Message.ToString() == "!violet")
            {
                chosenColorId = violet;
            }
            if (Context.Message.ToString() == "!orange")
            {
                chosenColorId = orange;
            }
            if (Context.Message.ToString() == "!red")
            {
                chosenColorId = red;
            }
            if (Context.Message.ToString() == "!brown")
            {
                chosenColorId = brown;
            }

            var colorRole = Context.Guild.Roles.FirstOrDefault(h => h.Id == chosenColorId);
            await sUser.AddRoleAsync(colorRole);

            switch (Language)
            {
                //English
                case 0:
                    await ReplyAsync($"Your role color is now {colorRole}");
                    break;
                //Danish
                case 1:
                    await ReplyAsync($"Din farve er nu {colorRole}");
                    break;
                default:
                    await ReplyAsync("No language selected, please select a language: '!languages' for options.");
                    break;
            }


        }

        [Command("coins")]
        public async Task ReadCoinsFromDB()
        {
            ulong discordID = Context.User.Id;
            if (!ChiatoBotDatabase.CheckIfUserExist(discordID))
            {
                await ChiatoBotDatabase.DBUpdateCoins(discordID, 0, true);
                int coins = await ChiatoBotDatabase.DBReadCoins(discordID);
                await ReplyAsync($"{Context.User.Mention} You have {coins} {coinsName}!");
            }
            else
            {
                int coins = await ChiatoBotDatabase.DBReadCoins(discordID);
                await ReplyAsync($"{Context.User.Mention} You have {coins} {coinsName}!");
            }
        }

        [Command("balance")]
        public async Task ReadBalanceFromDB(Discord.IGuildUser userArgs)
        {
            ulong discordID = userArgs.Id;
            if (!ChiatoBotDatabase.CheckIfUserExist(discordID))
            {
                await ChiatoBotDatabase.DBUpdateCoins(discordID, 0, true);
                int coins = await ChiatoBotDatabase.DBReadCoins(discordID);
                await ReplyAsync($"{userArgs.Username} Has {coins} {coinsName}!");
            }
            else
            {
                int coins = await ChiatoBotDatabase.DBReadCoins(discordID);
                await ReplyAsync($"{userArgs.Username} Has {coins} {coinsName}!");
            }
        }

        [Command("leaderboard")]
        public async Task ReadLeaderboardFromDB(int amountToShow)
        {
            int amountOfRows = await ChiatoBotDatabase.DBReadRows(); ;
            if (amountToShow < 1 || amountToShow > amountOfRows)
            {
                amountToShow = amountOfRows;
                await ReplyAsync($"There's only {amountOfRows} people on the leaderboard, showing top {amountOfRows}.");
            }

            List<string> arr = await ChiatoBotDatabase.DBLeaderBoard();
            int i = 0;
            int k = 1;
            int place = 1;
            amountToShow = amountToShow * 2;
            string leaderBoard = "";
            string addToLeaderboard = "";
            while (i < amountToShow)
            {
                ulong getUserID = Convert.ToUInt64(arr[i]);
                SocketUser userName = Context.Guild.GetUser(getUserID);
                addToLeaderboard = ($"{place}. {userName.Username} - {arr[k].ToString()}");
                leaderBoard = leaderBoard + "\n" + addToLeaderboard;
                place++;
                i += 2;
                k += 2;
            }
            await ReplyAsync(leaderBoard);
        }

        [Command("overunder")]
        public async Task overUnder(int amountBet, string userArgs)
        {
            if (amountBet < 1)
                return;

            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);

            if (userCoins < amountBet)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }

            Random rnd = new Random();
            int botRoll = 0;
            botRoll = rnd.Next(1, 15);
            int pot = amountBet;
            bool winOrLose = false;
            string outcome = "lose";
            await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, pot, winOrLose); //Take the price
            if (userArgs == "over")
            {
                if (botRoll > 7)
                {
                    pot *= 2;
                    winOrLose = true;
                    outcome = "win";
                    await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, pot, winOrLose); //Pay the pot
                }
            }
            else if (userArgs == "under")
            {
                if (botRoll < 7)
                {
                    pot *= 2;
                    winOrLose = true;
                    outcome = "win";
                    await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, pot, winOrLose); //Pay the pot
                }
            }
            else if (userArgs == "on")
            {
                if (botRoll == 7)
                {
                    pot *= 13;
                    winOrLose = true;
                    outcome = "win";
                    await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, pot, winOrLose); //Pay the pot
                }
            }
            else
            {
                await ReplyAsync("Please specify if you want to roll over, under or on. Command usage is !overunder #AmountOfCoinsToRoll over/under/on");
                return;
            }

            await ReplyAsync($"I rolled {botRoll}, you {outcome} {pot} {coinsName}!");
        }

        [Command("jackpot")]
        public async Task jackpotRoll(int amountBet)
        {
            if (amountBet < 1)
                return;

            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);

            if (userCoins < amountBet)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }
        }

        [Command("rps")]
        public async Task jackpotRoll(Discord.IGuildUser taggedUser, int amountBet)
        {
            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            SocketGuildUser vsUser = (SocketGuildUser)taggedUser;

            if (amountBet < 1)
                return;
            if (taggedUser.Id == 762683246941044747)
                return;

            if (!ChiatoBotDatabase.CheckIfUserExist(vsUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(vsUser.Id, 0, true);
            }
            else if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);
            int vsCoins = await ChiatoBotDatabase.DBReadCoins(vsUser.Id);

            if (userCoins < amountBet)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }
            else if (vsCoins < amountBet)
            {
                await ReplyAsync($"{taggedUser.Username} does not have enough {coinsName}!");
                return;
            }

            if (Context.User.Id != taggedUser.Id)
            {
                var gmblmsg = Program.gamblingMsgs;

                if (!gmblmsg.Exists(x => x.uID == sUser))
                {
                    if (gmblmsg.Exists(x => x.vsID == vsUser) || gmblmsg.Exists(x => x.uID == vsUser))
                    {
                        await ReplyAsync($"{taggedUser.Username} Already has an open request");
                        return;
                    }
                    var downvoteEmoji = new Emoji("👎");

                    var sendmsg = await ReplyAsync($"{Context.User.Mention} has requested to rock, paper, scissors {amountBet} {coinsName} vs {taggedUser.Mention}. Whisper me 'rock', 'paper' or 'scissors' to play.");
                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(downvoteEmoji);

                    var usermsg = new GamblingRequests();
                    usermsg.vsID = vsUser;
                    usermsg.uID = sUser;
                    usermsg.mID = msgID;
                    usermsg.rpsUser = 0;
                    usermsg.rpsVser = 0;
                    usermsg.msgChannel = Context.Channel;
                    usermsg.gamePlayed = 3;
                    usermsg.coinsToGamble = amountBet;
                    gmblmsg.Add(usermsg);



                }
                else
                {
                    await ReplyAsync($"{Context.Message.Author.Mention} You already have an open gambling request.");
                }
            }
            else
                await ReplyAsync($"{Context.Message.Author.Mention} something went wrong, did you try to play vs yourself?");
        }

        [Command("roll")]
        public async Task RandomRoll(int userArgs)
        {
            Random rnd = new Random();
            if (userArgs == 0)
            {
                await ReplyAsync("Please input a number to roll");
            }
            else
            {
                int rolledNumber = rnd.Next(1, userArgs + 1);
                await ReplyAsync($"{Context.User.Mention} rolled {rolledNumber}");
            }
        }

        [Command("rollbattle")]
        public async Task RollBattleCoins(int userArgs)
        {

            if (userArgs < 1)
                return;

            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);

            if (userCoins < userArgs)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }

            var gmblmsg = Program.gamblingMsgs;
            if (gmblmsg.Exists(x => x.uID == sUser))
            {
                await ReplyAsync($"{Context.Message.Author.Mention} You already have an open gambling request.");
                return;
            }


            Random rnd = new Random();
            int userRoll, botRoll;
            userRoll = rnd.Next(1, userArgs + 1);
            botRoll = rnd.Next(1, userArgs + 1);
            string outcome = "";
            int pot = 0;
            bool winOrLose = true;
            if (userRoll > botRoll)
            {
                pot = userRoll - botRoll;
                outcome = "win";
                winOrLose = true;
            }
            else
            {
                pot = botRoll - userRoll;
                outcome = "lose";
                winOrLose = false;
            }

            await ReplyAsync($"You rolled {userRoll}, I rolled {botRoll}, you {outcome} {pot} {coinsName}!");

            await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, pot, winOrLose);


        }

        [Command("guess")]
        public async Task TimedCommand(int guess)
        {
            int timeInMinutes = 10;
            var timedEvent = Program.TimedEventsList;
            DateTime StopTime;
            StopTime = DateTime.Now.AddMinutes(timeInMinutes);
            //Console.WriteLine(DateTime.Now.CompareTo(StopTime));
            SocketGuildUser sUser = (SocketGuildUser)Context.User;

            if (!timedEvent.Exists(x => x.uID == sUser))
            {
                var timedEventList = new TimedEventsLimit();
                timedEventList.uID = sUser;
                timedEventList.StopTime = StopTime;
                timedEventList.stage = 1;
                timedEvent.Add(timedEventList);
            }
            if (timedEvent.Exists(x => x.uID == sUser))
            {
                var eventInList = timedEvent.Find(x => x.uID == sUser);
                if (eventInList.StopTime.CompareTo(DateTime.Now) > 0 && eventInList.stage == -1)
                {
                    await ReplyAsync($"{sUser.Mention} You can only run the guess command once every {timeInMinutes} minutes.");
                    return;
                }
                else if (eventInList.StopTime.CompareTo(DateTime.Now) <= 0 && eventInList.stage == -1)
                {
                    eventInList.stage = 1;
                }
                int min, max, pot, botNumber, nextStage;
                int currentStage = eventInList.stage;
                Random rnd = new Random();

                if (currentStage > 0)
                {
                    min = 1;
                    max = currentStage + 1;
                    pot = (int)Math.Pow(2, currentStage + 4);
                    nextStage = currentStage + 1;
                    if (guess >= min && guess <= max)
                    {
                        botNumber = rnd.Next(min, max + 1);
                        if (guess == botNumber)
                        {
                            await ReplyAsync($"The number was {botNumber} you win {pot} {coinsName}!" +
                                $"\nfor your next number please guess between {min} and {max + 1}. Command usage '!guess #numbertoguess'.");
                            await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, pot, true);
                            eventInList.stage = nextStage;
                        }
                        else
                        {
                            await ReplyAsync($"The number was {botNumber} you lose! Try again in 10 minutes!");
                            eventInList.stage = -1;
                        }
                    }
                    else
                    {
                        await ReplyAsync($"Please guess between {min} and {max}, command usage '!guess #numbertoguess'.");
                        return;
                    }
                }

            }



        }

        [Command("rollvs")]
        public async Task RollVsOtherUser(Discord.IGuildUser taggedUser, int gambleAmount)
        {
            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            SocketGuildUser vsUser = (SocketGuildUser)taggedUser;

            if (gambleAmount < 1)
                return;
            if (taggedUser.Id == 762683246941044747)
                return;

            if (!ChiatoBotDatabase.CheckIfUserExist(vsUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(vsUser.Id, 0, true);
            }
            else if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);
            int vsCoins = await ChiatoBotDatabase.DBReadCoins(vsUser.Id);

            if (userCoins < gambleAmount)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }
            else if (vsCoins < gambleAmount)
            {
                await ReplyAsync($"{taggedUser.Username} does not have enough {coinsName}!");
                return;
            }

            if (Context.User.Id != taggedUser.Id)
            {
                var gmblmsg = Program.gamblingMsgs;

                if (!gmblmsg.Exists(x => x.uID == sUser))
                {
                    if (gmblmsg.Exists(x => x.vsID == vsUser) || gmblmsg.Exists(x => x.uID == vsUser))
                    {
                        await ReplyAsync($"{taggedUser.Username} Already has an open request");
                        return;
                    }
                    var upvoteEmoji = new Emoji("👍");
                    var downvoteEmoji = new Emoji("👎");

                    var sendmsg = await ReplyAsync($"{Context.User.Mention} has requested to roll {gambleAmount} {coinsName} vs {taggedUser.Mention}. Press {upvoteEmoji} to accept.");
                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(upvoteEmoji);
                    await sendmsg.AddReactionAsync(downvoteEmoji);

                    var usermsg = new GamblingRequests();
                    usermsg.vsID = vsUser;
                    usermsg.uID = sUser;
                    usermsg.mID = msgID;
                    usermsg.gamePlayed = 1;
                    usermsg.coinsToGamble = gambleAmount;
                    gmblmsg.Add(usermsg);

                }
                else
                {
                    await ReplyAsync($"{Context.Message.Author.Mention} You already have an open gambling request.");
                }
            }
            else
                await ReplyAsync($"{Context.Message.Author.Mention} something went wrong, did you try to roll vs yourself?");
        }

        [Command("give")]
        public async Task GiveCoins(Discord.IGuildUser taggedUser, int giveAmount)
        {
            if (giveAmount < 1)
                return;

            await ChiatoBotDatabase.DBUpdateCoins(taggedUser.Id, giveAmount, true);
            await ChiatoBotDatabase.DBUpdateCoins(Context.User.Id, giveAmount, false);
            await ReplyAsync($"{Context.User.Mention} has given {taggedUser.Username} {giveAmount} {coinsName}!");

        }

        [Command("coinflip")]
        public async Task CoinflipCoins(Discord.IGuildUser taggedUser, int gambleAmount)
        {

            if (gambleAmount < 1)
                return;
            if (taggedUser.Id == 762683246941044747)
                return;

            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            SocketGuildUser vsUser = (SocketGuildUser)taggedUser;

            if (!ChiatoBotDatabase.CheckIfUserExist(vsUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(vsUser.Id, 0, true);
            }
            else if (!ChiatoBotDatabase.CheckIfUserExist(sUser.Id))
            {
                await ChiatoBotDatabase.DBUpdateCoins(sUser.Id, 0, true);
            }

            int userCoins = await ChiatoBotDatabase.DBReadCoins(sUser.Id);
            int vsCoins = await ChiatoBotDatabase.DBReadCoins(vsUser.Id);

            if (userCoins < gambleAmount)
            {
                await ReplyAsync($"You do not have enough {coinsName}!");
                return;
            }
            else if (vsCoins < gambleAmount)
            {
                await ReplyAsync($"{taggedUser.Username} does not have enough {coinsName}!");
                return;
            }

            if (Context.User.Id != taggedUser.Id)
            {
                var gmblmsg = Program.gamblingMsgs;

                if (!gmblmsg.Exists(x => x.uID == sUser))
                {
                    if (gmblmsg.Exists(x => x.vsID == vsUser) || gmblmsg.Exists(x => x.uID == vsUser))
                    {
                        await ReplyAsync($"{taggedUser.Username} Already has an open request");
                        return;
                    }
                    var upvoteEmoji = new Emoji("👍");
                    var downvoteEmoji = new Emoji("👎");

                    var sendmsg = await ReplyAsync($"{Context.User.Mention} has requested to coinflip {gambleAmount} {coinsName} vs {taggedUser.Mention}. Press {upvoteEmoji} to accept.");
                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(upvoteEmoji);
                    await sendmsg.AddReactionAsync(downvoteEmoji);

                    var usermsg = new GamblingRequests();
                    usermsg.vsID = vsUser;
                    usermsg.uID = sUser;
                    usermsg.mID = msgID;
                    usermsg.gamePlayed = 2;
                    usermsg.coinsToGamble = gambleAmount;
                    gmblmsg.Add(usermsg);
                }
                else
                {
                    await ReplyAsync($"{Context.Message.Author.Mention} You already have an open gambling request.");
                }
            }
            else
                await ReplyAsync($"{Context.Message.Author.Mention} something went wrong, did you try to roll vs yourself?");
        }

        [Command("editcoins")]
        public async Task GiveCoinsDB(string args, Discord.IGuildUser userArgs, int userArgs2)
        {
            ulong otherUserDiscordID = userArgs.Id;
            ulong commandUser = Context.User.Id;
            bool give = true;
            string giveOrTake;
            if (args == "take")
                give = false;
            else
                give = true;

            if (give == true)
                giveOrTake = "given";
            else
                giveOrTake = "taken";

            if (!ChiatoBotDatabase.CheckIfUserExist(otherUserDiscordID))
            {
                await ChiatoBotDatabase.DBUpdateCoins(otherUserDiscordID, 0, true);
            }

            if (await CheckIfCommandUserIsAdmin(commandUser))
            {
                await ChiatoBotDatabase.DBUpdateCoins(otherUserDiscordID, userArgs2, give);
                int coins = await ChiatoBotDatabase.DBReadCoins(otherUserDiscordID);
                await ReplyAsync($"{Context.User.Mention} has {giveOrTake} {userArgs.Mention} {userArgs2} {coinsName}, {userArgs.Username} now has {coins} {coinsName}.");
            }
            else
            {
                return;
            }
        }

        [Command("editcoinsall")]
        public async Task GiveAll(string args, int userArgs2)
        {

            List<ulong> listOfDBUsers = new List<ulong>();
            listOfDBUsers = await ChiatoBotDatabase.GetAllUsersInDB();
            ulong commandUser = Context.User.Id;
            bool give = true;
            string giveOrTake;
            if (args == "take")
                give = false;
            else
                give = true;

            if (give == true)
                giveOrTake = "given";
            else
                giveOrTake = "taken from";

            int i = 0;
            if (await CheckIfCommandUserIsAdmin(commandUser))
            {
                foreach (var k in listOfDBUsers)
                {
                    ulong dbID = listOfDBUsers[i];
                    await ChiatoBotDatabase.DBUpdateCoins(dbID, userArgs2, give);
                    i++;
                }
                await ReplyAsync($"{Context.User.Mention} has {giveOrTake} everyone {userArgs2} {coinsName}!");
            }
            else
            {
                return;
            }
        }

        [Command("giveaway")]
        public async Task GiveAway(int coinsToGive)
        {
            ulong commandUser = Context.User.Id;
            var giveAwayList = Program.ListOfGiveaways;

            if (await CheckIfCommandUserIsAdmin(commandUser))
            {
                if (giveAwayList.Count < 1)
                {
                    var reactEmoji = new Emoji("💸");

                    var sendmsg = await ReplyAsync($"{Context.User.Mention} has started a giveaway of {coinsToGive} {coinsName}!. Press {reactEmoji} to enter.");
                    var msgID = sendmsg.Id;

                    await sendmsg.AddReactionAsync(reactEmoji);

                    var thisGiveaway = new ListOfGiveawaysClass();
                    thisGiveaway.mID = msgID;
                    thisGiveaway.giveawayAmount = coinsToGive;

                    giveAwayList.Add(thisGiveaway);

                }
                else
                {
                    await ReplyAsync($"{Context.Message.Author.Mention} There's already an on-going giveaway");
                }
            }
            else
            {
                return;
            }
        }

        [Command("giveawaylist")]
        public async Task PrintGiveAwayParticipants()
        {
            var giveAwayList = Program.GiveawayList;
            if (await CheckIfCommandUserIsAdmin(Context.User.Id))
            {

                int amountToShow = giveAwayList.Count();

                int place = 1;
                amountToShow = amountToShow * 2;
                string giveAwayStr = "";
                string addToListString = "";
                foreach (var k in giveAwayList)
                {
                    addToListString = ($"{place}. {k.enteredUsers.Username}");
                    giveAwayStr = giveAwayStr + "\n" + addToListString;
                    place++;
                }
                await ReplyAsync(giveAwayStr);
            }
            else
                return;
        }

        [Command("giveawayend")]
        [Alias("giveawaydraw")]
        public async Task DrawGiveawayWinner()
        {
            var giveAwayList = Program.GiveawayList;
            int peopleInGiveaway = giveAwayList.Count();
            Random rnd = new Random();
            int winnerNumber = rnd.Next(0, peopleInGiveaway + 1);
            var thisGiveawayInList = Program.ListOfGiveaways.ElementAt(0);
            int giveAwayAmount = thisGiveawayInList.giveawayAmount;
            var winner = giveAwayList.ElementAt(winnerNumber);
            Discord.WebSocket.SocketUser winnerUser = winner.enteredUsers;

            await ReplyAsync($"The giveaway has ended!" +
                $"\nThe number drawn between 1 and {peopleInGiveaway}, was {winnerNumber+1}." +
                $"\nThe winner is {winnerUser.Mention}. The prize was {giveAwayAmount} {coinsName}");
            await ChiatoBotDatabase.DBUpdateCoins(winnerUser.Id, giveAwayAmount, true);
            Program.ListOfGiveaways.Clear();
            Program.GiveawayList.Clear();
        }

        [Command("giveawayremove")]
        public async Task RemoveGiveawayFromList()
        {
            var thisGiveawayInList = Program.ListOfGiveaways.ElementAt(0);
            await ReplyAsync($"Current giveaway has been cancelled by {Context.User.Username}.");
            Program.ListOfGiveaways.Clear();
            Program.GiveawayList.Clear();
        }
        
        [Command("coinsname")]
        public async Task RenameCoins(string userArgs)
        {
            var allowedRoles = new List<ulong>();
            allowedRoles.Add(553614396669296640);
            allowedRoles.Add(553614468316397589);

            SocketGuildUser sUser = (SocketGuildUser)Context.User;
            if (sUser.Roles.Any(x => allowedRoles.Any(y => y == x.Id)))
            {
                Program.coinsName = userArgs;
                await ReplyAsync($"I've renamed the currency to {userArgs}");
            }
            else
            {
                await ReplyAsync($"Sorry {Context.User.Mention} you do not have the required role to execute this command.");
            }
        }

        [Command("w2g")]
        public async Task WatchTogether()
        {
            await ReplyAsync("My developer didn't implement this yet because he is an idiot.");
            //w2g
        }

        [Command("fortnite")]
        public async Task PrintFortnite()
        {
            int i = 0;
            ChatStrings.printFNDance();
            string[] fdArray = ChatStrings.fortniteDance;
            while (i < fdArray.Length)
            {
                await ReplyAsync(fdArray[i] + fdArray[i+1]);
                i++;
                i++;
            }
            await ReplyAsync(fdArray[10]);
        }
        public async Task<bool> CheckIfCommandUserIsAdmin(ulong commandUser)
        {
            if (commandUser == coinAdmins)
            {
                return true;
            }
            else
            {
                await ReplyAsync($"Sorry you must be an admin to execute this command!");
                return false;
            }
        }

        [Command("countdb")]
        public async Task CountDB()
        {
            int amountOfRows = await ChiatoBotDatabase.DBReadRows();
            await ReplyAsync($"The database has {amountOfRows} rows.");
        }
    }
}

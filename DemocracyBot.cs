using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Net;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using FreneticUtilities.FreneticExtensions;
using FreneticUtilities.FreneticDataSyntax;
using DiscordBotBase;
using DemocracyDiscordBot.CommandHandlers;

namespace DemocracyDiscordBot
{
    /// <summary>
    /// General program entry and handler.
    /// </summary>
    public class DemocracyBot
    {
        /// <summary>
        /// The ID of the guild this bot is for.
        /// </summary>
        public static ulong OwningGuildID;

        /// <summary>
        /// The FDS Section of current vote topics.
        /// </summary>
        public static FDSSection VoteTopicsSection;

        /// <summary>
        /// Save the config, after any votes are cast.
        /// </summary>
        public static void Save()
        {
            DiscordBotBaseHelper.CurrentBot.SaveConfig();
        }

        /// <summary>
        /// Software entry point - starts the bot.
        /// </summary>
        static void Main(string[] args)
        {
            InfoCommands infoCommands = new InfoCommands();
            VotingCommands voteCommands = new VotingCommands();
            DiscordBotBaseHelper.StartBotHandler(args, new DiscordBotConfig()
            {
                CommandPrefix = "!",
                Initialize = (bot) =>
                {
                    OwningGuildID = bot.ConfigFile.GetUlong("guild_id").Value;
                    VoteTopicsSection = bot.ConfigFile.GetSection("vote_topics");
                    bot.RegisterCommand(infoCommands.CMD_Help, "help", "halp", "hlp", "hal", "hel", "h", "?", "use", "usage");
                    bot.RegisterCommand(infoCommands.CMD_Hello, "hello", "hi", "whoareyou", "what", "link", "info");
                    bot.RegisterCommand(voteCommands.CMD_Ballot, "ballot", "b");
                    bot.RegisterCommand(voteCommands.CMD_Vote, "vote", "v");
                    bot.RegisterCommand(voteCommands.CMD_ClearVote, "clearvote", "voteclear", "cv");
                },
                ShouldPayAttentionToChannel = (channel) =>
                {
                    return channel is ISocketPrivateChannel;
                }
            });
        }
    }
}
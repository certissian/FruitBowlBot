﻿using Discord;
using System;
using System.Collections.Generic;
using TwitchLib;
using TwitchLib.Models.API;
using TwitchLib.Models.API.Stream;
using TwitchLib.Models.Client;

namespace JefBot.Commands
{
    internal class DebugPluginCommand : IPluginCommand
    {
        public string PluginName => "Debug";
        public string Command => "debug";
        public string Help => "!debug to get a bunch of stream data";
        public IEnumerable<string> Aliases => new string[0];
        public bool Loaded { get; set; } = true;

        public void Execute(ChatCommand command, TwitchClient client)
        {
            if (!Bot.IsStreaming(command.ChatMessage.Channel))
            {
                try
                {
                    //totally ok to add yourself to debug :^)
                    if (command.ChatMessage.Username == "mikaelssen" || command.ChatMessage.IsBroadcaster || command.ChatMessage.IsModerator)
                    {
                        Stream stream = TwitchApi.Streams.GetStream(command.ChatMessage.Channel);
                        client.SendMessage(command.ChatMessage.Channel, $"AvFPS:{stream.AverageFps} Delay:{stream.Delay} Game:{stream.Game} Viewers:{stream.Viewers} videoHeight:{stream.VideoHeight}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

        }


        //echo command
        public void Discord(MessageEventArgs arg, DiscordClient client)
        {
            arg.Channel.SendMessage($"{arg.Message.Text}");
        }

    }
}

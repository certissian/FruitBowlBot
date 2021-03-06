﻿using System;
using System.Collections.Generic;
using TwitchLib;
using Discord;
using Discord.Commands;
using TwitchLib.Models.Client;
using Discord.WebSocket;
using System.Diagnostics;
using System.Windows.Forms;

namespace JefBot.Commands
{
    internal class RestartPluginCommand : IPluginCommand
    {
        public string PluginName => "Reset";
        public string Command => "reset";
        public string Help => "!reset to restart the bot";
        public IEnumerable<string> Aliases => new string[0];
        public bool Loaded { get; set; } = true;

        public string Action(Message message)
        {
            if (message.IsModerator)
            {
                Process.Start(Application.StartupPath + "\\JefBot.exe");
                Process.GetCurrentProcess().Kill();
            }
            return "Fuck you, it's January! " + message.Username;
        }
        
    }
}

﻿using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;

namespace JefBot.Commands
{
    internal class NextStream : IPluginCommand
    {
        public string PluginName => "NextStream";
        public string Command => "next";
        public string Help => "!n will give the estimated time untill the next stream.";
        public IEnumerable<string> Aliases => new[] { "n", "nextstream", "countdown" };
        public bool Loaded { get; set; } = true;

        Dictionary<DayOfWeek, TimeSpan> streamtimes = new Dictionary<DayOfWeek, TimeSpan>();

        public NextStream()
        {
            streamtimes.Add(DayOfWeek.Monday, TimeSpan.FromHours(21)); //Monday at 8 Norweeb time
            streamtimes.Add(DayOfWeek.Wednesday, TimeSpan.FromHours(21)); 
            streamtimes.Add(DayOfWeek.Saturday, TimeSpan.FromHours(21)); 
        }

        public void Execute(ChatCommand command, TwitchClient client)
        {
            if (command.ChatMessage.Channel == "jefmajor")
            {
                client.SendMessage(command.ChatMessage.Channel, time().Result);
            }
        }

        public async Task<string> time()
        {
            var uptime = await TwitchApi.GetUptime("jefmajor");
            if (uptime.Ticks == 0)
            {
                List<DateTime> times = new List<DateTime>();
                foreach (var item in streamtimes)
                {
                    DateTime start = DateTime.Now;
                    DateTime then = start.AddDays(((int)item.Key - (int)start.DayOfWeek + 7) % 7);
                    then = then.Date + item.Value; // sets the time from whatever to the 20'th hour
                    times.Add(then);
                }
                times.Sort((a, b) => a.CompareTo(b)); //ascending sort
                TimeSpan span = times[0].Subtract(DateTime.Now);
                if (span.Minutes < 0)
                {
                    span = times[1].Subtract(DateTime.Now);
                }

                return $"Next stream in {span.Hours}:{span.Minutes}:{span.Seconds}:{span.Milliseconds}~~ on the {times[0].Day}{getSuffix(times[0].Day)}";
                
            }
            else
            {
                return $"He's on right now silly";
            }
        }

        private string getSuffix(int day)
        {
            return (day == 11 || day == 12 || day == 13) ? "th" : (day == 1) ? "st" : (day == 2) ? "nd" : (day == 3) ? "rd" : "th";
        }

        public void Discord(MessageEventArgs arg)
        {
            arg.Channel.SendMessage(time().Result);
        }
    }
}

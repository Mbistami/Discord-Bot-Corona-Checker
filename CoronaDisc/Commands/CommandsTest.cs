using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.Text.RegularExpressions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Drawing;
using System.Net;
using System.IO;
using System.Timers;

namespace CoronaDisc
{
    public class CommandsTest : BaseCommandModule
    {
        [Command("Allo"), ]
        public async Task Ping(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            builder.Description = "TEST";
            builder.Color = DiscordColor.Red;
            
            await ctx.Channel.SendMessageAsync("",false, builder.Build()).ConfigureAwait(false);

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

        }

        [Command("active"), Aliases("a")]
        public async Task Active(CommandContext ctx, string text)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://www.worldometers.info/coronavirus/");
            }
            text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            var find = ">" + text + "</td>";
            var find2 = ">" + text + "</a></td>";
            var ret = string.Empty;
            int pos = htmlCode.IndexOf(find);
            int pos1 = htmlCode.IndexOf(find2);

            string tmp;

            if (pos >= 0 || pos1 >= 0)
            {
                int total = 0;
                if (pos1 >= 0)
                {
                    tmp = htmlCode.Substring(pos1 + 3, 105);
                    tmp.Remove(',');
                    builder.AddField(text.ToUpper(), "Total Cases : " + string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")).ToString(),false);
                    total = Convert.ToInt32(string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")));
                }
                else if (pos >= 0)
                {
                    tmp = htmlCode.Substring(pos + 3, 105);
                    tmp.Remove(',');
                    total = Convert.ToInt32(string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")));
                    builder.AddField(text.ToUpper(), "Total Cases : " + string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")).ToString(), false);
                }
                if (total >= 10 && total < 100)
                    builder.Color = DiscordColor.Green;
                else if (total >= 100 && total < 1000)
                    builder.Color = DiscordColor.Orange;
                if (total >= 1000)
                    builder.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync("", false, builder.Build()).ConfigureAwait(false);
            }
        }
        [Command("f")]
        public async Task Full(CommandContext ctx, string text,string text1)
        {
            text1 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text1.ToLower());
            text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            if (text1 != string.Empty)
            {
                text = text + " " + text1;
            }
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://google.org/crisisresponse/covid19-map");
            }

            int start = htmlCode.IndexOf("<table>");
            int end = htmlCode.IndexOf("</tbody>");
            int len = end - start;

            htmlCode = htmlCode.Substring(start, len);

            string htmlCleaned = Regex.Replace(htmlCode, @">[\d]</td>", "");
            string tmp = htmlCleaned.Substring(htmlCleaned.IndexOf(text), 1000);

            tmp = tmp.Replace(".", string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            string[] s = new string[8];
            s = check(Regex.Replace(tmp, @"\s", "")).Split(',');
            //total 1
            //Recovred 2
            //Deaths 3
            int total = Convert.ToInt32(s[1]);
            if (total >= 10 && total < 100)
                builder.Color = DiscordColor.Green;
            else if (total >= 100 && total < 1000)
                builder.Color = DiscordColor.Orange;
            if (total >= 1000)
                builder.Color = DiscordColor.Red;
            builder.Color = DiscordColor.Red;
            int activeCases = Convert.ToInt32(s[1]) - Convert.ToInt32(s[5]) - Convert.ToInt32(s[7]);
            builder.AddField(text.ToUpper(), "Total : " + s[1] + "\nRecovred : " + s[5] + "\nDeaths : " + s[7] + "\nActive Cases : "  + activeCases , false);

            await ctx.Channel.SendMessageAsync("", false, builder.Build()).ConfigureAwait(false);
        }

        [Command("f")]
        public async Task Full(CommandContext ctx, string text)
        {
            text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://google.org/crisisresponse/covid19-map");
            }

            int start = htmlCode.IndexOf("<table>");
            int end = htmlCode.IndexOf("</tbody>");
            int len = end - start;

            htmlCode = htmlCode.Substring(start, len);

            string htmlCleaned = Regex.Replace(htmlCode, @">[\d]</td>", "");
            string tmp = htmlCleaned.Substring(htmlCleaned.IndexOf(text), 1000);

            tmp = tmp.Replace(".", string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            string[] s = new string[8];
            s = check(Regex.Replace(tmp, @"\s", "")).Split(',');
            //total 1
            //Recovred 2
            //Deaths 3
            int total = Convert.ToInt32(s[1]);
            if (total >= 10 && total < 100)
                builder.Color = DiscordColor.Green;
            else if (total >= 100 && total < 1000)
                builder.Color = DiscordColor.Orange;
            if (total >= 1000)
                builder.Color = DiscordColor.Red;
            builder.Color = DiscordColor.Red;
            int activeCases = Convert.ToInt32(s[1]) - Convert.ToInt32(s[5]) - Convert.ToInt32(s[7]);
            builder.AddField(text.ToUpper(), "Total : " + s[1] + "\nRecovred : " + s[5] + "\nDeaths : " + s[7] + "\nActive Cases : " + activeCases, false);

            await ctx.Channel.SendMessageAsync("", false, builder.Build()).ConfigureAwait(false);
        }
        public static void OnTimedEvent(object source, ElapsedEventArgs e)
        {

        }

        public static string check(string s)
        {
            string res = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                string tmp = s[i].ToString();
                if (s[i] == '+')
                {
                    if (i + 1 != s.Length)
                    {
                        if (Regex.IsMatch(s[i + 1].ToString(), @"^\d+$"))
                            res = res + '+';
                    }
                }
                if (Regex.IsMatch(s[i].ToString(), @"^\d+$"))
                {
                    res = res + s[i];
                    if (i + 1 != s.Length)
                    {
                        if (!Regex.IsMatch(s[i + 1].ToString(), @"^\d+$"))
                            res = res + ',';
                    }
                }
            }
            return res;
        }

        public async Task Send(CommandContext context)
        {

        }
    }
}

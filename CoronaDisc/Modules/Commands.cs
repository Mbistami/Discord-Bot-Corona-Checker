using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Net;

namespace CoronaDisc.Modules
{
    class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("active"), Alias("a")]
        [Summary("Make the bot say something")]
        public Task ActiveCases([Remainder]string text)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "Stats"
            };

            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://www.worldometers.info/coronavirus/");
            }
            var find = ">" + text + "</td>";
            var find2 = ">" + text + "</a></td>";
            var ret = string.Empty;
            int pos = htmlCode.IndexOf(find);
            int pos1 = htmlCode.IndexOf(find2);

            string tmp;

            if (pos >= 0 || pos1 >= 0)
            {
                if (pos1 >= 0)
                {
                    tmp = htmlCode.Substring(pos1 + 3, 105);
                    tmp.Remove(',');
                    builder.AddField(x =>
                    {
                        x.Name = text.ToUpper();
                        x.Value = "Total Cases : " + string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")).ToString();
                        x.IsInline = false;
                    });
                    ReplyAsync("", false, builder.Build());
                }
                else if (pos >= 0)
                {
                    tmp = htmlCode.Substring(pos + 3, 105);
                    tmp.Remove(',');
                    builder.AddField(x =>
                    {
                        x.Name = text.ToUpper();
                        x.Value = "Total Cases : " + string.Join(null, System.Text.RegularExpressions.Regex.Split(tmp, "[^\\d]")).ToString();
                        x.IsInline = false;
                    });
                    ReplyAsync("", false, builder.Build());
                }
            }
            else
                ReplyAsync("Not found !");

            Task t = Task.CompletedTask;
            return t;
        }
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using DSharpPlus.Entities;
using System.Threading.Tasks;
namespace CoronaDisc
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public DiscordActivity activity { get; set; }
        public DiscordRichPresence RichPresence { get; set; }
        public DiscordChannel channel { get; set; }

        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
                };
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsCongif = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = false,
            };


            

            Commands = Client.UseCommandsNext(commandsCongif);

            Commands.RegisterCommands<CommandsTest>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }
        
        private async Task OnClientReady(ReadyEventArgs e)
        {
            activity = new DiscordActivity();
            activity.Name = "use ?help";
            Client.UpdateStatusAsync(activity);
            await Task.Delay(1);
        }

    }
}

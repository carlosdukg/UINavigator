using McMaster.Extensions.CommandLineUtils;

namespace ConsoleTester.Commands
{
    [Command(Name = "login", Description = "login to ultipro, the login crendentials will be saved locally in the profile")]
    public class LoginCommand
    {
        readonly IConsole _console;

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "username", Description = "ultipro login username", ValueName = "login username", ShowInHelpText = true)]
        public string? Username { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "password", Description = "ultipro login password", ValueName = "login password", ShowInHelpText = true)]
        public string? Password { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "l", LongName = "location", Description = "ultipro login full url", ValueName = "login full url", ShowInHelpText = true)]
        public string? Location { get; set; }

        [Option(CommandOptionType.NoValue, LongName = "sso-user", Description = "sso user flag", ValueName = "sso user", ShowInHelpText = true)]
        public bool IsSSOUser { get; set; } = false;

        [Option(CommandOptionType.SingleValue, ShortName = "cc", LongName = "company code", Description = "ultipro login company code for SSO user", ValueName = "company code", ShowInHelpText = true)]
        public string? CompanyCode { get; set; }

        public LoginCommand(IConsole console) => _console = console;
    }
}

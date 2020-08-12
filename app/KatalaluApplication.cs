using ShadAhm.Katalalu.App.Services;
using Cons = ShadAhm.Katalalu.App.Services.ConsoleService;

namespace ShadAhm.Katalalu.App
{
    public class KatalaluApplication
    {
        private readonly PasswordService _passwordService;
        private readonly SettingService _settingService;

        public KatalaluApplication()
        {
            _passwordService = new PasswordService();
            _settingService = SettingService.Instance;
        }

        public void Run()
        {
            OutputWelcome();
            ReadCommand();
            Cons.AnyKeyToExit();
        }

        void OutputWelcome() => Cons.WriteLines("Welcome to Katalalu Local Password Manager", "------------------------------------------");

        void ReadCommand()
        {
            string masterPassword = string.Empty;

            switch (Cons.ReadCommand())
            {
                case "cls":
                    Cons.Clear();
                    break;
                    case "init":
                    string csvLocation = Cons.ReadCommand("Csv location: ");
                    _settingService.Init(@"" + csvLocation);
                    break;
                case "pwd-list":
                    masterPassword = Cons.ReadCommand("Master Password: ");
                    _passwordService.List(masterPassword);
                    break;
                case "enc-all":
                    masterPassword = Cons.ReadCommand("Master Password: ");
                    _passwordService.EncryptAll(masterPassword);
                    break;
                case "csv-location":
                    Cons.WriteLine(_settingService.GetCsvLocation());
                    break;
                case "quit":
                case "q": return;
                default:
                    Cons.Error("Command not found");
                    break;
            }

            ReadCommand();
        }
    }
}
namespace Spoonful.Settings
{
    public class EmailConfiguration
    {
        public const string Email = "EmailConfiguration";
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

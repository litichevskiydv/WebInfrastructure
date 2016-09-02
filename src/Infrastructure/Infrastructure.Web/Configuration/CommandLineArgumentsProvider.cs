namespace Infrastructure.Web.Configuration
{
    public class CommandLineArgumentsProvider 
    {
        public CommandLineArgumentsProvider(string[] args)
        {
            Arguments = args;
        }

        public string[] Arguments { get; }
    }
}
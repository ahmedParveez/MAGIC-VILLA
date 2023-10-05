namespace MagicVilla_API.Logging
{
    public class Logging : ILogging
    {
        public void Log(string type, string message)
        {
            if(type.ToLower() == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error - "+message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if(type.ToLower() == "success" || type.ToLower() == "")
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Success - "+message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}

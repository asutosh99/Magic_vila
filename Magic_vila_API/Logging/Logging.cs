namespace Magic_vila_API.Logging
{
    public class Logging:ILogging
    {
        
        public void Log(string message, string type)
        {
            if(type == "Error") {
            Console.WriteLine("Error - "+ message);
            }
            if (type == "Info")
            {
                Console.WriteLine("INFO - " + message);
            }
        }
    }
}

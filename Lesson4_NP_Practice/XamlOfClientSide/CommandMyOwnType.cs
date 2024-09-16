namespace XamlOfClientSide
{


    internal class CommandMyOwnType
    {
        public string CommandType { get; set; }

        public string ProcessName { get; set; }


        public CommandMyOwnType(string commandType, string processName)
        {
            CommandType = commandType;
            ProcessName = processName;
        }

    }


}
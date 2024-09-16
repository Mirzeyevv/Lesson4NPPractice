using System.Diagnostics;

namespace Lesson4_NP_Practice
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
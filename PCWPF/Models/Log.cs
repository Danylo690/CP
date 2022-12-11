using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCWPF.Models
{
    public class Log
    {
        private string _commandName;
        private string _commandResult;
        private string _commandTime;

        public string CommandName { get => _commandName; set => _commandName = value; }
        public string CommandResult { get => _commandResult; set => _commandResult = value; }
        public string CommandTime { get => _commandTime; set => _commandTime = value; }
    }
}

using PCWPF.Models;
using System.Collections.Generic;

namespace PCWPF.ViewModels
{
    public class MessageHistoryViewModel : ViewModelBase
    {
        private List<Log> _logs;

        public List<Log> Logs { 
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged(nameof(Logs));
            }
        }
    }
}

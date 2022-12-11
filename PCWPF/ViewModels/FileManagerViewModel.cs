using PCWPF.Models;
using PCWPF.Views;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        #region Fields
        private List<FileInformation> _files;
        private string _path;
        private List<string> pathHistory;
        private int currentPathIndex;
        private IFileManager fileManager;
        private FTPClient client;
        private PCFileManager pcFileManager;
        private string managerMode;
        private bool _nextFolderEnabled;
        private bool _prevFolderEnabled;
        public List<FileInformation> FilesToCopy;
        public string CopyFrom;
        private string pasteTo;
        private string _newFolderName;
        public bool IsDelete;
        #endregion

        #region Properties
        public List<FileInformation> Files
        {
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }
        public List<string> PathHistory { get => pathHistory; set => pathHistory = value; }
        public int CurrentPathIndex { get => currentPathIndex; set => currentPathIndex = value; }
        public FTPClient Client { get => client; set => client = value; }
        public PCFileManager PcFileManager { get => pcFileManager; set => pcFileManager = value; }
        public string ManagerMode { get => managerMode; set => managerMode = value; }
        public IFileManager FileManager { get => fileManager; set => fileManager = value; }
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        public bool NextFolderEnabled { 
            get => _nextFolderEnabled;
            set
            {
                _nextFolderEnabled = value;
                OnPropertyChanged(nameof(NextFolderEnabled));
            }
        }
        public bool PrevFolderEnabled { 
            get => _prevFolderEnabled;
            set
            {
                _prevFolderEnabled = value;
                OnPropertyChanged(nameof(PrevFolderEnabled));
            }
        }
        public string NewFolderName { 
            get => _newFolderName;
            set
            {
                _newFolderName = value;
                OnPropertyChanged(nameof(NewFolderName));
            }
        }
        #endregion

        #region Commands
        public ICommand ListCommand { get; }
        public ICommand NextFolderCommand { get; } 
        public ICommand PrevFolderCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand CreateNewFolderCommand { get; }
        public ICommand RenameCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion

        #region Constructors
        public FileManagerViewModel(IFileManager fileManager)
        {
            FileManager = fileManager;
            Files = FileManager.List("");
            PathHistory = new List<string>();
            PathHistory.Add("");
            CurrentPathIndex = 0;
            NextFolderEnabled = false;
            PrevFolderEnabled = false;
            Client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
            PcFileManager = (PCFileManager)Thread.GetData(Thread.GetNamedDataSlot("PCFileManager"));

            //Command
            ListCommand = new ViewModelCommand(ExecuteListCommand);
            NextFolderCommand = new ViewModelCommand(ExecuteNextFolderCommand);
            PrevFolderCommand = new ViewModelCommand(ExecutePrevFolderCommand);
            PasteCommand = new ViewModelCommand(ExecutePasteCommand);
            CreateNewFolderCommand = new ViewModelCommand(ExecuteCreateNewFolder);
            RenameCommand = new ViewModelCommand(ExecuteRenameCommand);
            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand);
        }

        private void ExecuteDeleteCommand(object SelectedItems)
        {
            if (PathHistory[CurrentPathIndex] == "" && FileManager.GetFileManagerMode() == "PC")
            {
                return;
            }
            if ((SelectedItems as IList).Count != 0)
            {
                new QuestionView()
                {
                    DataContext = this,
                    Owner = Application.Current.MainWindow,
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    Topmost = true,
                    QuestionTitle = "Delete files",
                    Question = "Are you sure you want to delete files?"
                }.ShowDialog();
                if (IsDelete)
                {
                    List<FileInformation> selectedFiles = (SelectedItems as IList).Cast<FileInformation>().ToList();
                    FileManager.Delete(selectedFiles);
                    Files = FileManager.List(PathHistory[CurrentPathIndex]);
                }
            }
        }

        private void ExecuteRenameCommand(object SelectedItems)
        {
            if (PathHistory[CurrentPathIndex] == "" && FileManager.GetFileManagerMode() == "PC")
            {
                return;
            }
            if ((SelectedItems as IList).Count != 0)
            {
                FileInformation selectedFile = (SelectedItems as IList).Cast<FileInformation>().First();
                NewFolderName = selectedFile.Name;
                new CreateOrRenameDialogView()
                {
                    DataContext = this,
                    Owner = Application.Current.MainWindow,
                    ShowActivated = true,
                    ShowInTaskbar = false,
                    Topmost = true,
                    Title = $"Rename {selectedFile.Name}"
                }.ShowDialog();

                if (!string.IsNullOrEmpty(NewFolderName) && NewFolderName != selectedFile.Name)
                {
                    string path = PathHistory[CurrentPathIndex];
                    FileManager.Rename(selectedFile, NewFolderName);
                    Files = FileManager.List(path);
                }
            }
        }

        private void ExecuteCreateNewFolder(object obj)
        {
            if (PathHistory[CurrentPathIndex] == "" && FileManager.GetFileManagerMode() == "PC")
            {
                return;
            }
            new CreateOrRenameDialogView()
            {
                DataContext = this,
                Owner = Application.Current.MainWindow,
                ShowActivated = true,
                ShowInTaskbar = false,
                Topmost = true,
                Title = "Create new folder"
            }.ShowDialog();

            if (!string.IsNullOrEmpty(NewFolderName))
            {
                string path = PathHistory[CurrentPathIndex];
                FileManager.Mkdir(path, NewFolderName);
                Files = FileManager.List(path);
            }
        }

        private void ExecutePasteCommand(object obj)
        {
            if (FilesToCopy.Count > 0)
            {
                pasteTo = FileManager.GetFileManagerMode();
                switch (CopyFrom + pasteTo)
                {
                    case "PC>PC":
                        {
                            break;
                        }
                    case "PC>Android":
                        {
                            string path = PathHistory[CurrentPathIndex];
                            Thread progressThread = new Thread(() =>
                            {
                                Thread.SetData(Thread.GetNamedDataSlot("PCFileManager"), PcFileManager);
                                Client.CanProgressClose = false;
                                Client.Upload(FilesToCopy, path);
                                Files = FileManager.List(path);
                                Client.CanProgressClose = true;
                            });
                            progressThread.Start();
                            new ProgressBarView()
                            {
                                DataContext = Client,
                                Owner = Application.Current.MainWindow,
                                ShowActivated = true,
                                ShowInTaskbar = false,
                                Topmost = true
                            }.ShowDialog();
                            break;
                        }
                    case "Android>PC":
                        {
                            string path = PathHistory[CurrentPathIndex];
                            if (path != "")
                            {
                                Thread progressThread = new Thread(() =>
                                {
                                    Client.CanProgressClose = false;
                                    Client.Download(FilesToCopy, path);
                                    Files = FileManager.List(path);
                                    Client.CanProgressClose = true;
                                });
                                progressThread.Start();
                                new ProgressBarView()
                                {
                                    DataContext = Client,
                                    Owner = Application.Current.MainWindow,
                                    ShowActivated = true,
                                    ShowInTaskbar = false,
                                    Topmost = true
                                }.ShowDialog();
                                progressThread.Abort();
                            }
                            break;
                        }
                    case "Android>Android":
                        {
                            break;
                        }
                }
            }
        }

        private void ExecutePrevFolderCommand(object obj)
        {
            string path = PathHistory[CurrentPathIndex - 1];
            Files = FileManager.List(path);
            Path = path;
            CurrentPathIndex--;
            NextFolderEnabled = true;
            if (CurrentPathIndex == 0)
            {
                PrevFolderEnabled = false;
            }
        }

        private void ExecuteNextFolderCommand(object obj)
        {
            string path = PathHistory[CurrentPathIndex + 1];
            Files = FileManager.List(path);
            Path = path;
            CurrentPathIndex++;
            PrevFolderEnabled = true;
            if (CurrentPathIndex == PathHistory.Count - 1)
            {
                NextFolderEnabled = false;
            }
        }

        private void ExecuteListCommand(object SelectedFiles)
        {
            FileInformation selectedFile = (SelectedFiles as IList).Cast<FileInformation>().First();
            if (selectedFile != null && selectedFile.IsFolder || selectedFile.IsDrive)
            {
                string path = $"{selectedFile.Path}";
                Files = FileManager.List(path);
                Path = path;
                if (CurrentPathIndex == PathHistory.Count - 1)
                {
                    PathHistory.Add(path);
                }
                else
                {
                    PathHistory.RemoveRange(CurrentPathIndex + 1, PathHistory.Count - (CurrentPathIndex + 1));
                    PathHistory.Add(path);
                    NextFolderEnabled = false;
                }
                CurrentPathIndex++;
                PrevFolderEnabled = true;
            }
        }
        #endregion


    }
}

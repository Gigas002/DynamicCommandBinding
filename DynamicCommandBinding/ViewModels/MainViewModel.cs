using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DynamicCommandBinding.Models;
using Prism.Mvvm;

namespace DynamicCommandBinding.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public static ObservableCollection<UIElement> Controls { get; } = new ObservableCollection<UIElement>();

        private static Dictionary<string, Delegate> MethodsDictionary { get; } = new Dictionary<string, Delegate>();

        public MainViewModel()
        {
            List<CommandModel> commands = Commands.GetModel("Resources/Commands.json").CommandsList;

            RegisterCommands();

            Button button = new Button();

            foreach (CommandModel command in commands)
            {
                command.BindWithSwitch(button); //, MethodsDictionary[command.MethodToExecute]);
            }

            Controls.Add(button);
        }

        private static void MessageBoxShow(string message) => MessageBox.Show(message);

        private static void CloseApp() => Application.Current.Shutdown();

        private static void RegisterCommands()
        {
            MethodsDictionary.Add(nameof(MessageBoxShow), new Action<string>(MessageBoxShow));
            MethodsDictionary.Add(nameof(CloseApp), new Action(CloseApp));
        }

        internal static void RunCommand(string commandName, string args)
        {
            if (string.IsNullOrWhiteSpace(args)) MethodsDictionary[commandName].DynamicInvoke();
            else MethodsDictionary[commandName].DynamicInvoke(args);
        }
    }
}

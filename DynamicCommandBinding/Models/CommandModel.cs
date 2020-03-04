using System;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls.Primitives;
using DynamicCommandBinding.ViewModels;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DynamicCommandBinding.Models
{
    public class CommandModel
    {
        [JsonPropertyName("EventName")]
        public string EventName { get; set; }

        [JsonPropertyName("MethodToExecute")]
        public string MethodToExecute { get; set; }

        [JsonPropertyName("Args")]
        public string Args { get; set; }

        public void BindWithSwitch(UIElement element)
        {
            switch (EventName)
            {
                case "Click":
                {
                    ButtonBase button = element as ButtonBase;
                    button.Click += (sender, args) => MainViewModel.RunCommand(MethodToExecute, Args);
                    break;
                }
                case "PreviewMouseLeftButtonDown":
                {
                    element.PreviewMouseLeftButtonDown += (sender, args) => MainViewModel.RunCommand(MethodToExecute, Args);
                    break;
                }
                case "MouseRightButtonUp":
                {
                    element.MouseRightButtonUp += (sender, args) => MainViewModel.RunCommand(MethodToExecute, Args);
                    break;
                }
                default: { throw new NotSupportedException($"Event {EventName} is not supported."); }
            }
        }

        public void BindWithReflection(UIElement element, Delegate command)
        {
            //Click event doesn't exist in UIElement. Need to implement switch on this.Event anyway?
            var eventInfo = typeof(UIElement).GetEvent(EventName);

            var methodInfo = command.Method;

            //Alternative
            //var methodInfo = typeof(MainWindowViewModel).GetMethod(MethodToExecute, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.CreateInstance);

            //I don't want to change methods signature and I also want to pass args to them somehow.

            //Throws System.ArgumentException: 'Cannot bind to the target method because its signature is not compatible with that of the delegate type.'
            //Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, element, methodInfo);

            //Throws System.ArgumentException: 'Object of type 'System.Action`2[System.Object,System.EventArgs]' cannot be converted to type 'System.Windows.Input.MouseButtonEventHandler'.'
            Delegate handler = new Action<object, EventArgs>((sender, args) => command.DynamicInvoke(Args));

            eventInfo.AddEventHandler(element, handler);
        }
    }
}

using System;
using System.Windows;
using static System.Windows.MessageBox;

namespace WpfApp1.Classes
{
    public static class Dialogs
    {
        /// <summary>
        /// Ask a question with No as the default button
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title">Title which defaults to 'Question'</param>
        /// <returns></returns>
        public static bool Question(string message, string title = "Question")
        {
            return (Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes);
        }
        /// <summary>
        /// Ask a question with the ability to define the default button to Yes or No
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title">Title for message box</param>
        /// <param name="defaultButton"></param>
        /// <returns></returns>
        public static bool Question(string message, string title, MessageBoxResult defaultButton)
        {
            MessageBoxResult button = 0;
            if (defaultButton == MessageBoxResult.No)
            {
                button = MessageBoxResult.No;
            }

            return (Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, button) == MessageBoxResult.Yes);
        }
        /// <summary>
        /// Present a message without an icon
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <remarks>
        /// No icon means no sound
        /// </remarks>
        public static void MessageBox(string message, string title = "Alert")
        {
            Show(message, title, MessageBoxButton.OK, MessageBoxImage.None);
        }
        /// <summary>
        /// Present a message without an icon
        /// </summary>
        /// <param name="text"></param>
        public static void InformationDialog(string text)
        {
            Show(text, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void InformationDialog(string message, string title)
        {
            Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Present a dialog with message, title and exception text.
        /// </summary>
        /// <param name="message">Text to prefix exception message</param>
        /// <param name="title">Title of dialog</param>
        /// <param name="exception">Thrown exception from a catch</param>
        public static void ExceptionDialog(string message, string title, Exception exception)
        {
            Show($"{message}\n{exception.Message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ExceptionDialog(string text, string title = "Fatal issue")
        {
            Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// This extension method is for quick and dirty usage when debugging, not for production.
        /// </summary>
        /// <param name="exception"></param>
        public static void ExceptionDeveloperDialog(this Exception exception)
        {
            Show($"\n{exception.Message}{exception.Message}", "Oh snap", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}

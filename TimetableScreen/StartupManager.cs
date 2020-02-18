using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Principal;

namespace TimetableScreen
{
    /// <summary>
    /// Управляет автозапуском приложения
    /// </summary>
    public class ApplicationStartUpManager
    {
        static string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        static string appFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string appName = Path.GetFileName(appFullPath);

        public static void AddToStartup()
        {
            if (IsUserAdministrator())
            {
                RemoveFromCurrentUserStartup();
                AddToAllUsersStartup();
            }
            else
            {
                using var key = Registry.LocalMachine.OpenSubKey(registryPath, true);
                if (key.GetValue(appName) == null)
                    AddToCurrentUsersStartup();
            }
        }

        public static void DeleteFromStartup()
        {
            RemoveFromCurrentUserStartup();
            RemoveFromAllUserStartup();
        }

        private static void AddToCurrentUsersStartup()
        {
            using var key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            if (key.GetValue(appName) == null)
                key.SetValue(appName, $"\"{appFullPath}\"");
        }

        private static void AddToAllUsersStartup()
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryPath, true);
            if (key.GetValue(appName) == null)
                key.SetValue(appName, $"\"{appFullPath}\"");
        }

        private static void RemoveFromCurrentUserStartup()
        {
            using var key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            if (key.GetValue(appName) == null)
                key.DeleteValue(appName, false);
        }

        private static void RemoveFromAllUserStartup()
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryPath, true);
            if (key.GetValue(appName) == null)
                key.DeleteValue(appName, false);
        }

        private static bool IsUserAdministrator()
        {
            try
            {
                var user = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

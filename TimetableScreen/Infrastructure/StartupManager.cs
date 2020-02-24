using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

namespace TimetableScreen.Infrastructure
{
    /// <summary>
    /// Управляет автозапуском приложения
    /// </summary>
    public class ApplicationStartUpManager
    {
        static string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        static string appFullPath = Process.GetCurrentProcess().MainModule.FileName;
        static string appName = Path.GetFileNameWithoutExtension(appFullPath);

        public static void Set(bool isEnabled)
        {
            if (isEnabled)
            {
                if (IsUserAdministrator())
                {
                    RemoveFromCurrentUserStartup();
                    AddToAllUsersStartup();
                }
                else
                {
                    using var key = Registry.LocalMachine.OpenSubKey(registryPath, false);
                    if (key.GetValue(appName) == null)
                        AddToCurrentUsersStartup();
                }
            }
            else
            {
                if (IsUserAdministrator())
                    RemoveFromAllUserStartup();

                RemoveFromCurrentUserStartup();
            }
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

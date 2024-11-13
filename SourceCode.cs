using System;
using System.IO;
using System.Runtime.InteropServices;
using Rainmeter;
using IWshRuntimeLibrary;

namespace PluginCreateShortcut
{
    internal class Measure
    {
        public string ShortcutName { get; set; }
        public string ShortcutIcon { get; set; }
        public string Target { get; set; }
        public string SaveLocation { get; set; }
        public string OnCompleteAction { get; set; }
        public string Location { get; set; } // For ShortcutFound
        private string Type { get; set; } // "Create" or "ShortcutFound"

        private API _api; // Store API instance here

        internal void Reload(API api, ref double maxValue)
        {
            _api = api; // Initialize the API instance

            // Read the Type parameter to determine function
            Type = api.ReadString("Type", "Create");

            // Parameters for creating a shortcut
            if (Type.Equals("Create", StringComparison.InvariantCultureIgnoreCase))
            {
                ShortcutName = api.ReadString("ShortcutName", "Shortcut");
                ShortcutIcon = api.ReadPath("ShortcutIcon", "");
                Target = api.ReadPath("Target", "");
                SaveLocation = api.ReadPath("SaveLocation", "");
                OnCompleteAction = api.ReadString("OnCompleteAction", "");

                if (string.IsNullOrEmpty(Target) || string.IsNullOrEmpty(SaveLocation))
                {
                    _api.Log(API.LogType.Error, "CreateShortcut.dll: Target or SaveLocation not specified.");
                }
            }
            // Parameters for checking if a shortcut exists
            else if (Type.Equals("ShortcutFound", StringComparison.InvariantCultureIgnoreCase))
            {
                Location = api.ReadPath("Location", "");
            }
        }

        internal double Update()
        {
            // Check if the shortcut exists if the Type is "ShortcutFound"
            if (Type.Equals("ShortcutFound", StringComparison.InvariantCultureIgnoreCase))
            {
                // Use System.IO.File to avoid ambiguity
                return System.IO.File.Exists(Location) ? 1.0 : 0.0;
            }
            return 0.0;
        }

        internal void Execute()
        {
            if (Type.Equals("Create", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    if (string.IsNullOrEmpty(Target) || string.IsNullOrEmpty(SaveLocation))
                    {
                        return;
                    }

                    // Ensure save location directory exists
                    Directory.CreateDirectory(SaveLocation);

                    string shortcutPath = Path.Combine(SaveLocation, $"{ShortcutName}.lnk");

                    // Create a new WshShell object
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                    // Set the shortcut properties
                    shortcut.TargetPath = Target;
                    if (!string.IsNullOrEmpty(ShortcutIcon))
                    {
                        shortcut.IconLocation = ShortcutIcon;
                    }
                    shortcut.Save();

                    _api.Log(API.LogType.Debug, $"CreateShortcut.dll: Shortcut created at {shortcutPath}");

                    // Execute OnCompleteAction if specified
                    if (!string.IsNullOrEmpty(OnCompleteAction))
                    {
                        _api.Execute(OnCompleteAction);
                    }
                }
                catch (Exception ex)
                {
                    _api.Log(API.LogType.Error, $"CreateShortcut.dll: Error creating shortcut - {ex.Message}");
                }
            }
        }
    }

    public static class Plugin
    {
        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            GCHandle.FromIntPtr(data).Free();
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Reload(new API(rm), ref maxValue);
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update();
        }

        [DllExport]
        public static void ExecuteBang(IntPtr data, [MarshalAs(UnmanagedType.LPWStr)] string args)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Execute();
        }
    }
}

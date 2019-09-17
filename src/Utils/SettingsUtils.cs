using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Tools.Utils
{
    public class SettingsUtils
    {
        public static ElementTheme Theme
        {
            get => App.GetEnum<ElementTheme>(GetSettings(nameof(Theme), ElementTheme.Default.ToString()));
            set
            {
                SetSettings(nameof(Theme), value.ToString());
                App.SetTheme();
            }
        }

        public bool SyncSettings
        {
            get => GetSettings(nameof(SyncSettings), true);
            set
            {
                SetSettings(nameof(SyncSettings), value);
            }
        }

        public static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public SettingsUtils()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        private static T GetSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }

            if (defaultValue != null)
            {
                return defaultValue;
            }

            return default(T);
        }

        private static void SetSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }
    }
}

/*
 * Copyright (C) 2014 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Original concept for Internal IoC came from: http://pclstorage.codeplex.com under Microsoft Public License (Ms-PL)
 * 
 */

using System;
using Plugin.Settings.Abstractions;

namespace Plugin.Settings
{
    public enum SettingsType
    {
        Local,
        Roaming
    }


    /// <summary>
    /// Cross Platform settings
    /// </summary>
    public static class CrossSettings
    {
        static Lazy<ISettings> settings = new Lazy<ISettings>(() => CreateSettings(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        static Lazy<ISettings> roamingSettings = new Lazy<ISettings>(() => CreateSettings(SettingsType.Roaming), System.Threading.LazyThreadSafetyMode.PublicationOnly);


        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ISettings Current
        {
            get
            {
                ISettings ret = settings.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        /// <summary>
        /// Current roaming settings to use
        /// </summary>
        public static ISettings CurrentRoaming
        {
            get
            {
                ISettings ret = roamingSettings.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingsType">This parameter has no effect on Android or iOS</param>
        /// <returns></returns>
        static ISettings CreateSettings(SettingsType settingsType = SettingsType.Local)
        {
#if PORTABLE
            return null;
#else
#if WINDOWS_UWP 
            if (settingsType == SettingsType.Local)
                return new SettingsImplementation();
            else
                return new RoamingSettingsImplementation();
#else
            return new SettingsImplementation();
#endif
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}

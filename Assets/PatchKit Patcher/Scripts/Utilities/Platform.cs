﻿using System.Linq;
using UnityEngine;

namespace PatchKit.Unity.Utilities
{
    public class Platform
    {
        public static PlatformResolver PlatformResolver { get; set; }

        static Platform()
        {
            PlatformResolver = new PlatformResolver();
        }

        public static RuntimePlatform GetRuntimePlatform()
        {
            return PlatformResolver.GetRuntimePlatform();
        }

        public static PlatformType GetPlatformType()
        {
            if (IsWindows())
            {
                return PlatformType.Windows;
            }
            if (IsOSX())
            {
                return PlatformType.OSX;
            }
            if (IsLinux())
            {
                return PlatformType.Linux;
            }
            return PlatformType.Unknown;
        }

        public static bool IsWindows()
        {
            return IsOneOf(RuntimePlatform.WindowsPlayer, RuntimePlatform.WindowsEditor);
        }

        public static bool IsOSX()
        {
            return IsOneOf(RuntimePlatform.OSXPlayer, RuntimePlatform.OSXEditor);
        }

        public static bool IsLinux()
        {
            return IsOneOf(RuntimePlatform.LinuxPlayer, RuntimePlatform.LinuxEditor);
        }

        public static bool IsPosix()
        {
            return IsLinux() || IsOSX();
        }

        public static bool IsOneOf(params RuntimePlatform[] platforms)
        {
            var runtimePlatform = GetRuntimePlatform();
            return platforms.Any(platform => platform == runtimePlatform);
        }
    }

    public class PlatformResolver
    {
        public virtual RuntimePlatform GetRuntimePlatform()
        {
            return Application.platform;
        }
    }

    public enum PlatformType
    {
        Unknown,
        Windows,
        OSX,
        Linux,
    }
}
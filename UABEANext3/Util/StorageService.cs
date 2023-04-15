﻿using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia;
using Avalonia.VisualTree;

namespace UABEANext3.Util
{
    internal static class StorageService
    {
        public static FilePickerFileType All { get; } = new("All")
        {
            Patterns = new[] { "*.*" },
            MimeTypes = new[] { "*/*" }
        };

        public static FilePickerFileType Json { get; } = new("Json")
        {
            Patterns = new[] { "*.json" },
            AppleUniformTypeIdentifiers = new[] { "public.json" },
            MimeTypes = new[] { "application/json" }
        };

        public static IStorageProvider? GetStorageProvider()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            {
                return window.StorageProvider;
            }

            if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
            {
                var visualRoot = mainView.GetVisualRoot();
                if (visualRoot is TopLevel topLevel)
                {
                    return topLevel.StorageProvider;
                }
            }

            return null;
        }
    }
}

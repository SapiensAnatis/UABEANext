﻿using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaEdit.Document;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UABEANext3.AssetWorkspace;

namespace UABEANext3.ViewModels.Tools
{
    internal class PreviewerToolViewModel : Tool
    {
        const string TOOL_TITLE = "Previewer";

        private Workspace _workspace;
        public Workspace Workspace
        {
            get => _workspace;
            set => this.RaiseAndSetIfChanged(ref _workspace, value);
        }

        private Bitmap? _activeImage;
        public Bitmap? ActiveImage
        {
            get => _activeImage;
            set => this.RaiseAndSetIfChanged(ref _activeImage, value);
        }

        private TextDocument? _activeDocument;
        public TextDocument? ActiveDocument
        {
            get => _activeDocument;
            set => this.RaiseAndSetIfChanged(ref _activeDocument, value);
        }

        const int TEXT_ASSET_MAX_LENGTH = 100000;

        [Reactive]
        public PreviewerToolPreviewType ActivePreviewType { get; set; } = PreviewerToolPreviewType.Mesh;

        [Obsolete("This is a previewer-only constructor")]
        public PreviewerToolViewModel()
        {
            Workspace = new();
            ActiveImage = null;
            ActiveDocument = new TextDocument();

            Id = TOOL_TITLE.Replace(" ", "");
            Title = TOOL_TITLE;
        }

        public PreviewerToolViewModel(Workspace workspace)
        {
            Workspace = workspace;
            ActiveImage = null;
            ActiveDocument = new TextDocument();

            Id = TOOL_TITLE.Replace(" ", "");
            Title = TOOL_TITLE;
        }

        public void SetImage(byte[] data, int width, int height)
        {
            if (ActiveImage != null)
            {
                ActiveImage.Dispose();
            }

            var bitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Rgba8888);
            using (var frameBuffer = bitmap.Lock())
            {
                Marshal.Copy(data, 0, frameBuffer.Address, data.Length);
            }
            ActivePreviewType = PreviewerToolPreviewType.Image;
            ActiveImage = bitmap;
        }

        public void SetText(byte[] text)
        {
            ActivePreviewType = PreviewerToolPreviewType.Text;
            string trimmedText;
            if (text.Length <= TEXT_ASSET_MAX_LENGTH)
            {
                trimmedText = Encoding.UTF8.GetString(text);
            }
            else
            {
                trimmedText = Encoding.UTF8.GetString(text[..TEXT_ASSET_MAX_LENGTH]) + $"... (and {text.Length - TEXT_ASSET_MAX_LENGTH} bytes more)";
            }
            ActiveDocument = new TextDocument(trimmedText.ToCharArray());
        }

        public void SetMesh()
        {
            ActivePreviewType = PreviewerToolPreviewType.Mesh;
        }
    }

    public enum PreviewerToolPreviewType
    {
        Image,
        Text,
        Mesh,
    }
}

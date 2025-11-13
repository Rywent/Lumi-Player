using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace LumiPlayer.BlurEffect
{
    internal class BlurBehavior : AttachableForStyleBehavior<Window, BlurBehavior>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += (s, e) => { EnableBlur(); };
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private uint _blurBackgroundColor = 0x0D000000;

        public uint BlurOpacity
        {
            get => (uint)GetValue(BlurOpacityProperty);
            set => SetValue(BlurOpacityProperty, value);
        }

        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(AssociatedObject);

            if (windowHelper.Handle == IntPtr.Zero)
            {
                AssociatedObject.SourceInitialized += (s, e) => EnableBlur();
                return;
            }

            var accent = new AccentPolicy();


            accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            accent.AccentFlags = 0x20 | 0x40; 
            accent.GradientColor = ToAbgr(_blurBackgroundColor); 
            accent.AnimationId = 0;

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }
        private uint ToAbgr(uint argb)
        {
            var a = (argb >> 24) & 0xFF;
            var r = (argb >> 16) & 0xFF;
            var g = (argb >> 8) & 0xFF;
            var b = (argb >> 0) & 0xFF;
            return (a << 24) | (b << 16) | (g << 8) | r;
        }

        public static readonly DependencyProperty BlurOpacityProperty =
            DependencyProperty.Register(
                nameof(BlurOpacity),
                typeof(uint),
                typeof(BlurBehavior),
                new PropertyMetadata((uint)5, OnBlurOpacityChanged));

        private static void OnBlurOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (BlurBehavior)d;
            uint alphaValue = (uint)e.NewValue;

            alphaValue = 20;

            behavior._blurBackgroundColor = (alphaValue << 24);
            behavior.EnableBlur();
        }


    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }
}
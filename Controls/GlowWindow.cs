using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using Livet.Commands;

namespace DraftWayfinder.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class GlowWindow : Window
    {
        static GlowWindow()
        {
            ActiveTitleCaptionBrush = Brushes.White;
            InactiveTitleCaptionBrush = Brushes.LightGray;
            TitleCaptionBrushProperty =
                DependencyProperty.Register("TitleCaptionBrush", typeof(Brush), typeof(GlowWindow), new PropertyMetadata(InactiveTitleCaptionBrush));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(typeof(GlowWindow)));
        }

        public GlowWindow()
        {
            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                CaptionHeight = SystemParameters.CaptionHeight,
                ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness,
                GlassFrameThickness = new Thickness(0),
                CornerRadius = new CornerRadius(0)
            });

            Activated += OnActivated;
            Deactivated += OnDeactivated;
        }

        #region Brushes
        /// <summary>
        /// アクティブ時のウィンドウボーダーのブラシ（デフォルト色）。
        /// </summary>
        protected static readonly Brush ActiveBorderBrush = new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF));
        /// <summary>
        /// 非アクティブ時のウィンドウボーダーのブラシ。
        /// </summary>
        protected static readonly Brush InactiveBorderBrush = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20));
        /// <summary>
        /// アクティブ時のタイトル文字色のブラシ。
        /// </summary>
        protected static readonly Brush ActiveTitleCaptionBrush;
        /// <summary>
        /// 非アクティブ時のタイトル文字色のブラシ。
        /// </summary>
        protected static readonly Brush InactiveTitleCaptionBrush;
        #endregion

        #region Dependecy Properties

        #region ThemeColor
        /// <summary>
        /// この GlowWindow のテーマカラーを設定または取得します。
        /// </summary>
        public Color ThemeColor
        {
            get => (Color)GetValue(ThemeColorProperty);
            set => SetValue(ThemeColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for ThemeColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemeColorProperty =
            DependencyProperty.Register("ThemeColor", typeof(Color), typeof(GlowWindow), new PropertyMetadata(Colors.Transparent, ApplyNewThemeColor));

        private static void ApplyNewThemeColor(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var gw = (GlowWindow)dependencyObject;
            gw.TitleBarBrush = gw.CreateTitleBarBrush((Color)dependencyPropertyChangedEventArgs.NewValue);

            var borderBrush = gw.CreateBorderBrush((Color)dependencyPropertyChangedEventArgs.NewValue);
            if (gw.IsActive)
            {
                gw.WindowBorderBrush = borderBrush;
            }

            gw._activeBrushCache = gw.CreateBorderBrush((Color)dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region TitleBarBrush
        public Brush TitleBarBrush
        {
            get => (Brush)GetValue(TitleBarBrushProperty);
            private set => SetValue(_titleBarBrushPropertyKey, value);
        }

        // Using a DependencyProperty as the backing store for TitleBarBrush.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey _titleBarBrushPropertyKey =
            DependencyProperty.RegisterReadOnly("TitleBarBrush", typeof(Brush), typeof(GlowWindow), null);

        public static readonly DependencyProperty TitleBarBrushProperty = _titleBarBrushPropertyKey.DependencyProperty;

        protected virtual Brush CreateTitleBarBrush(Color color)
        {
            // 透明色の場合は全体透明で返す
            if (color.A == 0)
            {
                return Brushes.Transparent;
            }

            // そうでないならグラデーションブラシに変換して返す
            var brush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5),
            };

            brush.GradientStops.Add(new GradientStop(color, 0));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0x66, color.R, color.G, color.B), 0.7));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0x66, color.R, color.G, color.B), 1));

            return brush;
        }
        #endregion

        #region BorderBrush

        private Brush _activeBrushCache;

        public Brush WindowBorderBrush
        {
            get => (Brush)GetValue(WindowBorderBrushProperty);
            private set => SetValue(_windowBorderBrushPropertyKey, value);
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey _windowBorderBrushPropertyKey =
            DependencyProperty.RegisterReadOnly("WindowBorderBrush", typeof(Brush), typeof(GlowWindow), null);

        public static DependencyProperty WindowBorderBrushProperty = _windowBorderBrushPropertyKey.DependencyProperty;

        protected virtual Brush CreateBorderBrush(Color color)
        {
            return color.A == 0 ? ActiveBorderBrush : new SolidColorBrush(color);
        }
        #endregion

        #region TitleCaptionBrush


        public Brush TitleCaptionBrush
        {
            get => (Brush)GetValue(TitleCaptionBrushProperty);
            set => SetValue(TitleCaptionBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for TitleCaptionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleCaptionBrushProperty;
        #endregion
        #endregion

        #region Commands

        #region Minimize Button
        private ICommand _minimizeCommand;
        public ICommand MinimizeCommand => _minimizeCommand ?? (_minimizeCommand = new ViewModelCommand(MinimizeCommandImpl));

        private void MinimizeCommandImpl()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Maximize Button
        private ICommand _maximizeCommand;
        public ICommand MaximizeCommand => _maximizeCommand ?? (_maximizeCommand = new ViewModelCommand(MaximizeCommandImpl));

        private void MaximizeCommandImpl()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        #endregion

        #region Maximize Button
        private ICommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new ViewModelCommand(Close));

        protected new virtual void Close()
        {
            base.Close();
        }
        #endregion

        #region アクティブ状態
        /// <summary>
        /// 非アクティブ状態になったときの色変え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnDeactivated(object sender, EventArgs eventArgs)
        {
            _activeBrushCache = WindowBorderBrush;
            WindowBorderBrush = InactiveBorderBrush;
            TitleCaptionBrush = InactiveTitleCaptionBrush;
        }

        /// <summary>
        /// アクティブ状態になったときの色変え   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnActivated(object sender, EventArgs eventArgs)
        {
            WindowBorderBrush = _activeBrushCache;
            TitleCaptionBrush = ActiveTitleCaptionBrush;
        }
        #endregion

        #endregion
    }
}

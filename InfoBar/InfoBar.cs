using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InfoBar;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Windows.Input;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace InfoBar
{
    public enum InfoBarSeverity
    {
        Critical, 
        Warning,
        Informational,
        Success,
        Default
    }
    public sealed class InfoBar : ContentControl
    {
        public InfoBar()
        {
            this.DefaultStyleKey = typeof(InfoBar);
        }

        Button _alternateCloseButton;
        Button _closeButton;
        Border _myBorder;
        Button _actionButton;

        public event EventHandler<RoutedEventArgs> ActionButtonClick;

        protected override void OnApplyTemplate()
        {
            _alternateCloseButton = GetTemplateChild<Button>("AlternateCloseButton");
            _closeButton = GetTemplateChild<Button>("CloseButton");
            _actionButton = GetTemplateChild<Button>("ActionButton");
            _myBorder = GetTemplateChild<Border>("Container");


            UpdateButtonsState();
            _closeButton.Click += new RoutedEventHandler(CloseButtonClick);

            _alternateCloseButton.Click += new RoutedEventHandler(CloseButtonClick);

            _actionButton.Click += (s, e) => ActionButtonClick?.Invoke(s, e);

            UpdateIsOpen();
            
        }

        T GetTemplateChild<T>(string name) where T: DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            if (child == null)
            {
                throw new NullReferenceException(name);
            }
            return child;
        }


        public ICommand ActionButtonCommand
        {
            get { return (ICommand)GetValue(ActionButtonCommandProperty); }
            set { SetValue(ActionButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonCommandProperty =
            DependencyProperty.Register(nameof(ActionButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));

        public ICommand CloseButtonCommand
        {
            get { return (ICommand)GetValue(CloseButtonCommandProperty); }
            set { SetValue(CloseButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoBar), new PropertyMetadata(null));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar), new PropertyMetadata("Message Here"));


        public IconSource IconSource
        {
            get { return (IconSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(IconSource), typeof(InfoBar), new PropertyMetadata(null));


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar), new PropertyMetadata(false));

        //Which Color am I supposed to use? 
        public Color StatusColor
        {
            get { return (Color)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register(nameof(StatusColor), typeof(Color), typeof(InfoBar), new PropertyMetadata(null));


        public object CloseButtonContent
        {
            get { return (object)GetValue(CloseButtonContentProperty); }
            set { SetValue(CloseButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonContentProperty =
            DependencyProperty.Register(nameof(CloseButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null));


        public object ActionButtonContent   
        {
            get { return (object)GetValue(ActionButtonContentProperty); }
            set { SetValue(ActionButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null));



        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCloseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(InfoBar), new PropertyMetadata(false));



        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Severity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Default));












        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
            _myBorder.Visibility = Visibility.Collapsed;

        }






        void UpdateIsOpen()
        {
            if (IsOpen)
            {
                _myBorder.Visibility = Visibility.Visible;
            } else
            {
                _myBorder.Visibility = Visibility.Collapsed;
            }
        }

        void UpdateButtonsState()
        {
            if (CloseButtonContent != null && ActionButtonContent != null)
            {
                VisualStateManager.GoToState(this, "BothButtonsVisible", false); 
                VisualStateManager.GoToState(this, "CustomCloseButton", false);
            }
            else if (CloseButtonContent != null)
            {
                VisualStateManager.GoToState(this, "CloseButtonVisible", false);
                VisualStateManager.GoToState(this, "CustomCloseButton", false);
            }
            else if (ActionButtonContent != null)
            {
                VisualStateManager.GoToState(this, "ActionButtonVisible", false);
                VisualStateManager.GoToState(this, "DefaultCloseButton", false);
            }
            else if (ActionButtonContent == null && CloseButtonContent == null)
            {
                VisualStateManager.GoToState(this, "NoButtonsVisible", false);
                VisualStateManager.GoToState(this, "UserCloseButton", false);
            }
        }




    }
}

/* 
 * InfoBar Contorl Prototype
 * @author Sophia Chen
 * t-soc@microsoft.com
 * 
 * C# Prototype for UWP InfoBar (Microsoft XAML Controls Team)
 * 
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Automation.Peers;
using Windows.ApplicationModel;
using System.ServiceModel.Channels;

namespace InfoBar
{
    public enum InfoBarCloseReason
    {
        CloseButton,
        Programattic
    }
    public enum InfoBarSeverity
    {
        Critical, 
        Warning,
        Informational,
        Success,
        Default,
        None
    }

    public class InfoBarClosedEventArgs : EventArgs
    {
        public InfoBarCloseReason Reason
        {
            get; set;
        }
    }
    
    public class InfoBarClosingEventArgs : EventArgs
    {
        public InfoBarCloseReason Reason 
        {
            get; set;
        }
        public bool Cancel
        {
            get; set; 
        }
        
    }
    public class CloseButtonClickEventArgs : EventArgs
    {
        public bool IsHandled
        {
            get; set;
        }
    }
    public sealed class InfoBar : ContentControl
    {
        Button _actionButton;
        Button _alternateCloseButton;
        Button _closeButton;

        public event EventHandler<RoutedEventArgs> ActionButtonClick;
        public event TypedEventHandler<InfoBar, CloseButtonClickEventArgs> CloseButtonClick;
        public event TypedEventHandler<InfoBar, InfoBarClosedEventArgs> Closed;
        public event TypedEventHandler<InfoBar, InfoBarClosingEventArgs> Closing;

        private InfoBarCloseReason lastCloseReason = InfoBarCloseReason.Programattic;
        private bool alreadyRaised = false;
        private Popup pop;


        public InfoBar()
        {
            this.DefaultStyleKey = typeof(InfoBar);
        }


        T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            return child;
        }


        protected override void OnApplyTemplate()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            _alternateCloseButton = GetTemplateChild<Button>("AlternateCloseButton");
            _closeButton = GetTemplateChild<Button>("CloseButton");
            _actionButton = GetTemplateChild<Button>("ActionButton");

            UpdateButtonsState();
            UpdateSeverityState();
            OnIsOpenChanged();
            OnTitleChanged();

            _alternateCloseButton.Click += new RoutedEventHandler(OnCloseButtonClick);
            _closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);
            _actionButton.Click += (s, e) => ActionButtonClick?.Invoke(s, e);
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            OnToastChanged();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyProperty property = e.Property;
            InfoBar infoBar = d as InfoBar;
            if (property == SeverityProperty)
            {
                infoBar.UpdateSeverityState();
            }
            else if (property == ActionButtonContentProperty || property == CloseButtonContentProperty || property == ShowCloseButtonProperty)
            {
                infoBar.UpdateButtonsState();
            }
            else if (property == IsOpenProperty)
            {
                infoBar.OnIsOpenChanged();
            }
            else if (property == IconSourceProperty)
            {
                infoBar.OnIconChanged();
            }
            else if (property == ContentProperty)
            {
                infoBar.OnContentChanged();
            } 
            else if (property == TitleProperty)
            {
                infoBar.OnTitleChanged();
            }
        }

        private void OnTitleChanged()
        {
            if (Title != "" && Title != null)
            {
                VisualStateManager.GoToState(this, "TitlePresent", false);
            } 
            else
            {
                VisualStateManager.GoToState(this, "TitleNotPresent", false);
            }
        }

        private void OnContentChanged()
        {
            if(Content != null)
            {
                VisualStateManager.GoToState(this, "Content", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "NoContent", false);
            }
        }

        /* Open Properties
         * 
         */
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar), new PropertyMetadata(false, OnPropertyChanged));


        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(InfoBar), new PropertyMetadata(true, OnPropertyChanged));




        /* Message Title Properties
         * 
         */
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoBar), new PropertyMetadata("", OnPropertyChanged));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar), new PropertyMetadata(""));




        /* Action Button Properties
         * 
         */
        public object ActionButtonContent
        {
            get { return (object)GetValue(ActionButtonContentProperty); }
            set { SetValue(ActionButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null, OnPropertyChanged));


        public Style ActionButtonStyle
        {
            get { return (Style)GetValue(ActionButtonStyleProperty); }
            set { SetValue(ActionButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonStyleProperty =
            DependencyProperty.Register(nameof(ActionButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));


        public ICommand ActionButtonCommand
        {
            get { return (ICommand)GetValue(ActionButtonCommandProperty); }
            set { SetValue(ActionButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonCommandProperty =
            DependencyProperty.Register(nameof(ActionButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));


        public object ActionButtonCommandParameter
        {
            get { return (object)GetValue(ActionButtonCommandParameterProperty); }
            set { SetValue(ActionButtonCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));





        /* Close Button Properties
         * 
         */
        public object CloseButtonContent
        {
            get { return (object)GetValue(CloseButtonContentProperty); }
            set { SetValue(CloseButtonContentProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonContentProperty =
            DependencyProperty.Register(nameof(CloseButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null, OnPropertyChanged));

        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register(nameof(CloseButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));


        public ICommand CloseButtonCommand
        {
            get { return (ICommand)GetValue(CloseButtonCommandProperty); }
            set { SetValue(CloseButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));


        public object CloseButtonCommandParameter
        {
            get { return (object)GetValue(CloseButtonCommandParameterProperty); }
            set { SetValue(CloseButtonCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));




         /* Severity-Related Properties
         * 
         */
        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Default, OnPropertyChanged));

        public Color StatusColor
        {
            get { return (Color)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register(nameof(StatusColor), typeof(Color), typeof(InfoBar), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));


        public IconSource IconSource
        {
            get { return (IconSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(IconSource), typeof(InfoBar), new PropertyMetadata(default, OnPropertyChanged));




        public bool Toast
        {
            get { return (bool)GetValue(ToastProperty); }
            set { SetValue(ToastProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToastProperty =
            DependencyProperty.Register(nameof(Toast), typeof(bool), typeof(InfoBar), new PropertyMetadata(false));







        // Methods that invoke the event handlers for Close Button and Action Button
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {

            lastCloseReason = InfoBarCloseReason.CloseButton;
            CloseButtonClickEventArgs args = new CloseButtonClickEventArgs();
            CloseButtonClick?.Invoke(this, args);
            if (args.IsHandled == false)
            {
                RaiseClosingEvent();
                alreadyRaised = false;
            }

        }


        void RaiseClosingEvent()
        {
            InfoBarClosingEventArgs args = new InfoBarClosingEventArgs();
            args.Reason = lastCloseReason;

            Closing?.Invoke(this, args);

            if (!args.Cancel)
            {
                alreadyRaised = true;
                IsOpen = false; 
                Open(IsOpen);
            }
            else
            {
                // The developer has changed the Cancel property to true, indicating that they wish to Cancel the
                // closing of this tip, so we need to revert the IsOpen property to true.
                IsOpen = true;
            }
        }


        void RaiseClosedEvent()
        {
            InfoBarClosedEventArgs args = new InfoBarClosedEventArgs();
            args.Reason = lastCloseReason;
            Closed?.Invoke(this, args);

        }

        void OnIconChanged()
        {
            if (IconSource != null)
            {
                VisualStateManager.GoToState(this, "UserIconVisible", false);
            } else
            {
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
            }
        }

        // Updates Severity state of InfoBar
        void UpdateSeverityState()
        {
            OnIconChanged();
            if (Severity == InfoBarSeverity.Critical)
            {
                VisualStateManager.GoToState(this, "Critical", false);
            }
            else if (Severity == InfoBarSeverity.Warning)
            {
                VisualStateManager.GoToState(this, "Warning", false);
            }
            else if (Severity == InfoBarSeverity.Informational)
            {
                VisualStateManager.GoToState(this, "Informational", false);
            }
            else if (Severity == InfoBarSeverity.Success)
            {
                VisualStateManager.GoToState(this, "Success", false);
            }
            else if (Severity == InfoBarSeverity.None)
            {
                VisualStateManager.GoToState(this, "None", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Default", false);
            }
        }


        // Updates visibility of buttons
        void UpdateButtonsState()
        {
            if (ShowCloseButton)
            {
                if (CloseButtonContent != null && ActionButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "BothButtonsVisible", false);
                    VisualStateManager.GoToState(this, "NoDefaultCloseButton", false);
                }
                else if (CloseButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "CloseButtonVisible", false);
                    VisualStateManager.GoToState(this, "NoDefaultCloseButton", false);
                }
                else if (ActionButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "ActionButtonVisible", false);
                    VisualStateManager.GoToState(this, "DefaultCloseButton", false);
                    if(_alternateCloseButton != null)
                    {
                        _alternateCloseButton.Visibility = Visibility.Visible;
                    }
                    
                }
                else if (ActionButtonContent == null && CloseButtonContent == null)
                {
                    VisualStateManager.GoToState(this, "NoButtonsVisible", false);
                    VisualStateManager.GoToState(this, "DefaultCloseButton", false);
                    _closeButton.Visibility = Visibility.Collapsed;
                    _actionButton.Visibility = Visibility.Collapsed;
                    _alternateCloseButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "NoDefaultCloseButton", false);
                if (ActionButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "ActionButtonVisible", false);

                }
                else
                {
                    VisualStateManager.GoToState(this, "NoButtonsVisible", false);
                }
            }
        }


        // Updates if InfoBar is opened
        void OnIsOpenChanged()
        {

            if (IsOpen)
            {
                lastCloseReason = InfoBarCloseReason.Programattic;
                Open(IsOpen);

            }
            else if (!alreadyRaised)
            {

                RaiseClosingEvent();
                alreadyRaised = false;
            }
            
            
        }


        // Opens or closes the InfoBar
        private void Open(bool value)
        {

            if (value)
            {
                VisualStateManager.GoToState(this, "Visible", false);
                IsOpen = true;
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", false);
                IsOpen = false;
                RaiseClosedEvent();
            }
            OnToastChanged();
            /*
            InfoBarAutomationPeer infoBarPeer = FrameworkElementAutomationPeer.FromElement(this) as InfoBarAutomationPeer ?? null;
            if (infoBarPeer != null)
            {
                string notificationString = getNotificationString();
            }
            */
        }

        private string getAppName()
        {
            try
            {
                var package = Package.Current ?? null;
                if (package != null)
                {
                    return package.DisplayName;
                }
            }
            catch { }

            return null;
        }

       /* private string getNotificationString()
        {
            string appName = getAppName();
            if (!String.IsNullOrEmpty(appName))
            {
                return String.Format(GetLocalizedStringResources)
            }
        } */

        void OnToastChanged()
        {
            if (Toast)
            {
                InfoBar bar = this;
                ShowAsToast(bar);
            }
        }

        static void ShowAsToast(InfoBar bar)
        { 
            VisualStateManager.GoToState(bar, "InfoBarFloatingSize", false);
            var parent = bar.Parent as Grid;
            if (bar.pop == null)
            {
                bar.pop = new Popup();
            }

            double wid = ((Frame)Window.Current.Content).ActualWidth;
            double hei = ((Frame)Window.Current.Content).ActualHeight;

            double barHeight = bar.ActualHeight;
            double barWidth = bar.ActualWidth;
            bar.pop.VerticalOffset = (hei - barHeight);
            bar.pop.HorizontalOffset = (wid - barWidth);
            if (parent != null)
            {
                parent.Children.Remove(bar);
                bar.pop.Child = bar; 
                if (bar.IsOpen)
                {
                    bar.pop.IsOpen = true;
                } else
                {
                    bar.pop.IsOpen = false;
                }
            } 
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new InfoBarAutomationPeer(this);
        }

    }
}

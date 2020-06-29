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
using Windows.Foundation;
using Windows.UI.Composition;

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
        public InfoBarCloseReason Reason //is this allowed to be public? 
        {
            get; set;
        }
        public bool Cancel
        {
            get; set; 
        }
        
    }
    public class InfoBarEventArgs : EventArgs
    {
        public bool IsHandled
        {
            get; set;
        }
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
        IconSourceElement _myIcon;
        //String _myTitle;
        //String _myMessage;
        //Color _myStatusColor;

        public event EventHandler<RoutedEventArgs> ActionButtonClick;
        public event EventHandler<InfoBarEventArgs> CloseButtonClick;
        public event TypedEventHandler<InfoBar, InfoBarClosedEventArgs> Closed;
        public event TypedEventHandler<InfoBar, InfoBarClosingEventArgs> Closing; 

        private InfoBarCloseReason lastCloseReason = InfoBarCloseReason.Programattic; 

        
        protected override void OnApplyTemplate()
        {
            _alternateCloseButton = GetTemplateChild<Button>("AlternateCloseButton");
            _closeButton = GetTemplateChild<Button>("CloseButton");
            _actionButton = GetTemplateChild<Button>("ActionButton");
            _myBorder = GetTemplateChild<Border>("Container");
            _myIcon = GetTemplateChild<IconSourceElement>("MyIcon");
            //_myTitle = GetTemplateChild<string>("MyTitle");
           // _myMessage = GetTemplateChild<String>("MyMessage");
           //_myStatusColor = GetTemplateChild<Color>("MyBrush");


            UpdateButtonsState();
            _closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);

            _alternateCloseButton.Click += new RoutedEventHandler(OnCloseButtonClick);

          

            _actionButton.Click += (s, e) => ActionButtonClick?.Invoke(s, e);

            OnIsOpenChanged();
            UpdateSeverityState();
            
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



        public object ActionButtonCommandParameter
        {
            get { return (object)GetValue(ActionButtonCommandParameterProperty); }
            set { SetValue(ActionButtonCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionButtonCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));


        public Style ActionButtonStyle
        {
            get { return (Style)GetValue(ActionButtonStyleProperty); }
            set { SetValue(ActionButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonStyleProperty =
            DependencyProperty.Register(nameof(ActionButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));





        public ICommand CloseButtonCommand
        {
            get { return (ICommand)GetValue(CloseButtonCommandProperty); }
            set { SetValue(CloseButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));


        public object CloseButtonCommandParameter
        {
            get { return (object)GetValue(CloseButtonCommandParameterProperty); }
            set { SetValue(CloseButtonCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));


        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register(nameof(CloseButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));





        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoBar), new PropertyMetadata(""));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar), new PropertyMetadata(""));


        public IconSource IconSource
        {
            get { return (IconSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(IconSource), typeof(InfoBar), new PropertyMetadata(default));


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar), new PropertyMetadata(false, OnPropertyChanged));


        public Color StatusColor
        {
            get { return (Color)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register(nameof(StatusColor), typeof(Color), typeof(InfoBar), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));


        public object CloseButtonContent
        {
            get { return (object)GetValue(CloseButtonContentProperty); }
            set { SetValue(CloseButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonContentProperty =
            DependencyProperty.Register(nameof(CloseButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null, OnPropertyChanged));


        public object ActionButtonContent   
        {
            get { return (object)GetValue(ActionButtonContentProperty); }
            set { SetValue(ActionButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null, OnPropertyChanged));



        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCloseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(InfoBar), new PropertyMetadata(true));



        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Severity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Default, OnPropertyChanged));



        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {

            lastCloseReason = InfoBarCloseReason.CloseButton;
            InfoBarEventArgs newE = new InfoBarEventArgs();
            if (CloseButtonClick != null)
            {
                CloseButtonClick.Invoke(this, newE);
            }
            if (newE.IsHandled == false)
            {
                RaiseClosingEvent();
                Open(IsOpen);
                RaiseClosedEvent();
            }

        }

        private void Open(bool value)
        {
            if (_myBorder != null)
            {

                if (value)
                {
                    _myBorder.Visibility = Visibility.Visible;
                    IsOpen = true;
                }
                else
                {
                    _myBorder.Visibility = Visibility.Collapsed;
                    IsOpen = false;
                }
            }
        }

        void OnIsOpenChanged()
        {
            if (IsOpen)
            {
                lastCloseReason = InfoBarCloseReason.Programattic;

            } else
            {
                
                RaiseClosingEvent();
                RaiseClosedEvent();
            }
            Open(IsOpen);
        }

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
                }
                else if (ActionButtonContent == null && CloseButtonContent == null)
                {
                    VisualStateManager.GoToState(this, "NoButtonsVisible", false);
                    VisualStateManager.GoToState(this, "DefaultCloseButton", false);
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


        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyProperty property = e.Property;
            InfoBar infoBar = d as InfoBar;
            if (property == SeverityProperty)
            {
                
                infoBar.UpdateSeverityState();
            }
            else if (property == ActionButtonContentProperty || property == CloseButtonContentProperty)
            {
                infoBar.UpdateButtonsState();
            }
            else if (property == IsOpenProperty)
            {
                infoBar.OnIsOpenChanged(); 
            }

        }

        void UpdateSeverityState()
        {
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
            } else
            {
                VisualStateManager.GoToState(this, "Default", false);
            }
        }

        void RaiseClosingEvent()
        {
            InfoBarClosingEventArgs args =  new InfoBarClosingEventArgs();
            args.Reason = lastCloseReason;

            if (Closing != null)
            {
                Closing.Invoke(this, args);
            }

            if (!args.Cancel)
            {
                _myBorder.Visibility = Visibility.Collapsed;
                IsOpen = false;
            } else
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
            if (Closed!= null)
            {
                Closed.Invoke(this, args);
            }
            
        }




    }


}

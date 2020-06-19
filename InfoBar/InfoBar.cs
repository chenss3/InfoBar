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
using System.Drawing;
using Windows.UI;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace InfoBar
{
    public sealed class InfoBar : ContentControl
    {
        public InfoBar()
        {
            this.DefaultStyleKey = typeof(InfoBar);
        }

        Button _alternateCloseButton;
        Button _closeButton;
        Border _myBorder;


        protected override void OnApplyTemplate()
        {
            _alternateCloseButton = GetTemplateChild<Button>("AlternateCloseButton");
            _closeButton = GetTemplateChild<Button>("CloseButton");
            _myBorder = GetTemplateChild<Border>("Container");


            UpdateButtonsState();
            _closeButton.Click += new RoutedEventHandler(CloseButtonClick);

            _alternateCloseButton.Click += new RoutedEventHandler(CloseButtonClick);


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


        public Symbol Icon
        {
            get { return (Symbol)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Symbol), typeof(InfoBar), new PropertyMetadata(null));


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar), new PropertyMetadata(false));


        public Brush StatusColor
        {
            get { return (Brush)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register(nameof(StatusColor), typeof(Brush), typeof(InfoBar), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));


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
            get { return (object)GetValue(ActionButtonContentyProperty); }
            set { SetValue(ActionButtonContentyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionButtonContentyProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null));





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
            }
            else if (CloseButtonContent != null)
            {
                VisualStateManager.GoToState(this, "CloseButtonVisible", false);
            }
            else if (ActionButtonContent != null)
            {
                VisualStateManager.GoToState(this, "ActionButtonVisible", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "NoButtonsVisible", false);
            }
        }




    }
}

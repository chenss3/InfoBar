﻿using System;
using System.Runtime.ConstrainedExecution;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InfoBar
{

    public sealed partial class MainPage : Page
    {
        InfoBarSeverity severity;
        IconSource icon;
        String title;
        String message;
        String actionButtonContent;
        Color color;
        bool open;
        bool cancel;
        bool showClose;
        bool hyperlink;

        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void Test_ActionButtonClick(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("Thank you, mate").ShowAsync();
        }

        private async void Test_CloseButtonClick(object sender, CloseButtonClickEventArgs e)
        {
            await new MessageDialog("Thank you, mate").ShowAsync();
        }

        private void Test_Closing(InfoBar sender, InfoBarClosingEventArgs args)
        {
            args.Cancel = cancel;
        }

        private async void Test_Closed(InfoBar sender, InfoBarClosedEventArgs args)
        {
            await new MessageDialog("Thank you, mate im closed").ShowAsync();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string severityName = e.AddedItems[0].ToString();
         
            switch (severityName)
            {
                case "Critical":
                    severity = InfoBarSeverity.Critical;
                    break;
                case "Warning":
                    severity = InfoBarSeverity.Warning;
                    break;
                case "Informational":
                    severity = InfoBarSeverity.Informational;
                    break;
                case "Success":
                    severity = InfoBarSeverity.Success;
                    break;
                case "Default":
                    severity = InfoBarSeverity.Default;
                    break;
            }
        }

        private void SeverityButton_Click(object sender, RoutedEventArgs e)
        {
            Test.Severity = severity;
        }

        private void IconComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Pin Icon":

                    SymbolIconSource sym2 = new SymbolIconSource();
                    sym2.Symbol = Symbol.Pin;

                    icon = (IconSource) sym2;
                    break;
                case "No Icon":
                    SymbolIconSource sym3 = new SymbolIconSource();
                    sym3.Symbol = new Symbol();
                    icon = sym3;
                    break;

            }
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            Test.IconSource = icon;
        }

        private void TitleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Short Title":
                    title = "Short Title.";
                    break;
                case "Long Title":
                    title = "Long Title. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
                    break;
                case "No Title":
                    title = null;
                    break;
            }
        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            Test.Title = title;
        }

        private void MessageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Short Message":
                    message = "Short Message.";
                    break;
                case "Long Message":
                    message = "Long Message. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
                    break;
                case "No Message":
                    message = null;
                    break;
            }
        }

        private void MessageButton_Click(object sender, RoutedEventArgs e)
        {
            Test.Message = message;
        }

       

        private void ActionButtonContentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Short Text":
                    actionButtonContent = "A:Short";
                    break;
                case "Long Text":
                    actionButtonContent = "A:LongTextLorem ipsum dolor sit amet.";
                    break;
                case "No Text":
                    actionButtonContent = null;
                    break;
            }
        }

        private void ActionButtonContent_Click(object sender, RoutedEventArgs e)
        {
            Test.ActionButtonContent = actionButtonContent;
        }

        private void IsOpen_Checked(object sender, RoutedEventArgs e)
        {
            open = true;
        }

        private void IsOpen_Unchecked(object sender, RoutedEventArgs e)
        {
            open = false;
        }

        private void IsOpenButton_Click(object sender, RoutedEventArgs e)
        {
            Test.IsOpen = open;
        }

        private void ColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Purple":
                    color = Color.FromArgb(255, 128, 0, 128);
                    break;
                case "No Color":
                    color = Color.FromArgb(0, 0, 0, 0);
                    break;
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Test.StatusColor = color;
        }

        private void Cancel_Checked(object sender, RoutedEventArgs e)
        {
            cancel = true;
        }

        private void Cancel_Unchecked(object sender, RoutedEventArgs e)
        {
            cancel = false;
        }

        private void ShowClose_Checked(object sender, RoutedEventArgs e)
        {
            showClose = true;
        }

        private void ShowClose_Unchecked(object sender, RoutedEventArgs e)
        {
            showClose = false;
        }

        private void ShowCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Test.ShowCloseButton = showClose;
        }

        private void Hyperlink_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string iconName = e.AddedItems[0].ToString();

            switch (iconName)
            {
                case "Hyperlink":
                    hyperlink = true;
                    break;
                case "No Hyperlink":
                    hyperlink = false;
                    break;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (hyperlink)
            {
                HyperlinkButton hyp = new HyperlinkButton();
                hyp.Content = "www.microsoft.com";
                hyp.NavigateUri = new Uri("http://www.microsoft.com");
                Test.HyperlinkButtonContent = hyp;
            } else
            {
                HyperlinkButton hyp = null;
                Test.HyperlinkButtonContent = hyp;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace InfoBar
{
    public class InfoBarAutomationPeer : FrameworkElementAutomationPeer
    {
        public InfoBarAutomationPeer(FrameworkElement owner) : base(owner)
        {

        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.StatusBar;
        }

        protected override string GetClassNameCore()
        {
            return nameof(InfoBar);
        }

        void RaiseWindowOpenedEvent(string displayString)
        {
            AutomationPeer autPeer = this;
            autPeer.RaiseNotificationEvent(AutomationNotificationKind.Other, AutomationNotificationProcessing.CurrentThenMostRecent, displayString, "InfoBarOpenedActivityId");

            if (AutomationPeer.ListenerExists(AutomationEvents.WindowOpened))
            {
                RaiseAutomationEvent(AutomationEvents.WindowOpened);
            }
        }

        void RaiseWindowClosedEvent()
        {
            if (AutomationPeer.ListenerExists(AutomationEvents.WindowClosed))
            {
                RaiseAutomationEvent(AutomationEvents.WindowClosed);
            }
        }

        InfoBar GetInfoBar()
        {
            UIElement owner = Owner;
            return owner as InfoBar;
        }


    }
}

using System.Windows;
using System.Windows.Documents;
using AddressBook.CustomControls;

namespace AddressBook.Behaviours
{
    public static class BusyIndicatorBehaviour
    {
        public static readonly DependencyProperty BusyStateProperty = DependencyProperty.RegisterAttached("BusyState", typeof (bool), typeof (BusyIndicatorBehaviour), new PropertyMetadata(false, OnBusyStateChanged));
        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof(Adorner), typeof(BusyIndicatorBehaviour));

        private static void OnBusyStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                var isBusy = (bool) e.NewValue;

                if (isBusy)
                {
                    var busyCtrl = new BusyControl();
                    Adorner adorner = new ControlAdorner(element, busyCtrl);
                    AdornerLayer.GetAdornerLayer(element).Add(adorner);

                    element.SetValue(AdornerProperty, adorner);
                }
                else
                {
                    var adorner = element.GetValue(AdornerProperty) as Adorner;
                    if (adorner != null)
                    {
                        AdornerLayer.GetAdornerLayer(element).Remove(adorner);
                        element.SetValue(AdornerProperty, null);
                    }
                }
            }
        }
        

        public static void SetBusyState(DependencyObject element, bool value)
        {
            element.SetValue(BusyStateProperty, value);
        }

        public static bool GetBusyState(DependencyObject element)
        {
            return (bool) element.GetValue(BusyStateProperty);
        }
    }
}

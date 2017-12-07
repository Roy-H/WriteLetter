using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppCore.SDK.Helper
{
    public static class RegisterElementHelper
    {
        private static Dictionary<string, FrameworkElement> registeredElements = new Dictionary<string, FrameworkElement>();

        public static FrameworkElement FindElement(string name)
        {
            FrameworkElement value;
            if (registeredElements.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }

        #region RegisterElementName
        public static string GetRegisterElementName(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return (string)obj.GetValue(RegisterElementNameProperty);
        }
        public static void SetRegisterElementName(DependencyObject obj, string value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.SetValue(RegisterElementNameProperty, value);
        }
        public static readonly DependencyProperty RegisterElementNameProperty =
            DependencyProperty.RegisterAttached("RegisterElementName", typeof(string), typeof(RegisterElementHelper), new PropertyMetadata(default(string), RegisterElementNameChanged));

        private static void RegisterElementNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as FrameworkElement;
            if (element == null)
            {
                Debug.Fail("RegisterElementName: Element is not a FrameworkElement?");
                return;
            }
            element.Loaded += element_Loaded;
            element.Unloaded += element_Unloaded;
            RegisterElement(element);
        }

        static void RegisterElement(FrameworkElement element)
        {
            registeredElements[GetRegisterElementName(element)] = element;
        }

        static void element_Loaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
            {
                Debug.Fail("RegisterElementName Loaded: Element is not a FrameworkElement?");
                return;
            }

            RegisterElement(element);
        }

        /// <summary>
        /// Remove unloaded elements to avoid memory leak.
        /// </summary>
        static void element_Unloaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
            {
                Debug.Fail("RegisterElementName Loaded: Element is not a FrameworkElement?");
                return;
            }

            registeredElements.Remove(GetRegisterElementName(element));
        }

        #endregion // RegisterElementName

    }
}

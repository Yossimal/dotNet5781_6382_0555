using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToolsWPF
{
    /// <summary>
    /// Class for working with listboxes.

    /// </summary>
    public static class VisualTools
    {
        /// <summary>
        /// Returns item from list box buy another control in it
        /// </summary>
        /// <param name="control">A control that in the item that you want to get</param>
        /// <param name="list">The list that contains that item</param>
        /// <returns>The item in the list box with the given control</returns>
        public static ListBoxItem GetItemInList(Control control, ListBox list)
        {
            object context = control.DataContext;
            for (int i = 0; i < list.Items.Count; i++)
            {
                ListBoxItem item = (ListBoxItem)(list.ItemContainerGenerator.ContainerFromIndex(i));
                if (context == item.DataContext)
                {
                    return item;
                }
            }

            return null;
        }
        /// <summary>
        /// Returns item from list view buy another control in it
        /// </summary>
        /// <param name="control">A control that in the item that you want to get</param>
        /// <param name="list">The list that contains that item</param>
        /// <returns>The item in the list box with the given control</returns>
        public static ListViewItem GetItem(this ListBox list, Control control)
        {
            object context = control.DataContext;
            for (int i = 0; i < list.Items.Count; i++)
            {
                ListViewItem item = (ListViewItem)(list.ItemContainerGenerator.ContainerFromIndex(i));
                if (context == item.DataContext)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// Helper method for getting  control
        /// </summary>
        /// <param name="obj">The item that contains the control</param>
        /// <returns></returns>
        private static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        /// <summary>
        /// Returns item from list box buy another control in it
        /// </summary>
        /// <param name="list">The listbox that you want to get the item from</param>
        /// <param name="from">control in the item</param>
        /// <returns>The item in the list box with the given control</returns>
        public static ListBoxItem GetItemInList(this ListBox list,Control from)
        {
            return GetItemInList(from, list);
        }
        /// <summary>
        /// Get a control from a listbox buy the i=control name (can be in a template) and another control in the same item in the listbox
        /// </summary>
        /// <param name="list">The list that you want to get the item from</param>
        /// <param name="from">A control in the same ListBoxItem with the control that you want to get</param>
        /// <param name="controlName">The name of the control that you want to get in the DataTemplate</param>
        /// <returns>The control with the given name</returns>
        public static FrameworkElement GetControl(this ListBox list, Control from,string controlName)
        {
            ListBoxItem item = list.GetItemInList(from);
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(item);
            DataTemplate dataTemplate = contentPresenter.ContentTemplate;
            return (FrameworkElement)dataTemplate.FindName(controlName,contentPresenter);
        }
        /// <summary>
        /// Get a control from a listview buy the i=control name (can be in a template) and another control in the same item in the listbox
        /// </summary>
        /// <param name="list">The list that you want to get the item from</param>
        /// <param name="from">A control in the same ListBoxItem with the control that you want to get</param>
        /// <param name="controlName">The name of the control that you want to get in the DataTemplate</param>
        /// <returns>The control with the given name</returns>
        public static FrameworkElement GetControl(this ListView list, Control from, string controlName)
        {
            ListViewItem item = list.GetItem(from);
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(item);
            DataTemplate dataTemplate = contentPresenter.ContentTemplate;
            return (FrameworkElement)dataTemplate.FindName(controlName, contentPresenter);
        }
        /// <summary>
        /// Get a control from a listbox buy the control name (can be in a template) and another control in the same item in the listbox
        /// This method casting for you the control to the given type.
        /// </summary>
        /// <typeparam name="TC">The type og the control that you want to get</typeparam>
        /// <param name="list">The list that you want to get the item from</param>
        /// <param name="from">A control in the same ListBoxItem with the control that you want to get</param>
        /// <param name="controlName">The name of the control that you want to get in the DataTemplate</param>
        /// <returns>The control with the given name</returns>
        public static TC GetControl<TC>(this ListBox list, Control from,string controlName) where TC:FrameworkElement
        {
            return (list.GetControl(from,controlName) as TC);
        }
        /// <summary>
        /// Get a control from a listview buy the control name (can be in a template) and another control in the same item in the listbox
        /// This method casting for you the control to the given type.
        /// </summary>
        /// <typeparam name="TC">The type og the control that you want to get</typeparam>
        /// <param name="list">The list that you want to get the item from</param>
        /// <param name="from">A control in the same ListBoxItem with the control that you want to get</param>
        /// <param name="controlName">The name of the control that you want to get in the DataTemplate</param>
        /// <returns>The control with the given name</returns>
        public static TC GetControl<TC>(this ListView list, Control from, string controlName) where TC : FrameworkElement
        {
            return (list.GetControl(from, controlName) as TC);
        }

    }
}

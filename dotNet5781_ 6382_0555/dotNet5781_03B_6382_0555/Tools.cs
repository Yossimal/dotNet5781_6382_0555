using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotNet5781_03B_6382_0555
{
    class Tools
    {
        /// <summary>
        /// random for the random methods
        /// </summary>
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);
        #region old
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
        public static childItem FindVisualChild<childItem>(DependencyObject obj)
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
#endregion
        /// <summary>
        /// Working as random.Next in [min,max]
        /// </summary>
        /// <param name="min">The minimum number</param>
        /// <param name="max">The maximum number</param>
        /// <returns></returns>
        public static int RandomInt(int min, int max)
        {
            return Rand.Next(min, max + 1);
        }
        /// <summary>
        /// Get formatted string for time span
        /// </summary>
        /// <param name="ts">The time span to format</param>
        /// <returns>0:0:0 format string</returns>
        public static string FormatTimeSpan(TimeSpan ts)
        {
            return $"{ts.Hours}:{ts.Minutes}:{ts.Seconds}";
        }
        /// <summary>
        /// Get simulation time from real time
        /// </summary>
        /// <param name="ts">The real time</param>
        /// <returns>The simulated time</returns>
        public static TimeSpan SimulationTime(TimeSpan ts)
        {
            double simulationSeconds = ts.TotalSeconds * (0.1/60f);
            return TimeSpan.FromSeconds(simulationSeconds);
        }
        /// <summary>
        /// Get real time from simulated time
        /// </summary>
        /// <param name="ts">the simulated time</param>
        /// <returns></returns>
        public static TimeSpan RealFromSimulationTime(TimeSpan ts)
        {
            double realSeconds = ts.TotalSeconds / (0.1/60f);
            return TimeSpan.FromSeconds(realSeconds);
        }
    }

}

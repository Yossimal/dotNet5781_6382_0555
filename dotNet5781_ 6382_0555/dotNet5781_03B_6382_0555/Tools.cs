using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;

namespace dotNet5781_03B_6382_0555
{
    class Tools
    {
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);
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

        public static int RandomInt(int min, int max)
        {
            return Rand.Next(min, max + 1);
        }

        public static string FormatTimeSpan(TimeSpan ts)
        {
            return $"{ts.Hours}:{ts.Minutes}:{ts.Seconds}";
        }

        public static TimeSpan SimulationTime(TimeSpan ts)
        {
            double simulationSeconds = ts.TotalSeconds * (0.1/60f);
            return TimeSpan.FromSeconds(simulationSeconds);
        }

        public static TimeSpan RealFromSimulationTime(TimeSpan ts)
        {
            double realSeconds = ts.TotalSeconds / (0.1/60f);
            return TimeSpan.FromSeconds(realSeconds);
        }

        //public static Location GetRandomLocation(double minLat, double maxLat, double minLong, double maxLong)
        //{
        //    Random rand = new Random(DateTime.Now.Millisecond);
        //    if (minLat < -90 || minLong < -180 || maxLat > 90 || maxLong > 180)
        //    {
        //        throw new InvalidOperationException("One of the parameters is out of range");
        //    }
        //    double latitude = rand.NextDouble() * (maxLat - minLat) + minLat;
        //    double longitude = rand.NextDouble() * (maxLong - minLong) + minLong;
        //    return new Location(latitude, longitude);
        //}
        //public static Location GetRandomLocation()
        //{
        //    return GetRandomLocation(31, 33.3, 34.3, 35.5);
        //}
    }

}

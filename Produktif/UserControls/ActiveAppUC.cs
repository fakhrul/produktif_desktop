using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Produktif.Models;

namespace Produktif
{
    public partial class ActiveAppUC : UserControl
    {
        Dictionary<string, TimeSpan> _prevMostBrowsingUrl = new Dictionary<string, TimeSpan>();
        Dictionary<string, TimeSpan> _prevActiveAppDict = new Dictionary<string, TimeSpan>();

        public ActiveAppUC()
        {
            InitializeComponent();
        }

        internal void UpdateMostUseApps(Dictionary<string, TimeSpan> activeAppDict)
        {
            if (!IsSignificantDifference(_prevActiveAppDict, activeAppDict))
            {
                return;
            }
            try
            {
                cartesianChart1.DefaultLegend.Visibility = System.Windows.Visibility.Visible;
                cartesianChart1.LegendLocation = LegendLocation.Right;

                cartesianChart1.Series = new SeriesCollection();
                foreach (var item in activeAppDict.OrderByDescending(c => c.Value))
                {
                    if (item.Value.TotalMinutes > 5)
                    {
                        cartesianChart1.Series.Add(new RowSeries
                        {
                            Title = item.Key,
                            Values = new ChartValues<double> { item.Value.TotalSeconds },

                        });
                    }
                }

                cartesianChart1.AxisX = new AxesCollection();
                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Hours",

                    LabelFormatter = value => Extensions.ToReadableString(TimeSpan.FromSeconds(value))
                });

                cartesianChart1.AxisY = new AxesCollection();
                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Application",
                    //LabelFormatter = value => "asd"
                });
                _prevActiveAppDict = activeAppDict;
            }
            catch (Exception e)
            {
            }


        }

        IEnumerable<UserActivity> _prevTaskProgress = new List<UserActivity>();
        internal void UpdateTaskProgress(IEnumerable<UserActivity> userActivities)
        {
            if (!IsTaskProgressDifference(_prevTaskProgress.ToList(), userActivities.ToList()))
            {
                return;
            }
            try
            {
                cartesianChartTaskProgress.DefaultLegend.Visibility = System.Windows.Visibility.Visible;
                cartesianChartTaskProgress.LegendLocation = LegendLocation.Right;

                cartesianChartTaskProgress.Series = new SeriesCollection();
                foreach (var item in userActivities.OrderByDescending(c => c.TotalSpend))
                {

                    cartesianChartTaskProgress.Series.Add(new RowSeries
                    {
                        Title = item.Name.Length < 20 ? item.Name: item.Name.Substring(0,20),
                        Values = new ChartValues<double> { item.TotalSpend.TotalSeconds },

                    });
                }

                cartesianChartTaskProgress.AxisX = new AxesCollection();
                cartesianChartTaskProgress.AxisX.Add(new Axis
                {
                    Title = "Hours",

                    LabelFormatter = value => Extensions.ToReadableString(TimeSpan.FromSeconds(value))
                });

                cartesianChartTaskProgress.AxisY = new AxesCollection();
                cartesianChartTaskProgress.AxisY.Add(new Axis
                {
                    Title = "Application",
                    //LabelFormatter = value => "asd"
                });
                var aaa = userActivities.ToList();
                _prevTaskProgress = Extensions.Clone<UserActivity>(aaa);
            }
            catch (Exception e)
            {
            }
        }

        private bool IsTaskProgressDifference(List<UserActivity> obj1, List<UserActivity> obj2)
        {
            try
            {
                if (obj1.Count() != obj2.Count())
                    return true;

                for (int i = 0; i < obj1.Count(); i++)
                {
                    if (obj1[i].TotalSpend != obj2[i].TotalSpend)
                    {
                        return true;
                    }

                }
                return false;

            }
            catch (Exception ex)
            {
            }
            return true;
        }

        internal void UpdateMostBrowseUrl(Dictionary<string, TimeSpan> mostBrowsingUrl)
        {
            if (!IsSignificantDifference(_prevMostBrowsingUrl, mostBrowsingUrl))
            {
                return;
            }
            try
            {
                Func<ChartPoint, string> labelPoint = chartPoint =>
                   string.Format("{0} ({1:P})", Extensions.ToReadableString(TimeSpan.FromSeconds(chartPoint.Y)), chartPoint.Participation);

                pieChart1.Series = new SeriesCollection();

                //var top2 = (Dictionary<string, TimeSpan>)mostBrowsingUrl.OrderByDescending(c => c.Value).Take(2);

                foreach (var item in mostBrowsingUrl.OrderByDescending(c => c.Value).Take(10))
                {
                    //if (keys.Key > 5)
                    //{
                        pieChart1.Series.Add(new PieSeries
                        {
                            Title = item.Key,
                            Values = new ChartValues<double> { item.Value.TotalSeconds },
                            //PushOut = mostBrowsingUrl[keys].TotalSeconds,
                            DataLabels = true,
                            LabelPoint = labelPoint
                        });
                    //}

                }

                pieChart1.LegendLocation = LegendLocation.Bottom;
                _prevMostBrowsingUrl = mostBrowsingUrl;

            }
            catch (Exception ex)
            {
            }


        }

        private bool IsSignificantDifference(Dictionary<string, TimeSpan> obj1, Dictionary<string, TimeSpan> obj2)
        {
            try
            {
                if (_prevMostBrowsingUrl.Count != obj2.Count)
                    return true;
                foreach (var keys in obj1.Keys)
                {
                    if (!obj2.ContainsKey(keys))
                        return true;
                    if (obj2[keys].TotalSeconds - obj1[keys].TotalSeconds > 5)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
            }
            return true;
        }


    }

}

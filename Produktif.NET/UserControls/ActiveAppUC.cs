using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Produktif
{
    public partial class ActiveAppUC : UserControl
    {
        public ActiveAppUC()
        {
            InitializeComponent();
        }

        internal void UpdateInfo(Dictionary<string, TimeSpan> activeAppDict)
        {
            listView1.Items.Clear();

            foreach (var activeApp in activeAppDict.Keys)
            {
                string processName = activeApp.Split('|')[0];
                string url = activeApp.Split('|')[1];
                TimeSpan ts = activeAppDict[activeApp];

                listView1.Items.Add(new ListViewItem(new string[] {
                    processName,
                    url,
                    UtilString.ToReadableString(ts)
                }));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produktif
{
    public class ActiveWindowUpdateEventArgs : EventArgs
    {
        public ActiveWindow ActiveWindow { get; set; }
        public ActiveWindowUpdateEventArgs(ActiveWindow activeWindow)
        {
            ActiveWindow = activeWindow;
        }
    }
}

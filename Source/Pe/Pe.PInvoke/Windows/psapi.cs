﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    partial class NativeMethods
    {
        [DllImport("psapi")]
        public static extern bool EmptyWorkingSet(IntPtr hProcess);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RGiesecke.DllExport;

namespace connector
{
    public class Class1
    {
        [DllExport("add", CallingConvention = CallingConvention.Cdecl)]
        public static int TestExport(int left, int right)
        {
            return left + right;
        }
    }
}

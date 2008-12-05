using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mogre;

namespace Squamster
{
    static class Program
    {
        [STAThread]

        static void Main()
        {
            OgreForm form = new OgreForm();
            form.Init();
            form.Go();
        }
    }
}
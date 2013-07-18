using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutlookSampleWebRole
{
    public class MyUtil
    {
        public static string EscapeForICS(string p)
        {
            return p.Replace("\r", @"\r").Replace("\n", @"\n").Replace("\\", @"\");
            //string res = System.Text.RegularExpressions.Regex.Escape(p);
            //あ*い+う?え|お{か[き(く)け^こ$さ#し.す せ
        }
    }
}
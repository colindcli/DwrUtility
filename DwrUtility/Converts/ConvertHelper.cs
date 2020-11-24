using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DwrUtility.Converts
{
    /// <summary>
    /// 转换类
    /// </summary>
    public class ConvertHelper
    {
        /*
         * 对应在记事本的编码为
         * gb2312  ANSI
         * gbk  ANSI
         * Unicode  UCS-2 LE BOM
         * BigEndianUnicode  UCS-2 BE BOM
         */

        /// <summary>
        /// 从字节流判断编码（返回null是不能判断出编码）
        /// </summary>
        /// <param name="bt">输入字节流</param>
        /// <returns></returns>
        public static string GetEncoding(byte[] bt)
        {
            if (bt == null || bt.Length == 0)
            {
                return null;
            }

            //带BOM的utf-8
            var utf8 = new byte[] { 0xEF, 0xBB, 0xBF };
            if (bt[0] == utf8[0] && bt[1] == utf8[1] && bt[2] == utf8[2])
            {
                return "utf-8";
            }

            //UTF-32-BE
            var utf32Be = new byte[] { 0x00, 0x00, 0xFE, 0xFF };
            if (bt[0] == utf32Be[0] &&
                bt[1] == utf32Be[1] &&
                bt[2] == utf32Be[2] &&
                bt[3] == utf32Be[3])
            {
                return "utf-32";
            }

            //UTF-32-LE
            var utf32Le = new byte[] { 0xFF, 0xFE, 0x00, 0x00 };
            if (bt[0] == utf32Le[0] &&
                bt[1] == utf32Le[1] &&
                bt[2] == utf32Le[2] &&
                bt[3] == utf32Le[3])
            {
                return "utf-32";
            }

            //UTF-32-2143
            var utf322143 = new byte[] { 0x00, 0x00, 0xFF, 0xFE };
            if (bt[0] == utf322143[0] &&
                bt[1] == utf322143[1] &&
                bt[2] == utf322143[2] &&
                bt[3] == utf322143[3])
            {
                return "utf-32";
            }

            //UTF-32-3412
            var utf323412 = new byte[] { 0xFE, 0xFF, 0x00, 0x00 };
            if (bt[0] == utf323412[0] &&
                bt[1] == utf323412[1] &&
                bt[2] == utf323412[2] &&
                bt[3] == utf323412[3])
            {
                return "utf-32";
            }

            //UTF-16-BE
            var utf16Be = new byte[] { 0xFE, 0xFF };
            if (bt[0] == utf16Be[0] &&
                bt[1] == utf16Be[1])
            {
                return "utf-16";
            }

            //UTF-16-LE
            var utf16Le = new byte[] { 0xFF, 0xFE };
            if (bt[0] == utf16Le[0] &&
                bt[1] == utf16Le[1])
            {
                return "utf-16";
            }

            return null;
        }

        /// <summary>
        /// 将流读取到内存
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCloseStream">读取完是否关闭流</param>
        /// <returns></returns>
        public static MemoryStream ReadAsMemoryStream(Stream input, bool isCloseStream)
        {
            if (input == null)
            {
                return null;
            }

            var buffer = new byte[16384];
            using (var ms = new MemoryStream())
            {
                int count;
                input.Position = 0;
                while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, count);
                }

                if (isCloseStream)
                {
                    input.Close();
                    input.Dispose();
                }

                return ms;
            }
        }

        /// <summary>
        /// 字符串转内存流
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(string str, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(str));
        }

        /// <summary>
        /// 转二进制，如100转二进制1100100
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="padLeft">不足8位是否补0</param>
        /// <returns></returns>
        public static string ToBinary(byte bt, bool padLeft)
        {
            return padLeft ? Convert.ToString(bt, 2).PadLeft(8, '0') : Convert.ToString(bt, 2);
        }

        /// <summary>
        /// 转二进制，如999转二进制1111100111
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static string ToBinary(int bt)
        {
            return Convert.ToString(bt, 2);
        }

        /// <summary>
        /// 转十六进制，如255转十六进制ff
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="capital">是否大写</param>
        /// <returns></returns>
        public static string ToHexadecimal(byte bt, bool capital)
        {
            return bt.ToString(capital ? "X2" : "x2");
        }

        /// <summary>
        /// 转十六进制，如999转十六进制3e7
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static string ToHexadecimal(int bt)
        {
            return Convert.ToString(bt, 16);
        }

        #region 汉字转拼音

        //定义拼音区编码数组
        private static readonly int[] GetValue = {
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };

        //定义拼音数组
        private static readonly string[] GetName = {
            "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
            "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
            "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
            "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
            "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
            "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
            "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
            "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
            "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
            "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
            "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
            "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
            "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
            "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
            "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
            "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
            "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
            "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
            "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
            "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
            "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
            "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
            "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
            "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
            "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
            "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
            "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
            "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
            "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
            "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
            "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
            "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
            "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };

        /// <summary>
        /// 汉字转换成全拼的拼音
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <returns>转换后的拼音字符串</returns>
        public static string ToPingYin(string str)
        {
#if NETSTANDARD
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            var reg = new Regex("^[\u4e00-\u9fa5]$");//验证是否输入汉字
            // ReSharper disable once TooWideLocalVariableScope
            byte[] arr;
            var sb = new StringBuilder();
            // ReSharper disable once TooWideLocalVariableScope
            int asc;
            // ReSharper disable once TooWideLocalVariableScope
            int m1;
            // ReSharper disable once TooWideLocalVariableScope
            int m2;
            var mChar = str.ToCharArray();//获取汉字对应的字符数组
            var encoding = Encoding.GetEncoding("gb2312");
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var j = 0; j < mChar.Length; j++)
            {
                //如果不是汉字
                if (!reg.IsMatch(mChar[j].ToString()))
                {
                    sb.Append(mChar[j].ToString());
                }
                else
                {
                    arr = encoding.GetBytes(mChar[j].ToString());
                    m1 = arr[0];
                    m2 = arr[1];
                    asc = m1 * 256 + m2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        sb.Append(mChar[j]);
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                sb.Append("Zhen");
                                break;
                            case -8985:
                                sb.Append("Qian");
                                break;
                            case -5463:
                                sb.Append("Jia");
                                break;
                            case -8274:
                                sb.Append("Ge");
                                break;
                            case -5448:
                                sb.Append("Ga");
                                break;
                            case -5447:
                                sb.Append("La");
                                break;
                            case -4649:
                                sb.Append("Chen");
                                break;
                            case -5436:
                                sb.Append("Mao");
                                break;
                            case -5213:
                                sb.Append("Mao");
                                break;
                            case -3597:
                                sb.Append("Die");
                                break;
                            case -5659:
                                sb.Append("Tian");
                                break;
                            default:
                                for (var i = (GetValue.Length - 1); i >= 0; i--)
                                {
                                    if (GetValue[i] > asc)
                                    {
                                        continue;
                                    }

                                    sb.Append(GetName[i]); //如果不超出范围则获取对应的拼音
                                    break;
                                }
                                break;
                        }
                    }
                }
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 字符串转Unicode编码
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string StringToUnicode(string str)
        {
            var bytes = Encoding.Unicode.GetBytes(str);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode解码成字符串
        /// </summary>
        /// <param name="unicode">经过Unicode编码的字符串</param>
        /// <returns>解码后字符串</returns>
        public static string UnicodeToString(string unicode)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                unicode, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}

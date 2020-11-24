#if DEBUG
using DwrUtility.TaskExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DwrUtility.Test
{
    [TestClass]
    public class TaskHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                return 1;
            }).SetTimeout(100);
            var b1 = t1.Result == 0;

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 1;
            }).SetTimeout(200);
            var b2 = t2.Result == 1;

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                return 1;
            }).SetTimeoutResult(100);
            var b1 = t1.Result.Value == 0 && t1.Result.IsTimeout;

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 1;
            }).SetTimeoutResult(200);
            var b2 = t2.Result.Value == 1 && !t2.Result.IsTimeout;

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                var i = int.Parse("s");
                return 1;
            });

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                var i = DateTime.Parse("1");
                return 1;
            });

            try
            {
                TaskHelper.WaitAll(t1, t2);
            }
            catch (Exception ex)
            {
                var msg = ex.ToString();
#if NETSTANDARD
                Assert.IsTrue(msg.IsContains("One or more errors occurred."));
#else
                Assert.IsTrue(msg.IsContains("输入字符串的格式不正确") && msg.IsContains("该字符串未被识别为有效的 DateTime"));
#endif
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var list = new List<int>()
            {
                6,7,8,9,
                4,5,
                1,2,3,10,11,12,13,14,15,16,17,18,19,20
            };

            //线程运行
            var tasks = list.RunTask(p =>
            {
                Thread.Sleep(p * 100);
                return p;
            });

            //返回线程状态
            var row = new List<int>();
            tasks.TaskStatus(80, (total, rs) =>
            {
                WriteHelper.Log(total.ToString() + " : " + string.Join(";", rs));
                row.AddRange(rs);
            });

            var b = row.Distinct().Count() == 20;
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var t = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "6数据";
            });

            var i = 0;
            t.TaskStatus(100, () =>
            {
                i++;
            });

            Assert.IsTrue(i > 5);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var list = new List<int>()
            {
                6,7,8,9,
                4,5,
                1,2,3,10,11,12,13,14,15,16,17,18,19,100,20
            };

            var nums = new List<int>();
            var thread = new List<bool>();

            list.RunTaskLimite(new TaskParam<int>()
            {
                Method = p => Thread.Sleep(p * 50),
                MaxThread = 3,
                Sleep = 0,
                Report = (type, start, end, item, n) =>
                {
                    WriteHelper.Log($"{start}-{end}:{type.GetReportTypeName()}{item}");
                    //验证是否全部完成
                    nums.Add(item);
                    //验证最大线程数
                    thread.Add(end - start == 3);
                }
            });

            var num = nums.Distinct().Count();
            var b1 = thread.Exists(p => !p);
            Assert.IsTrue(num == list.Count && b1);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var maxThread = 8;
            var list = new List<int>()
            {
                10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,
                10,11,12,13,14,15,16,17,18,19,20,
                100,100,100,100,100,100,100,100,
                30,31,32,33,34,35,36,37,38,39,40,
                50,51,52,53,54,55,56,57,58,59,60,
            };

            var nums = new List<int>();
            var threads = new List<bool>();

            list.RunTaskLimite(new TaskParam<int>()
            {
                Method = p => Thread.Sleep(p * 10),
                MaxThread = maxThread,
                Sleep = 0,
                Report = (type, start, end, item, n) =>
                {
                    WriteHelper.Log($"{start}-{end}:{type.GetReportTypeName()}{item}");
                    //验证是否全部完成
                    nums.Add(item);
                    //验证最大线程数
                    threads.Add(end - start <= maxThread);
                }
            });

            var num = nums.Distinct().Count();
            var b1 = threads.Where(p => !p).ToList();
            Assert.IsTrue(num == list.Distinct().Count(), $"执行数：{num}");
            Assert.IsTrue(b1.Count == 0, $"超出线程：{threads.Count}");
        }

        [TestMethod]
        public void TestMethod9()
        {
            var maxThread = 8;
            var list = new List<int>()
            {
                10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,
                10,11,12,13,14,15,16,17,18,19,20,
                100,100,100,100,100,100,100,100,
                30,31,32,33,34,35,36,37,38,39,40,
                50,51,52,53,54,55,56,57,58,59,60,
            };

            var nums = new List<int>();
            var threads = new List<bool>();

            var rs = list.RunTaskLimite(new TaskParam<int, string>()
            {
                Method = p =>
                {
                    Thread.Sleep(p * 10);
                    return p.ToString();
                },
                MaxThread = maxThread,
                Sleep = 0,
                Report = (type, start, end, item, n) =>
                {
                    WriteHelper.Log($"{start}-{end}:{type.GetReportTypeName()}{item}");
                    //验证是否全部完成
                    nums.Add(item);
                    //验证最大线程数
                    threads.Add(end - start <= maxThread);
                }
            });

            var num = nums.Distinct().Count();
            var b1 = threads.Where(p => !p).ToList();
            Assert.IsTrue(num == list.Distinct().Count(), $"执行数：{num}");
            Assert.IsTrue(rs.Count == list.Count, $"执行数2：{rs.Count} {list.Count}");
            Assert.IsTrue(b1.Count == 0, $"超出线程：{threads.Count}");
        }

        [TestMethod]
        public void TestMethod10()
        {
            var maxThread = 8;
            var list = new List<int>()
            {
                10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,
                10,11,12,13,14,15,16,17,18,19,20,
                100,100,100,100,100,100,100,100,
                30,31,32,33,34,35,36,37,38,39,40,
                50,51,52,53,54,55,56,57,58,59,60,
            };

            var nums = new List<int>();
            var threads = new List<bool>();

            var rs = list.RunTaskLimite(new TaskParam<int, string>()
            {
                Method = p =>
                {
                    Thread.Sleep(p * 10);
                    return p.ToString();
                },
                MaxThread = maxThread,
                Sleep = 250,
                Report = (type, start, end, item, n) =>
                {
                    WriteHelper.Log($"{start}-{end}:{type.GetReportTypeName()}{item}");
                    //验证是否全部完成
                    nums.Add(item);
                    //验证最大线程数
                    threads.Add(end - start <= maxThread);
                },
            });

            var num = nums.Distinct().Count();
            var b1 = threads.Where(p => !p).ToList();
            Assert.IsTrue(num == list.Distinct().Count(), $"执行数：{num}");
            Assert.IsTrue(rs.Count == list.Count, $"执行数2：{rs.Count} {list.Count}");
            Assert.IsTrue(b1.Count == 0, $"超出线程：{threads.Count}");
        }
    }
}
#endif

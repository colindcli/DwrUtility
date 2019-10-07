using DwrUtility.Lists;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwrUtility.Test
{
    [TestClass]
    public class ObjectExtensionTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            var rows = new List<Row>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new Row()
                {
                    Id = i,
                    Name = $"Value_{i}"
                });
            }

            rows.Add(new Row()
            {
                Id = 1,
                Name = $"Value_2"
            });

            rows.Add(new Row()
            {
                Id = 1,
                Name = $"Value_3"
            });


            var obj = rows.ToDict(p => p.Id, p => p.Name, true);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<int, string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_31_result.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod1A()
        {
            var rows = new List<RowA>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowA()
                {
                    Id = $"a{i}",
                    Name = $"Value_{i}"
                });
            }

            rows.Add(new RowA()
            {
                Id = $"a2",
                Name = $"Value_2a"
            });

            rows.Add(new RowA()
            {
                Id = $"A2",
                Name = $"Value_2A"
            });

            var obj = rows.ToDict(p => p.Id, p => p.Name, true, StringComparer.OrdinalIgnoreCase);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_31_result_A.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var rows = new List<Row>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new Row()
                {
                    Id = i,
                    Name = $"Value_{i}"
                });
            }

            rows.Add(new Row()
            {
                Id = 1,
                Name = $"Value_2"
            });

            rows.Add(new Row()
            {
                Id = 1,
                Name = $"Value_3"
            });


            var obj = rows.ToDict(p => p.Id, p => p.Name, false);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<int, string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_34_result.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 2,
                Name = $"Value_2",
                Desc = "222"
            });


            var obj = rows.ToDict(p => new { p.Id, p.Name }, p => p, true);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<object, RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_35_result.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<object, RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 2,
                Name = $"Value_2",
                Desc = "222"
            });


            var obj = rows.ToDict(p => new { p.Id, p.Name }, p => p, false);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<object, RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_36_result.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<object, RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 2,
                Name = $"Value_2",
                Desc = "222"
            });


            var obj = rows.ToDist(p => new { p.Id, p.Name });
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_37_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5B()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 2; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 2,
                Name = $"Value_2",
                Desc = "222"
            });

            var obj = rows.ToDist((p, q) => p.Id == q.Id && p.Name == q.Name).ToList();
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_37_result_B.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5C()
        {
            var rows = new List<string>() { "111", "111", "222" };

            var obj = rows.Distinct().ToList();

            //正确结果
            var pathResult = $"{FileDir}test_37_result_C.json";
            var resultOk = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(obj, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5A()
        {
            var rows = new List<RowTwoA>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwoA()
                {
                    Id = $"a{i}",
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwoA()
            {
                Id = $"a1",
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwoA()
            {
                Id = $"A1",
                Name = $"Value_1",
                Desc = "222"
            });


            var obj = rows.ToDist(p => p.Id, StringComparer.OrdinalIgnoreCase);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<List<string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_37_result_A.json";
            var resultOk = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod6()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });


            var b = rows.HasRepeat(p => new { p.Id, p.Name });

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            var b = !rows.HasRepeat(p => new { p.Id, p.Name });

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "Desc_1"
            });

            // 不是匿名对象不会重复
            var b = !rows.HasRepeat(p => p);

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "Desc_1"
            });

            // 不是匿名对象不会重复
            var b = !rows.HasRepeat(p => new RowTwo() { Id = p.Id });

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var rows = new List<RowTwo>
            {
                null,
                null
            };

            var b = rows.HasRepeat(p => p);

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod11()
        {
            long t = 1557041613320;
            var dt = t.ToDatetime();
            var ticks = dt.ToTicks();

            Assert.IsTrue(dt == Convert.ToDateTime("2019-05-05 15:33:33.320"));
            Assert.IsTrue(ticks == t);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"VAlue_1",
                Desc = "111"
            });


            var items = rows.GetRepeatKeys(p => new { p.Id, p.Name });

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_38_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod12A()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"valuE_1",
                Desc = "111"
            });

            var items = rows.GetRepeatKeys(p => p.Name, StringComparer.OrdinalIgnoreCase);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_38_result_A.json";
            var resultOk = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod12B()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"VAlue_1",
                Desc = "111"
            });

            var items = rows.GetRepeatKeys(p => p.Name);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_38_result_B.json";
            var resultOk = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod13()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"valuE_1",
                Desc = "111"
            });

            var items = rows.GetRepeatLists(p => p.Name, StringComparer.OrdinalIgnoreCase);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_39_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod13A()
        {
            var rows = new List<RowTwo>();
            for (var i = 0; i < 10; i++)
            {
                rows.Add(new RowTwo()
                {
                    Id = i,
                    Name = $"Value_{i}",
                    Desc = $"Desc_{i}"
                });
            }

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"Value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"value_1",
                Desc = "111"
            });

            rows.Add(new RowTwo()
            {
                Id = 1,
                Name = $"valuE_1",
                Desc = "111"
            });

            var items = rows.GetRepeatLists(p => p.Name);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_39_result_A.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod14()
        {
            var html = "<span id=\"test\">te   st </span> &nbsp;";
            var res = html.ClearHtml();

            Assert.IsTrue(res.IsEquals("test"));
        }

        [TestMethod]
        public void TestMethod15()
        {
            string html = string.Empty;
            var res = html.ClearHtml();

            Assert.IsTrue(res.IsEquals(""));
        }

        [TestMethod]
        public void TestMethod16()
        {
            string s1 = null;
            var s2 = string.Empty;
            var s3 = "";

            var b1 = s1.IsWhiteSpace() && s2.IsWhiteSpace() && s3.IsWhiteSpace() && " ".IsWhiteSpace();
            var b2 = s1.IsEmpty() && s2.IsEmpty();
            var b3 = s3.IsEmpty();
            var b4 = !(" ".IsEmpty());

            Assert.IsTrue(b1 && b2 && b3 && b4);
        }

        [TestMethod]
        public void TestMethod17()
        {
            string s1 = null;
            var s2 = string.Empty;

            var b1 = s2.IsContains(s2);
            var b2 = s1.IsContains(s2);
            var b3 = s2.IsContains(s1);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod]
        public void TestMethod18()
        {
            Assert.IsTrue(" /11/22   ".TrimSlash() == "11/22");
            Assert.IsTrue(" /11/22/   ".TrimSlash() == "11/22");
            Assert.IsTrue("   /11/22\\   ".TrimSlash() == "11/22");
            Assert.IsTrue(" \\11/22   ".TrimSlash() == "11/22");
            Assert.IsTrue("   \\11/22/   ".TrimSlash() == "11/22");
            Assert.IsTrue(" \\11/22\\      ".TrimSlash() == "11/22");

            Assert.IsTrue(" /11/22   ".TrimStartSlash() == "11/22   ");
            Assert.IsTrue(" /11/22/   ".TrimStartSlash() == "11/22/   ");
            Assert.IsTrue("  /11/22\\   ".TrimStartSlash() == "11/22\\   ");
            Assert.IsTrue(" \\11/22   ".TrimStartSlash() == "11/22   ");
            Assert.IsTrue(" \\11/22/   ".TrimStartSlash() == "11/22/   ");
            Assert.IsTrue("   \\11/22\\      ".TrimStartSlash() == "11/22\\      ");

            Assert.IsTrue(" /11/22   ".TrimEndSlash() == " /11/22");
            Assert.IsTrue(" /11/22/   ".TrimEndSlash() == " /11/22");
            Assert.IsTrue("   /11/22\\   ".TrimEndSlash() == "   /11/22");
            Assert.IsTrue(" \\11/22   ".TrimEndSlash() == " \\11/22");
            Assert.IsTrue("   \\11/22/   ".TrimEndSlash() == "   \\11/22");
            Assert.IsTrue(" \\11/22\\      ".TrimEndSlash() == " \\11/22");
        }

        /// <summary>
        /// 两个null相等
        /// </summary>
        [TestMethod]
        public void TestMethod19()
        {
            string s1 = null;
            string s2 = null;

            var b = s1.IsEquals(s2);
            Assert.IsTrue(b);
        }

        /// <summary>
        /// 空或者null相等
        /// </summary>
        [TestMethod]
        public void TestMethod20()
        {
            string s1 = null;
            string s2 = "";

            var b = s1.IsEquals(s2);
            Assert.IsTrue(b);
        }

        public class Row
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class RowA
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class RowTwo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
        }
        public class RowTwoA
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
        }
    }
}

using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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


            var obj = rows.ToDictionary(p => p.Id, p => p.Name, true);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<Dictionary<int, string>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_31_result.json";
            var resultOk = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(pathResult));

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


            var obj = rows.ToDictionary(p => p.Id, p => p.Name, false);
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


            var obj = rows.ToDictionary(p => new { p.Id, p.Name }, p => p, true);
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


            var obj = rows.ToDictionary(p => new { p.Id, p.Name }, p => p, false);
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


            var obj = rows.Distinct(p => new { p.Id, p.Name });
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_37_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

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


            var items = rows.GetRepeats(p => new { p.Id, p.Name });

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowTwo>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_38_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        public class Row
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class RowTwo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
        }
    }
}

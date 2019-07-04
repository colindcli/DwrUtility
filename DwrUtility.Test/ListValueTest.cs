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
    public class ListValueTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            var rows = new List<RowOne>();
            var sources = new List<SourceOne>();

            for (var i = 0; i < 10; i++)
            {
                var id1 = Guid.NewGuid();
                var id2 = Guid.NewGuid();
                var id3 = Guid.NewGuid();
                rows.Add(new RowOne()
                {
                    Ids = $"{id1};{id2};{id3}"
                });

                if (i != 1)
                {
                    sources.Add(new SourceOne()
                    {
                        Id = id1,
                        Value = $"Value{i}"
                    });
                    sources.Add(new SourceOne()
                    {
                        Id = id2,
                        Value = $"Value{i + 1}"
                    });
                    sources.Add(new SourceOne()
                    {
                        Id = id3,
                        Value = $"Value{i + 2}"
                    });
                }
            }

            rows.Add(new RowOne()
            {
                Ids = Guid.Empty.ToString(),
            });

            rows.SetListValuesByIdsString(sources, p => p.Ids, p => p.Values, p => p.Id, p => p.Value, ';');

            Assert.IsTrue(rows[0].Values == "Value0;Value1;Value2");
            Assert.IsTrue(rows[1].Values == "");
            Assert.IsTrue(rows[5].Values == "Value5;Value6;Value7");
            Assert.IsTrue(rows[10].Values == "");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var rows = new List<RowTwo>();
            var sources = new List<SourceTwo>();

            for (var i = 0; i < 3; i++)
            {
                var id = Guid.NewGuid();
                rows.Add(new RowTwo()
                {
                    Id = id
                });

                if (i != 1)
                {
                    sources.Add(new SourceTwo()
                    {
                        Id = id,
                        Value = i
                    });
                    sources.Add(new SourceTwo()
                    {
                        Id = id,
                        Value = i + 1
                    });
                }
            }

            var items = ListHelper.SetListValue(new ListValueById<RowTwo, SourceTwo, Guid, int?>()
            {
                List = rows,
                Source = sources,
                ListIdField = p => p.Id,
                ListValueField = p => p.Value,
                SourceIdField = p => p.Id,
                SourceValueField = p => p.Value,
                UseSourceFirstValue = true
            }).ToList();

            var b = rows.Count == items.Count;
            var b0 = items[0].Value == 0;
            var b1 = items[1].Value == null;
            var b2 = items[2].Value == 2;

            Assert.IsTrue(b && b0 && b1 && b2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var rows = new List<RowTwo>();
            var sources = new List<SourceTwo>();

            for (var i = 0; i < 3; i++)
            {
                var id = Guid.NewGuid();
                rows.Add(new RowTwo()
                {
                    Id = id
                });
                if (i != 1)
                {
                    sources.Add(new SourceTwo()
                    {
                        Id = id,
                        Value = i
                    });
                    sources.Add(new SourceTwo()
                    {
                        Id = id,
                        Value = i + 1
                    });
                }
            }

            var items = ListHelper.SetListValue(new ListValueById<RowTwo, SourceTwo, Guid, int?>()
            {
                List = rows,
                Source = sources,
                ListIdField = p => p.Id,
                ListValueField = p => p.Value,
                SourceIdField = p => p.Id,
                SourceValueField = p => p.Value,
                UseSourceFirstValue = false
            }).ToList();

            var b = rows.Count == items.Count;
            var b0 = items[0].Value == 1;
            var b1 = items[1].Value == null;
            var b2 = items[2].Value == 3;

            Assert.IsTrue(b && b0 && b1 && b2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var lists = new List<RowThree>();
            var sources = new List<SourceThree>();

            for (var i = 0; i < 4; i++)
            {
                lists.Add(new RowThree()
                {
                    Id = new List<int>()
                    {
                        i, i+10000, i+2000
                    }
                });

                if (i != 1)
                {
                    sources.Add(new SourceThree()
                    {
                        Id = i,
                        Type = $"Type{i}"
                    });
                    sources.Add(new SourceThree()
                    {
                        Id = i + 10000,
                        Type = $"Type{i + 10000}"
                    });
                }
            }


            var items = lists.SetListValuesByIds(sources, p => p.Id, p => p.Value, p => p.Id, p => p);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowThree>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_32_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowThree>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var lists = new List<RowFour>();
            var sources = new List<SourceFour>();

            for (var i = 0; i < 4; i++)
            {
                lists.Add(new RowFour()
                {
                    Id = new List<int>()
                    {
                        i, i+10000, i+2000
                    }
                });

                if (i != 1)
                {
                    sources.Add(new SourceFour()
                    {
                        Id = i,
                        Value = $"Value{i}"
                    });
                    sources.Add(new SourceFour()
                    {
                        Id = i + 10000,
                        Value = $"Value{i + 10000}"
                    });
                }
            }
            
            var items = lists.SetListValuesByIds(sources, p => p.Id, p => p.Value, p => p.Id, p => p.Value);

            var str = JsonConvert.SerializeObject(items);
            var result = JsonConvert.DeserializeObject<List<RowFour>>(str);

            //正确结果
            var pathResult = $"{FileDir}test_33_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<RowFour>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod6()
        {
            //待验证
            var path = $"{FileDir}test_30_1.json";
            var json = File.ReadAllText(path);
            var rows = JsonConvert.DeserializeObject<List<RowFive>>(json);
            var path1 = $"{FileDir}test_30_2.json";
            var json1 = File.ReadAllText(path1);
            var source = JsonConvert.DeserializeObject<List<RowFive>>(json1);

            var obj = rows.GetDiffList(source, p => p.Id, p => p.Id, false);

            obj.LeftDiffs = obj.LeftDiffs.OrderBy(p => p.Id).ThenBy(p => p.Number).ToList();
            obj.RightDiffs = obj.RightDiffs.OrderBy(p => p.Id).ThenBy(p => p.Number).ToList();
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot>(str);

            //正确结果
            var pathResult = $"{FileDir}test_30_result.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod6B()
        {
            //待验证
            var path = $"{FileDir}test_30_1.json";
            var json = File.ReadAllText(path);
            var rows = JsonConvert.DeserializeObject<List<RowFive>>(json);
            var path1 = $"{FileDir}test_30_2.json";
            var json1 = File.ReadAllText(path1);
            var source = JsonConvert.DeserializeObject<List<RowFive>>(json1);

            var obj = rows.GetDiffList(source, p => p.Id, p => p.Id, true);

            obj.LeftDiffs = obj.LeftDiffs.OrderBy(p => p.Id).ThenBy(p => p.Number).ToList();
            obj.RightDiffs = obj.RightDiffs.OrderBy(p => p.Id).ThenBy(p => p.Number).ToList();
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot>(str);

            //正确结果
            var pathResult = $"{FileDir}test_30_result.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod7()
        {
            var left = new List<int>() { 1, 2 };
            var right = new List<int>() { 1, 2 };
            var b = left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var left = new List<int>() { 1 };
            var right = new List<int>() { 1, 2 };
            var b = !left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var left = new List<int>() { 1, 2 };
            var right = new List<int>() { 1 };
            var b = !left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 2, };
            var b = !left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod11()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 2, 2 };
            var b = left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 1, 2 };
            var b = !left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod13()
        {
            var left = new List<int>() { 1, 1, 2, 2 };
            var right = new List<int>() { 1, 1, 2, 2 };
            var b = left.IsEquals(right, p => p, p => p, false);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod15()
        {
            var left = new List<int>() { 1, 2 };
            var right = new List<int>() { 1, 2 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod16()
        {
            var left = new List<int>() { 1 };
            var right = new List<int>() { 1, 2 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = !left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod17()
        {
            var left = new List<int>() { 1, 2 };
            var right = new List<int>() { 1 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = !left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod18()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 2, };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = !left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod19()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 2, 2 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod20()
        {
            var left = new List<int>() { 1, 2, 2 };
            var right = new List<int>() { 1, 1, 2 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = !left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod21()
        {
            var left = new List<int>() { 1, 1, 2, 2 };
            var right = new List<int>() { 1, 1, 2, 2 };
            left.AddRange(Enumerable.Range(3, 100000));
            right.AddRange(Enumerable.Range(3, 100000));
            var b = left.IsEquals(right, p => p, p => p, true);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod22()
        {
            //待验证
            var path = $"{FileDir}test_28_1.json";
            var json = File.ReadAllText(path);
            var rows = JsonConvert.DeserializeObject<List<RowFive>>(json);
            var path1 = $"{FileDir}test_28_2.json";
            var json1 = File.ReadAllText(path1);
            var source = JsonConvert.DeserializeObject<List<RowFive>>(json1);

            var obj = rows.GetDiffList(source, p => new { p.Id, p.Name }, p => new { p.Id, p.Name }, false);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot>(str);

            //正确结果
            var pathResult = $"{FileDir}test_28_result.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod23()
        {
            var rows = new List<int>() { 1, 2, 2, 2, 3 };
            var sources = new List<int>() { 2, 3, 3, 3, 3, 4 };
            var obj = rows.GetDiffList(sources, p => p, p => p, false);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot2>(str);

            //正确结果
            var pathResult = $"{FileDir}test_29_result.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot2>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod24()
        {
            //待验证
            var path = $"{FileDir}test_28_1.json";
            var json = File.ReadAllText(path);
            var rows = JsonConvert.DeserializeObject<List<RowFive>>(json);
            var path1 = $"{FileDir}test_28_2.json";
            var json1 = File.ReadAllText(path1);
            var source = JsonConvert.DeserializeObject<List<RowFive>>(json1);

            var obj = rows.GetDiffList(source, p => new { p.Id, p.Name }, p => new { p.Id, p.Name }, true);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot>(str);

            //正确结果
            var pathResult = $"{FileDir}test_28_result2.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod25()
        {
            var rows = new List<int>() { 1, 2, 2, 2, 3 };
            var sources = new List<int>() { 2, 3, 3, 3, 3, 4 };
            var obj = rows.GetDiffList(sources, p => p, p => p, true);
            var str = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<DiffRoot2>(str);

            //正确结果
            var pathResult = $"{FileDir}test_29_result.json";
            var resultOk = JsonConvert.DeserializeObject<DiffRoot2>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod26()
        {
            var list = Enumerable.Range(1, 100).ToList();
            list.Add(100);

            var item = new List<int>();
            list.ForBatch(7, ns => { item.AddRange(ns); });

            var result = list.GetDiffList(item, p => p, p => p, false);
            var b = result.Repeats.Count == 0 && result.LeftDiffs.Count == 0 && result.RightDiffs.Count == 0;

            Assert.IsTrue(b);
        }

        public class RowOne
        {
            public string Ids { get; set; }
            public string Values { get; set; }
        }

        public class SourceOne
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
        }

        public class RowTwo
        {
            public Guid Id { get; set; }
            public int? Value { get; set; }
        }

        public class SourceTwo
        {
            public Guid Id { get; set; }
            public int? Value { get; set; }
        }

        public class RowThree
        {
            public List<int> Id { get; set; }
            public List<SourceThree> Value { get; set; }
        }

        public class SourceThree
        {
            public int Id { get; set; }
            public string Type { get; set; }
        }

        public class RowFour
        {
            public List<int> Id { get; set; }
            public List<string> Value { get; set; }
        }

        public class SourceFour
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public class Row
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class RowFive
        {
            public int Number { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class SourceFive
        {
            public int Number { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class LeftDiffsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
        }

        public class RightDiffsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
        }

        public class Object
        {
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
        }

        public class LeftsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
        }

        public class RightsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
        }

        public class RepeatsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Object Object { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<LeftsItem> Lefts { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RightsItem> Rights { get; set; }
        }

        public class DiffRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public List<LeftDiffsItem> LeftDiffs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RightDiffsItem> RightDiffs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RepeatsItem> Repeats { get; set; }
        }

        public class RepeatsItem2
        {
            /// <summary>
            /// 
            /// </summary>
            public int Object { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<int> Lefts { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<int> Rights { get; set; }
        }

        public class DiffRoot2
        {
            /// <summary>
            /// 
            /// </summary>
            public List<int> LeftDiffs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<int> RightDiffs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RepeatsItem2> Repeats { get; set; }
        }
    }
}

using DwrUtility.Lists;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DwrUtility.Test
{
    [TestClass]
    public class ListHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<Source>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new Source()
                {
                    Id = i,
                    Name = $"Name_{i}"
                });
            }

            var row = new List<Target>
            {
                new Target()
                {
                    Id = 2,
                },
                new Target()
                {
                    Id = 5,
                }
            };


            var result = list.Except(row, p => p.Id, p => p.Id).ToList();
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_41_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Source>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var list = new List<Source>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new Source()
                {
                    Id = i,
                    Name = $"Name_{i}"
                });
            }

            var row = new List<Target>
            {
                new Target()
                {
                    Id = 2,
                },
                new Target()
                {
                    Id = 5,
                }
            };

            row.Add(new Target()
            {
                Id = 2
            });

            var result = list.Except(row, p => p.Id, p => p.Id).ToList();
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_41_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Source>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var list = new List<Source>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new Source()
                {
                    Id = i,
                    Name = $"Name_{i}"
                });
            }

            var row = new List<Target>
            {
                new Target()
                {
                    Id = 2,
                },
                new Target()
                {
                    Id = 5,
                }
            };

            list.Add(new Source()
            {
                Id = 1,
                Name = $"Name_1"
            });
            list.Add(new Source()
            {
                Id = 2,
                Name = $"Name_2"
            });

            row.Add(new Target()
            {
                Id = 2
            });

            var result = list.Except(row, p => p.Id, p => p.Id).ToList();
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_42_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Source>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var list = new List<Source>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new Source()
                {
                    Id = i,
                    Name = $"Name_{i}"
                });
            }

            var row = new List<Target>
            {
                new Target()
                {
                    Id = 2,
                },
                new Target()
                {
                    Id = 5,
                }
            };

            list.Add(new Source()
            {
                Id = 1,
                Name = $"Name_1"
            });
            list.Add(new Source()
            {
                Id = 2,
                Name = $"Name_2"
            });

            list.Add(new Source()
            {
                Id = 1,
                Name = $"Name_11"
            });
            list.Add(new Source()
            {
                Id = 2,
                Name = $"Name_2"
            });

            row.Add(new Target()
            {
                Id = 2
            });

            var result = list.Except(row, p => p.Id, p => p.Id).ToList();
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_43_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Source>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var list = new List<Source>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new Source()
                {
                    Id = i,
                    Name = $"Name_{i}"
                });
            }

            var table = list.ToDataTable();
            Assert.IsTrue(table.Rows.Count == 10);


            var items = table.ToList<Source>();
            var eq = new CompareLogic().Compare(list, items);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod6()
        {
            var list = new List<Source2>();
            list.Add(new Source2()
            {
                Index = 1000,
                Sleep = 1000,
                Url = $"https://www.baidu.com/"
            });
            list.Add(new Source2()
            {
                Index = 10,
                Sleep = 10,
                Url = $"https://www.baidu.com/"
            });

            var sw = new Stopwatch();
            sw.Start();
            //
            list.ForeachAsync(p =>
            {
                Thread.Sleep(p.Sleep);
                p.Content = DateTime.Now.Ticks / 10000000;
            });

            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            Assert.IsTrue(ms >= 1000 && ms < 1500, $"使用时间：{ms}");

            var s = string.Join(";", list.OrderBy(p => p.Content).Select(p => p.Index));

            Assert.IsTrue(s.IsEquals("10;1000"), s);
        }

        public class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Target
        {
            public int Id { get; set; }
        }

        public class Source2
        {
            public int Index { get; set; }
            public int Sleep { get; set; }
            public string Url { get; set; }

            public long Content { get; set; }
        }
    }
}

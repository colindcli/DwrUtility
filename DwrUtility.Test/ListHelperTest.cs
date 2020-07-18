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

        /// <summary>
        /// 测试按某字段去重，取第一个记录对象
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            var list = new List<Source2>()
            {
                new Source2()
                {
                    Index = 1,
                    Sleep = 1,
                    //小写
                    Url = "http://public.cdn.viposs.com/code/jquery/1.10.2/jquery.min.js",
                    Content = 1,
                },
                new Source2()
                {
                    Index = 2,
                    Sleep = 2,
                    //与1同，但最后两个大些
                    Url = "http://public.cdn.viposs.com/code/jquery/1.10.2/jquery.min.JS",
                    Content = 2,
                },
                new Source2()
                {
                    Index = 3,
                    Sleep = 3,
                    //与1同，但最后两个大些
                    Url = "http://public.cdn.viposs.com/code/layui/2.5.6/layui.js",
                    Content = 3,
                }
            };

            var dict1 = list.ToDict(p => p.Url, p => p, true, StringComparer.OrdinalIgnoreCase);
            Assert.IsTrue(dict1.Count == 2);

            var dict2 = list.ToDict(p => p.Url, p => p, true, StringComparer.Ordinal);
            Assert.IsTrue(dict2.Count == 3);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var list = new List<Source>()
            {
                new Source()
                {
                    Id = 1,
                    Name = $"Name_1"
                },
                new Source()
                {
                    Id = 2,
                    Name = $"Name_2"
                },
            };

            var row = new List<Target2>
            {
                new Target2()
                {
                    Id = 2,
                    TypeName = "Type2"
                },
                new Target2()
                {
                    Id = 3,
                    TypeName = "Type3"
                }
            };

            var result = list.LeftJoin(row, p => p.Id, p => p.Id, (p, q) => new Result()
            {
                Id = p.Id,
                Name = p.Name,
                TypeName = q?.TypeName
            }).OrderBy(p => p.Id).ToList();

            var b1 = result.Count == 2;
            var b2 = result[0].Id == 1 && result[0].Name == "Name_1" && result[0].TypeName == null;
            var b3 = result[1].Id == 2 && result[1].Name == "Name_2" && result[1].TypeName == "Type2";
            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod]
        public void TestMethod8A()
        {
            var list = new List<int>()
            {
                1,2
            };

            var row = new List<int>
            {
                2,3
            };

            var result = list.LeftJoin(row, p => p, p => p, (p, q) => new Result()
            {
                Id = p
            }).OrderBy(p => p.Id).ToList();

            var b1 = result.Count == 2;
            var b2 = result[0].Id == 1;
            var b3 = result[1].Id == 2;
            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var list = new List<Source>()
            {
                new Source()
                {
                    Id = 1,
                    Name = $"Name_1"
                },
                new Source()
                {
                    Id = 2,
                    Name = $"Name_2"
                },
            };

            var row = new List<Target2>
            {
                new Target2()
                {
                    Id = 2,
                    TypeName = "Type2"
                },
                new Target2()
                {
                    Id = 3,
                    TypeName = "Type3"
                }
            };

            var result = list.FullJoin(row, p => p.Id, p => p.Id, (id, p, q) => new Result()
            {
                Id = id,
                Name = p?.Name,
                TypeName = q?.TypeName
            }).OrderBy(p => p.Id).ToList();

            var b1 = result.Count == 3;
            var b2 = result[0].Id == 1 && result[0].Name == "Name_1" && result[0].TypeName == null;
            var b3 = result[1].Id == 2 && result[1].Name == "Name_2" && result[1].TypeName == "Type2";
            var b4 = result[2].Id == 3 && result[2].Name == null && result[2].TypeName == "Type3";

            Assert.IsTrue(b1 && b2 && b3 && b4);
        }

        [TestMethod]
        public void TestMethod9A()
        {
            var list = new List<int>()
            {
                1,2
            };

            var row = new List<int>
            {
                2,3
            };

            var result = list.FullJoin(row, p => p, p => p, (id, p, q) => new Result()
            {
                Id = id,
            }).OrderBy(p => p.Id).ToList();

            var b1 = result.Count == 3;
            var b2 = result[0].Id == 1;
            var b3 = result[1].Id == 2;
            var b4 = result[2].Id == 3;

            Assert.IsTrue(b1 && b2 && b3 && b4);
        }

        [TestMethod]
        public void TestMethod9B()
        {
            var list = new List<int>()
            {
                1,2
            };

            var row = new List<Target2>
            {
                new Target2()
                {
                    Id = 2,
                    TypeName = "Type2"
                },
                new Target2()
                {
                    Id = 3,
                    TypeName = "Type3"
                }
            };

            var result = list.FullJoin(row, p => p, p => p.Id, (id, p, q) => new Result()
            {
                Id = id,
                TypeName = q?.TypeName
            }).OrderBy(p => p.Id).ToList();

            var b1 = result.Count == 3;
            var b2 = result[0].Id == 1 && result[0].TypeName == null;
            var b3 = result[1].Id == 2 && result[1].TypeName == "Type2";
            var b4 = result[2].Id == 3 && result[2].TypeName == "Type3";

            Assert.IsTrue(b1 && b2 && b3 && b4);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var list = new List<Source>
            {
                new Source()
                {
                    Name = "1;2;3"
                },
                new Source()
                {
                    Name = "2;3"
                },
                new Source()
                {
                    Name = "1;3"
                },
                new Source()
                {
                    Name = "4"
                }
            };

            var ids = new List<string>() { "1", "2", "3" };
            var res = list.IsContains(p => p.Name, ids);

            Assert.IsTrue(res.Count == 3);
        }

        [TestMethod]
        public void TestMethod11()
        {
            var list = new List<Source>();
            for (var i = 0; i < 2000; i++)
            {
                list.Add(new Source()
                {
                    Name = $"{i + 1};{i + 2};{i + 3}"
                });
            }

            var ids2 = new List<string>() { "10", "2", "38" };

            var sw = new Stopwatch();
            sw.Start();
            var res1 = ListHelper.IsContains(list, p => p.Name, ids2, new[] { ';' });
            sw.Stop();
            var ms1 = sw.ElapsedMilliseconds;

            Assert.IsTrue(res1.Count == 8);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var source = new List<Result>()
            {
                new Result()
                {
                    Id = 1,
                    Name = "lucy",
                    TypeName = "girl",
                },
                new Result()
                {
                    Id = 2,
                    Name = "tony",
                    TypeName = "Man",
                },
                new Result()
                {
                    Id = 3,
                    Name = "pony",
                    TypeName = "man",
                },
            };

            var target = new List<Result>()
            {
                new Result()
                {
                    Id = 1,
                    Name = "lucy",
                    TypeName = "girl",
                },
                new Result()
                {
                    Id = 2,
                    Name = "Tony",
                    TypeName = "Man",
                },
                new Result()
                {
                    Id = 3,
                    Name = "jack",
                    TypeName = "man",
                },
            };

            var result = source.Except(target, p => new { p.Id, Name = p.Name.ToLower(), p.TypeName }, p => new { p.Id, Name = p.Name.ToLower(), p.TypeName }).ToList();

            Assert.IsTrue(result.Count == 1 && result[0].Id == 3);
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

        public class Target2
        {
            public int Id { get; set; }
            public string TypeName { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string TypeName { get; set; }
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

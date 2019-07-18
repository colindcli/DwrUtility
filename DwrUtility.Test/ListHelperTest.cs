using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DwrUtility.Lists;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        public class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Target
        {
            public int Id { get; set; }
        }
    }
}

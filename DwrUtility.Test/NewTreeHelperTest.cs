using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using DwrUtility.Trees;

namespace DwrUtility.Test
{
    [TestClass]
    public class NewTreeHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_1.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var numberingDevices = new List<long>();
            for (var i = 100; i < 200; i++)
            {
                numberingDevices.Add(i);
            }

            TreeHelper.ToTreeNewId(new TreeIdParam<Team, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.Id,
                ParentIdField = p => p.ParentId,
            });

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_1_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }


        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_2.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var numberingDevices = new List<long>();
            for (var i = 100; i < 200; i++)
            {
                numberingDevices.Add(i);
            }

            TreeHelper.ToTreeNewId(new TreeIdParam<Team, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.Id,
                ParentIdField = p => p.ParentId,
            });

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_2_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_3.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var numberingDevices = new List<long>();
            for (var i = 100; i < 200; i++)
            {
                numberingDevices.Add(i);
            }

            TreeHelper.ToTreeNewId(new TreeIdParam<Team, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.Id,
                ParentIdField = p => p.ParentId,
            });

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_3_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            //待验证
            var path = $"{FileDir}test_7.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team2>>(json);

            var numberingDevices = new List<long>();
            for (var i = 100; i < 102; i++)
            {
                numberingDevices.Add(i);
            }

            var param = new TreeIdParam<Team2, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.TeamId,
                ParentIdField = p => p.ParentId,
            };
            TreeHelper.ToTreeNewId(param);

            var result = list.ToTreeView(p => p.TeamId, p => p.ParentId, p => p.Childs);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_7_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team2>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }
    }

    public class Team
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Name { get; set; }

        public List<Team> Childs { get; set; }
    }

    public class Team2
    {
        public long TeamId { get; set; }
        public long ParentId { get; set; }

        public List<Team2> Childs { get; set; }
    }
}

using DwrUtility.Trees;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class ParentChildTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_20.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetChildNodes(1, p => p.Id, p => p.ParentId, true);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_20_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_21.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetChildNodes(1, p => p.Id, p => p.ParentId, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_21_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_23.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetParentNodes(4, p => p.Id, p => p.ParentId, true);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_23_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            //待验证
            var path = $"{FileDir}test_24.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetParentNodes(4, p => p.Id, p => p.ParentId, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_24_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        public class Team
        {
            public int Id { get; set; }

            public int ParentId { get; set; }
            public string Name { get; set; }
        }
    }
}

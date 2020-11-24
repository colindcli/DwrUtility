using DwrUtility.Trees;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class DeleteNodeHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_12.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);

            list.DeleteNodes(new List<long>() { 6 }, p => p.Id, p => p.ParentId);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_12_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_13.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);

            list.DeleteNodes(new List<long>() { 2, 6 }, p => p.Id, p => p.ParentId);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_13_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }


        public class Team3
        {
            public long Id { get; set; }
            public long ParentId { get; set; }
            public string Name { get; set; }

            public List<Team3> Childs { get; set; }
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using DwrUtility.Lists;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;

namespace DwrUtility.Test
{
    [TestClass]
    public class PreviousNextTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetPrevious(0);
            
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetPrevious(1);

            Assert.IsTrue(result.Id == 1);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetNext(5);

            Assert.IsTrue(result.Id == 7);
        }

        [TestMethod]
        public void TestMethod6()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetNext(6);

            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void TestMethod7()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetPreviousList(2, 5);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_25_result1.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod8()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetNextList(3, 5);
            var s = JsonConvert.SerializeObject(result);
            
            //正确结果
            var pathResult = $"{FileDir}test_25_result2.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod9()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetPreviousList(2, 1);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_25_result3.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod10()
        {
            //待验证
            var path = $"{FileDir}test_25.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.GetNextList(3, 1);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_25_result4.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}

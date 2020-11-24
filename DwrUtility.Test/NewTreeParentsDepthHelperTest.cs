using DwrUtility.Trees;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class NewTreeParentsDepthHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_8.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);


            var param = new TreeParentsDepthParam<Team3, long>()
            {
                TreeLists = list,
                IdField = p => p.TeamId,
                ParentIdField = p => p.ParentId,
                ParentIdsField = p => p.ParentIds,
                DepthField = p => p.Depth,
            };
            TreeHelper.ToParentsDepth(param);

            var result = list.ToTreeView(p => p.TeamId, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_8_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_9.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);


            var param = new TreeParentsDepthParam<Team3, long>()
            {
                TreeLists = list,
                IdField = p => p.TeamId,
                ParentIdField = p => p.ParentId,
                ParentIdsField = p => p.ParentIds,
                DepthField = p => p.Depth,
                PrefixParentIds = ",0,500,",
                PrefixDepth = 2,
            };
            TreeHelper.ToParentsDepth(param);

            var result = list.ToTreeView(p => p.TeamId, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_9_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_10.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);


            var param = new TreeParentsDepthParam<Team3, long>()
            {
                TreeLists = list,
                IdField = p => p.TeamId,
                ParentIdField = p => p.ParentId,
                ParentIdsField = p => p.ParentIds,
                DepthField = p => p.Depth,
                PrefixParentIds = ",0,",
                PrefixDepth = 1,
            };
            TreeHelper.ToParentsDepth(param);

            var result = list.ToTreeView(p => p.TeamId, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_10_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            //待验证
            var path = $"{FileDir}test_11.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team3>>(json);


            var param = new TreeParentsDepthParam<Team3, long>()
            {
                TreeLists = list,
                IdField = p => p.TeamId,
                ParentIdField = p => p.ParentId,
                ParentIdsField = p => p.ParentIds,
                DepthField = p => p.Depth,
                PrefixParentIds = ";0;",
                PrefixDepth = 1,
                Split = ';'
            };
            TreeHelper.ToParentsDepth(param);

            var result = list.ToTreeView(p => p.TeamId, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_11_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team3>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }
    }

    public class Team3
    {
        public long TeamId { get; set; }
        public long ParentId { get; set; }
        public string ParentIds { get; set; }
        public int Depth { get; set; }

        public List<Team3> Childs { get; set; }
    }
}

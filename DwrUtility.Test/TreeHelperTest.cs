using DwrUtility.Trees;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
#if NETFULL
using System.Windows.Forms;
#endif

namespace DwrUtility.Test
{
    [TestClass]
    public class TreeHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_22.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_22_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod1A()
        {
            //待验证
            var path = $"{FileDir}test_22.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, true);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_22_resultA.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_26.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = TreeHelper.ToTreeList(list, p => p.Childs);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_26_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_27.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json).ToTreeList(p => p.Childs);

            var result = list.GetTopNodes(p => p.Id, p => p.ParentId);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_27_result1.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod4()
        {
            //待验证
            var path = $"{FileDir}test_27.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json).ToTreeList(p => p.Childs);

            var result = list.GetLeafNodes(p => p.Id, p => p.ParentId);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_27_result2.json";
            var resultOk = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var guids = TreeHelper.NewGuid(10);
            Assert.IsTrue(guids.Count == 10);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var guids = TreeHelper.NewGuidNull(10);
            Assert.IsTrue(guids.Count == 10);
        }

        [TestMethod]
        public void TestMethod7()
        {
            //待验证
            var path = $"{FileDir}test_40.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Team>>(json);

            var result = new List<int>();
            list.ForEachTreeView(p => p.Childs, p =>
            {
                result.Add(p.Id);
            });
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_40_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

#if NETFULL
        /// <summary>
        /// 未能实现 TODO
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            var ns = new[]
            {
                Cn(new UserControl()),
                Cn(new UserControl(), new []
                {
                    Cn(new UserControl())
                }),
            };

            //var list = ns.ToList().ToTreeList(p => p.Nodes).ToList();
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        private TreeNode Cn(UserControl uc, TreeNode[] ns = null)
        {
            var text = uc.Text;
            var name = uc.Name;
            var tn = ns == null ? new TreeNode(text) : new TreeNode(text, ns);
            tn.Name = name;
            tn.ExpandAll();
            return tn;
        }
#endif

        public class Team
        {
            public int Id { get; set; }

            public int ParentId { get; set; }
            public string Name { get; set; }
            public List<Team> Childs { get; set; }
        }
    }
}

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
    public class NewTreeTypeHelperTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            //待验证
            var path = $"{FileDir}test_4.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<TeamTwo>>(json);

            var numberingDevices = new List<Guid?>
            {
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab01"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab02"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab03"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab04"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab05"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab06"),
                Guid.Parse("4d50839f-6800-4c8e-80e8-6c104fb1ab07"),
            };

            var convertParam = new TreeTypeParam<TeamTwo, long, Guid?>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.Id,
                ParentIdField = p => p.ParentId,
                ToIdField = p => p.Guid,
                ToParentIdField = p => p.ParentGuid,
            };
            TreeHelper.ToTreeNewIdType(convertParam);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);


            //正确结果
            var pathResult = $"{FileDir}test_4_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<TeamTwo>>(File.ReadAllText(pathResult));


            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod2()
        {
            //待验证
            var path = $"{FileDir}test_5.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<TeamTwo>>(json);

            var numberingDevices = new List<long>
            {
                1,2,3,4,5,6,7
            };

            var convertParam = new TreeTypeParam<TeamTwo, Guid?, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.Guid,
                ParentIdField = p => p.ParentGuid,
                ToIdField = p => p.Id,
                ToParentIdField = p => p.ParentId,
            };
            TreeHelper.ToTreeNewIdType(convertParam);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_5_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<TeamTwo>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        [TestMethod]
        public void TestMethod3()
        {
            //待验证
            var path = $"{FileDir}test_6.json";
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<TemplateTask>>(json);

            //临时Id，负数
            var numberingDevices = new List<long>();
            for (var i = 0; i < list.Count; i++)
            {
                numberingDevices.Add(-(i + 1));
            }

            var convertParam = new TreeTypeParam<TemplateTask, Guid?, long>()
            {
                TreeLists = list,
                NewIds = numberingDevices,
                IdField = p => p.TemplateTaskGuid,
                ParentIdField = p => p.TemplateTaskParentGuid,
                ToIdField = p => p.Id,
                ToParentIdField = p => p.ParentId,
            };
            TreeHelper.ToTreeNewIdType(convertParam);

            var result = list.ToTreeView(p => p.Id, p => p.ParentId, p => p.Childs, false);
            var s = JsonConvert.SerializeObject(result);

            //正确结果
            var pathResult = $"{FileDir}test_6_result.json";
            var resultOk = JsonConvert.DeserializeObject<List<TemplateTask>>(File.ReadAllText(pathResult));

            var eq = new CompareLogic().Compare(result, resultOk);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }
    }

    public class TeamTwo
    {
        public long Id { get; set; }
        public long ParentId { get; set; }

        public Guid? Guid { get; set; }
        public Guid? ParentGuid { get; set; }
        public string Name { get; set; }

        public List<TeamTwo> Childs { get; set; }
    }

    public partial class TemplateTask
    {
        public List<TemplateTask> Childs { get; set; }

        public virtual Guid TemplateTaskGuid { get; set; }
        /// <summary>
        /// 模板Id
        /// </summary>
        public virtual Guid TemplateGuid { get; set; }
        /// <summary>
        /// 模板树父节点
        /// </summary>
        public virtual Guid? TemplateTaskParentGuid { get; set; }

        /// <summary>
        /// 任务的Id
        /// </summary>
        public long TaskItemId { get; set; }
        /// <summary>
        /// 任务Guid
        /// </summary>
        public Guid TaskItemGuid { get; set; }

        /// <summary>
        /// TeamId
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// TeamParentId
        /// </summary>
        public long ParentId { get; set; }
    }
}

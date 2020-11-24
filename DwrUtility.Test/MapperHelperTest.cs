using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;

namespace DwrUtility.Test
{
    [TestClass]
    public class MapperHelperTest
    {
        private static readonly string Root =
            Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/json/");

        [TestMethod]
        public void TestMethod1()
        {
            var source = new List<Source>()
            {
                new Source()
                {
                    Id = 1,
                    Name = "N1",
                    Price = 100
                },
                new Source()
                {
                    Id = 2,
                    Name = "N2",
                    Price = 200
                },
            };

            var result = MapperHelper.Mapper<Source, Target>(source);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Id == 1 && result[0].Name == "N1" && result[0].Number == null);
            Assert.IsTrue(result[1].Id == 2 && result[1].Name == "N2" && result[1].Number == null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var source = new Source()
            {
                Id = 1,
                Name = "N1",
                Price = 100
            };

            var result = MapperHelper.Mapper<Source, Target>(source);

            Assert.IsTrue(result.Id == 1 && result.Name == "N1" && result.Number == null);
        }


        [TestMethod]
        public void TestMethod3()
        {
            var pageLists = JsonConvert.DeserializeObject<List<D60>>(File.ReadAllText($"{Root}json_1.json"));
            var map = MapperHelper.Mapper<D60, D6>(pageLists);

            var json = JsonConvert.SerializeObject(map);

            var result = JsonConvert.DeserializeObject<List<D6>>(File.ReadAllText($"{Root}json_1_result.json"));
            var eq = new CompareLogic().Compare(map, result);
            Assert.IsTrue(eq.AreEqual, JsonConvert.SerializeObject(eq.Differences));
        }

        public class Source
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }
        }

        public class Target
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int? Number { get; set; }
        }

        /// <summary>
        /// PageUrlModel
        /// </summary>
        internal class D6
        {
            /// <summary>
            /// 父Guid
            /// </summary>
            //public Guid ParentGuid { get; set; }
            public Guid Guid => PageGuid;
            public int _id { get; set; }
            public virtual Guid PageGuid { get; set; }
            /// <summary>
            /// PageUrl
            /// </summary>
            public virtual string ObjectUrl { get; set; }
            /// <summary>
            /// ResponseUrl
            /// 请求后返回的Uri，和原网址之间的变化，相对页面图片、样式、脚本链接
            /// </summary>
            public virtual string ResUrl { get; set; }
            //public virtual DateTime CreateDate { get; set; }
            //
            public string TempFilePath { get; set; }
            public string TempFileName { get; set; }
            public virtual int HttpStatusCode { get; set; }
            public virtual string ContentType { get; set; }
            public virtual bool IsDown { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            /// <summary>
            /// 文件扩展名
            /// </summary>
            public string FileExtensionName { get; set; }
            /// <summary>
            /// 文件类型Fcte：0未知；1html；2图片；3文件；4默认是html，但可能是附件的，也可能是html；5在错误记录表找到的链接，标志为错误的链接；6Js；7Css；8Font；9Json; 10js提取的html链接
            /// </summary>
            public int FileCustomType { get; set; }
            public string Md5Value { get; set; }

            //页面里的链接
            public List<D7> PageHyperLinks { get; set; } = new List<D7>();

            public int Depth { get; set; }

            /// <summary>
            /// 非页面时添加html地址，一个素材来源多个html页面，只要一个
            /// </summary>
            public string PageUrl { get; set; }
        }

        /// <summary>
        /// PageUrlModel
        /// </summary>
        internal class D60
        {
            /// <summary>
            /// 父Guid
            /// </summary>
            //public Guid ParentGuid { get; set; }
            public Guid Guid => PageGuid;
            public int _id { get; set; }
            public virtual Guid PageGuid { get; set; }
            /// <summary>
            /// PageUrl
            /// </summary>
            public virtual string ObjectUrl { get; set; }
            /// <summary>
            /// ResponseUrl
            /// 请求后返回的Uri，和原网址之间的变化，相对页面图片、样式、脚本链接
            /// </summary>
            public virtual string ResUrl { get; set; }
            //public virtual DateTime CreateDate { get; set; }
            //
            public string TempFilePath { get; set; }
            public string TempFileName { get; set; }
            public virtual int HttpStatusCode { get; set; }
            public virtual string ContentType { get; set; }
            public virtual bool IsDown { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            /// <summary>
            /// 文件扩展名
            /// </summary>
            public string FileExtensionName { get; set; }
            /// <summary>
            /// 文件类型Fcte：0未知；1html；2图片；3文件；4默认是html，但可能是附件的，也可能是html；5在错误记录表找到的链接，标志为错误的链接；6Js；7Css；8Font；9Json; 10js提取的html链接
            /// </summary>
            public int FileCustomType { get; set; }
            public string Md5Value { get; set; }

            //页面里的链接
            public List<D7> PageHyperLinks { get; set; } = new List<D7>();

            public int Depth { get; set; }

            /// <summary>
            /// 非页面时添加html地址，一个素材来源多个html页面，只要一个
            /// </summary>
            public string PageUrl { get; set; }
        }

        /// <summary>
        /// PageHyperLinkModel
        /// </summary>
        internal class D7
        {
            /// <summary>
            /// 父Guid
            /// </summary>
            public Guid ParentGuid { get; set; }
            public int Id { get; set; }
            public virtual Guid PageHyperLinkGuid { get; set; }
            public virtual Guid PageGuid { get; set; }
            public string PageUrl { get; set; }
            /// <summary>
            /// 页面上的超链接
            /// </summary>
            public virtual string PageHyperLink { get; set; }
            /// <summary>
            /// 页面上的超链接转网址
            /// </summary>
            public virtual string PageHyperLinkToUrl { get; set; }
            public virtual DateTime CreateDate { get; set; }
            //public HtmlAttribute HtmlAttribute { get; set; }
            /// <summary>
            /// 是否用Replace方式替换链接
            /// </summary>
            public bool IsReplace { get; set; }

            public int Depth { get; set; }

            /// <summary>
            /// 1普通标签链接；2匹配链接
            /// </summary>
            public int PageType { get; set; }
        }
    }
}

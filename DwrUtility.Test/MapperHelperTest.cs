using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DwrUtility.Test
{
    [TestClass]
    public class MapperHelperTest
    {
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
    }
}

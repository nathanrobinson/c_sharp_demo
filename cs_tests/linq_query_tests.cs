using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace cs_tests
{
    [TestClass]
    public class linq_query_tests
    {
        public linq_query_tests()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        private readonly List<ObjectReference> _items = new List<ObjectReference> {
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 1", Value = 1 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 2", Value = 2 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 3", Value = 2 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 4", Value = 3 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 5", Value = 3 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 6", Value = 3 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 7", Value = 4 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 8", Value = 4 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 9", Value = 4 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 10", Value = 4 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 11", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 12", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 13", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 14", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 15", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 16", Value = 6 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 17", Value = 6 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 18", Value = 6 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 19", Value = 6 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 20", Value = 6 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 21", Value = 6 }
        };

        private readonly List<ObjectReference> _items2 = new List<ObjectReference> {
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 101", Value = 1 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 102", Value = 2 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 103", Value = 3 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 104", Value = 4 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 105", Value = 5 },
            new ObjectReference { Id = Guid.NewGuid(), Name = "Object 106", Value = 6 }
        };

        [TestMethod]
        public void linq_query()
        {
            var hasErrors1 = false;
            var errors1 = new List<KeyValuePair<int, Exception>>();
            var count = -1;
            foreach (var item in _items)
            {
                count++;
                if (item.Value < 6) continue;
                hasErrors1 = true;
                errors1.Add(new KeyValuePair<int, Exception>(count, new Exception("Value is too high")));
            }

            var errors2 = _items.Select((item, index) => new { item, index })
                                .Where(x => x.item.Value >= 6)
                                .Select(x => new KeyValuePair<int, Exception>(x.index, new Exception("Value is too high")))
                                .ToArray();

            var hasErrors2 = errors2.Any();

            hasErrors1.ShouldBeEquivalentTo(hasErrors2);
            errors1.ShouldAllBeEquivalentTo(errors2);

        }

        [TestMethod]
        public void linq_group_by()
        {
            var itemGroups = new Dictionary<int, List<ObjectReference>>();
            var group = new List<ObjectReference>();
            var processedGroups = new List<int>();
            foreach (var objectReference in _items)
            {
                if (!processedGroups.Contains(objectReference.Value))
                {
                    foreach (var reference in _items)
                    {
                        if (reference.Value == objectReference.Value)
                        {
                            group.Add(reference);
                        }
                    }
                    itemGroups.Add(objectReference.Value, group);
                    processedGroups.Add(objectReference.Value);
                    group = new List<ObjectReference>();
                }
            }

            var itemGroups2 = _items.GroupBy(x => x.Value).ToDictionary(x => x.Key);

            itemGroups.ShouldAllBeEquivalentTo(itemGroups2);
        }

        [TestMethod]
        public void linq_join()
        {
            var joined1 = new List<Tuple<ObjectReference, ObjectReference>>();
            foreach (var objectReference in _items)
            {
                foreach (var reference in _items2)
                {
                    if (reference.Value == objectReference.Value)
                    {
                        joined1.Add(Tuple.Create(objectReference, reference));
                    }
                }
            }


            var joined2 = _items.Join(_items2, item1 => item1.Value, item2 => item2.Value, Tuple.Create);

            joined1.ShouldAllBeEquivalentTo(joined2);
        }

        [TestMethod]
        public void linq_order_by()
        {
            var ordered = _items.ToArray();
            for (var i=0; i<ordered.Length; i++)
            {
                for (var j = i+1; j < ordered.Length; j++)
                {
                    if(ordered[i].Value > ordered[j].Value)
                    {
                        var temp = ordered[i];
                        ordered[i] = ordered[j];
                        ordered[j] = temp;
                    }
                }
            }

            var ordered2 = _items.OrderBy(x => x.Value);

            ordered.ShouldAllBeEquivalentTo(ordered2);
            Console.Write("nested foreach:");
            Console.WriteLine(JsonConvert.SerializeObject(ordered));
            Console.WriteLine("-------------------------------------------------------------");
            Console.Write("linq:");
            Console.WriteLine(JsonConvert.SerializeObject(ordered2));
        }

        [TestMethod]
        public void linq_where_filter()
        {
            var itemToFilter = _items.First().Clone();
            var filtered = _items.Where(x => x == itemToFilter);
            Assert.IsTrue(!filtered.Any());

            var filtered2 = _items.Where(x => x.Id == itemToFilter.Id);
            Assert.IsTrue(filtered2.Count() == 1);
        }
    }
}
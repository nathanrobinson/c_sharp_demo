using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cs_tests
{
    public class ObjectReference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public struct StructValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }

    [TestClass]
    public class object_struct_tests
    {
        [TestMethod]
        public void Reference_equals_null()
        {
            ObjectReference objectReference = null;

            Assert.IsTrue(ReferenceEquals(objectReference, null));
            Assert.IsTrue(objectReference == null);
        }
        
        [TestMethod]
        public void Struct_not_equals_null()
        {
            //StructValue structValue = null;
            StructValue structValue = new StructValue();

            Assert.IsFalse(ReferenceEquals(structValue, null));
        }
        
        [TestMethod]
        public void New_reference_not_equals_null()
        {
            ObjectReference objectReference = new ObjectReference();

            Assert.IsFalse(ReferenceEquals(objectReference, null));
            Assert.IsFalse(objectReference == null);
        }
        
        [TestMethod]
        public void New_reference_equals_copy()
        {
            ObjectReference objectReference = new ObjectReference();
            ObjectReference objectReference2 = objectReference;

            Assert.IsTrue(ReferenceEquals(objectReference, objectReference2));
            Assert.IsTrue(objectReference == objectReference2);
        }

        [TestMethod]
        public void String_reference_equals_copy()
        {
            string string1 = "Hello, World!";
            string string2 = string1;

            Assert.IsTrue(ReferenceEquals(string1, string2));
            Assert.IsTrue(string1 == string2);
        }

        [TestMethod]
        public void String_reference_does_not_update_copy()
        {
            string string1 = "Hello, World!";
            string string2 = string1;

            Assert.IsTrue(ReferenceEquals(string1, string2));
            Assert.IsTrue(string1 == string2);

            string2 = "Goodbye, World!";
            Assert.IsTrue(string1 != string2);
        }

        [TestMethod]
        public void Struct_not_equals_copy()
        {
            //StructValue structValue = null;
            StructValue structValue = new StructValue();
            StructValue structValue2 = structValue;

            Assert.IsFalse(ReferenceEquals(structValue, structValue2));
        }
        
        [TestMethod]
        public void New_reference_properties_equal_copy()
        {
            ObjectReference objectReference = new ObjectReference {
                Id = Guid.NewGuid(),
                Name = "New Reference",
                Value = 5
            };
            ObjectReference objectReference2 = objectReference;

            Assert.IsTrue(ReferenceEquals(objectReference, objectReference2));
            Assert.IsTrue(objectReference == objectReference2);
            Assert.IsTrue(objectReference.Id == objectReference2.Id);
            Assert.IsTrue(objectReference.Name == objectReference2.Name);
            Assert.IsTrue(objectReference.Value == objectReference2.Value);
        }

        [TestMethod]
        public void Struct_properties_equal_copy()
        {
            //StructValue structValue = null;
            StructValue structValue = new StructValue{
                Id = Guid.NewGuid(),
                Name = "New Struct",
                Value = 6
            };
            StructValue structValue2 = structValue;

            Assert.IsFalse(ReferenceEquals(structValue, structValue2));
            Assert.IsTrue(structValue.Id == structValue2.Id);
            Assert.IsTrue(structValue.Name == structValue2.Name);
            Assert.IsTrue(structValue.Value == structValue2.Value);
        }
        
        [TestMethod]
        public void References_update_each_others_properties()
        {
            ObjectReference objectReference = new ObjectReference {
                Id = Guid.NewGuid(),
                Name = "New Reference",
                Value = 5
            };
            ObjectReference objectReference2 = objectReference;

            Assert.IsTrue(ReferenceEquals(objectReference, objectReference2));
            Assert.IsTrue(objectReference == objectReference2);
            Assert.IsTrue(objectReference.Id == objectReference2.Id);
            Assert.IsTrue(objectReference.Name == objectReference2.Name);
            Assert.IsTrue(objectReference.Value == objectReference2.Value);

            objectReference2.Id = Guid.NewGuid();
            objectReference2.Name = "Object Reference 2";
            objectReference2.Value = 9;            

            Assert.IsTrue(objectReference == objectReference2);
            Assert.IsTrue(objectReference.Id == objectReference2.Id);
            Assert.IsTrue(objectReference.Name == objectReference2.Name);
            Assert.IsTrue(objectReference.Value == objectReference2.Value);
        }

        [TestMethod]
        public void Struct_copy_properties_do_not_update_each_other()
        {
            //StructValue structValue = null;
            StructValue structValue = new StructValue{
                Id = Guid.NewGuid(),
                Name = "New Struct",
                Value = 6
            };
            StructValue structValue2 = structValue;

            Assert.IsFalse(ReferenceEquals(structValue, structValue2));
            Assert.IsTrue(structValue.Id == structValue2.Id);
            Assert.IsTrue(structValue.Name == structValue2.Name);
            Assert.IsTrue(structValue.Value == structValue2.Value);

            structValue2.Id = Guid.NewGuid();
            structValue2.Name = "Struct Copy 2";
            structValue2.Value = 11;    
            
            Assert.IsTrue(structValue.Id != structValue2.Id);
            Assert.IsTrue(structValue.Name != structValue2.Name);
            Assert.IsTrue(structValue.Value != structValue2.Value);
        }

        [TestMethod]
        public void Seperate_reference_properties_do_not_update_each_other()
        {
            //StructValue structValue = null;
            ObjectReference objectReference = new ObjectReference
            {
                Id = Guid.NewGuid(),
                Name = "New Object",
                Value = 6
            };
            ObjectReference objectReference2 = objectReference.Clone();

            Assert.IsFalse(ReferenceEquals(objectReference, objectReference2));
            Assert.IsTrue(objectReference.Id == objectReference2.Id);
            Assert.IsTrue(objectReference.Name == objectReference2.Name);
            Assert.IsTrue(objectReference.Value == objectReference2.Value);

            objectReference2.Id = Guid.NewGuid();
            objectReference2.Name = "Object Copy 2";
            objectReference2.Value = 11;

            Assert.IsTrue(objectReference.Id != objectReference2.Id);
            Assert.IsTrue(objectReference.Name != objectReference2.Name);
            Assert.IsTrue(objectReference.Value != objectReference2.Value);
        }
    }
}

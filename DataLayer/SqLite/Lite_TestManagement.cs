using NUnit.Framework;
using SchoolGrades.BusinessObjects;
using System.Collections.Generic;
using System.Data.Common;

namespace SchoolGrades
{
    internal partial class SqLite_DataLayer : DataLayer
    {
        [SetUp]
        public void Setup()
        {
            Test_Commons.SetDataLayer();
        }
        [Test]
        internal override SchoolTest GetTestFromRow(DbDataReader Row)
        {
        }
        internal override SchoolTest GetTest(int? IdTest)
        {
            
        }
        internal override List<SchoolTest> GetTests()
        {
        }
        internal override void SaveTest(SchoolTest TestToSave)
        {
            
            
        }
    }
}

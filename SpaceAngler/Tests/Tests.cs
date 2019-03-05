using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{

    [TestFixture]
    public class Tests : TestsBase
    {
        [Test]
        public async Task FillerHandlesMultipleRoots()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            10      
            > 20    
            > > 50  
            > > 60  
            > 30    
            > > 70  
            > > 90  
            > 40    
            > > 80  
            ";
            var test = await Test(tree);
            await test.AssertAll();
        }

        [Test]
        public async Task InsertingInTreeWithOneNode()
        {
            var test = await Test("1");
            await test.ExecuteAsync($"INSERT INTO [{test.Table}] (Id, Parent_Id) VALUES (2, 1)");
            await test.AssertAll("1 > 2");
        }

        [Test]
        public async Task InsertingInEmptyTree()
        {
            var test = await Test("");
            await test.ExecuteAsync($"INSERT INTO [{test.Table}] (Id) VALUES (1)");
            await test.AssertAll("1");
        }

        [Test]
        public async Task DeleteLeaf()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ";
            var test = await Test(tree);
            await test.ExecuteAsync($"DELETE FROM [{test.Table}] WHERE Id = 5");
            await test.AssertAll(@"
            1       
            > 2     
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ");
        }

        [Test]
        public async Task DeleteRoot()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ";
            var test = await Test(tree);
            await test.ExecuteAsync($"DELETE FROM [{test.Table}] WHERE Id = 1");
            await test.AssertAll(@"");
        }

        [Test]
        public async Task DeleteMiddle()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ";
            var test = await Test(tree);
            await test.ExecuteAsync($"DELETE FROM [{test.Table}] WHERE Id = 2");
            await test.AssertAll(@"
            1       
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ");
        }


        [Test]
        public async Task DeleteMultiple()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            > 3     
            > > 7   
            > > 9   
            > 4     
            > > 8   
            ";
            var test = await Test(tree);
            await test.ExecuteAsync($"DELETE FROM [{test.Table}] WHERE Id > 3 AND Id <> 5");
            await test.AssertAll(@"
            1       
            > 2     
            > > 5   
            > 3    
            ");
        }
    }
}

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
        public async Task InsertingNewRoot()
        {
            var test = await Test("1");
            await test.ExecuteAsync($"INSERT INTO [{test.Table}] (Id) VALUES (2)");
            await test.AssertAll("1 2");
        }


        [Test]
        public async Task InsertingNewLeafOnNodeWithLeafsAlready()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"INSERT INTO [{test.Table}] (Id, Parent_Id) VALUES (10, 2)");
            await test.AssertAll(@"1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > > 10
            > 7
            > 8
            > > 9");
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


        [Test]
        public async Task DeleteOneOfManyRoots()
        {
            var tree = @"
            1       
            > 2     
            > > 5   
            > > 6   
            3     
            > 7   
            > > 9   
            > 4     
            > > 8   
            ";
            var test = await Test(tree);
            await test.ExecuteAsync($"DELETE FROM [{test.Table}] WHERE Id = 1");
            await test.AssertAll(@"
            3     
            > 7   
            > > 9   
            > 4     
            > > 8   
            ");
        }

        [Test]
        public async Task UpdateMakesALeafNewRoot()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = NULL WHERE Id = 6");
            await test.AssertAll(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > 7
            > 8
            > > 9
            6
            ");
        }

        [Test]
        public async Task UpdateMakesANonLeafNewRoot()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = NULL WHERE Id = 5");
            await test.AssertAll(@"
            1       
            > 2
            > > 4
            > 7
            > 8
            > > 9
            5
            > 3
            > 6            
            ");
        }

        [Test]
        public async Task UpdateMovesALeafLeft()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = 4 WHERE Id = 3");
            await test.AssertAll(@"
            1       
            > 2
            > > 4
            > > > 3
            > > 5
            > > > 6
            > 7
            > 8
            > > 9
            ");
        }

        [Test]
        public async Task UpdateMovesALeafRightAndDown()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = 6 WHERE Id = 4");
            await test.AssertAll(@"
            1       
            > 2
            > > 5
            > > > 3
            > > > 6
            > > > > 4
            > 7
            > 8
            > > 9
            ");
        }

        [Test]
        public async Task UpdateMovesASubTreeLeft()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = 1 WHERE Id = 5");
            await test.AssertAll(@"
            1       
            > 2
            > > 4
            > 7
            > 8
            > > 9
            > 5
            > > 3
            > > 6
            ");
        }

        public async Task UpdateMovesASubTreeRight()
        {
            var test = await Test(@"
            1       
            > 2
            > > 4
            > > 5
            > > > 3
            > > > 6
            > 7
            > 8
            > > 9
            ");
            await test.ExecuteAsync($"UPDATE [{test.Table}] SET Parent_Id = 9 WHERE Id = 5");
            await test.AssertAll(@"
            1       
            > 2
            > > 4
            > 7
            > 8
            > > 9
            > > > 5
            > > > > 3
            > > > > 6
            ");
        }
    }
}

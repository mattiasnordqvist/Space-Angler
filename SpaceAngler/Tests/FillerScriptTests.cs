using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{

    [TestFixture]
    public class FillerScriptTests : TestsBase
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
    }
}

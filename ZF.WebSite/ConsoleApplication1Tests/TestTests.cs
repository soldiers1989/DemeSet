using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FakeItEasy;
namespace ConsoleApplication1.Tests
{
    [TestClass( )]
    public class TestTests
    {
        [TestMethod( )]
        public void GetScoreTest( )
        {
            var answer = "a";
            var rightAnswer = "a,b,c";
           var score= Test.GetScore( answer,rightAnswer);
            decimal result = 0.5M;
            Assert.AreEqual( result,score);
        }

        [TestMethod()]
        public void TestAnswer( ) {
            var answer = new List<string>();
            answer.Add( "a");
            answer.Add( "b" );
            var rightAnswer = new List<string>( );
            rightAnswer.Add( "a");
            rightAnswer.Add( "b" );
            rightAnswer.Add( "c" );
            Assert.AreEqual( 2,Test.dosome( answer,rightAnswer));
        }

        public void TestFake( ) {
            var stu=A.Fake<Student>( );
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
   public class Test
    {
        public static decimal GetScore( string answer, string rightAnswer )
        {
            var answerArr = answer.Split( ',' );
            var rightCount = 0;
           
                for ( int j = 0; j < answerArr.Length; j++ )
                {
                    if ( rightAnswer.IndexOf(answerArr[j].ToString( ) ) >-1 )
                    {
                        rightCount += 1;

                        continue;
                    } else
                    {
                        return 0;
                    }
                }
            if ( rightCount > 0 )
            {
                return ( decimal )( rightCount * 0.5 );
            }
            return 0;
        }



        public static int dosome(List<string>one , List<string> two ) {
            var count = 0;
            var oneCount = one.Count;
            var twoCount = two.Count;
            for ( var i = 0; i < oneCount; i++ ) {
                for ( var j = 0; j < twoCount; j++ ) {
                    if ( one[i] == two[j] ) {
                        one.Remove( one[i] );
                        oneCount -= 1;
                        count += 1;
                        if ( oneCount == 0 ) {
                            return count;
                        }
                        continue;
                    }
                }
            }
            return count;
        }



    }
}

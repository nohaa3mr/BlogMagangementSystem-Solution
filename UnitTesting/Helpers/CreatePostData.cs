using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Helpers
{
    public static class CreatePostData
    {
        public static IEnumerable<object[]> InvalidPostData => new List<object[]>
        {
            new object[] {""} ,
            new object[] {"   "},
            new object[] {new string('a', 10001)}

        };
    }
}

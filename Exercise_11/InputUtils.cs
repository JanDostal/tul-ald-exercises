using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_11
{
    public static class InputUtils
    {
        public static async Task<string[]> ReadAll(TextReader reader) 
        {
            var input = await reader.ReadToEndAsync();

            var words = input.ToLower().Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
    }
}

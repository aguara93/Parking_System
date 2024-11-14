using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
  
    static class Helper
    {
        private static Random random = new Random();
        public static string GenerateRegNumber()
        {
            string letters = new string(Enumerable.Range(0, 3).Select(_ => (char)random.Next('A', 'Z')).ToArray());
            string numbers = new string(Enumerable.Range(0, 3).Select(_ => (char)random.Next('0', '9')).ToArray());
            return letters + numbers;
        }
    }
}

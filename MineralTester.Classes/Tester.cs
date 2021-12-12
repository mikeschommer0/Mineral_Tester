using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public class Tester
    {
        public string Name
        {
            get;
            set;
        }

        public float Hardness
        {
            get;
            set;
        }

        public Tester (string name, float hardness)
        {
            Name = name;
            Hardness = hardness;
        }

        public Tester()
        {

        }
    }
}

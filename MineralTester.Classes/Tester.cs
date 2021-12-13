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

        public Enums.TestType TestType
        {
            get;
            set;
        }

        public bool Magnet
        {
            get;
            set;
        }

        public bool Acid
        {
            get;
            set;
        }

        public Tester(string name, float hardness, Enums.TestType testType)
        {
            Name = name;
            Hardness = hardness;
            TestType = testType;
        }

        public Tester(Enums.TestType testType)
        {
            Magnet = true;
            Acid = true;
            TestType = testType;
        }


        public Tester()
        {

        }
    }
}

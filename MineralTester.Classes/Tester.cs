/// <summary>
/// Written by: Mike Schommer
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Tester class used in the mineral playground
    /// </summary>
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

        public string ImgSource
        {
            get;
            set;
        }

        public Tester(string name, float hardness, Enums.TestType testType, string imgsrc)
        {
            Name = name;
            Hardness = hardness;
            TestType = testType;
            ImgSource = imgsrc;
        }

        public Tester(Enums.TestType testType, string imgsrc)
        {
            Magnet = true;
            Acid = true;
            TestType = testType;
            ImgSource = imgsrc;
        }


        public Tester()
        {

        }
    }
}

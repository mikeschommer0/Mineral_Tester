/// <summary>
/// Written by: Mike Schommer
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Mineral class.
    /// </summary>
    public class Mineral
    {
        private int _ID;
        private string _Hidden;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

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

        public bool IsMagnetic
        {
            get;
            set;
        }

        public bool AcidReaction
        {
            get;
            set;
        }

        public byte[] Image
        {
            get;
            set;
        }

        public string Hidden
        {
            get
            {
                return _Hidden;
            }
            set
            {
                _Hidden = "";
            }
        }

        public string StreakColor
        {
            get;
            set;
        }

        public Mineral()
        {

        }

        public Mineral(string name, float hardness, bool isMagnetic, bool acidReaction, byte[] image, string streakColor)
        {

        }
        public Mineral(int id, string name, float hardness, bool isMagnetic, bool acidReaction, byte[] image, string streakColor)
        {
            _ID = id;
            Name = name;
            Hardness = hardness;
            IsMagnetic = isMagnetic;
            AcidReaction = acidReaction;
            Image = image;
            Hidden = _Hidden;
            StreakColor = streakColor;
        }

    }
}

namespace MineralTester.Classes
{
    public class Mineral
    {
        private int _ID;

        public int ID
        {
            get 
            { 
                return _ID; 
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

        public string Image
        {
            get;
            set;
        }

        public Mineral(int id, string name, float hardness, bool isMagnetic, bool acidReaction, string image) // Image object cannot be used as a parameter as per our class diagram.
        {
            _ID = id;
            Name = name;
            Hardness = hardness;
            IsMagnetic = isMagnetic;
            AcidReaction = acidReaction;
            Image = image;
        }

    }
}

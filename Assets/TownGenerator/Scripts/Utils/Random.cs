namespace Utils
{
    public class Random : System.Random
    {
        int seed;
        public Random (int Seed) : base (Seed)
        {
            seed = Seed;
        }

        public int Range (int min, int max)
        {
            return Next (min, max);
        }

        public float Range (float min, float max)
        {
            return (float) NextDouble () * max + min;
        }

        public bool Boolean (float min)
        {
            return NextDouble () < min;
        }

        public float value
        {
            get
            {
                return (float) NextDouble ();
            }
        }

        public bool boolean
        {
            get
            {
                return NextDouble () < 0.5;
            }
        }
    }
}
using ProjectWilson.Lookups;

namespace ProjectWilson.Actions
{
    public class AttackContext
    {
        public string Type;
        public Side Side;
        public int Number;
        public float Range;
        public int Amount;

        public AttackContext(string type, Side side, int number = -1, float range = 3.0f, int amount = 50)
        {
            Type = type;
            Side = side;
            Number = number;
            Range = range;
            Amount = amount;
        }
    }
}
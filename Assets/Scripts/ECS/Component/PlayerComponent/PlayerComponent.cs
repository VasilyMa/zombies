namespace Client 
{
    struct PlayerComponent
    {
        public int Money;
        public int Experience;
        public int Level;

        public bool TrySpendMoney(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                return true;
            }
            return false;
        }
    }
}

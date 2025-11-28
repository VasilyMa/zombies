namespace Client 
{
    struct PlayerComponent
    {
        public int Money;
        public int Experience; 
        public int NextLevelExperience; 
        public int Level;

        public bool TrySpendMoney(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                ObserverEntity.instance.ResourcesChange(Money);
                return true;
            }
            return false;
        }

        public void AddMoney(int money)
        {
            Money += money;

            ObserverEntity.instance.ResourcesChange(Money);
        }

        public void AddNeedeExperience(int needed)
        {
            NextLevelExperience = needed;

            float value = (float)Experience / NextLevelExperience;

            ObserverEntity.instance.ExperienceChange(value);
        }

        public void AddExperience(int experience)
        {
            Experience += experience;

            float value = (float)Experience / NextLevelExperience;

            ObserverEntity.instance.ExperienceChange(value);
        }

        public void AddLevel(int level)
        {
            Level += level;

            ObserverEntity.instance.LevelChange(Level);
        }
    }
}

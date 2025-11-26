using System.Collections.Generic;

namespace Client 
{
    struct MovementComponent
    {
        public float BaseMaxValue;
        public float CurrentValue;

        // Единый список модификаторов (например, бафы/дебафы/урон/лечение)
        public List<float> Modifiers;

        /// <summary>
        /// Инициализация компонента передвижение.
        /// </summary>
        public void Init(float baseMaxValue)
        {
            BaseMaxValue = baseMaxValue;
            CurrentValue = baseMaxValue;

            if (Modifiers == null)
                Modifiers = new List<float>();
            else
                Modifiers.Clear();
        }

        /// <summary>
        /// Получить итоговый максимальный move с учетом модификаторов (мультипликативно).
        /// </summary>
        public float MaxValue
        {
            get
            {
                float result = BaseMaxValue;

                if (Modifiers != null)
                {
                    foreach (var mod in Modifiers)
                        result *= (1f + mod); // каждый модификатор — коэффициент
                }

                return result > 0 ? result : 0;
            }
        }

        /// <summary>
        /// Добавить модификатор (положительный или отрицательный).
        /// </summary>
        public void AddModifier(float modifier)
        {
            if (Modifiers == null)
                Modifiers = new List<float>();
            Modifiers.Add(modifier);

            ClampCurrentValue();
        }

        /// <summary>
        /// Удалить модификатор (по значению, только первое вхождение).
        /// </summary>
        public void RemoveModifier(float modifier)
        {
            if (Modifiers != null)
            {
                Modifiers.Remove(modifier);

                ClampCurrentValue();
            }
        }

        /// <summary>
        /// Ограничить текущее здоровье диапазоном [0, MaxValue].
        /// </summary>
        private void ClampCurrentValue()
        {
            float max = MaxValue;
            if (CurrentValue > max)
                CurrentValue = max;
            if (CurrentValue < 0)
                CurrentValue = 0;
        }

        /// <summary>
        /// Сбросить все модификаторы.
        /// </summary>
        public void ClearModifiers()
        {
            Modifiers?.Clear();
            ClampCurrentValue();
        }
    }
}

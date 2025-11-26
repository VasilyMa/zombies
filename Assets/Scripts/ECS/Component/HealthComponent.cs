using System.Collections.Generic;

namespace Client 
{
    /// <summary>
    /// Компонент здоровья с поддержкой единого списка модификаторов (бафы/дебафы).
    /// Модификаторы могут быть положительными (лечение/баф) или отрицательными (урон/дебаф).
    /// </summary>
    struct HealthComponent 
    {
        public float BaseMaxValue; // Базовое максимальное здоровье
        public float CurrentValue; // Текущее здоровье

        // Единый список модификаторов (например, бафы/дебафы/урон/лечение)
        public List<float> Modifiers;

        /// <summary>
        /// Инициализация компонента здоровья.
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
        public void Sub(float value)
        {
            CurrentValue -= value;
        }

        public void Add(float value)
        {
            CurrentValue += value;

            ClampCurrentValue();
        }

        /// <summary>
        /// Получить итоговое максимальное здоровье с учетом модификаторов.
        /// </summary>
        public float MaxValue
        {
            get
            {
                float result = BaseMaxValue;
                if (Modifiers != null)
                {
                    foreach (var mod in Modifiers)
                        result += mod;
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

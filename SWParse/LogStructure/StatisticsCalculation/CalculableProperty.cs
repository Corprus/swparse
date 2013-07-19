using System;

namespace SWParse.LogStructure.StatisticsCalculation
{
    public interface ICalculable
    {
        void Calculate();
    }

    public interface ICalculable<T>: ICalculable
    {
        T Value { get; }
    }


    internal class CalculableProperty<T>: ICalculable<T>
    {
        private readonly Func<T> _calculator;
        private T _value;
        public T Value
        {
            get
            {
                if (!_calculated)
                {
                    Calculate();
                }
                return _value;
            }
            private set { _value = value; }
        }

        public bool Calculated
        {
            get { return _calculated; }
        }

        private bool _calculated = false;

        public CalculableProperty(Func<T> calculator)
        {
            _calculator = calculator;
        }

        public void Calculate()
        {
            Value = _calculator();
            _calculated = true;
        }
    }
}

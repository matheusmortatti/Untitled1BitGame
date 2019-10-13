using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Xna.Framework;

namespace SharedCode.Misc
{
    public class StateMachine<T> where T : struct, IConvertible
    {
        public T State { get; private set; }

        private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private IDictionary<T, MethodInfo> states = new Dictionary<T, MethodInfo>();
        private IDictionary<T, MethodInfo> transitions = new Dictionary<T, MethodInfo>();

        public StateMachine(T init)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumeration");
            }

            // Cache state and transition functions
            foreach (T value in typeof(T).GetEnumValues())
            {
                var s = GetType().GetMethod(value.ToString() + "State", FLAGS);
                if (s != null)
                {
                    states.Add(value, s);
                }

                var t = GetType().GetMethod(value.ToString() + "Init", FLAGS);
                if (t != null)
                {
                    transitions.Add(value, t);
                }
            }

            State = init;
        }

        public void Init(T next)
        {
            MethodInfo method;
            if (transitions.TryGetValue(next, out method))
            {
                method.Invoke(this, new object[] { State });
            }

            State = next;
        }

        public void StateDo(GameTime gameTime)
        {
            MethodInfo method;
            if (states.TryGetValue(State, out method))
            {
                method.Invoke(this, new object[] { gameTime });
            }
        }
    }
}

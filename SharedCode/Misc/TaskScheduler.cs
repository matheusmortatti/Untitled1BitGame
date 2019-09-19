using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Misc
{
    public static class TaskScheduler
    {
        protected struct Task
        {
            public Action func;
            public double timePassed;
            public double triggerTime;
            public bool done;
            public bool isRecurrent;
        }
        private static List<Task> currentTasks = new List<Task>();
        private static List<Task> nextTasks = new List<Task>();

        public static void Update(GameTime gameTime)
        {

            for (int i = currentTasks.Count - 1; i >= 0; --i)
            {
                var temp = new Task
                {
                    func = currentTasks[i].func,
                    timePassed = currentTasks[i].timePassed,
                    triggerTime = currentTasks[i].triggerTime,
                    done = currentTasks[i].done,
                    isRecurrent = currentTasks[i].isRecurrent
                };

                temp.timePassed += gameTime.ElapsedGameTime.TotalSeconds;
                if (temp.timePassed > temp.triggerTime)
                {
                    temp.func?.Invoke();

                    if (temp.isRecurrent)
                    {
                        temp.timePassed = 0;
                    }
                    else
                    {
                        temp.done = true;
                    }
                }


                if (temp.done) currentTasks.RemoveAt(i);
                else currentTasks[i] = temp;
            }

            currentTasks.AddRange(nextTasks);
            nextTasks.Clear();
        }

        public static void AddTask(Action task, double time, bool isRecurrent)
        {
            nextTasks.Add(new Task() { func = task, timePassed = 0, triggerTime = time, done = false, isRecurrent = isRecurrent });
        }
    }
}

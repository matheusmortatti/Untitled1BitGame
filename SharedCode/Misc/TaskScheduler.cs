﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Misc
{
    public static class TaskScheduler
    {
        public class Task
        {
            public Action func;
            public double timePassed;
            public double triggerTime;
            public double stopAfter;
            public double lifeTime;
            public bool done;

            // Id that refers to the game object that created it.
            public int id;
        }
        private static List<Task> currentTasks = new List<Task>();
        private static List<Task> nextTasks = new List<Task>();

        public static int numberOfTasks
        {
            get
            {
                return currentTasks.Count;
            }
        }

        public static void Update(GameTime gameTime)
        {

            for (int i = currentTasks.Count - 1; i >= 0; --i)
            {
                currentTasks[i].timePassed += gameTime.ElapsedGameTime.TotalSeconds;
                currentTasks[i].lifeTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTasks[i].timePassed > currentTasks[i].triggerTime)
                {
                    currentTasks[i].func?.Invoke();
                    currentTasks[i].timePassed = 0;
                }

                if (currentTasks[i].stopAfter > 0 && currentTasks[i].lifeTime > currentTasks[i].stopAfter)
                {
                    currentTasks[i].done = true;
                }
                
                if (currentTasks[i].done) currentTasks.RemoveAt(i);
            }

            currentTasks.AddRange(nextTasks);
            nextTasks.Clear();
        }

        public static Task AddTask(Action task, double triggerEvery, double stopAfter, int id)
        {
            var t = new Task()
            {
                func = task, timePassed = 0,
                triggerTime = triggerEvery,
                done = false,
                stopAfter = stopAfter,
                lifeTime = 0,
                id = id
            };

            nextTasks.Add(t);
            return t;
        }

        public static Task RemoveTask(Task task)
        {
            if (task != null && currentTasks.Contains(task))
                currentTasks.Remove(task);
            return task;
        }

        public static void RemoveTasksUnderId(int id)
        {
            for (int i = currentTasks.Count - 1; i >= 0; i -= 1)
            {
                if (currentTasks[i].id == id)
                {
                    currentTasks.RemoveAt(i);
                }
            }
        }
    }
}

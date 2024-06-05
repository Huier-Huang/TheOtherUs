using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheOtherUs.Modules;

public class TaskQueue
{
    private static List<TaskQueue> queues = [];
    public static TaskQueue GetOrCreate(int Count)
    {
        if (queues.Count < Count)
        {
            queues.Add(new TaskQueue());
        }
        return queues[Count - 1];
    }
    
    
    public Task CurrentTask;
    public Queue<Task> Tasks = [];

    public bool TaskStarting;

    private Dictionary<Task, Action> TaskOnCompleted = new();
    public TaskQueue StartTask(Action action, string Id, Action OnCompleted = null)
    {
        var task = new Task(() =>
        {
            Info($"Start TaskQueue Id:{Id}");
            try
            {
                action();
            }
            catch (Exception e)
            {
                Exception(e);
                Error($"加载失败 TaskQueue Id:{Id}");
            }
        });
        if (OnCompleted != null)
            TaskOnCompleted[task] = OnCompleted;
        Tasks.Enqueue(task);

        if (!TaskStarting) 
            StartNew();
        return this;
    }
    
    public void StartNew()
    {
        if (!Tasks.Any() || TaskStarting) return;
        TaskStarting = true;
        Task.Run(() =>
            {
                Start();
                TaskStarting = false;
            }
        );
        return;

        void Start()
        {
            if (!Tasks.Any()) return;
            CurrentTask = Tasks.Dequeue();
            CurrentTask.Start();
            CurrentTask.GetAwaiter().OnCompleted(() =>
            {
                if (TaskOnCompleted.TryGetValue(CurrentTask, out var ac))
                {
                    ac();
                    TaskOnCompleted.Remove(CurrentTask);
                }
                Start();
            });
        }
    }
}
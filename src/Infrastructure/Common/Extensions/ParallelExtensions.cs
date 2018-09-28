namespace Skeleton.Common.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ParallelExtensions
    {
        public static IEnumerable<Task<TResult>> ForEachAsync<TItem, TResult>(
            this IEnumerable<TItem> source,
            Func<TItem, TResult> body,
            int degreeOfParallelism)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            var sourceArray = source.ToArray();
            var completedTask = -1;

            var taskCompletions = new TaskCompletionSource<TResult>[sourceArray.Length];
            for (var i = 0; i < taskCompletions.Length; i++)
                taskCompletions[i] = new TaskCompletionSource<TResult>();

            foreach (var partition in Partitioner.Create(sourceArray).GetPartitions(degreeOfParallelism))
                Task.Run(
                    () =>
                    {
                        while (partition.MoveNext())
                        {
                            var result = body(partition.Current);

                            var finishedTaskIndex = Interlocked.Increment(ref completedTask);
                            taskCompletions[finishedTaskIndex].SetResult(result);
                        }
                    }
                );

            return taskCompletions.Select(tcs => tcs.Task);
        }
    }
}
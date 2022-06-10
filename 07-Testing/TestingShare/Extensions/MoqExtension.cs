using Moq.Language.Flow;

namespace TestingShare
{
	public static class MoqExtension
    {
        //
        //  ISetup
        //

        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params TResult[] results)
            where T : class
        {
            var q = new Queue<TResult>(results);
            setup.Returns(q.Dequeue);
        }

        public static void ReturnsInOrderTaskResult<T, TResult>(this ISetup<T, Task<TResult>> setup, params TResult[] results)
            where T : class
        {
            var q = new Queue<Task<TResult>>();
            if (results.Any())
            {
                foreach (var result in results)
                    q.Enqueue(Task.FromResult(result));
            }

            setup.Returns(q.Dequeue);
        }

        //
        //  IReturnsThrows
        //

        public static void ReturnsInOrder<T, TResult>(this IReturnsThrows<T, TResult> setup, params TResult[] results)
            where T : class
        {
            var q = new Queue<TResult>(results);
            setup.Returns(q.Dequeue);
        }

        public static void ReturnsInOrderTaskResult<T, TResult>(this IReturnsThrows<T, Task<TResult>> setup, params TResult[] results)
           where T : class
        {
            var q = new Queue<Task<TResult>>();
            if (results.Any())
            {
                foreach (var result in results)
                    q.Enqueue(Task.FromResult(result));
            }

            setup.Returns(q.Dequeue);
        }
    }
}

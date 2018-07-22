namespace Bars.NuGet.Querying
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using global::Bars.Linq.Async;
    using global::Bars.NuGet.Querying.Feed;

    public class NuGetFeed : IAsyncQueryable<NuGetPackage>
    {
        public NuGetFeed(params string[] feeds)
        {
            Expression = Expression.Constant(this);
            AsyncProvider = new NuGetFeedQueryProvider(feeds, Expression);
            Provider = AsyncProvider;
        }

        internal NuGetFeed(IQueryProvider provider, Expression expression)
        {
            var asyncQueryProvider = provider as IAsyncQueryProvider<NuGetPackage>;
            Provider = provider;
            AsyncProvider = asyncQueryProvider;
            Expression = expression;
        }

        public IEnumerator<NuGetPackage> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<NuGetPackage>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        public IAsyncEnumerator<NuGetPackage> GetAsyncEnumerator()
        {
            return AsyncProvider.AsyncExecute(Expression).GetAsyncEnumerator();
        }

        public Type ElementType { get { return typeof(NuGetPackage); } }

        public Expression Expression { get; private set; }

        public IQueryProvider Provider { get; private set; }

        public IAsyncQueryProvider<NuGetPackage> AsyncProvider { get; private set; }
    }
}
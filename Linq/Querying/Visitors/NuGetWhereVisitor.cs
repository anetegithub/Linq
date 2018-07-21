﻿namespace Bars.NuGet.Querying.Visitors
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Bars.NuGet.Querying.Types;

    internal class NuGetWhereVisitor : NuGetVisitor
    {
        internal NuGetWhereVisitor(NuGetQueryFilter nuGetQueryFilter) : base(nuGetQueryFilter)
        {
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var body = node.Body;
            if (body.NodeType == ExpressionType.Equal)
            {
                new NuGetEqualsVisitor(this.nuGetQueryFilter).Visit(body);
            }

            if (body.NodeType == ExpressionType.Call)
            {
                new NuGetCallVisitor(this.nuGetQueryFilter).Visit(body);
            }

            return Expression.Lambda<Func<NuGetPackage, bool>>(Expression.Constant(true), node.Parameters);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var args = node.Arguments;
            this.Visit(args.Last());
            return args.First();
        }
    }
}
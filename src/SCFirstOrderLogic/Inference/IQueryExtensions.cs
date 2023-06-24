﻿// Copyright (c) 2021-2023 Simon Condon.
// You may use this file in accordance with the terms of the MIT license.
using System.Runtime.CompilerServices;

namespace SCFirstOrderLogic.Inference
{
    /// <summary>
    /// Helpful extension methods for <see cref="IQuery"/> instances.
    /// </summary>
    public static class IQueryExtensions
    {
        /// <summary>
        /// Gets an awaiter used to await a given <see cref="IQuery"/>
        /// </summary>
        /// <param name="query">The query to get an awaiter for.</param>
        /// <returns>An awaiter instance.</returns>
        public static TaskAwaiter<bool> GetAwaiter(this IQuery query)
        {
            return query.ExecuteAsync().GetAwaiter();
        }

        /// <summary>
        /// Executes a query to completion.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the query.</returns>
        public static bool Execute(this IQuery query)
        {
            return query.ExecuteAsync().GetAwaiter().GetResult();
        }
    }
}

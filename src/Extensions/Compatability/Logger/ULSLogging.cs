﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Microsoft.Omex.Extensions.Compatability.Logger
{
	/// <summary>
	/// Unified logging, allows registering for logging events sent by
	/// code implementations and to raise logging events
	/// </summary>
	public static class ULSLogging
	{
		/// <summary>
		/// Log a trace
		/// </summary>
		/// <param name="eventId">tagid (uniqueid) of the trace</param>
		/// <param name="category">logging category</param>
		/// <param name="level">logging level</param>
		/// <param name="message">message to log</param>
		/// <param name="parameters">additional parameters</param>
		[Obsolete(ObsoleteMessage, IsObsoleteError)]
		public static void LogTraceTag(EventId eventId, Category category, LogLevel level, string message, params object[] parameters) =>
			GetLogger(category)?.Log(level, eventId, message, parameters);


		/// <summary>
		/// Report an exception
		/// </summary>
		/// <param name="eventId">tag</param>
		/// <param name="category">category</param>
		/// <param name="exception">exception</param>
		/// <param name="message">message</param>
		/// <param name="parameters">message format parameters</param>
		[Obsolete(ObsoleteMessage, IsObsoleteError)]
		public static void ReportExceptionTag(EventId eventId, Category category, Exception exception, string message, params object[] parameters) =>
			GetLogger(category)?.LogError(eventId, exception, message, parameters);


		internal static void Initialize(ILoggerFactory loggerFactory)
		{
			s_factory = loggerFactory;
			s_loggersDictionary = new ConcurrentDictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);
		}


		private static ILogger? GetLogger(Category category) =>
			s_loggersDictionary != null && s_factory != null
			? s_loggersDictionary.GetOrAdd(category.Name, s_factory.CreateLogger)
			: throw new OmexCompatabilityInitializationException();


		private static ILoggerFactory? s_factory;
		private static ConcurrentDictionary<string, ILogger>? s_loggersDictionary;
		private const string ObsoleteMessage = "Please consider using ILogger instead";
		private const bool IsObsoleteError = false;
	}
}

﻿using FluentValidation.Results;

namespace RommanelTeste.Application.Common.Exceptions;

public class ValidationException : Exception
{
	public ValidationException() : base("One or more validation failures have occurred.")
	{
		Failures = new Dictionary<string, string>();
	}

	public ValidationException(IEnumerable<ValidationFailure> failures) : this()
	{
		var propertyNames = failures
			.Select(e => e.PropertyName)
			.Distinct();

		foreach (var propertyName in propertyNames)
		{
			var propertyFailures = failures
				.Where(e => e.PropertyName == propertyName)
				.Select(e => e.ErrorMessage)
				.ToArray();

			foreach (var propertyFailure in propertyFailures)
				Failures.Add(propertyName, propertyFailure);

		}
	}

	public IDictionary<string, string> Failures { get; }
}

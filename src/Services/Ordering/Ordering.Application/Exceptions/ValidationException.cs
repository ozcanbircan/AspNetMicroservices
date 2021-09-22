using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions {
	public class ValidationException : System.ApplicationException {
		public ValidationException() : base("One or more validation failures have occured.") {
			Errors = new Dictionary<string, string[]>();
		}

		public ValidationException(IEnumerable<ValidationFailure> failures) : this() {
			Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage).ToDictionary(g => g.Key, g => g.ToArray());
		}

		public Dictionary<string, string[]> Errors { get; }
	}
}
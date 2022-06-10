using System.Runtime.Serialization;

namespace DomainExceptions
{
	public class ObjectDoesNotExistException : Exception
	{
		public ObjectDoesNotExistException() { }
		public ObjectDoesNotExistException(string? message) : base(message) { }
		public ObjectDoesNotExistException(string? message, Exception? innerException) : base(message, innerException) { }
		protected ObjectDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
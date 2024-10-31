namespace Codebridge.TechnicalTask.Domain.Common.Constants;

public static class DomainErrorCodes
{
    public static class Dog
    {
        public static class Validation
        {
            public const string InvalidNameLength = "Dog.InvalidNameLength";
            public const string InvalidColorLength = "Dog.InvalidColorLength";
            public const string InvalidTailLength = "Dog.InvalidTailLength";
            public const string InvalidWeight = "Dog.InvalidWeight";
            public const string InvalidSortProperty = "Dog.InvalidSortProperty";
        }

        public static class Common
        {
            public const string NotFound = "Dog.NotFound";
        }

        public static class Conflict
        {
            public const string NameExists = "Dog.NameExists";
        }
    }
}
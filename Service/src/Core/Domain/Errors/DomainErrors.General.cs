using Domain.Primitives;

namespace Domain.Errors;

public static partial class DomainErrors
{
    public static class General
    {
        public static Error RecordNotFound(long? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return new Error("Record.NotFound", $"Record not found{forId}.");
        }

        public static Error RecordDuplicate(long? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return new Error("Record.Duplicate", $"Duplicate record found{forId}.");
        }

        public static Error ValueIsInvalid(string? name = null, string? value = null)
        {
            var forName = name == null ? "Value" : $"For {name} value";
            var forValue = value == null ? " " : $" '{value}' ";
            return new Error("Value.Invalid", $"{forName}{forValue}is invalid.");
        }

        public static Error ValueIsRequired()
        {
            return new Error("Value.Required", "Value is required.");
        }

        public static Error InvalidLength(string? name = null)
        {
            var label = name == null ? " " : $" {name} ";
            return new Error("String.InvalidLength", $"Invalid{label}length.");
        }

        public static Error CollectionIsTooSmall(int min, int current)
        {
            return new Error(
                "Collection.TooSmall",
                $"The collection must contain {min} items or more. It contains {current} items.");
        }

        public static Error CollectionIsTooLarge(int max, int current)
        {
            return new Error(
                "Collection.TooLarge",
                $"The collection must contain {max} items or more. It contains {current} items.");
        }

        public static Error InternalServerError(string message)
        {
            return new Error("Server.InternalError", message);
        }


        public static Error MultiError(params Error[] args)
        {
            var message = args.Select(e => e.Message).Aggregate((a, b) =>
                $"{a}{Environment.NewLine}{b}");
            return new Error("General.MultiError", message);
        }
    }
}
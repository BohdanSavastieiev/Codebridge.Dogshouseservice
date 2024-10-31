using Codebridge.TechnicalTask.Domain.Shared.Models;

namespace Codebridge.TechnicalTask.Domain.Common.Constants;

public static class DomainErrors
 {
     public static class Dog
     {
         public static Error NotFound =>
             Error.NotFound(
                 code: DomainErrorCodes.Dog.Common.NotFound,
                 message: "Dog not found.");
         
         public static Error NameExists =>
             Error.Conflict(
                 code: DomainErrorCodes.Dog.Conflict.NameExists,
                 message: "Dog with this name already exists.");
     }
 }
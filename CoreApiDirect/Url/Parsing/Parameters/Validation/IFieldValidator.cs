using System;

namespace CoreApiDirect.Url.Parsing.Parameters.Validation
{
    internal interface IFieldValidator
    {
        bool ValidateField(string field, Type parentType);
    }
}

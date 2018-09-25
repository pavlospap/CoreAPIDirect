using System;
using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class ParameterParserFactory : IParameterParserFactory
    {
        private readonly IBooleanParameterParser _booleanParameterParser;
        private readonly IIntegerParameterParser _integerParameterParser;
        private readonly IStringParameterParser _stringParameterParser;
        private readonly IFieldsParameterParser _fieldsParameterParser;
        private readonly ISortParameterParser _sortParameterParser;
        private readonly IFilterParameterParser _filterParameterParser;

        public ParameterParserFactory(
            IBooleanParameterParser booleanParameterParser,
            IIntegerParameterParser integerParameterParser,
            IStringParameterParser stringParameterParser,
            IFieldsParameterParser fieldsParameterParser,
            ISortParameterParser sortParameterParser,
            IFilterParameterParser filterParameterParser)
        {
            _booleanParameterParser = booleanParameterParser;
            _integerParameterParser = integerParameterParser;
            _stringParameterParser = stringParameterParser;
            _fieldsParameterParser = fieldsParameterParser;
            _sortParameterParser = sortParameterParser;
            _filterParameterParser = filterParameterParser;
        }

        public IParameterParser Create(string property)
        {
            switch (property)
            {
                case nameof(QueryString.ValidateRoute):
                case nameof(QueryString.ValidateQueryString):
                case nameof(QueryString.CaseSensitiveSearch):
                    return _booleanParameterParser;
                case nameof(QueryString.RelatedDataLevel):
                case nameof(QueryString.PageNumber):
                case nameof(QueryString.PageSize):
                    return _integerParameterParser;
                case nameof(QueryParams.Search):
                    return _stringParameterParser;
                case nameof(QueryParams.Fields):
                    return _fieldsParameterParser;
                case nameof(QueryParams.Sort):
                    return _sortParameterParser;
                case nameof(QueryParams.Filter):
                    return _filterParameterParser;
            }

            throw new NotImplementedException(nameof(IParameterParser));
        }
    }
}

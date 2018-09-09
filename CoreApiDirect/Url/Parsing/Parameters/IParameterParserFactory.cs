namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal interface IParameterParserFactory
    {
        IParameterParser Create(string property);
    }
}

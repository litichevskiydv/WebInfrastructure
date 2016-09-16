namespace Web.Application.Services
{
    using System.Collections.Generic;

    public interface IValuesProvider
    {
        IEnumerable<string> Get();

        string Get(int id);
    }
}
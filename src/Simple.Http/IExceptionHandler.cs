using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Http
{
    using Protocol;

    public interface IExceptionHandler
    {
        Status Handle(Exception exception, IContext context);
    }
}

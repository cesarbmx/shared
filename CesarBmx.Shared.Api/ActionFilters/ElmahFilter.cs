﻿using ElmahCore;
using CesarBmx.Shared.Application.Exceptions;
using System;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class ElmahFilter :  IErrorFilter
    {
        public void OnErrorModuleFiltering(object sender, ExceptionFilterEventArgs args)
        {
            // We skip our custom exceptions
            if(args.Exception is ApplicationException)
                args.Dismiss();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public class ViewLayout : ResultFilterAttribute
    {
        private string Layout;
        public ViewLayout(string Layout)
        {
            this.Layout = Layout;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var ViewResult = context.Result as ViewResult;
            if (ViewResult != null)
            {
                ViewResult.ViewData["Layout"] = this.Layout;
            }
        }
    }
}

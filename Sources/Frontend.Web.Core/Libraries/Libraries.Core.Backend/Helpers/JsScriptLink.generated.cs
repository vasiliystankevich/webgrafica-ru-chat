﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Helpers/JsScriptLink.cshtml")]
    public partial class _Helpers_JsScriptLink_cshtml : System.Web.Mvc.WebViewPage<Uri>
    {
        public _Helpers_JsScriptLink_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<script");

WriteAttribute("src", Tuple.Create(" src=\"", 19), Tuple.Create("\"", 42)
            
            #line 2 "..\..\Helpers\JsScriptLink.cshtml"
, Tuple.Create(Tuple.Create("", 25), Tuple.Create<System.Object, System.Int32>(Model.ToString()
            
            #line default
            #line hidden
, 25), false)
);

WriteLiteral("></script>");

        }
    }
}
#pragma warning restore 1591
#pragma checksum "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d0386f1278a05d8b1d43482fb369bb89564cb02c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auto_MyCars), @"mvc.1.0.view", @"/Views/Auto/MyCars.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\_ViewImports.cshtml"
using APCassandra;

#line default
#line hidden
#line 2 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\_ViewImports.cshtml"
using APCassandra.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d0386f1278a05d8b1d43482fb369bb89564cb02c", @"/Views/Auto/MyCars.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4fbe6ced9c1d8d95cc2009b33960771cd3081a7c", @"/Views/_ViewImports.cshtml")]
    public class Views_Auto_MyCars : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#line 2 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
  
    ViewData["Title"] = "MyCars";

#line default
#line hidden
            WriteLiteral("\r\n<h1>MyCars</h1>\r\n\r\n");
#line 8 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
 foreach (var car in Model)
{

#line default
#line hidden
            WriteLiteral("    <hr />\r\n    <div><a");
            BeginWriteAttribute("href", " href=\"", 120, "\"", 149, 2);
            WriteAttributeValue("", 127, "/auto/edit/?id=", 127, 15, true);
#line 11 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
WriteAttributeValue("", 142, car.Id, 142, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(">edit</a></div>\r\n    <div>Preview: <a");
            BeginWriteAttribute("href", " href=\"", 187, "\"", 206, 2);
            WriteAttributeValue("", 194, "/car/", 194, 5, true);
#line 12 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
WriteAttributeValue("", 199, car.Id, 199, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(">");
#line 12 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
                                    Write(car.Brand);

#line default
#line hidden
            WriteLiteral(" ");
#line 12 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
                                               Write(car.Model);

#line default
#line hidden
            WriteLiteral("</a></div>\r\n    <div>Year: ");
#line 13 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
          Write(car.Year);

#line default
#line hidden
            WriteLiteral("</div>\r\n    <div>Type: ");
#line 14 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
          Write(car.Type);

#line default
#line hidden
            WriteLiteral("</div>\r\n    <div>Fuel: ");
#line 15 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
          Write(car.Fuel);

#line default
#line hidden
            WriteLiteral("</div>\r\n    <div>Brand: ");
#line 16 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
           Write(car.Price);

#line default
#line hidden
            WriteLiteral("</div>\r\n    <img");
            BeginWriteAttribute("src", " src=\"", 379, "\"", 407, 2);
            WriteAttributeValue("", 385, "/images/", 385, 8, true);
#line 17 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
WriteAttributeValue("", 393, car.ShowImage, 393, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <hr />\r\n");
#line 19 "D:\NBPProjekti\APCassandra\APCassandra\APCassandra\Views\Auto\MyCars.cshtml"
}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

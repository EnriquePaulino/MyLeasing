using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboPropertyTypes();
    }
}
﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MyLeasing.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboPropertyTypes();

        IEnumerable<SelectListItem> GetComboLessees();

        IEnumerable<SelectListItem> GetComboRoles();
    }
}
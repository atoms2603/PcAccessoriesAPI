﻿using PcAccessories.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAccessories.Services.CategoryService
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<Category> GetCategoryById(Guid id);
    }
}

﻿using DoiFApp.Data.Models;

namespace DoiFApp.Services
{
    public interface ITeacherFinder
    {
        Task<List<EducationTeacherModel>?> FindByPart(string? part, bool needInclude = false);
    }
}

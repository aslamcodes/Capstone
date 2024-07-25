﻿using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Course
{
    public class CourseRequestDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public int EducatorId { get; set; }
        public float Price { get; set; }

        [RegularExpression("^(Begginer|Intermediate|Advanced)$", ErrorMessage = "Invalid value. Allowed values are: Begginer, Intermediate, Advanced.")]
        public string Level { get; set; }
    }
}
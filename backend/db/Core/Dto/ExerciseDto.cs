using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record ExerciseDto(string Name, string Description, string Language, Year Year, string Subject, ArrayOfSnippetsDto arrayOfSnippets);
}

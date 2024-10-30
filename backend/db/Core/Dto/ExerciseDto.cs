using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
public record ExerciseDto(string Name, string Creator, string Description, string Language, List<Tag> Tags, SnippetDto[] arrayOfSnippets, DateTime DateCreated, DateTime DateUpdated);

}

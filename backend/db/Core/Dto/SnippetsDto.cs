using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record class SnippetDto(string Code, bool ReadOnlySection, string FileName);
}

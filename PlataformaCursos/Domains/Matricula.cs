using System;
using System.Collections.Generic;

namespace PlataformaCursos.Domains;

public partial class Matricula
{
    public int IdMatricula { get; set; }

    public int IdAluno { get; set; }

    public int IdCurso { get; set; }

    public DateTime? DataMatricula { get; set; }

    public virtual Aluno IdAlunoNavigation { get; set; } = null!;

    public virtual Curso IdCursoNavigation { get; set; } = null!;
}

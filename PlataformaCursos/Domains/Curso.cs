using System;
using System.Collections.Generic;

namespace PlataformaCursos.Domains;

public partial class Curso
{
    public int IdCurso { get; set; }

    public string Nome { get; set; } = null!;

    public int CargaHoraria { get; set; }

    public bool Disponivel { get; set; }

    public int IdInstrutor { get; set; }

    public virtual Instrutor IdInstrutorNavigation { get; set; } = null!;

    public virtual ICollection<Matricula> Matricula { get; set; } = new List<Matricula>();
}

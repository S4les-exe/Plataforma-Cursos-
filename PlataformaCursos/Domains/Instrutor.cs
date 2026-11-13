using System;
using System.Collections.Generic;

namespace PlataformaCursos.Domains;

public partial class Instrutor
{
    public int IdInstrutor { get; set; }

    public string Nome { get; set; } = null!;

    public string AreaEspecializacao { get; set; } = null!;

    public byte[] Senha { get; set; } = null!;

    public virtual ICollection<Curso> Curso { get; set; } = new List<Curso>();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Interfaces
{
    public interface IMailService
    {
        void Enviar(string destinatario, string assunto, string conteudo);
    }
}

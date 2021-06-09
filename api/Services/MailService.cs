using api.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace api.Services
{
    public class MailService : IMailService
    {
        protected readonly IWebHostEnvironment env;
        protected readonly SmtpClient smtpClient;

        public MailService(IWebHostEnvironment _env)
        {
            this.env = _env;
        }
        public void Send(string from, string to, string subject, string content)
        {
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = this.env.ContentRootPath
            };
            smtpClient.Send(from, to, subject, content);
        }

        public void Enviar(string destinatario, string assunto, string conteudo)
        {
            BackgroundJob.Enqueue(() => this.Send("from@email.com", destinatario, assunto, conteudo));
        }
    }
}

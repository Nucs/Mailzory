﻿using Mailzory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            NormalSenario();
            ModelBasedNormalSenario();
            CustomSmptClientSenario();
        }

        private static void CustomSmptClientSenario()
        {
            var smtpClient = new SmtpClient("host")
            {
                EnableSsl=true,
                Host="",
                Timeout=60,
                UseDefaultCredentials=false,
                Port=587,
                Credentials = new NetworkCredential("username", "password")
            };
            // template path
            var viewPath = Path.Combine("Views/Emails", "hello.cshtml");
            // read the content of template and pass it to the Email constructor
            var template = File.ReadAllText(viewPath);
            var email = new Email(template, smtpClient);
            email.Send("test@outlook.com", "subject");
        }

        private static void ModelBasedNormalSenario()
        {
            var viewPath = Path.Combine("Views/Emails", "MessageModelHello.cshtml");
            // read the content of template and pass it to the Email constructor
            var template = File.ReadAllText(viewPath);
            // fill model
            var model = new MessageModel
            {
                Content = "Mailzory Is Funny. Its a Model Based message.",
                Name = "Johnny"
            };

            var email = new Email<MessageModel>(template,model);
            // send it
            var task =
                email.SendAsync(new[] { "test@outlook.com" }
                , "subject");

            task.Wait();
        }

        private static void NormalSenario()
        {
            // template path
            var viewPath = Path.Combine("Views/Emails", "hello.cshtml");
            // read the content of template and pass it to the Email constructor
            var email = new Email(File.ReadAllText(viewPath));
            // set ViewBag properties
            email.ViewBag.Name = "Johnny";
            email.ViewBag.Content = "Mailzory Is Funny";
            // send it
            email.Send("test@outlook.com", "subject");
        }
    }
}
